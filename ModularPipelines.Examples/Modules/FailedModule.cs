using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class FailedModule : Module
{
    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

        throw new Exception();
    }
}
