using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for managing parallel execution limits.
/// </summary>
internal class ParallelLimitHandler : IParallelLimitHandler
{
    private readonly IParallelLimitProvider _parallelLimitProvider;
    private readonly ILogger<ParallelLimitHandler> _logger;

    public ParallelLimitHandler(
        IParallelLimitProvider parallelLimitProvider,
        ILogger<ParallelLimitHandler> logger)
    {
        _parallelLimitProvider = parallelLimitProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IDisposable> AcquireParallelLimitAsync(
        IModule module,
        CancellationToken cancellationToken)
    {
        if (module.Configuration.ParallelLimitKey is { } key)
        {
            _logger.LogDebug("Module {ModuleName} acquiring parallel limit for {LimitKey}", module.GetType().Name, key);
            var keyedSemaphore = _parallelLimitProvider.GetLock(key);
            await keyedSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            return new SemaphoreReleaser(keyedSemaphore);
        }

        var parallelLimiterAttribute = module.Configuration.ParallelLimiter;

        if (parallelLimiterAttribute != null)
        {
            _logger.LogDebug(
                "Module {ModuleName} acquiring parallel limit from {LimiterType}",
                module.GetType().Name,
                parallelLimiterAttribute.Type.Name);

            // Use the attribute's GetLock method to avoid reflection on IParallelLimit
            var semaphore = parallelLimiterAttribute.GetLock(_parallelLimitProvider);
            await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            return new SemaphoreReleaser(semaphore);
        }

        return NoOpDisposable.Instance;
    }

    /// <inheritdoc />
    public async Task<IDisposable> AcquireExecutionHintLimitAsync(
        ModuleState moduleState,
        CancellationToken cancellationToken)
    {
        var executionHintLock = _parallelLimitProvider.GetExecutionHintLock(moduleState.ExecutionHint);

        if (executionHintLock != null)
        {
            _logger.LogDebug(
                "Module {ModuleName} waiting for {ExecutionHint} execution slot",
                moduleState.ModuleType.Name,
                moduleState.ExecutionHint);

            await executionHintLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            return new SemaphoreReleaser(executionHintLock);
        }

        return NoOpDisposable.Instance;
    }

    private sealed class SemaphoreReleaser(SemaphoreSlim semaphore) : IDisposable
    {
        private SemaphoreSlim? _semaphore = semaphore;

        public void Dispose()
        {
            Interlocked.Exchange(ref _semaphore, null)?.Release();
        }
    }
}
