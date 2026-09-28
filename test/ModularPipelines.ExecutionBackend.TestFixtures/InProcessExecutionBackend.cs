using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.ExecutionBackend.TestFixtures;

// This assembly has no InternalsVisibleTo access to the engine.
public sealed class InProcessExecutionBackend : IExecutionBackend
{
    public bool OwnsEntirePlan => true;

    public async Task<IReadOnlyList<IModuleResult>> ExecuteAsync(ExecutionBackendRequest request, CancellationToken cancellationToken)
    {
        var modules = request.Modules;
        var context = request.Context;
        return await Task.WhenAll(modules.Select(module =>
            context.ExecuteModuleAsync(module, cancellationToken)));
    }
}
