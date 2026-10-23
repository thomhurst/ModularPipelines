using System.Text.Json;
using System.Text.Json.Nodes;
using Kevlar;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Redis;
using ModularPipelines.Distributed.Redis.Artifacts;
using Respire;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Redis-based implementation of <see cref="IDistributedMasterCoordinator"/>.
/// All keys are isolated by run identifier to support concurrent pipeline runs.
/// </summary>
/// <remarks>
/// Enqueueing, claims, heartbeats, lease expiry and result publication run as Lua scripts so each is
/// atomic and costs one round trip; the scripts that change run state also renew the run keys' expiry.
/// A claim that finds legacy capability metadata first normalizes it in .NET and retries; conditional
/// writes preserve concurrent registrations and cleanup, along with existing class IDs and queue members.
/// Claims read one queue member per capability class rather than scanning the queue. Worker heartbeats use
/// hash field expiry, so the coordinator needs Redis 7.4 or later (or Valkey 9.0 or later).
/// Sharded pub/sub only wakes waiters early: every wait is bounded and re-reads Redis, so a message lost
/// while the connection reconnects delays a waiter by at most <see cref="PollInterval"/>.
/// Cancelling a claim abandons its wait until its assignment is received; Redis may still run a command
/// already sent. Once received, the assignment is returned even after cancellation so it can be finalized.
/// Claims retry with the same lease token until success or cancellation, including after a retry batch fails.
/// Delivery acknowledgement is attempted once before returning a lease. An ambiguous acknowledgement failure
/// returns the received assignment instead of waiting for retries. Worker pools confirm the lease with a fenced
/// heartbeat before executing user code. Only acknowledged or heartbeat-renewed leases count toward
/// <see cref="DistributedOptions.MaxModuleLeaseExpirations"/>, so lost claim responses cannot exhaust it.
/// The master's coordinator records a heartbeat every <see cref="DistributedOptions.WorkerHeartbeatInterval"/>
/// until it is disposed or cleans up the run, so workers detect a master that stopped without signalling
/// completion.
/// </remarks>
internal sealed class RedisDistributedCoordinator : IDistributedMasterCoordinator, IDistributedRunCleanup, IAsyncDisposable
{
    internal static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(2);

    private const string ClosedSentinel = "__closed__";
    private const string NormalizeSentinel = "__normalize__";

    private static readonly RespireScript HashFieldExpiryProbeScript = RespireScript.Create(
        "return redis.call('HPEXPIRE', KEYS[1], 1000, 'FIELDS', 1, '__modularpipelines_probe__')");
    private static readonly RespireScript EnqueueScript = RespireScript.Create(RedisCoordinationScripts.Enqueue);
    private static readonly RespireScript ClaimScript = RespireScript.Create(RedisCoordinationScripts.Claim);
    private static readonly RespireScript NormalizeCapabilitiesScript = RespireScript.Create(RedisCoordinationScripts.NormalizeCapabilities);
    private static readonly RespireScript AcknowledgeClaimScript = RespireScript.Create(RedisCoordinationScripts.AcknowledgeClaim);
    private static readonly RespireScript HeartbeatScript = RespireScript.Create(RedisCoordinationScripts.Heartbeat);
    private static readonly RespireScript ReserveMasterRunScript = RespireScript.Create(RedisCoordinationScripts.ReserveMasterRun);
    private static readonly RespireScript ReconcileHeartbeatScript = RespireScript.Create(RedisCoordinationScripts.ReconcileHeartbeat);
    private static readonly RespireScript MasterHeartbeatScript = RespireScript.Create(RedisCoordinationScripts.MasterHeartbeat);
    private static readonly RespireScript GetMasterStateScript = RespireScript.Create(RedisCoordinationScripts.GetMasterState);
    private static readonly RespireScript RegisterWorkerScript = RespireScript.Create(RedisCoordinationScripts.RegisterWorker);
    private static readonly RespireScript PublishResultScript = RespireScript.Create(RedisCoordinationScripts.PublishResult);
    private static readonly RespireScript RequeueExpiredLeasesScript = RespireScript.Create(RedisCoordinationScripts.RequeueExpiredLeases);
    private static readonly RespireScript WithdrawScript = RespireScript.Create(RedisCoordinationScripts.Withdraw);

