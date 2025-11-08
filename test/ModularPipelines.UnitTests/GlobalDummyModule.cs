using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class GlobalDummyModule : ModuleNew
{
    /// <inheritdoc/>
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();
        return null;
    }
}