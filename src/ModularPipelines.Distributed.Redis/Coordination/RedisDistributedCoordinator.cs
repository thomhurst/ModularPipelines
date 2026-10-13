using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Redis;
using Respire;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Redis-based implementation of <see cref="IDistributedMasterCoordinator"/>.
/// All keys are isolated by run identifier to support concurrent pipeline runs.
/// </summary>
/// <remarks>
/// Claims, lease renewal, lease expiry and result publication run as Lua scripts so each is atomic.
/// Pub/sub only wakes waiters early: every wait is bounded and re-reads Redis, so a message lost
/// while the connection reconnects delays a waiter by at most <see cref="PollInterval"/>.
/// Cancelling a call abandons its wait; Redis may still run a command already sent, and later
/// commands are not issued.
/// The master's coordinator records a heartbeat every <see cref="DistributedOptions.WorkerHeartbeatInterval"/>
/// until it is disposed or cleans up the run, so workers detect a master that stopped without signalling
/// completion.
/// </remarks>
internal sealed class RedisDistributedCoordinator : IDistributedMasterCoordinator, IDistributedRunCleanup, IAsyncDisposable
{
    internal static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(2);

    private const char QueueMemberSeparator = '|';
    private const string ClosedSentinel = "__closed__";
    private const double PriorityScoreBand = 1_000_000_000_000;
    private const double MaximumCriticalPathScore = PriorityScoreBand - 1;

    private static readonly RespireScript ClaimScript = RespireScript.Create(RedisCoordinationScripts.Claim);
    private static readonly RespireScript HeartbeatScript = RespireScript.Create(RedisCoordinationScripts.Heartbeat);
    private static readonly RespireScript MasterHeartbeatScript = RespireScript.Create(RedisCoordinationScripts.MasterHeartbeat);
    private static readonly RespireScript GetMasterStateScript = RespireScript.Create(RedisCoordinationScripts.GetMasterState);
    private static readonly RespireScript RegisterWorkerScript = RespireScript.Create(RedisCoordinationScripts.RegisterWorker);
    private static readonly RespireScript PublishResultScript = RespireScript.Create(RedisCoordinationScripts.PublishResult);
    private static readonly RespireScript RequeueExpiredLeasesScript = RespireScript.Create(RedisCoordinationScripts.RequeueExpiredLeases);
    private static readonly RespireScript WithdrawScript = RespireScript.Create(RedisCoordinationScripts.Withdraw);

    private readonly IRespireClient _redis;
    private readonly RedisKeyBuilder _keys;
    private readonly TimeSpan _keyExpiration;
    private readonly TimeSpan _workerTimeout;
    private readonly TimeSpan _masterTimeout;
    private readonly CancellationTokenSource _masterHeartbeatCts = new();
    private int _disposed;
    private Task _masterHeartbeatTask = Task.CompletedTask;
    // Lets real-backend contract tests synchronize after the race-closing reads complete.
    private readonly Action? _onWaitReady;
    private readonly ILogger<RedisDistributedCoordinator>? _logger;

    public RedisDistributedCoordinator(
        IRespireClient redis,
        RedisKeyBuilder keys,
        RedisOptions options,
        Action? onWaitReady = null,
        DistributedOptions? distributedOptions = null,
        ILogger<RedisDistributedCoordinator>? logger = null)
    {
        _redis = redis;
        _keys = keys;
        _keyExpiration = options.TimeToLive;
        _workerTimeout = distributedOptions?.WorkerTimeout ?? TimeSpan.FromSeconds(30);
        _masterTimeout = distributedOptions?.MasterTimeout ?? TimeSpan.FromMinutes(1);
        _onWaitReady = onWaitReady;
        _logger = logger;
        ValidateKeyExpiration(options, distributedOptions);
    }

