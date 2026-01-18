using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

[DependsOn<SuccessModule3>(Optional = true)]
public class FailedModule : Module<IDictionary<string, object>?>
{
    /// <inheritdoc/>
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(9), cancellationToken);
        return null;
    }
}