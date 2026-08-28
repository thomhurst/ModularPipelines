namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for managing parallel execution limits.
/// Handles both custom parallel limiters (via ParallelLimiterAttribute) and
/// execution hint limits (CPU-bound, I/O-bound).
/// </summary>
internal interface IParallelLimitHandler
{
    /// <summary>
    /// Acquires a parallel limit semaphore for the specified module type.
    /// </summary>
    /// <param name="moduleType">The type of the module.</param>
    /// <param name="cancellationToken">The token that cancels waiting for a slot.</param>
    /// <returns>A disposable that releases the semaphore when disposed.</returns>
    Task<IDisposable> AcquireParallelLimitAsync(Type moduleType, CancellationToken cancellationToken);

    /// <summary>
    /// Acquires an execution hint limit semaphore for the specified module state.
    /// </summary>
    /// <param name="moduleState">The module state containing execution hint information.</param>
    /// <param name="cancellationToken">The token that cancels waiting for a slot.</param>
    /// <returns>A disposable that releases the semaphore when disposed.</returns>
    Task<IDisposable> AcquireExecutionHintLimitAsync(ModuleState moduleState, CancellationToken cancellationToken);
}
