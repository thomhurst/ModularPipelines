using System.Collections.Concurrent;
using ModularPipelines.Helpers;

namespace ModularPipelines.Distributed.Worker;

internal static class DistributedWorkerPool
{
    private static readonly TimeSpan DequeueRetryDelay = TimeSpan.FromMilliseconds(100);

    public static int GetMaxConcurrency(
        IParallelLimitProvider parallelLimitProvider,
        DistributedOptions options)
    {
        var pipelineLimit = parallelLimitProvider.GetMaxDegreeOfParallelism();
        var nodeLimit = options.MaxParallelism ?? pipelineLimit;
        if (nodeLimit < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                nodeLimit,
                "Distributed MaxParallelism must be at least 1.");
        }

        return Math.Min(pipelineLimit, nodeLimit);
    }

    /// <summary>
    /// Claims and executes leases with at most <paramref name="maxConcurrency"/> in flight. A slot is
    /// acquired before each claim, so the worker never holds a claimed lease it cannot start.
    /// </summary>
    public static async Task RunAsync(
        Func<CancellationToken, Task<ModuleLease?>> dequeueAsync,
        Func<ModuleLease, DateTimeOffset, CancellationToken, Task> executeAsync,
        int maxConcurrency,
        Action<Exception> onError,
        CancellationToken cancellationToken,
        TimeProvider? timeProvider = null,
        InFlightLeases? inFlightLeases = null)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxConcurrency, 1);
        var clock = timeProvider ?? TimeProvider.System;

        using var concurrencyGate = new SemaphoreSlim(maxConcurrency, maxConcurrency);
        var running = new List<Task>();
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await concurrencyGate.WaitAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            var (lease, claimedAt) = await DequeueAsync(dequeueAsync, onError, clock, cancellationToken)
                .ConfigureAwait(false);
            if (lease is null)
            {
                concurrencyGate.Release();
                break;
            }

            // The coordinator has transferred ownership. Execute even after cancellation so the
            // execution path can publish a terminal result for the lease.
            running.RemoveAll(static task => task.IsCompletedSuccessfully);
            running.Add(ExecuteAndReleaseAsync(
                lease,
                claimedAt,
                executeAsync,
                onError,
                concurrencyGate,
                inFlightLeases,
                cancellationToken));
        }

        await Task.WhenAll(running).ConfigureAwait(false);
    }

    private static async Task<(ModuleLease? Lease, DateTimeOffset ClaimedAt)> DequeueAsync(
        Func<CancellationToken, Task<ModuleLease?>> dequeueAsync,
        Action<Exception> onError,
        TimeProvider clock,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var lease = await dequeueAsync(cancellationToken).ConfigureAwait(false);
                return (lease, clock.GetUtcNow());
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return (null, default);
            }
            catch (Exception exception)
            {
                onError(exception);
                try
                {
                    await Task.Delay(DequeueRetryDelay, cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    return (null, default);
                }
            }
        }

        return (null, default);
    }

    private static async Task ExecuteAndReleaseAsync(
        ModuleLease lease,
        DateTimeOffset claimedAt,
        Func<ModuleLease, DateTimeOffset, CancellationToken, Task> executeAsync,
        Action<Exception> onError,
        SemaphoreSlim concurrencyGate,
        InFlightLeases? inFlightLeases,
        CancellationToken cancellationToken)
    {
        inFlightLeases?.Add(lease);
        try
        {
            await executeAsync(lease, claimedAt, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException exception)
            when (cancellationToken.IsCancellationRequested ||
                  exception.CancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            onError(exception);
        }
        finally
        {
            inFlightLeases?.Remove(lease);
            concurrencyGate.Release();
        }
    }
}

/// <summary>
/// Tracks the leases a process is executing so its heartbeats can renew them.
/// </summary>
internal sealed class InFlightLeases
{
    private readonly ConcurrentDictionary<string, ModuleLease> _leases = new(StringComparer.Ordinal);

    public void Add(ModuleLease lease) => _leases[lease.LeaseId] = lease;

    public void Remove(ModuleLease lease) => _leases.TryRemove(lease.LeaseId, out _);

    public IReadOnlyList<ModuleId> GetModuleIds() =>
        [.. _leases.Values.Select(static lease => lease.Assignment.ModuleId).Distinct()];
}
