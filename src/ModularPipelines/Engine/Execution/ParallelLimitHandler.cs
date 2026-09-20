using System.Reflection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;

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
        Type moduleType,
        CancellationToken cancellationToken)
    {
        var parallelLimiterAttribute =
            moduleType.GetCustomAttributes<ParallelLimiterAttribute>().FirstOrDefault();

        if (parallelLimiterAttribute != null)
        {
            _logger.LogDebug(
                "Module {ModuleName} acquiring parallel limit from {LimiterType}",
                moduleType.Name,
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
