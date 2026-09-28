using ModularPipelines.Models;

namespace ModularPipelines.Modules;

internal interface IInternalModule : IModule
{
    Task<IModuleResult> ResultTask { get; }

    bool TrySetDistributedResult(IModuleResult result);

    /// <summary>
    /// Completes the module's awaitable with <paramref name="result"/> unless another writer
    /// completed it first, and returns the result the awaitable holds.
    /// </summary>
    IModuleResult CompleteResult(IModuleResult result);
}
