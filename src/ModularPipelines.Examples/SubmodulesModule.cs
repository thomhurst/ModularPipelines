using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples;

public class SubmodulesModule : ModuleNew
{
    protected override Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        return base.ShouldSkip(context);
    }

    /// <inheritdoc/>
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        foreach (var c in Guid.NewGuid().ToString().Take(3))
        {
            await SubModule(c.ToString(), async () =>
            {
                context.Logger.LogInformation("{Submodule}", c.ToString());
                await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
            });
        }

        return new Dictionary<string, object>();
    }
}