using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal sealed class ExecutionBackendContext(IModuleResultRegistry resultRegistry) : IExecutionBackendContext
{
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;

    public bool TryApplyResult(IModule module, IModuleResult result)
    {
        ArgumentNullException.ThrowIfNull(module);
        ArgumentNullException.ThrowIfNull(result);

        var applied = module.AsInternal().TrySetDistributedResult(result);
        if (applied)
        {
            _resultRegistry.RegisterResult(module.GetType(), result);
        }

        return applied;
    }
}
