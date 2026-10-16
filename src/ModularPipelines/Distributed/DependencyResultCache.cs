using System.Collections.Concurrent;

namespace ModularPipelines.Distributed;

internal sealed class DependencyResultCache(
    IDistributedWorkerCoordinator coordinator,
    CancellationToken workerCancellationToken,
    TimeProvider? timeProvider = null)
{
    private static readonly TimeSpan FetchTimeout = TimeSpan.FromSeconds(30);
    private readonly ConcurrentDictionary<ModuleId, Lazy<Task<SerializedModuleResult>>> _results = new();

    public async Task<SerializedModuleResult> GetAsync(ModuleId moduleId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var lazyResult = _results.GetOrAdd(
            moduleId,
            name => new Lazy<Task<SerializedModuleResult>>(
                () => FetchAsync(name),
                LazyThreadSafetyMode.ExecutionAndPublication));

        // Cancel only this assignment's wait, not the fetch shared with other assignments.
        return await lazyResult.Value.WaitAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task<SerializedModuleResult> FetchAsync(ModuleId moduleId)
    {
        using var timeoutCts = new CancellationTokenSource(FetchTimeout, timeProvider ?? TimeProvider.System);
        using var fetchCts = CancellationTokenSource.CreateLinkedTokenSource(workerCancellationToken, timeoutCts.Token);
        try
        {
            // Bound the wait even when a coordinator ignores cancellation.
            return await coordinator.WaitForResultAsync(moduleId, fetchCts.Token)
                .WaitAsync(fetchCts.Token)
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            // Evict before completing the fetch, even if all assignment waiters already canceled.
            _results.TryRemove(moduleId, out _);
            if (exception is OperationCanceledException
                && timeoutCts.IsCancellationRequested && !workerCancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException(
                    $"Dependency result unavailable for '{moduleId}' after waiting {FetchTimeout}.", exception);
            }

            throw;
        }
    }
}
