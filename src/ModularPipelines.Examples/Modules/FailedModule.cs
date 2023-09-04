using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

[DependsOn<SuccessModule3>(IgnoreIfNotRegistered = true)]
public class FailedModule : Module
{
    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(9), cancellationToken);

        throw new Exception();
    }
}
