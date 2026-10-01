using System.Text.Json;
using ModularPipelines.Distributed.Redis;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Redis-based implementation of <see cref="IDistributedMasterCoordinator"/>.
/// All keys are isolated by run identifier to support concurrent pipeline runs.
/// </summary>
/// <remarks>
/// Claims, lease renewal, lease expiry and result publication run as Lua scripts so each is atomic.
/// Pub/sub only wakes waiters early: every wait is bounded and re-reads Redis, so a message lost
/// while the connection reconnects delays a waiter by at most <see cref="PollInterval"/>.
/// Redis cannot cancel a command already sent, so each call is bounded with
/// <see cref="Task.WaitAsync(CancellationToken)"/> and later commands are not issued.
/// The master's coordinator records a heartbeat every <see cref="DistributedOptions.WorkerHeartbeatInterval"/>
/// until it is disposed, so workers detect a master that stopped without signalling completion.
/// </remarks>
internal sealed class RedisDistributedCoordinator : IDistributedMasterCoordinator, IAsyncDisposable
{
    internal static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(2);

    private const char QueueMemberSeparator = '|';
    private const string ClosedSentinel = "__closed__";
    private const double PriorityScoreBand = 1_000_000_000_000;
    private const double MaximumCriticalPathScore = PriorityScoreBand - 1;

    private readonly IDatabase _database;
    private readonly ISubscriber _subscriber;
    private readonly RedisKeyBuilder _keys;
    private readonly TimeSpan _keyExpiration;
    private readonly TimeSpan _workerTimeout;
    private readonly TimeSpan _masterTimeout;
    private readonly CancellationTokenSource _disposeCts = new();
    private Task _masterHeartbeatTask = Task.CompletedTask;
    // Lets real-backend contract tests synchronize after the race-closing reads complete.
    private readonly Action? _onWaitReady;

    public RedisDistributedCoordinator(
        IDatabase database,
        ISubscriber subscriber,
        RedisKeyBuilder keys,
        RedisOptions options,
        Action? onWaitReady = null,
        DistributedOptions? distributedOptions = null)
    {
        _database = database;
        _subscriber = subscriber;
        _keys = keys;
        _keyExpiration = options.TimeToLive;
        _workerTimeout = distributedOptions?.WorkerTimeout ?? TimeSpan.FromSeconds(30);
        _masterTimeout = distributedOptions?.MasterTimeout ?? TimeSpan.FromMinutes(1);
        _onWaitReady = onWaitReady;
        ValidateKeyExpiration(options, distributedOptions);
    }

    /// <summary>
    /// Rejects a key expiration that could delete run state while the master still waits on it.
    /// </summary>
    internal static void ValidateKeyExpiration(RedisOptions options, DistributedOptions? distributedOptions)
    {
        var workerTimeout = distributedOptions?.WorkerTimeout ?? TimeSpan.FromSeconds(30);
        var resultTimeout = distributedOptions?.ModuleResultTimeout ?? TimeSpan.Zero;
        if (options.TimeToLive <= workerTimeout || options.TimeToLive <= resultTimeout)
        {
            throw new InvalidOperationException(
                $"{nameof(RedisOptions)}.{nameof(RedisOptions.TimeToLive)} ({options.TimeToLive}) "
                + $"must exceed {nameof(DistributedOptions.WorkerTimeout)} ({workerTimeout}) and "
                + $"{nameof(DistributedOptions.ModuleResultTimeout)} ({resultTimeout}).");
        }
    }

