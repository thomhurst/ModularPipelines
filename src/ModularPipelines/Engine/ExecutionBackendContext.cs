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
        if (applied)
        {
            _resultRegistry.RegisterResult(module.GetType(), result);
        }
        else if (_resultRegistry.GetResult(module.GetType()) is null
                 && internalModule.ResultTask.IsCompletedSuccessfully)
        {
            _resultRegistry.RegisterResult(module.GetType(), internalModule.ResultTask.Result);
        }

        return applied;
    }
}
