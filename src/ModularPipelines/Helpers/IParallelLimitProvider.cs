using ModularPipelines.Enums;
using ModularPipelines.Interfaces;

namespace ModularPipelines.Helpers;

internal interface IParallelLimitProvider
{
    /// <summary>
    /// Gets a semaphore lock for the specified parallel limit type.
    /// Uses static abstract interface member to avoid reflection.
    /// </summary>
    SemaphoreSlim GetLock<TParallelLimit>() where TParallelLimit : IParallelLimit;

    int GetMaxDegreeOfParallelism();

    /// <summary>
    /// Gets a semaphore lock for execution hint throttling (CPU-bound or I/O-bound).
    /// Returns null if no limit is configured for the execution hint.
    /// </summary>
    SemaphoreSlim? GetExecutionHintLock(ExecutionHint executionHint);
}
