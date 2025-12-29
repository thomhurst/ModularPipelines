namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for managing parallel execution limits.
/// Handles both custom parallel limiters (via ParallelLimiterAttribute) and
/// execution type limits (CPU-intensive, IO-intensive).
/// </summary>
internal interface IParallelLimitHandler
{
    /// <summary>
    /// Acquires a parallel limit semaphore for the specified module type.
    /// </summary>
    /// <param name="moduleType">The type of the module.</param>
    /// <returns>A disposable that releases the semaphore when disposed.</returns>
    Task<IDisposable> AcquireParallelLimitAsync(Type moduleType);

    /// <summary>
    /// Acquires an execution type limit semaphore for the specified module state.
    /// </summary>
    /// <param name="moduleState">The module state containing execution type information.</param>
    /// <returns>A disposable that releases the semaphore when disposed.</returns>
    Task<IDisposable> AcquireExecutionTypeLimitAsync(ModuleState moduleState);
}
