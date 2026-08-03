using ModularPipelines.Context;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Interface for the module execution pipeline.
/// </summary>
internal interface IModuleExecutionPipeline
{
    /// <summary>
    /// Executes a module with all applicable behaviors.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<ModuleResult<T>> ExecuteAsync<T>(
        Module<T> module,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext,
        CancellationToken engineCancellationToken,
        Func<CancellationToken, Task>? prepareExecutionAsync = null,
        Func<CancellationToken, Task>? completeExecutionAsync = null);
}
