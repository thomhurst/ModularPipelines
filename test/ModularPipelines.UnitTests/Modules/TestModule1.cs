using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Modules;

public class TestModule1 : Module<IDictionary<string, object>?>
{
    /// <inheritdoc/>
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();
        return null;
    }
}