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

    internal static bool ShouldCleanup(DistributedRunCleanupPolicy policy, bool pipelineSucceeded) =>
        policy switch
        {
            DistributedRunCleanupPolicy.Always => true,
            DistributedRunCleanupPolicy.OnSuccess => pipelineSucceeded,
            _ => false,
        };

    private async Task<List<IDistributedRunCleanup>> ResolveBackendsAsync(TimeSpan timeout)
    {
        // Resolve each source separately, so one service that fails to construct leaves the others.
        var candidates = Resolve(static provider => provider.GetServices<IDistributedRunCleanup>(), nameof(IDistributedRunCleanup))
            .Concat(Resolve(static provider => [provider.GetService<IDistributedMasterCoordinator>()], nameof(IDistributedMasterCoordinator)))
            .Concat(Resolve(static provider => [provider.GetService<IDistributedArtifactStore>()], nameof(IDistributedArtifactStore)))
            .Concat(Resolve(static provider => [provider.GetService<IMasterDiscovery>()], nameof(IMasterDiscovery)))
            .ToArray();

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

    private IEnumerable<object?> Resolve(Func<IServiceProvider, IEnumerable<object?>> resolve, string serviceName)
    {
        try
        {
            return [.. resolve(serviceProvider)];
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
