using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.Options;
using Semaphores;

namespace ModularPipelines.Helpers;

internal class ParallelLimitProvider : IParallelLimitProvider
{
    private readonly ConcurrentDictionary<Type, AsyncSemaphore> _locks = new();

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

    /// <summary>
    /// Gets a semaphore lock for the specified parallel limit type.
    /// Uses static abstract interface member to avoid reflection.
    /// </summary>
    public AsyncSemaphore GetLock<TParallelLimit>() where TParallelLimit : IParallelLimit
    {
        var limit = TParallelLimit.Limit;

        if (limit <= 0)
        {
            throw new ArgumentException(
                $"Parallel limit for type '{typeof(TParallelLimit).FullName}' must be a positive integer, but was {limit}.");
        }

        return _locks.GetOrAdd(typeof(TParallelLimit), _ => new AsyncSemaphore(limit));
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