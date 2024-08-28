using Semaphores;

namespace ModularPipelines.Helpers;

internal interface IParallelLimitProvider
{
    AsyncSemaphore GetLock(Type parallelLimitType);
}