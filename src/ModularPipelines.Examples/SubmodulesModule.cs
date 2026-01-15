using Microsoft.Extensions.Logging;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples;

public class SubmodulesModule : Module<IDictionary<string, object>?>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(() => SkipDecision.DoNotSkip)
        .Build();

    /// <inheritdoc/>
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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