    private readonly IRespireClient _redis;
    private readonly RedisCoordinationDiagnostics _diagnostics;
    private readonly Shield _scriptRetry = RedisTransientRetry.Create(RedisTransientRetry.DefaultBaseDelay);
    private readonly RedisKeyBuilder _keys;
    private readonly RespireKey[] _scriptKeys;
    private readonly TimeSpan _keyExpiration;
    private readonly long _keyExpirationMilliseconds;
    private readonly long _workerTimeoutMilliseconds;
    private readonly TimeSpan _workerTimeout;
    private readonly int _maxModuleLeaseExpirations;
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
        ILogger<RedisDistributedCoordinator>? logger = null,
        RedisCoordinationDiagnostics? diagnostics = null)
    {
        _redis = redis;
        _keys = keys;
        _scriptKeys = [.. keys.ScriptKeys.Select(static key => (RespireKey) key)];
        _keyExpiration = options.TimeToLive;
        _keyExpirationMilliseconds = (long) options.TimeToLive.TotalMilliseconds;
        _workerTimeout = distributedOptions?.WorkerTimeout ?? TimeSpan.FromSeconds(30);
        _maxModuleLeaseExpirations = distributedOptions?.MaxModuleLeaseExpirations ?? 2;
        _workerTimeoutMilliseconds = (long) _workerTimeout.TotalMilliseconds;
        _masterTimeout = distributedOptions?.MasterTimeout ?? TimeSpan.FromMinutes(1);
        _onWaitReady = onWaitReady;
        _logger = logger;
        // The coordinator owns and disposes diagnostics, including an injected instance.
        _diagnostics = diagnostics ?? new RedisCoordinationDiagnostics(options, logger);
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
        // Reuse the queue member's identifier when an executed script loses its response.
        RespireValue[] arguments = [_keyExpirationMilliseconds, classId, classJson, member, assignment.ModuleId.Value];
        await _scriptRetry.ExecuteAsync(
                async token => await _redis.Scripts.ExecuteIntegerAsync(EnqueueScript, _scriptKeys, arguments, token)
                    .ConfigureAwait(false),
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
            var (closed, lease) = await TryClaimAsync(workerId, workerCapabilities, cancellationToken).ConfigureAwait(false);
            if (closed)
            {
                return null;
            }

            if (lease is not null)
            {
                return lease;
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
        if (lease is not null
            && (lease.Assignment.ModuleId != result.ModuleId || string.IsNullOrEmpty(lease.LeaseId)))
        {
            return;
        }

        var json = JsonSerializer.Serialize(result);
        RespireValue[] arguments = [_keyExpirationMilliseconds, result.ModuleId.Value, json, lease?.LeaseId ?? string.Empty, lease?.WorkerId.Value ?? string.Empty];

        // Validate the lease atomically with accepting the result and releasing ownership.
        // Replaying the same result and lease cannot overwrite a result already accepted by Redis.
        await _scriptRetry.ExecuteAsync(
                async token => await _redis.Scripts.ExecuteIntegerAsync(PublishResultScript, _scriptKeys, arguments, token)
                    .ConfigureAwait(false),
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
        var registrationJson = JsonSerializer.SerializeToNode(registration)!;
        registrationJson["NormalizedCapabilities"] = JsonSerializer.SerializeToNode(RedisCapabilityNames.Normalize(registration.Capabilities));
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
                    registrationJson.ToJsonString(),
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
        try
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
        catch (Exception exception) when (RedisTransientRetry.IsTransient(exception))
        {
            _ = _diagnostics.ReportFailure("worker heartbeat");
            throw;
        }
    }

    public async Task<IReadOnlyList<string>> ReconcileHeartbeatAsync(
        WorkerStatus status,
        IReadOnlyList<ModuleLease> leases,
        CancellationToken cancellationToken)
    {
        try
        {
            var tokens = leases.Select(static lease => new
            {
                ModuleId = lease.Assignment.ModuleId.Value,
                WorkerId = lease.WorkerId.Value,
                lease.LeaseId,
            });
            using var result = await _redis.Scripts.ExecuteAsync(
                    ReconcileHeartbeatScript,
                    _scriptKeys,
                    [_keyExpirationMilliseconds, status.WorkerId.Value, _workerTimeoutMilliseconds,
                        JsonSerializer.Serialize(status), JsonSerializer.Serialize(tokens)],
                    cancellationToken)
                .ConfigureAwait(false);
            return [.. result.Select(static item => item.AsString())];
        }
        catch (Exception exception) when (RedisTransientRetry.IsTransient(exception))
        {
            _ = _diagnostics.ReportFailure("lease heartbeat");
            throw;
        }
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
        var entries = await ReadHashAsync(_keys.Leases, cancellationToken).ConfigureAwait(false);
        return [.. entries.Values.Select(static entry => ToLease(JsonSerializer.Deserialize<LeaseRecord>(entry)!))];
    }

    public async Task<IReadOnlyList<ModuleId>> RequeueExpiredLeasesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ModuleId[] requeued;
        ModuleId[] failed;
        using (var result = await _redis.Scripts.ExecuteAsync(
                       RequeueExpiredLeasesScript,
                       _scriptKeys,
                       [_keyExpirationMilliseconds, _maxModuleLeaseExpirations, DateTimeOffset.UtcNow.ToString("O")],
                       cancellationToken)
                   .ConfigureAwait(false))
        {
            requeued = [.. result[0].Select(static item => new ModuleId(item.AsString()))];
            failed = [.. result[1].Select(static item => new ModuleId(item.AsString()))];
        }

        // The master calls this periodically, so the script also keeps the run's keys alive.
        if (requeued.Length > 0)
        {
            await NotifyAsync(_keys.WorkAvailableChannel, cancellationToken).ConfigureAwait(false);
        }

        foreach (var moduleId in failed)
        {
            await NotifyAsync(_keys.ResultChannel(moduleId), cancellationToken).ConfigureAwait(false);
        }

        return requeued;
    }

    public async Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        var registrations = ReadHashAsync(_keys.Workers, cancellationToken);
        var heartbeats = ReadHashAsync(_keys.WorkerHeartbeats, cancellationToken);
        var statuses = ReadHashAsync(_keys.WorkerStatuses, cancellationToken);
        await Task.WhenAll(registrations, heartbeats, statuses).ConfigureAwait(false);

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
        var entries = await ReadHashAsync(_keys.WorkerStatuses, cancellationToken).ConfigureAwait(false);
        return [.. entries.Values.Select(static entry => JsonSerializer.Deserialize<WorkerStatus>(entry)!)];
    }

    private async Task<Dictionary<string, string>> ReadHashAsync(string key, CancellationToken cancellationToken)
    {
        var entries = new Dictionary<string, string>(StringComparer.Ordinal);
        await foreach (var entry in _redis.Hashes.ScanAsync(key, cancellationToken: cancellationToken).ConfigureAwait(false))
        {
            // Cursor scans may return the same field more than once.
            entries[entry.Key] = entry.Value;
        }

        return entries;
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
                RespireSetWhen.NotExists,
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
        try
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
        catch (Exception exception) when (RedisTransientRetry.IsTransient(exception))
        {
            _ = _diagnostics.ReportFailure("master liveness");
            throw;
        }
    }

    /// <summary>
    /// Atomically reserves a fresh run and records its first heartbeat. Recording both together keeps
    /// workers that registered first from seeing a started master with no heartbeat as lost.
    /// </summary>
    internal async Task ReserveMasterRunAsync(string runId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var reserved = await _redis.Scripts.ExecuteIntegerAsync(
                ReserveMasterRunScript,
                _scriptKeys,
                [_keyExpirationMilliseconds],
                cancellationToken)
            .ConfigureAwait(false);
        if (reserved == 0)
        {
            throw new InvalidOperationException(
                $"RunId already used: '{runId}'. Redis contains state from a started or completed run. "
                + "Choose a unique RunId for each pipeline run.");
        }
    }

    /// <summary>
    /// Records a master heartbeat now and then every <paramref name="interval"/> until this
    /// coordinator is disposed. Artifact renewal runs independently so a large registry cannot
    /// delay subsequent heartbeats. Disposal and run cleanup cancel and await both loops.
    /// </summary>
    internal void StartMasterHeartbeat(TimeSpan interval) =>
        _masterHeartbeatTask = Task.WhenAll(
            RunMasterMaintenanceAsync(SendMasterHeartbeatAsync, "Could not record the distributed master heartbeat; retrying", interval, _masterHeartbeatCts.Token),
            RunMasterMaintenanceAsync(
                token => RedisDistributedArtifactStore.RefreshTimeToLiveAsync(_redis, _keys, _keyExpiration, token),
                "Could not renew distributed artifact expiration; retrying",
                interval,
                _masterHeartbeatCts.Token));

    internal async Task SendMasterHeartbeatAsync(CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _redis.Scripts.ExecuteIntegerAsync(
                    MasterHeartbeatScript,
                    _scriptKeys,
                    [_keyExpirationMilliseconds],
                    cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception exception) when (RedisTransientRetry.IsTransient(exception))
        {
            _ = _diagnostics.ReportFailure("master heartbeat");
            throw;
        }
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
        await _diagnostics.DisposeAsync().ConfigureAwait(false);
    }

    private async Task StopMasterHeartbeatAsync()
    {
        await _masterHeartbeatCts.CancelAsync().ConfigureAwait(false);
        await _masterHeartbeatTask.ConfigureAwait(false);
    }

    private async Task RunMasterMaintenanceAsync(
        Func<CancellationToken, Task> action,
        string failureMessage,
        TimeSpan interval,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await action(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                _logger?.LogWarning(exception, "{MaintenanceFailure}", failureMessage);
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
        if (string.IsNullOrEmpty(existing))
        {
            return null;
        }

        using var document = JsonDocument.Parse(existing);
        return document.RootElement.TryGetProperty(nameof(LeaseExpirationFailure.LeaseExpirationWorkers), out _)
            ? document.RootElement.Deserialize<LeaseExpirationFailure>()!.ToResult()
            : document.RootElement.Deserialize<SerializedModuleResult>()!;
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

    internal async Task<(bool Closed, ModuleLease? Lease)> TryClaimAsync(
        WorkerId workerId,
        IReadOnlySet<Capability> workerCapabilities,
        CancellationToken cancellationToken)
    {
        // The lease token is also the claim ID. An ambiguous command is retried with the same token.
        var leaseId = Guid.NewGuid().ToString("N");
        var capabilities = JsonSerializer.Serialize(RedisCapabilityNames.Normalize(workerCapabilities));
        RespireValue[] arguments = [capabilities, workerId.Value, leaseId, _workerTimeoutMilliseconds, _keyExpirationMilliseconds];
        string? assignment;
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            assignment = await ExecuteClaimAsync(arguments, cancellationToken).ConfigureAwait(false);
            if (assignment is null || !assignment.StartsWith(NormalizeSentinel, StringComparison.Ordinal))
            {
                break;
            }

            await NormalizeLegacyCapabilitiesAsync(assignment[NormalizeSentinel.Length..], cancellationToken).ConfigureAwait(false);
        }

        if (assignment is null || assignment == ClosedSentinel)
        {
            return (assignment == ClosedSentinel, null);
        }

        // Receipt transfers ownership. Do not retry an ambiguous acknowledgement or let cancellation
        // discard this assignment: the worker pool must execute or finalize the work it already received.
        try
        {
            var acknowledged = await _redis.Scripts.ExecuteStringAsync(
                    AcknowledgeClaimScript, _scriptKeys, arguments, CancellationToken.None)
                .ConfigureAwait(false);
            if (acknowledged is null)
            {
                // A definitive rejection means the lease expired or was reassigned before acknowledgement.
                return (false, null);
            }
        }
        catch (Exception exception) when (RedisTransientRetry.IsTransient(exception))
        {
            // Redis may have acknowledged delivery. Return the known assignment either way; the normal
            // execution-start heartbeat confirms it before user code runs. Publication still checks ownership.
            _logger?.LogWarning(exception, "Redis claim acknowledgement was ambiguous; returning the received assignment");
        }

        return (false, new ModuleLease
        {
            LeaseId = leaseId,
            WorkerId = workerId,
            Assignment = JsonSerializer.Deserialize<ModuleAssignment>(assignment)!,
        });
    }

    private async Task NormalizeLegacyCapabilitiesAsync(string json, CancellationToken cancellationToken)
    {
        var entries = JsonSerializer.Deserialize<CapabilityMetadata[]>(json)!;
        List<RespireValue> arguments = [];
        foreach (var entry in entries)
        {
            var metadata = JsonNode.Parse(entry.Value)!;
            if (entry.KeyIndex == 2)
            {
                var clauses = metadata["RequiredCapabilities"]!.Deserialize<Capability[][]>()!;
                metadata["RequiredCapabilities"] = JsonSerializer.SerializeToNode(clauses.Select(RedisCapabilityNames.Normalize).ToArray());
                metadata["CapabilitiesNormalized"] = true;
            }
            else
            {
                var capabilities = metadata["Capabilities"]!.Deserialize<Capability[]>()!;
                metadata["NormalizedCapabilities"] = JsonSerializer.SerializeToNode(RedisCapabilityNames.Normalize(capabilities));
            }

            arguments.AddRange([entry.KeyIndex, entry.Field, entry.Value, metadata.ToJsonString()]);
        }

        await _redis.Scripts.ExecuteIntegerAsync(NormalizeCapabilitiesScript, _scriptKeys, [.. arguments], cancellationToken)
            .ConfigureAwait(false);
    }

    private sealed record CapabilityMetadata(int KeyIndex, string Field, string Value);

    private async Task<string?> ExecuteClaimAsync(
        RespireValue[] arguments,
        CancellationToken cancellationToken)
    {
        while (true)
        {
            try
            {
                return await _scriptRetry.ExecuteAsync(
                        async token => await _redis.Scripts.ExecuteStringAsync(ClaimScript, _scriptKeys, arguments, token)
                            .ConfigureAwait(false),
                        cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception exception) when (RedisTransientRetry.IsTransient(exception))
            {
                // Surfacing an ambiguous failure lets the worker pool start a new claim for this slot.
                // Keep this token until Redis confirms the outcome or the caller abandons the wait.
                _logger?.LogWarning(exception, "Retrying unresolved Redis claim with the same lease token");
                await Task.Delay(PollInterval, cancellationToken).ConfigureAwait(false);
            }
        }
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
        RespireChannel[] subscriptionChannels = [.. channels.Select(static channel => (RespireChannel) channel)];
        var subscription = await _redis.SubscribeShardedAsync(subscriptionChannels, cancellationToken: cancellationToken).ConfigureAwait(false);
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