    public async Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var json = JsonSerializer.Serialize(assignment);
        var queueMember = $"{Guid.NewGuid():N}{QueueMemberSeparator}{json}";
        await _database.SortedSetAddAsync(_keys.WorkQueue, queueMember, GetQueueScore(assignment))
            .WaitAsync(cancellationToken).ConfigureAwait(false);
        await RefreshRunKeysAsync(cancellationToken).ConfigureAwait(false);
        await NotifyAsync(_keys.WorkAvailableChannel, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ModuleLease?> DequeueModuleAsync(
        WorkerId workerId,
        IReadOnlySet<Capability> workerCapabilities,
        CancellationToken cancellationToken)
    {
        using var signal = new SemaphoreSlim(0);
        var subscriptions = await SubscribeAsync(
                signal,
                cancellationToken,
                _keys.WorkAvailableChannel,
                _keys.CompletionChannel,
                _keys.CancellationChannel)
            .ConfigureAwait(false);
        await using var subscriptionsLifetime = subscriptions.ConfigureAwait(false);

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
        await _database.ScriptEvaluateAsync(
                RedisCoordinationScripts.PublishResult,
                [(RedisKey) _keys.Results, (RedisKey) _keys.Leases],
                [result.ModuleId.Value, json])
            .WaitAsync(cancellationToken).ConfigureAwait(false);
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
        var subscriptions = await SubscribeAsync(
                signal,
                cancellationToken,
                _keys.ResultChannel(moduleId))
            .ConfigureAwait(false);
        await using var subscriptionsLifetime = subscriptions.ConfigureAwait(false);

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
        var result = await _database.ScriptEvaluateAsync(
                RedisCoordinationScripts.RegisterWorker,
                [(RedisKey) _keys.Workers, (RedisKey) _keys.WorkerStatuses],
                [
                    registration.WorkerId.Value,
                    JsonSerializer.Serialize(registration),
                    registeredAt,
                    (long) _workerTimeout.TotalMilliseconds,
                    initialStatus,
                    registration.RunId ?? string.Empty,
                ])
            .WaitAsync(cancellationToken).ConfigureAwait(false);
        if (result.ToString() == "duplicate")
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
        await _database.HashSetAsync(_keys.WorkerStatuses, status.WorkerId.Value, JsonSerializer.Serialize(status))
            .WaitAsync(cancellationToken).ConfigureAwait(false);
        await _database.ScriptEvaluateAsync(
                RedisCoordinationScripts.Heartbeat,
                [(RedisKey) _keys.Workers, (RedisKey) _keys.Leases],
                [
                    status.WorkerId.Value,
                    (long) _workerTimeout.TotalMilliseconds,
                    .. status.InFlightModules.Select(static moduleId => (RedisValue) moduleId.Value),
                ])
            .WaitAsync(cancellationToken).ConfigureAwait(false);
        await RefreshRunKeysAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<bool> WithdrawAssignmentAsync(ModuleId moduleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var removed = await _database.ScriptEvaluateAsync(
                RedisCoordinationScripts.Withdraw,
                [(RedisKey) _keys.WorkQueue],
                [moduleId.Value])
            .WaitAsync(cancellationToken).ConfigureAwait(false);
        return (long) removed > 0;
    }

    public async Task<IReadOnlyList<ModuleLease>> GetActiveLeasesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var entries = await _database.HashGetAllAsync(_keys.Leases).WaitAsync(cancellationToken).ConfigureAwait(false);
        return [.. entries.Select(static entry => ToLease(JsonSerializer.Deserialize<LeaseRecord>(entry.Value.ToString())!))];
    }

    public async Task<IReadOnlyList<ModuleId>> RequeueExpiredLeasesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = await _database.ScriptEvaluateAsync(
                RedisCoordinationScripts.RequeueExpiredLeases,
                [(RedisKey) _keys.Leases, (RedisKey) _keys.Results, (RedisKey) _keys.WorkQueue])
            .WaitAsync(cancellationToken).ConfigureAwait(false);

        // The master calls this periodically, so it also keeps the run's keys alive.
        await RefreshRunKeysAsync(cancellationToken).ConfigureAwait(false);
        var requeued = ((RedisResult[]?) result ?? [])
            .Select(static item => new ModuleId(item.ToString()!))
            .ToArray();
        if (requeued.Length > 0)
        {
            await NotifyAsync(_keys.WorkAvailableChannel, cancellationToken).ConfigureAwait(false);
        }

        return requeued;
    }