    /// <summary>
    /// Rejects a key expiration that could delete run state while the master still waits on it.
    /// </summary>
    internal static void ValidateKeyExpiration(RedisOptions options, DistributedOptions? distributedOptions)
    {
        var workerTimeout = distributedOptions?.WorkerTimeout ?? TimeSpan.FromSeconds(30);
        var resultTimeout = distributedOptions?.ModuleResultTimeout ?? TimeSpan.Zero;
        var masterTimeout = distributedOptions?.MasterTimeout ?? TimeSpan.FromMinutes(1);

        // The master heartbeat must stay readable for longer than MasterTimeout, or a stale
        // heartbeat could expire before workers see it as stale.
        if (options.TimeToLive <= workerTimeout
            || options.TimeToLive <= resultTimeout
            || options.TimeToLive <= masterTimeout)
        {
            throw new InvalidOperationException(
                $"{nameof(RedisOptions)}.{nameof(RedisOptions.TimeToLive)} ({options.TimeToLive}) "
                + $"must exceed {nameof(DistributedOptions.WorkerTimeout)} ({workerTimeout}), "
                + $"{nameof(DistributedOptions.ModuleResultTimeout)} ({resultTimeout}) and "
                + $"{nameof(DistributedOptions.MasterTimeout)} ({masterTimeout}).");
        }
    }

