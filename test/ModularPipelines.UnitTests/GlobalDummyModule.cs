using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class GlobalDummyModule : IModule<IDictionary<string, object>?>
{
    /// <inheritdoc/>
    public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();
        return null;
    }
}