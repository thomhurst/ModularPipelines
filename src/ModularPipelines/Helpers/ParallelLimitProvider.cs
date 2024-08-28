using System.Collections.Concurrent;
using ModularPipelines.Interfaces;
using Semaphores;

namespace ModularPipelines.Helpers;

internal class ParallelLimitProvider : IParallelLimitProvider
{
    private static readonly ConcurrentDictionary<Type, AsyncSemaphore> Locks = new();

    public AsyncSemaphore GetLock(Type parallelLimitType)
    {
        var parallelLimit = Activator.CreateInstance(parallelLimitType) as IParallelLimit;
        if (parallelLimit?.Limit is null or <= 0)
        {
            throw new Exception("Parallel Limit must be positive");
        }
        
        return Locks.GetOrAdd(parallelLimit.GetType(), _ => new AsyncSemaphore(parallelLimit.Limit));
    }
}