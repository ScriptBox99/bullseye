namespace BullseyeSmokeTester.DragonFruit
{
    using Bullseye;
    using static Bullseye.Targets;

    internal static class Program
    {
        /// <summary>Run or list targets.</summary>
        /// <param name="foo">A value used for something.</param>
        /// <param name="targets">A list of targets to run or list.</param>
        /// <param name="clear">Clear the console before execution.</param>
        /// <param name="dryRun">Do a dry run without executing actions.</param>
        /// <param name="host">Force the mode for a specific host environment (normally auto-detected).</param>
        /// <param name="listDependencies">List all (or specified) targets and dependencies, then exit.</param>
        /// <param name="listInputs">List all (or specified) targets and inputs, then exit.</param>
        /// <param name="listTargets">List all (or specified) targets, then exit.</param>
        /// <param name="listTree">List all (or specified) targets and dependency trees, then exit.</param>
        /// <param name="noColor">Disable colored output.</param>
        /// <param name="parallel">Run targets in parallel.</param>
        /// <param name="skipDependencies">Do not run targets' dependencies.</param>
        /// <param name="verbose">Enable verbose output.</param>
        private static void Main(
            string foo,
            string[] targets,
            bool clear,
            bool dryRun,
            Host host,
            bool listDependencies,
            bool listInputs,
            bool listTargets,
            bool listTree,
            bool noColor,
            bool parallel,
            bool skipDependencies,
            bool verbose)
        {
            var options = new Options
            {
                Clear = clear,
                DryRun = dryRun,
                Host = host,
                ListDependencies = listDependencies,
                ListInputs = listInputs,
                ListTargets = listTargets,
                ListTree = listTree,
                NoColor = noColor,
                Parallel = parallel,
                SkipDependencies = skipDependencies,
                Verbose = verbose,
            };

            Target("build", () => System.Console.WriteLine($"foo = {foo}"));

            Target("default", DependsOn("build"));

            RunTargetsAndExit(targets, options);
        }
    }
}
