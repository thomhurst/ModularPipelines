using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Modules;

public class TestModule1 : IModule<IDictionary<string, object>?>
{
    /// <inheritdoc/>
    public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();
        return null;
    }
}