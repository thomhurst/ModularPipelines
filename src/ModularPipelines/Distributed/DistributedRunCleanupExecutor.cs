using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;

namespace ModularPipelines.Distributed;

/// <summary>
/// Asks every <see cref="IDistributedRunCleanup"/> backend to delete the run's data once the pipeline ends.
/// </summary>
/// <remarks>
/// Backends are resolved only when cleanup runs, so a run that keeps its data constructs nothing.
/// Cleanup is best effort: no failure escapes, so it cannot replace the pipeline result.
/// </remarks>
internal sealed class DistributedRunCleanupExecutor(
    IServiceProvider serviceProvider,
    IOptions<DistributedOptions> options,
    IExecutionLocationContext executionLocationContext,
    ILogger<DistributedRunCleanupExecutor> logger)
{
    private static readonly TimeSpan WorkerPollInterval = TimeSpan.FromMilliseconds(250);

    public async Task CleanupAsync(bool pipelineSucceeded)
    {
        try
        {
            // Only a distributed master owns the run's data; workers finish before it. Local pipelines
            // can reuse a run identifier to share artifacts across separate runs, so they keep their data.
            var distributedOptions = options.Value;
            if (!distributedOptions.Enabled || executionLocationContext.IsWorker)
            {
                return;
            }

            if (!ShouldCleanup(distributedOptions.RunCleanup, pipelineSucceeded))
            {
                return;
            }

            if (!await WaitForWorkersToFinishAsync(distributedOptions).ConfigureAwait(false))
            {
                return;
            }

            var backends = await ResolveBackendsAsync(distributedOptions.RunCleanupTimeout).ConfigureAwait(false);
            if (backends.Count == 0)
            {
                return;
            }

            var context = new DistributedRunCleanupContext
            {
                RunId = distributedOptions.RunId,
                PipelineSucceeded = pipelineSucceeded,
            };
            await Task.WhenAll(backends.Select(backend =>
                    CleanupBackendAsync(backend, context, distributedOptions.RunCleanupTimeout)))
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Could not clean up distributed run data");
        }
    }

    /// <summary>
    /// Waits until every worker registered for the run has published its final status. A worker stops its
    /// heartbeat when execution ends but publishes the final status only after its pipeline end handlers, so
    /// the final status is the only proof that it no longer uses the run's data.
    /// </summary>
    /// <remarks>
    /// A worker whose heartbeat expired drops out of the live registrations while its end handlers can still be
    /// running, so participants also include every worker with a reported status. Coordinators record a status
    /// when a worker registers and keep it after the heartbeat expires.
    /// </remarks>
    /// <returns><see langword="false"/> when a worker did not finish in time, so the run's data is kept.</returns>
    private async Task<bool> WaitForWorkersToFinishAsync(DistributedOptions distributedOptions)
    {
        IReadOnlyCollection<WorkerId> pending = [];
        using var timeoutSource = new CancellationTokenSource(distributedOptions.RunCleanupTimeout);
        try
        {
            if (serviceProvider.GetService<IDistributedMasterCoordinator>() is not { } coordinator)
            {
                return true;
            }

            var token = timeoutSource.Token;
            var expected = (await coordinator.GetRegisteredWorkersAsync(token).WaitAsync(token).ConfigureAwait(false))
                .Where(worker => IsCurrentRun(worker.RunId, distributedOptions.RunId))
                .Select(static worker => worker.WorkerId)
                .ToHashSet();
            while (true)
            {
                var statuses = (await coordinator.GetWorkerStatusesAsync(token).WaitAsync(token).ConfigureAwait(false))
                    .Where(status => IsCurrentRun(status.RunId, distributedOptions.RunId))
                    .ToArray();
                expected.UnionWith(statuses.Select(static status => status.WorkerId));
                expected.Remove(distributedOptions.LocalWorkerId);
                var finished = statuses.Where(static status => status.IsFinal).Select(static status => status.WorkerId);
                pending = [.. expected.Except(finished)];
                if (pending.Count == 0)
                {
                    return true;
                }

                await Task.Delay(WorkerPollInterval, token).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException) when (timeoutSource.IsCancellationRequested)
        {
            logger.LogWarning(
                "Keeping distributed run data because workers {Workers} did not publish a final status within {Timeout}",
                string.Join(", ", pending),
                distributedOptions.RunCleanupTimeout);
            return false;
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Keeping distributed run data because worker completion could not be checked");
            return false;
        }
    }

    private static bool IsCurrentRun(string? workerRunId, string runId) =>
        string.IsNullOrWhiteSpace(workerRunId) || string.Equals(workerRunId, runId, StringComparison.Ordinal);

    internal static bool ShouldCleanup(DistributedRunCleanupPolicy policy, bool pipelineSucceeded) =>
        policy switch
        {
            DistributedRunCleanupPolicy.Always => true,
            DistributedRunCleanupPolicy.OnSuccess => pipelineSucceeded,
            _ => false,
        };

    private async Task<List<IDistributedRunCleanup>> ResolveBackendsAsync(TimeSpan timeout)
    {
        // Resolve each source separately and in parallel, so one service that fails or blocks while it is
        // constructed leaves the others.
        var sources = await Task.WhenAll(
                ResolveAsync(static provider => provider.GetServices<IDistributedRunCleanup>(), nameof(IDistributedRunCleanup), timeout),
                ResolveAsync(static provider => [provider.GetService<IDistributedMasterCoordinator>()], nameof(IDistributedMasterCoordinator), timeout),
                ResolveAsync(static provider => [provider.GetService<IDistributedArtifactStore>()], nameof(IDistributedArtifactStore), timeout),
                ResolveAsync(static provider => [provider.GetService<IMasterDiscovery>()], nameof(IMasterDiscovery), timeout))
            .ConfigureAwait(false);
        var candidates = sources.SelectMany(static source => source).ToArray();

        var backends = new List<IDistributedRunCleanup>();
        foreach (var candidate in candidates)
        {
            // A deferred wrapper and its backend can both be registered, so compare the backends themselves.
            var resolved = candidate is IDeferredDistributedBackend deferred
                ? await GetDeferredBackendAsync(deferred, timeout).ConfigureAwait(false)
                : candidate;
            if (resolved is IDistributedRunCleanup backend
                && !backends.Any(existing => ReferenceEquals(existing, backend)))
            {
                backends.Add(backend);
            }
        }

        return backends;
    }

    private async Task<object?[]> ResolveAsync(
        Func<IServiceProvider, IEnumerable<object?>> resolve,
        string serviceName,
        TimeSpan timeout)
    {
        try
        {
            // Service factories run synchronously; a thread-pool task lets a blocked one time out.
            return await Task.Run(() => resolve(serviceProvider).ToArray())
                .WaitAsync(timeout)
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Could not resolve {Service} services to clean up run data", serviceName);
            return [];
        }
    }

    // Workers can upload artifacts this process never touched, so the backend is created to clean up.
    private async Task<object?> GetDeferredBackendAsync(IDeferredDistributedBackend deferred, TimeSpan timeout)
    {
        using var timeoutSource = new CancellationTokenSource(timeout);
        try
        {
            return await deferred.GetBackendAsync(timeoutSource.Token)
                .AsTask()
                .WaitAsync(timeoutSource.Token)
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogWarning(
                exception,
                "Could not create the distributed backend behind {Wrapper} to clean up run data",
                deferred.GetType().FullName);
            return null;
        }
    }

    private async Task CleanupBackendAsync(
        IDistributedRunCleanup backend,
        DistributedRunCleanupContext context,
        TimeSpan timeout)
    {
        var backendName = backend.GetType().FullName;
        using var timeoutSource = new CancellationTokenSource(timeout);
        try
        {
            await backend.CleanupRunAsync(context, timeoutSource.Token)
                .WaitAsync(timeoutSource.Token)
                .ConfigureAwait(false);
            logger.LogDebug("Cleaned up distributed run data stored by {Backend}", backendName);
        }
        catch (OperationCanceledException) when (timeoutSource.IsCancellationRequested)
        {
            logger.LogWarning(
                "Timed out after {Timeout} cleaning up distributed run data stored by {Backend}",
                timeout,
                backendName);
        }
        catch (Exception exception)
        {
            logger.LogWarning(
                exception,
                "Could not clean up distributed run data stored by {Backend}",
                backendName);
        }
    }
}
