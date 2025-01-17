using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bullseye.Internal
{
    public class ActionTarget : Target
    {
        private readonly Func<Task> action;

        public ActionTarget(string name, string description, IEnumerable<string> dependencies, Func<Task> action)
            : base(name, description, dependencies) => this.action = action;

        public override async Task RunAsync(bool dryRun, bool parallel, Output output, Func<Exception, bool> messageOnly, IReadOnlyCollection<Target> dependencyPath)
        {
            await output.BeginGroup(this).Tax();

            try
            {
                await output.Starting(this, dependencyPath).Tax();

                var stopWatch = new Stopwatch();

                if (!dryRun)
                {
                    await this.RunAsync(output, messageOnly, dependencyPath, stopWatch).Tax();
                }

                await output.Succeeded(this, dependencyPath, stopWatch.Elapsed).Tax();
            }
            finally
            {
                await output.EndGroup().Tax();
            }
        }

        private async Task RunAsync(Output output, Func<Exception, bool> messageOnly, IReadOnlyCollection<Target> dependencyPath, Stopwatch stopWatch)
        {
            stopWatch.Start();

            try
            {
                await this.action().Tax();
            }
            catch (Exception ex)
            {
                var duration = stopWatch.Elapsed;

                if (!messageOnly(ex))
                {
                    await output.Error(this, ex).Tax();
                }

                await output.Failed(this, ex, duration, dependencyPath).Tax();

                throw new TargetFailedException($"Target '{this.Name}' failed.", ex);
            }
        }
    }
}
