using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;

namespace ModularPipelines.Examples;

public class SubmodulesModule : Module<IDictionary<string, object>?>, ISkippable
{
    public Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        return Task.FromResult(SkipDecision.DoNotSkip);
    }

    /// <inheritdoc/>
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        foreach (var c in Guid.NewGuid().ToString().Take(3))
        {
            await context.SubModule(c.ToString(), async () =>
            {
                context.Logger.LogInformation("{Submodule}", c.ToString());
                await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
            });
        }

        return new Dictionary<string, object>();
    }
}