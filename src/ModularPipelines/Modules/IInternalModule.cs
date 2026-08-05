using ModularPipelines.Models;

namespace ModularPipelines.Modules;

internal interface IInternalModule : IModule
{
    Task<IModuleResult> ResultTask { get; }

    bool TrySetDistributedResult(IModuleResult result);
}
