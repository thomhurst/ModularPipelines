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
        var leaseLostToken = inFlightLeases?.Add(lease) ?? CancellationToken.None;
        using var executionCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, leaseLostToken);
        try
        {
            await executeAsync(lease, claimedAt, executionCts.Token).ConfigureAwait(false);
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
internal sealed class InFlightLeases(TimeSpan? leaseTimeout = null, TimeProvider? timeProvider = null)
{
    private readonly ConcurrentDictionary<string, Entry> _leases = new(StringComparer.Ordinal);
    private readonly TimeProvider _clock = timeProvider ?? TimeProvider.System;

    public CancellationToken Add(ModuleLease lease)
    {
        var entry = new Entry(lease, leaseTimeout, _clock);
        _leases[lease.LeaseId] = entry;
        return entry.Token;
    }

    public void Remove(ModuleLease lease)
    {
        if (_leases.TryRemove(lease.LeaseId, out var entry))
        {
            entry.Dispose();
        }
    }

    public CancellationToken GetLossToken(ModuleLease lease) =>
        _leases.TryGetValue(lease.LeaseId, out var entry) ? entry.Token : CancellationToken.None;

    public IReadOnlyList<ModuleLease> GetLeases() =>
        [.. _leases.Values.Where(static entry => !entry.Token.IsCancellationRequested).Select(static entry => entry.Lease)];

    public IReadOnlyList<ModuleId> GetModuleIds() =>
        [.. GetLeases().Select(static lease => lease.Assignment.ModuleId).Distinct()];

    public long GetTimestamp() => _clock.GetTimestamp();

    public async Task ReconcileAsync(IReadOnlyList<ModuleLease> sentLeases, IReadOnlyList<string> lost, long sentAt)
    {
        var lostIds = lost.ToHashSet(StringComparer.Ordinal);
        foreach (var lease in sentLeases)
        {
            if (_leases.TryGetValue(lease.LeaseId, out var entry))
            {
                await entry.ReconcileAsync(lostIds.Contains(lease.LeaseId), _clock.GetElapsedTime(sentAt))
                    .ConfigureAwait(false);
            }
        }
    }

    private sealed class Entry : IDisposable
    {
        private readonly Lock _lock = new();
        private readonly CancellationTokenSource _lost = new();
        private readonly ITimer? _timer;
        private readonly TimeSpan? _timeout;
        private bool _disposed;

        public Entry(ModuleLease lease, TimeSpan? timeout, TimeProvider clock)
        {
            Lease = lease;
            Token = _lost.Token;
            _timeout = timeout;
            if (timeout is { } duration)
            {
                _timer = clock.CreateTimer(_ => _ = ReconcileAsync(true, TimeSpan.Zero), null, duration, Timeout.InfiniteTimeSpan);
            }
        }

        public ModuleLease Lease { get; }
        public CancellationToken Token { get; }

        public Task ReconcileAsync(bool lost, TimeSpan elapsed)
        {
            lock (_lock)
            {
                if (_disposed || Token.IsCancellationRequested)
                {
                    return Task.CompletedTask;
                }

                if (lost || (_timeout is { } timeout && elapsed >= timeout))
                {
                    return _lost.CancelAsync();
                }

                if (_timeout is { } duration)
                {
                    // Count the request and reply time: a delayed reply cannot extend local ownership.
                    _timer?.Change(duration - elapsed, Timeout.InfiniteTimeSpan);
                }

                return Task.CompletedTask;
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _disposed = true;
                _timer?.Dispose();
                _lost.Dispose();
            }
        }
    }
}
