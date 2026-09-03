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

        var internalModule = module.AsInternal();
        var applied = internalModule.TrySetDistributedResult(result);
        var acceptedResult = !applied && internalModule.ResultTask.IsCompletedSuccessfully
            ? internalModule.ResultTask.Result
            : result;
        _resultRegistry.TryRegisterResult(module.GetType(), acceptedResult);

        return applied;
    }
}
