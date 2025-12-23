using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.Options;
using Semaphores;

namespace ModularPipelines.Helpers;

internal class ParallelLimitProvider : IParallelLimitProvider
{
    private static readonly ConcurrentDictionary<Type, AsyncSemaphore> Locks = new();

    private readonly ConcurrencyOptions _concurrencyOptions;
    private readonly Lazy<AsyncSemaphore?> _cpuIntensiveLock;
    private readonly Lazy<AsyncSemaphore?> _ioIntensiveLock;

    public ParallelLimitProvider(IOptions<PipelineOptions> pipelineOptions)
    {
        _concurrencyOptions = pipelineOptions.Value.Concurrency;

        _cpuIntensiveLock = new Lazy<AsyncSemaphore?>(() =>
            _concurrencyOptions.MaxCpuIntensiveModules.HasValue
                ? new AsyncSemaphore(_concurrencyOptions.MaxCpuIntensiveModules.Value)
                : null);

        _ioIntensiveLock = new Lazy<AsyncSemaphore?>(() =>
            _concurrencyOptions.MaxIoIntensiveModules.HasValue
                ? new AsyncSemaphore(_concurrencyOptions.MaxIoIntensiveModules.Value)
                : null);
    }

    public AsyncSemaphore GetLock(Type parallelLimitType)
    {
        var parallelLimit = Activator.CreateInstance(parallelLimitType) as IParallelLimit;
        if (parallelLimit?.Limit is null or <= 0)
        {
            throw new Exception("Parallel Limit must be positive");
        }

        return Locks.GetOrAdd(parallelLimit.GetType(), _ => new AsyncSemaphore(parallelLimit.Limit));
    }

    public int GetMaxDegreeOfParallelism()
    {
        return _concurrencyOptions.MaxParallelism;
    }

    public AsyncSemaphore? GetExecutionTypeLock(ExecutionType executionType)
    {
        return executionType switch
        {
            ExecutionType.CpuIntensive => _cpuIntensiveLock.Value,
            ExecutionType.IoIntensive => _ioIntensiveLock.Value,
            _ => null,
        };
    }
}