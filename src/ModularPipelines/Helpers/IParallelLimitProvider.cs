using ModularPipelines.Enums;
using Semaphores;

namespace ModularPipelines.Helpers;

internal interface IParallelLimitProvider
{
    AsyncSemaphore GetLock(Type parallelLimitType);

    int GetMaxDegreeOfParallelism();

    /// <summary>
    /// Gets a semaphore lock for execution type throttling (CPU or IO intensive).
    /// Returns null if no limit is configured for the execution type.
    /// </summary>
    AsyncSemaphore? GetExecutionTypeLock(ExecutionType executionType);
}