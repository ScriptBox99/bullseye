using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using Bullseye;
using static Bullseye.Targets;

var cmd = new RootCommand()
{
    new Option<string>( new[] { "--foo", "-f" }, "A value used for something."),
};

// translate from Bullseye to System.CommandLine
cmd.Add(new Argument("targets") { Arity = ArgumentArity.ZeroOrMore, Description = "A list of targets to run or list. If not specified, the \"default\" target will be run, or all targets will be listed." });
foreach (var option in Options.Definitions)
{
    cmd.Add(new Option(new[] { option.ShortName, option.LongName }.Where(n => !string.IsNullOrWhiteSpace(n)).ToArray(), option.Description));
}

cmd.Handler = CommandHandler.Create<string>(foo =>
{
    // translate from System.CommandLine to Bullseye
    var cmdLine = cmd.Parse(args);
    var targets = cmdLine.CommandResult.Tokens.Select(token => token.Value);
    var options = new Options(Options.Definitions.Select(o => (o.LongName, cmdLine.ValueForOption<bool>(o.LongName))));

    Target("build", () => System.Console.WriteLine($"foo = {foo}"));

    Target("default", DependsOn("build"));

    RunTargetsAndExit(targets, options);
});

return cmd.Invoke(args);
