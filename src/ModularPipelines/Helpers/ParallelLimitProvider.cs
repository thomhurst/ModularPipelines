using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.Options;

namespace ModularPipelines.Helpers;

internal class ParallelLimitProvider : IParallelLimitProvider, IDisposable
{
    private readonly ConcurrentDictionary<Type, SemaphoreSlim> _locks = new();

    private readonly ConcurrencyOptions _concurrencyOptions;
    private readonly Lazy<SemaphoreSlim?> _cpuIntensiveLock;
    private readonly Lazy<SemaphoreSlim?> _ioIntensiveLock;

    public ParallelLimitProvider(IOptions<PipelineOptions> pipelineOptions)
    {
        _concurrencyOptions = pipelineOptions.Value.Concurrency;

        _cpuIntensiveLock = new Lazy<SemaphoreSlim?>(() =>
            _concurrencyOptions.MaxCpuIntensiveModules.HasValue
                ? new SemaphoreSlim(_concurrencyOptions.MaxCpuIntensiveModules.Value, _concurrencyOptions.MaxCpuIntensiveModules.Value)
                : null);

        _ioIntensiveLock = new Lazy<SemaphoreSlim?>(() =>
            _concurrencyOptions.MaxIoIntensiveModules.HasValue
                ? new SemaphoreSlim(_concurrencyOptions.MaxIoIntensiveModules.Value, _concurrencyOptions.MaxIoIntensiveModules.Value)
                : null);
    }

    /// <summary>
    /// Gets a semaphore lock for the specified parallel limit type.
    /// Uses static abstract interface member to avoid reflection.
    /// </summary>
    public SemaphoreSlim GetLock<TParallelLimit>() where TParallelLimit : IParallelLimit
    {
        var limit = TParallelLimit.Limit;

        if (limit <= 0)
        {
            throw new ArgumentException(
                $"Parallel limit for type '{typeof(TParallelLimit).FullName}' must be a positive integer, but was {limit}.");
        }

        return _locks.GetOrAdd(typeof(TParallelLimit), _ => new SemaphoreSlim(limit, limit));
    }

    public int GetMaxDegreeOfParallelism()
    {
        return _concurrencyOptions.MaxParallelism;
    }

    public SemaphoreSlim? GetExecutionHintLock(ExecutionHint executionHint)
    {
        return executionHint switch
        {
            ExecutionHint.CpuBound => _cpuIntensiveLock.Value,
            ExecutionHint.IoBound => _ioIntensiveLock.Value,
            _ => null,
        };
    }

    public void Dispose()
    {
        foreach (var semaphore in _locks.Values)
        {
            semaphore.Dispose();
        }

        if (_cpuIntensiveLock.IsValueCreated)
        {
            _cpuIntensiveLock.Value?.Dispose();
        }

        if (_ioIntensiveLock.IsValueCreated)
        {
            _ioIntensiveLock.Value?.Dispose();
        }
    }
}