    public async Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var json = JsonSerializer.Serialize(assignment);
        var queueMember = $"{Guid.NewGuid():N}{QueueMemberSeparator}{json}";
        await _redis.SortedSets.AddAsync(_keys.WorkQueue, (RespireValue) queueMember, GetQueueScore(assignment), cancellationToken)
            .ConfigureAwait(false);
        await RefreshRunKeysAsync(cancellationToken).ConfigureAwait(false);
        await NotifyAsync(_keys.WorkAvailableChannel, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ModuleLease?> DequeueModuleAsync(
        WorkerId workerId,
        IReadOnlySet<Capability> workerCapabilities,
        CancellationToken cancellationToken)
    {
        using var signal = new SemaphoreSlim(0);
        var subscription = await SubscribeAsync(
                signal,
                cancellationToken,
                _keys.WorkAvailableChannel,
                _keys.CompletionChannel,
                _keys.CancellationChannel)
            .ConfigureAwait(false);
        await using var subscriptionLifetime = subscription.ConfigureAwait(false);

        var waitReadySignalled = false;
        while (true)
        {
            var claim = await TryClaimAsync(workerId, workerCapabilities, cancellationToken).ConfigureAwait(false);
            if (claim.Closed)
            {
                return null;
            }

            if (claim.Lease is not null)
            {
                return claim.Lease;
            }

            if (!waitReadySignalled)
            {
                waitReadySignalled = true;
                _onWaitReady?.Invoke();
            }

            await signal.WaitAsync(PollInterval, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task PublishResultAsync(
        SerializedModuleResult result,
        ModuleLease? lease,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var json = JsonSerializer.Serialize(result);

        // HSETNX keeps the first result final; the lease is released either way.
        await _redis.Scripts.ExecuteIntegerAsync(
                PublishResultScript,
                [_keys.Results, _keys.Leases],
                [result.ModuleId.Value, json],
                cancellationToken)
            .ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        await RefreshRunKeysAsync(cancellationToken).ConfigureAwait(false);
        await NotifyAsync(_keys.ResultChannel(result.ModuleId), cancellationToken).ConfigureAwait(false);
    }

    public async Task<SerializedModuleResult> WaitForResultAsync(ModuleId moduleId, CancellationToken cancellationToken)
    {
        var existing = await TryGetResultAsync(moduleId, cancellationToken).ConfigureAwait(false);
        if (existing is not null)
        {
            return existing;
        }

        using var signal = new SemaphoreSlim(0);
        var subscription = await SubscribeAsync(
                signal,
                cancellationToken,
                _keys.ResultChannel(moduleId))
            .ConfigureAwait(false);
        await using var subscriptionLifetime = subscription.ConfigureAwait(false);

        var waitReadySignalled = false;
        while (true)
        {
            // Notifications only wake the wait; the stored hash entry is the final result.
            existing = await TryGetResultAsync(moduleId, cancellationToken).ConfigureAwait(false);
            if (existing is not null)
            {
                return existing;
            }

            if (!waitReadySignalled)
            {
                waitReadySignalled = true;
                _onWaitReady?.Invoke();
            }

            await signal.WaitAsync(PollInterval, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var registeredAt = JsonSerializer.Serialize(registration.RegisteredAt).Trim('"');
        var initialStatus = JsonSerializer.Serialize(new WorkerStatus
        {
            WorkerId = registration.WorkerId,
            RunId = registration.RunId,
        });
        var result = await _redis.Scripts.ExecuteStringAsync(
                RegisterWorkerScript,
                [_keys.Workers, _keys.WorkerStatuses],
                [
                    registration.WorkerId.Value,
                    JsonSerializer.Serialize(registration),
                    registeredAt,
                    (long) _workerTimeout.TotalMilliseconds,
                    initialStatus,
                    registration.RunId ?? string.Empty,
                ],
                cancellationToken)
            .ConfigureAwait(false);
        if (result == "duplicate")
        {
            throw new InvalidOperationException(
                $"Worker '{registration.WorkerId}' is already registered by another live process. "
                + "Give every distributed process a unique instance index.");
        }

        await RefreshRunKeysAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _redis.Hashes.SetAsync(
                _keys.WorkerStatuses,
                status.WorkerId.Value,
                (RespireValue) JsonSerializer.Serialize(status),
                cancellationToken)
            .ConfigureAwait(false);
        await _redis.Scripts.ExecuteIntegerAsync(
                HeartbeatScript,
                [_keys.Workers, _keys.Leases],
                [
                    status.WorkerId.Value,
                    (long) _workerTimeout.TotalMilliseconds,
                    .. status.InFlightModules.Select(static moduleId => (RespireValue) moduleId.Value),
                ],
                cancellationToken)
            .ConfigureAwait(false);
        await RefreshRunKeysAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<bool> WithdrawAssignmentAsync(ModuleId moduleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var removed = await _redis.Scripts.ExecuteIntegerAsync(
                WithdrawScript,
                [_keys.WorkQueue],
                [moduleId.Value],
                cancellationToken)
            .ConfigureAwait(false);
        return removed > 0;
    }

    public async Task<IReadOnlyList<ModuleLease>> GetActiveLeasesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var entries = await _redis.Hashes.GetAllAsync(_keys.Leases, cancellationToken).ConfigureAwait(false);
        return [.. entries.Values.Select(static entry => ToLease(JsonSerializer.Deserialize<LeaseRecord>(entry)!))];
    }

    public async Task<IReadOnlyList<ModuleId>> RequeueExpiredLeasesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ModuleId[] requeued;
        using (var result = await _redis.Scripts.ExecuteAsync(
                       RequeueExpiredLeasesScript,
                       [_keys.Leases, _keys.Results, _keys.WorkQueue],
                       cancellationToken: cancellationToken)
                   .ConfigureAwait(false))
        {
            requeued = [.. result.Select(static item => new ModuleId(item.AsString()))];
        }

        // The master calls this periodically, so it also keeps the run's keys alive.
        await RefreshRunKeysAsync(cancellationToken).ConfigureAwait(false);
        if (requeued.Length > 0)
        {
            await NotifyAsync(_keys.WorkAvailableChannel, cancellationToken).ConfigureAwait(false);
        }

        return requeued;
    }

    public async Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        var serverTime = await _redis.Server.TimeAsync(cancellationToken).ConfigureAwait(false);
        var entries = await _redis.Hashes.GetAllAsync(_keys.Workers, cancellationToken).ConfigureAwait(false);
        var statuses = (await GetWorkerStatusesAsync(cancellationToken).ConfigureAwait(false))
            .ToDictionary(static status => status.WorkerId);
        var oldestLiveHeartbeat = serverTime.ToUnixTimeMilliseconds() - _workerTimeout.TotalMilliseconds;
        const string HeartbeatPrefix = "heartbeat:";
        var heartbeats = entries
            .Where(static entry => entry.Key.StartsWith(HeartbeatPrefix, StringComparison.Ordinal))
            .ToDictionary(
                static entry => entry.Key[HeartbeatPrefix.Length..],
                static entry => long.Parse(entry.Value, NumberStyles.Integer, CultureInfo.InvariantCulture),
                StringComparer.Ordinal);
        var workers = new List<WorkerRegistration>(entries.Count);
        foreach (var entry in entries.Where(static entry =>
                     !entry.Key.StartsWith(HeartbeatPrefix, StringComparison.Ordinal)))
        {
            var registration = JsonSerializer.Deserialize<WorkerRegistration>(entry.Value)!;
            // A final status keeps a worker visible after its heartbeat expires.
            if (statuses.GetValueOrDefault(registration.WorkerId)?.IsFinal == true
                || (heartbeats.TryGetValue(registration.WorkerId.Value, out var heartbeat)
                    && heartbeat >= oldestLiveHeartbeat))
            {
                workers.Add(registration);
            }
        }

        return workers;
    }

    public async Task<IReadOnlyList<WorkerStatus>> GetWorkerStatusesAsync(CancellationToken cancellationToken)
    {
        var entries = await _redis.Hashes.GetAllAsync(_keys.WorkerStatuses, cancellationToken).ConfigureAwait(false);
        return [.. entries.Values.Select(static entry => JsonSerializer.Deserialize<WorkerStatus>(entry)!)];
    }

    public async Task SignalCompletionAsync(CancellationToken cancellationToken)
    {
        await SetCompletionFlagAsync(cancellationToken).ConfigureAwait(false);
        await NotifyAsync(_keys.CompletionChannel, cancellationToken).ConfigureAwait(false);
    }

    public async Task BroadcastCancellationAsync(
        DistributedCancellationReason reason,
        CancellationToken cancellationToken)
    {
        // The first reason is durable.
        await _redis.SetAsync(
                _keys.CancellationFlag,
                (RespireValue) reason.ToString(),
                _keyExpiration,
                SetWhen.NotExists,
                cancellationToken)
            .ConfigureAwait(false);
        await NotifyAsync(_keys.CancellationChannel, cancellationToken).ConfigureAwait(false);
    }

    public async Task<DistributedCancellationReason> WaitForCancellationAsync(CancellationToken cancellationToken)
    {
        var reason = await TryGetCancellationReasonAsync(cancellationToken).ConfigureAwait(false);
        if (reason is not null)
        {
            return reason.Value;
        }

        using var signal = new SemaphoreSlim(0);
        var subscription = await SubscribeAsync(
                signal,
                cancellationToken,
                _keys.CancellationChannel)
            .ConfigureAwait(false);
        await using var subscriptionLifetime = subscription.ConfigureAwait(false);

        var waitReadySignalled = false;
        while (true)
        {
            reason = await TryGetCancellationReasonAsync(cancellationToken).ConfigureAwait(false);
            if (reason is not null)
            {
                return reason.Value;
            }

            if (!waitReadySignalled)
            {
                waitReadySignalled = true;
                _onWaitReady?.Invoke();
            }

            await signal.WaitAsync(PollInterval, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task<DistributedMasterState> GetMasterStateAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = await _redis.Scripts.ExecuteIntegerAsync(
                GetMasterStateScript,
                [_keys.MasterHeartbeat, _keys.CompletionFlag, _keys.MasterStarted],
                [(long) _masterTimeout.TotalMilliseconds],
                cancellationToken)
            .ConfigureAwait(false);
        return (DistributedMasterState) state;
    }

    /// <summary>
    /// Records a master heartbeat now and then every <paramref name="interval"/> until this
    /// coordinator is disposed.
    /// </summary>
    internal void StartMasterHeartbeat(TimeSpan interval) =>
        _masterHeartbeatTask = SendMasterHeartbeatsAsync(interval, _masterHeartbeatCts.Token);

    internal async Task SendMasterHeartbeatAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _redis.Scripts.ExecuteIntegerAsync(
                MasterHeartbeatScript,
                [_keys.MasterHeartbeat, _keys.MasterStarted],
                [(long) _keyExpiration.TotalMilliseconds],
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the run's coordination keys. The completion flag stays until it expires, so a worker that
    /// starts after cleanup sees a completed master and exits instead of waiting for one to start.
    /// </summary>
    public async Task CleanupRunAsync(DistributedRunCleanupContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        // A later heartbeat would recreate the deleted master keys.
        await StopMasterHeartbeatAsync().ConfigureAwait(false);

        // Without the completion flag, deleting the started marker would make workers report a missing master
        // as not started yet and wait forever, so write it again in case signalling completion failed.
        await SetCompletionFlagAsync(cancellationToken).ConfigureAwait(false);
        RespireKey[] keys = [.. _keys.RunCleanupKeys.Select(static key => (RespireKey) key)];
        await _redis.DeleteAsync(keys, cancellationToken).ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        await StopMasterHeartbeatAsync().ConfigureAwait(false);
        _masterHeartbeatCts.Dispose();
    }

    private async Task StopMasterHeartbeatAsync()
    {
        await _masterHeartbeatCts.CancelAsync().ConfigureAwait(false);
        await _masterHeartbeatTask.ConfigureAwait(false);
    }

    private async Task SendMasterHeartbeatsAsync(TimeSpan interval, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await SendMasterHeartbeatAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                // Workers tolerate missed heartbeats for MasterTimeout; retry on the next interval.
                _logger?.LogWarning(exception, "Could not record the distributed master heartbeat; retrying");
            }

            try
            {
                await Task.Delay(interval, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
        }
    }

    internal static double GetQueueScore(ModuleAssignment assignment)
    {
        var criticalPathScore = Math.Clamp(
            assignment.CriticalPathWeight.TotalSeconds,
            0,
            MaximumCriticalPathScore);
        return ((int) assignment.Priority * PriorityScoreBand) + criticalPathScore;
    }

    private async Task SetCompletionFlagAsync(CancellationToken cancellationToken) =>
        await _redis.SetAsync(_keys.CompletionFlag, (RespireValue) "1", _keyExpiration, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

    private async Task<SerializedModuleResult?> TryGetResultAsync(ModuleId moduleId, CancellationToken cancellationToken)
    {
        var existing = await _redis.Hashes.GetStringAsync(_keys.Results, moduleId.Value, cancellationToken)
            .ConfigureAwait(false);
        return string.IsNullOrEmpty(existing)
            ? null
            : JsonSerializer.Deserialize<SerializedModuleResult>(existing)!;
    }

    private async Task<DistributedCancellationReason?> TryGetCancellationReasonAsync(CancellationToken cancellationToken)
    {
        var value = await _redis.GetStringAsync(_keys.CancellationFlag, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        return Enum.TryParse<DistributedCancellationReason>(value, out var reason)
            ? reason
            : DistributedCancellationReason.Stopped;
    }

    private async Task<(bool Closed, ModuleLease? Lease)> TryClaimAsync(
        WorkerId workerId,
        IReadOnlySet<Capability> workerCapabilities,
        CancellationToken cancellationToken)
    {
        var leaseId = Guid.NewGuid().ToString("N");
        var capabilities = JsonSerializer.Serialize(workerCapabilities.ToArray());
        var member = await _redis.Scripts.ExecuteStringAsync(
                ClaimScript,
                [
                    _keys.WorkQueue,
                    _keys.Workers,
                    _keys.Leases,
                    _keys.Results,
                    _keys.CancellationFlag,
                    _keys.CompletionFlag,
                ],
                [capabilities, (long) _workerTimeout.TotalMilliseconds, workerId.Value, leaseId],
                cancellationToken)
            .ConfigureAwait(false);

        if (member is null)
        {
            return (false, null);
        }

        return member == ClosedSentinel
            ? (true, null)
            : (false, new ModuleLease
            {
                LeaseId = leaseId,
                WorkerId = workerId,
                Assignment = ParseQueueMember(member),
            });
    }

    private static ModuleAssignment ParseQueueMember(string queueMember)
    {
        var separatorIndex = queueMember.IndexOf(QueueMemberSeparator);
        var hasUniquePrefix = separatorIndex == 32
            && Guid.TryParseExact(queueMember.AsSpan(0, separatorIndex), "N", out _);
        var assignmentJson = hasUniquePrefix
            ? queueMember[(separatorIndex + 1)..]
            : queueMember;
        return JsonSerializer.Deserialize<ModuleAssignment>(assignmentJson)!;
    }

    private static ModuleLease ToLease(LeaseRecord record) => new()
    {
        LeaseId = record.LeaseId,
        WorkerId = new WorkerId(record.WorkerId),
        Assignment = ParseQueueMember(record.Member),
    };

    private async Task NotifyAsync(string channel, CancellationToken cancellationToken) =>
        await _redis.PublishAsync(channel, (RespireValue) "1", cancellationToken).ConfigureAwait(false);

    private async Task RefreshRunKeysAsync(CancellationToken cancellationToken)
    {
        // Queue every refresh so they share one flush instead of one round trip each.
        using var batch = _redis.CreateBatch();
        foreach (var key in _keys.CoordinationKeys)
        {
            _ = batch.Expire(key, _keyExpiration);
        }

        await batch.ExecuteAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Subscribes to every channel and releases <paramref name="signal"/> for each message. A delivery
    /// gap also releases it, because a message may have been lost while the connection reconnected.
    /// </summary>
    private async Task<Subscription> SubscribeAsync(
        SemaphoreSlim signal,
        CancellationToken cancellationToken,
        params string[] channels)
    {
        var subscription = await _redis.SubscribeAsync(channels.AsSpan(), cancellationToken).ConfigureAwait(false);
        return new Subscription(subscription, signal);
    }

    private sealed record LeaseRecord(string LeaseId, string WorkerId, string Member, string Score, string ExpiresAt);

    /// <summary>
    /// Pumps one subscription into a wake-up signal until disposed. Disposing the subscription
    /// unsubscribes only this call's channels from the shared connection.
    /// </summary>
    private sealed class Subscription : IAsyncDisposable
    {
        private readonly RespireSubscription _subscription;
        private readonly CancellationTokenSource _stop = new();
        private readonly Task _pump;

        public Subscription(RespireSubscription subscription, SemaphoreSlim signal)
        {
            _subscription = subscription;
            _pump = PumpAsync(subscription, signal, _stop.Token);
        }

        public async ValueTask DisposeAsync()
        {
            await _stop.CancelAsync().ConfigureAwait(false);
            try
            {
                await _subscription.DisposeAsync().ConfigureAwait(false);
            }
            catch (Exception exception) when (exception is RespireException or ObjectDisposedException)
            {
                // The connection is gone; the subscription went with it.
            }

            await _pump.ConfigureAwait(false);
            _stop.Dispose();
        }

        private static async Task PumpAsync(
            RespireSubscription subscription,
            SemaphoreSlim signal,
            CancellationToken cancellationToken)
        {
            try
            {
                // A delivery gap wakes the wait too: a message may have been lost while reconnecting.
                await foreach (var message in subscription.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    signal.Release();
                }
            }
            catch (Exception)
            {
                // Disposal ended the stream, or the subscription failed. Messages only wake waits early,
                // so every wait falls back to polling Redis.
            }
        }
    }
}
