using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

[DependsOn<DependentOn2>]
public class DependentOn3 : IModule<IDictionary<string, object>?>
{
    /// <inheritdoc/>
    public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
        return null;
    }
}