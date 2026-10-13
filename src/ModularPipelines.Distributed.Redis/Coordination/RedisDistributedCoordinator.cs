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
/// Enqueueing, claims, heartbeats, lease expiry and result publication run as Lua scripts so each is
/// atomic and costs one round trip; the scripts that change run state also renew the run keys' expiry.
/// Claims read one queue member per capability class rather than scanning the queue. Worker heartbeats use
/// hash field expiry, so the coordinator needs Redis 7.4 or later (or Valkey 9.0 or later).
/// Sharded pub/sub only wakes waiters early: every wait is bounded and re-reads Redis, so a message lost
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

    private const string ClosedSentinel = "__closed__";

    private static readonly RespireScript HashFieldExpiryProbeScript = RespireScript.Create(
        "return redis.call('HPEXPIRE', KEYS[1], 1000, 'FIELDS', 1, '__modularpipelines_probe__')");
    private static readonly RespireScript EnqueueScript = RespireScript.Create(RedisCoordinationScripts.Enqueue);
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
    private readonly RespireKey[] _scriptKeys;
    private readonly TimeSpan _keyExpiration;
    private readonly long _keyExpirationMilliseconds;
    private readonly long _workerTimeoutMilliseconds;
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
        _scriptKeys = [.. keys.ScriptKeys.Select(static key => (RespireKey) key)];
        _keyExpiration = options.TimeToLive;
        _keyExpirationMilliseconds = (long) options.TimeToLive.TotalMilliseconds;
        _workerTimeout = distributedOptions?.WorkerTimeout ?? TimeSpan.FromSeconds(30);
        _workerTimeoutMilliseconds = (long) _workerTimeout.TotalMilliseconds;
        _masterTimeout = distributedOptions?.MasterTimeout ?? TimeSpan.FromMinutes(1);
        _onWaitReady = onWaitReady;
        _logger = logger;
        ValidateKeyExpiration(options, distributedOptions);
    }

    /// <summary>
    /// Fails fast when the server lacks hash field expiry, which worker heartbeats need. Without this check
    /// an older server reports only "Unknown Redis command called from script" when a worker registers.
    /// </summary>
    internal static async Task EnsureServerSupportAsync(
        IRespireClient redis,
        RedisKeyBuilder keys,
        CancellationToken cancellationToken)
    {
        try
        {
            // A field that never exists, so the probe changes nothing.
            using var result = await redis.Scripts.ExecuteAsync(
                    HashFieldExpiryProbeScript,
                    [keys.WorkerHeartbeats],
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        catch (RespireServerException exception) when (exception.Message.Contains("Unknown Redis command", StringComparison.OrdinalIgnoreCase)
                                                        || exception.Message.Contains("unknown command", StringComparison.OrdinalIgnoreCase))
        {
            throw new NotSupportedException(
                "The Redis distributed coordinator requires hash field expiry (HPEXPIRE): Redis 7.4 or later, or Valkey 9.0 or later.",
                exception);
        }
    }

    /// <summary>
    /// Rejects a key expiration that could delete run state while the master still waits on it.
    /// </summary>
    internal static void ValidateKeyExpiration(RedisOptions options, DistributedOptions? distributedOptions)
    {
        var workerTimeout = distributedOptions?.WorkerTimeout ?? TimeSpan.FromSeconds(30);
        var resultTimeout = distributedOptions?.ModuleResultTimeout ?? Timeout.InfiniteTimeSpan;
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
        var classJson = JsonSerializer.Serialize(RedisQueueMember.GetClass(assignment));
        var classId = RedisQueueMember.GetClassId(classJson);
        var member = RedisQueueMember.Create(classId, assignment, JsonSerializer.Serialize(assignment));
        await _redis.Scripts.ExecuteIntegerAsync(
                EnqueueScript,
                _scriptKeys,
                [_keyExpirationMilliseconds, classId, classJson, member, assignment.ModuleId.Value],
                cancellationToken)
            .ConfigureAwait(false);
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
                _scriptKeys,
                [_keyExpirationMilliseconds, result.ModuleId.Value, json],
                cancellationToken)
            .ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
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
                _scriptKeys,
                [
                    _keyExpirationMilliseconds,
                    registration.WorkerId.Value,
                    JsonSerializer.Serialize(registration),
                    registeredAt,
                    _workerTimeoutMilliseconds,
                    initialStatus,
                    registration.RunId ?? string.Empty,
                ],
                cancellationToken)
            .ConfigureAwait(false);
        if (result == "completed")
        {
            throw new DistributedRunCompletedException();
        }

        if (result == "duplicate")
        {
            throw new InvalidOperationException(
                $"Worker '{registration.WorkerId}' is already registered by another live process. "
                + "Give every distributed process a unique instance index.");
        }
    }

    public async Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _redis.Scripts.ExecuteIntegerAsync(
                HeartbeatScript,
                _scriptKeys,
                [
                    _keyExpirationMilliseconds,
                    status.WorkerId.Value,
                    _workerTimeoutMilliseconds,
                    JsonSerializer.Serialize(status),
                    .. status.InFlightModules.Select(static moduleId => (RespireValue) moduleId.Value),
                ],
                cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<bool> WithdrawAssignmentAsync(ModuleId moduleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var removed = await _redis.Scripts.ExecuteIntegerAsync(
                WithdrawScript,
                _scriptKeys,
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
                       _scriptKeys,
                       [_keyExpirationMilliseconds],
                       cancellationToken)
                   .ConfigureAwait(false))
        {
            requeued = [.. result.Select(static item => new ModuleId(item.AsString()))];
        }

        // The master calls this periodically, so the script also keeps the run's keys alive.
        if (requeued.Length > 0)
        {
            await NotifyAsync(_keys.WorkAvailableChannel, cancellationToken).ConfigureAwait(false);
        }

        return requeued;
    }

    public async Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        using var batch = _redis.CreateBatch();
        var registrations = batch.Hashes.GetAll(_keys.Workers);
        var heartbeats = batch.Hashes.GetAll(_keys.WorkerHeartbeats);
        var statuses = batch.Hashes.GetAll(_keys.WorkerStatuses);
        await batch.ExecuteAsync(cancellationToken).ConfigureAwait(false);

        // Heartbeat fields expire after the worker timeout, so only live workers have one.
        var finalStatuses = statuses.Result.Values
            .Select(static entry => JsonSerializer.Deserialize<WorkerStatus>(entry)!)
            .Where(static status => status.IsFinal)
            .Select(static status => status.WorkerId)
            .ToHashSet();
        var workers = new List<WorkerRegistration>(registrations.Result.Count);
        foreach (var entry in registrations.Result)
        {
            var registration = JsonSerializer.Deserialize<WorkerRegistration>(entry.Value)!;
            // A final status keeps a worker visible after its heartbeat expires.
            if (heartbeats.Result.ContainsKey(entry.Key) || finalStatuses.Contains(registration.WorkerId))
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
                _scriptKeys,
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
                _scriptKeys,
                [_keyExpirationMilliseconds],
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
        // UNLINK frees large hashes on a background thread instead of blocking the server.
        RespireKey[] keys = [.. _keys.RunCleanupKeys.Select(static key => (RespireKey) key)];
        await _redis.Keys.UnlinkAsync(keys, cancellationToken).ConfigureAwait(false);
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
        var assignment = await _redis.Scripts.ExecuteStringAsync(
                ClaimScript,
                _scriptKeys,
                [capabilities, workerId.Value, leaseId, _workerTimeoutMilliseconds, _keyExpirationMilliseconds],
                cancellationToken)
            .ConfigureAwait(false);

        if (assignment is null)
        {
            return (false, null);
        }

        return assignment == ClosedSentinel
            ? (true, null)
            : (false, new ModuleLease
            {
                LeaseId = leaseId,
                WorkerId = workerId,
                Assignment = JsonSerializer.Deserialize<ModuleAssignment>(assignment)!,
            });
    }

    private static ModuleLease ToLease(LeaseRecord record) => new()
    {
        LeaseId = record.LeaseId,
        WorkerId = new WorkerId(record.WorkerId),
        Assignment = RedisQueueMember.ParseAssignment(record.Member),
    };

    // Every channel carries the run's hash tag, so sharded pub/sub keeps a run's messages on one
    // Redis Cluster shard instead of broadcasting them to every node.
    private async Task NotifyAsync(string channel, CancellationToken cancellationToken) =>
        await _redis.PublishShardedAsync(channel, (RespireValue) "1", cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Subscribes to every channel and releases <paramref name="signal"/> for each message. A delivery
    /// gap also releases it, because a message may have been lost while the connection reconnected.
    /// </summary>
    private async Task<Subscription> SubscribeAsync(
        SemaphoreSlim signal,
        CancellationToken cancellationToken,
        params string[] channels)
    {
        var subscription = await _redis.SubscribeShardedAsync(channels.AsSpan(), cancellationToken).ConfigureAwait(false);
        return new Subscription(subscription, signal);
    }

    private sealed record LeaseRecord(string LeaseId, string WorkerId, string Member, string ExpiresAt);

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