    public async Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        var serverTimeMilliseconds = await GetServerTimeMillisecondsAsync(cancellationToken).ConfigureAwait(false);
        var entries = await _database.HashGetAllAsync(_keys.Workers).WaitAsync(cancellationToken).ConfigureAwait(false);
        var statuses = (await GetWorkerStatusesAsync(cancellationToken).ConfigureAwait(false))
            .ToDictionary(static status => status.WorkerId);
        var oldestLiveHeartbeat = serverTimeMilliseconds - _workerTimeout.TotalMilliseconds;
        const string HeartbeatPrefix = "heartbeat:";
        var heartbeats = entries
            .Where(static entry => entry.Name.ToString().StartsWith(HeartbeatPrefix, StringComparison.Ordinal))
            .ToDictionary(
                static entry => entry.Name.ToString()[HeartbeatPrefix.Length..],
                static entry => (long) entry.Value,
                StringComparer.Ordinal);
        var workers = new List<WorkerRegistration>(entries.Length);
        foreach (var entry in entries.Where(static entry =>
                     !entry.Name.ToString().StartsWith(HeartbeatPrefix, StringComparison.Ordinal)))
        {
            var registration = JsonSerializer.Deserialize<WorkerRegistration>(entry.Value.ToString())!;
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
        var entries = await _database.HashGetAllAsync(_keys.WorkerStatuses)
            .WaitAsync(cancellationToken).ConfigureAwait(false);
        return [.. entries.Select(static entry => JsonSerializer.Deserialize<WorkerStatus>(entry.Value.ToString())!)];
    }

    public async Task SignalCompletionAsync(CancellationToken cancellationToken)
    {
        await _database.StringSetAsync(_keys.CompletionFlag, "1", _keyExpiration)
            .WaitAsync(cancellationToken).ConfigureAwait(false);
        await NotifyAsync(_keys.CompletionChannel, cancellationToken).ConfigureAwait(false);
    }

    public async Task BroadcastCancellationAsync(
        DistributedCancellationReason reason,
        CancellationToken cancellationToken)
    {
        // The first reason is durable.
        await _database.StringSetAsync(_keys.CancellationFlag, reason.ToString(), _keyExpiration, When.NotExists)
            .WaitAsync(cancellationToken).ConfigureAwait(false);
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
        var subscriptions = await SubscribeAsync(
                signal,
                cancellationToken,
                _keys.CancellationChannel)
            .ConfigureAwait(false);
        await using var subscriptionsLifetime = subscriptions.ConfigureAwait(false);

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
        var result = await _database.ScriptEvaluateAsync(
                RedisCoordinationScripts.GetMasterState,
                [(RedisKey) _keys.MasterHeartbeat, (RedisKey) _keys.CompletionFlag],
                [(long) _masterTimeout.TotalMilliseconds])
            .WaitAsync(cancellationToken).ConfigureAwait(false);
        var state = (long) result;
        return (DistributedMasterState) state;
    }

    /// <summary>
    /// Records a master heartbeat now and then every <paramref name="interval"/> until this
    /// coordinator is disposed.
    /// </summary>
    internal void StartMasterHeartbeat(TimeSpan interval) =>
        _masterHeartbeatTask = SendMasterHeartbeatsAsync(interval, _disposeCts.Token);

