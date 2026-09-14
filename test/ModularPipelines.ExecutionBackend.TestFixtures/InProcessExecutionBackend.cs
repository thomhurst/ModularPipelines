using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.ExecutionBackend.TestFixtures;

// This assembly has no InternalsVisibleTo access to the engine.
public sealed class InProcessExecutionBackend : IExecutionBackend
{
    public bool OwnsEntirePlan => true;

    public async Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
        IReadOnlyList<IModule> modules,
        IReadOnlyDictionary<Type, TimeSpan> estimatedDurations,
        IExecutionBackendContext context,
        CancellationToken cancellationToken)
    {
        return await Task.WhenAll(modules.Select(module =>
            context.ExecuteModuleAsync(module, cancellationToken)));
    }
}