    internal async Task SendMasterHeartbeatAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _database.ScriptEvaluateAsync(
                RedisCoordinationScripts.MasterHeartbeat,
                [(RedisKey) _keys.MasterHeartbeat],
                [(long) _keyExpiration.TotalMilliseconds])
            .WaitAsync(cancellationToken).ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposeCts.IsCancellationRequested)
        {
            return;
        }

        await _disposeCts.CancelAsync().ConfigureAwait(false);
        await _masterHeartbeatTask.ConfigureAwait(false);
        _disposeCts.Dispose();
    }

    private async Task SendMasterHeartbeatsAsync(TimeSpan interval, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await SendMasterHeartbeatAsync(cancellationToken).ConfigureAwait(false);
                await Task.Delay(interval, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception) when (exception is RedisException or TimeoutException)
            {
                // Workers tolerate missed heartbeats for MasterTimeout; retry on the next interval.
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
    }

    internal static double GetQueueScore(ModuleAssignment assignment)
    {
        var criticalPathScore = Math.Clamp(
            assignment.CriticalPathWeight.TotalSeconds,
            0,
            MaximumCriticalPathScore);
        return ((int) assignment.Priority * PriorityScoreBand) + criticalPathScore;
    }

    private async Task<SerializedModuleResult?> TryGetResultAsync(ModuleId moduleId, CancellationToken cancellationToken)
    {
        var existing = await _database.HashGetAsync(_keys.Results, moduleId.Value)
            .WaitAsync(cancellationToken).ConfigureAwait(false);
        return existing.IsNullOrEmpty
            ? null
            : JsonSerializer.Deserialize<SerializedModuleResult>(existing.ToString())!;
    }

    private async Task<DistributedCancellationReason?> TryGetCancellationReasonAsync(CancellationToken cancellationToken)
    {
        var value = await _database.StringGetAsync(_keys.CancellationFlag)
            .WaitAsync(cancellationToken).ConfigureAwait(false);
        if (value.IsNullOrEmpty)
        {
            return null;
        }

        return Enum.TryParse<DistributedCancellationReason>(value.ToString(), out var reason)
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
        var result = await _database.ScriptEvaluateAsync(
                RedisCoordinationScripts.Claim,
                [
                    (RedisKey) _keys.WorkQueue,
                    (RedisKey) _keys.Workers,
                    (RedisKey) _keys.Leases,
                    (RedisKey) _keys.Results,
                    (RedisKey) _keys.CancellationFlag,
                    (RedisKey) _keys.CompletionFlag,
                ],
                [capabilities, (long) _workerTimeout.TotalMilliseconds, workerId.Value, leaseId])
            .WaitAsync(cancellationToken).ConfigureAwait(false);

        if (result.IsNull)
        {
            return (false, null);
        }

        var member = result.ToString()!;
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
        await _subscriber.PublishAsync(RedisChannel.Literal(channel), "1")
            .WaitAsync(cancellationToken).ConfigureAwait(false);

    private async Task RefreshRunKeysAsync(CancellationToken cancellationToken)
    {
        var refreshes = _keys.CoordinationKeys
            .Select(key => _database.KeyExpireAsync(key, _keyExpiration));
        await Task.WhenAll(refreshes).WaitAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Subscribes with one message queue per channel, so unsubscribing removes only this call's
    /// handlers from the shared connection.
    /// </summary>
    private async Task<Subscriptions> SubscribeAsync(
        SemaphoreSlim signal,
        CancellationToken cancellationToken,
        params string[] channels)
    {
        var subscriptions = new Subscriptions();
        try
        {
            foreach (var channel in channels)
            {
                var queue = await _subscriber.SubscribeAsync(RedisChannel.Literal(channel))
                    .WaitAsync(cancellationToken).ConfigureAwait(false);
                subscriptions.Add(queue);
                queue.OnMessage(_ => signal.Release());
            }
        }
        catch
        {
            await subscriptions.DisposeAsync().ConfigureAwait(false);
            throw;
        }

        return subscriptions;
    }

    private async Task<long> GetServerTimeMillisecondsAsync(CancellationToken cancellationToken)
    {
        var result = await _database.ExecuteAsync(
                "TIME",
                Array.Empty<object>(),
                CommandFlags.None)
            .WaitAsync(cancellationToken).ConfigureAwait(false);
        var parts = (RedisResult[]?) result;
        if (parts is not { Length: 2 })
        {
            throw new InvalidOperationException("Redis TIME returned an invalid response.");
        }

        return checked(((long) parts[0] * 1000) + ((long) parts[1] / 1000));
    }

    private sealed record LeaseRecord(string LeaseId, string WorkerId, string Member, string Score, string ExpiresAt);

    private sealed class Subscriptions : IAsyncDisposable
    {
        private readonly List<ChannelMessageQueue> _queues = [];

        public void Add(ChannelMessageQueue queue) => _queues.Add(queue);

        public async ValueTask DisposeAsync()
        {
            foreach (var queue in _queues)
            {
                try
                {
                    await queue.UnsubscribeAsync().ConfigureAwait(false);
                }
                catch (Exception exception) when (exception is RedisException or ObjectDisposedException)
                {
                    // The connection is gone; the subscription went with it.
                }
            }
        }
    }
}
