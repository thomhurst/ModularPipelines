using System.Text.Json;
using System.Threading;
using ModularPipelines.Distributed.Redis.Configuration;
using ModularPipelines.Distributed.Serialization;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Redis-based implementation of <see cref="IDistributedMasterCoordinator"/>.
/// All keys are isolated by run identifier to support concurrent pipeline runs.
/// </summary>
internal sealed class RedisDistributedCoordinator : IDistributedMasterCoordinator
{
    private const char QueueMemberSeparator = '|';
    private const double PriorityScoreBand = 1_000_000_000_000;
    private const double MaximumCriticalPathScore = PriorityScoreBand - 1;

    private readonly IDatabase _database;
    private readonly ISubscriber _subscriber;
    private readonly RedisKeyBuilder _keys;
    private readonly TimeSpan _keyExpiration;
    private readonly TimeSpan _workerTimeout;
    private readonly JsonSerializerOptions _jsonOptions;
    // Lets real-backend contract tests synchronize after the race-closing reads complete.
    private readonly Action? _onWaitReady;

    public RedisDistributedCoordinator(
        IDatabase database,
        ISubscriber subscriber,
        RedisKeyBuilder keys,
        RedisDistributedOptions options,
        Action? onWaitReady = null,
        DistributedOptions? distributedOptions = null)
    {
        _database = database;
        _subscriber = subscriber;
        _keys = keys;
        _keyExpiration = options.KeyExpiration;
        _workerTimeout = distributedOptions?.WorkerTimeout ?? TimeSpan.FromSeconds(30);
        _onWaitReady = onWaitReady;
        _jsonOptions = new JsonSerializerOptions
        {
            Converters = { new ReadOnlySetJsonConverter() },
        };
    }

    public async Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(assignment, _jsonOptions);
        var queueMember = $"{Guid.NewGuid():N}{QueueMemberSeparator}{json}";
        await _database.SortedSetAddAsync(_keys.WorkQueue, queueMember, GetQueueScore(assignment));
        await _database.KeyExpireAsync(_keys.WorkQueue, _keyExpiration);

        // Notify waiting workers that work is available
        await _subscriber.PublishAsync(RedisChannel.Literal(_keys.WorkAvailableChannel), "1");
    }

    public async Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<Capability> workerCapabilities, CancellationToken cancellationToken)
    {
        // Check if completion was already signalled before subscribing
        var completionFlag = await _database.StringGetAsync(_keys.CompletionFlag);
        if (!completionFlag.IsNullOrEmpty)
        {
            return null;
        }

        // Subscribe to work-available and completion notifications
        using var signal = new SemaphoreSlim(0);
        var completed = 0; // 0 = false, 1 = true; using int for thread-safe Volatile access
        var workChannel = RedisChannel.Literal(_keys.WorkAvailableChannel);
        var completionChannel = RedisChannel.Literal(_keys.CompletionChannel);

        await _subscriber.SubscribeAsync(workChannel, (_, _) => signal.Release());
        await _subscriber.SubscribeAsync(completionChannel, (_, _) =>
        {
            Volatile.Write(ref completed, 1);
            signal.Release();
        });

        try
        {
            // Check for items already in the queue before we subscribed
            var found = await TryScanAndClaimAsync(workerCapabilities);
            if (found is not null)
            {
                return found;
            }

            // Re-check completion flag after subscribing (close race condition)
            completionFlag = await _database.StringGetAsync(_keys.CompletionFlag);
            if (!completionFlag.IsNullOrEmpty)
            {
                return null;
            }

            _onWaitReady?.Invoke();

            // Wait for notifications — only scan the sorted set when work is available.
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await signal.WaitAsync(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return null;
                }

                if (Volatile.Read(ref completed) == 1)
                {
                    return null;
                }

                // Drain any extra notifications that arrived while we were scanning
                while (signal.CurrentCount > 0)
                {
                    signal.Wait(0);
                }

                found = await TryScanAndClaimAsync(workerCapabilities);
                if (found is not null)
                {
                    return found;
                }
            }

            return null;
        }
        finally
        {
            await _subscriber.UnsubscribeAsync(workChannel);
            await _subscriber.UnsubscribeAsync(completionChannel);
        }
    }

    public async Task PublishResultAsync(SerializedModuleResult result, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(result, _jsonOptions);
        await _database.HashSetAsync(_keys.Results, result.ModuleTypeName, json);
        await _database.KeyExpireAsync(_keys.Results, _keyExpiration);
        await _subscriber.PublishAsync(RedisChannel.Literal(_keys.ResultChannel(result.ModuleTypeName)), json);
    }

    public async Task<SerializedModuleResult> WaitForResultAsync(string moduleTypeName, CancellationToken cancellationToken)
    {
        // Check if result already exists
        var existing = await _database.HashGetAsync(_keys.Results, moduleTypeName);
        if (!existing.IsNullOrEmpty)
        {
            return JsonSerializer.Deserialize<SerializedModuleResult>(existing.ToString(), _jsonOptions)!;
        }

        // Subscribe and wait
        var tcs = new TaskCompletionSource<SerializedModuleResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        var channel = RedisChannel.Literal(_keys.ResultChannel(moduleTypeName));

        var subscription = await _subscriber.SubscribeAsync(channel);
        subscription.OnMessage(msg =>
        {
            var result = JsonSerializer.Deserialize<SerializedModuleResult>(msg.Message.ToString(), _jsonOptions)!;
            tcs.TrySetResult(result);
        });

        try
        {
            // Re-check after subscribing to close race condition
            existing = await _database.HashGetAsync(_keys.Results, moduleTypeName);
            if (!existing.IsNullOrEmpty)
            {
                tcs.TrySetResult(JsonSerializer.Deserialize<SerializedModuleResult>(existing.ToString(), _jsonOptions)!);
            }

            if (!tcs.Task.IsCompleted)
            {
                _onWaitReady?.Invoke();
            }

            using var reg = cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken));
            return await tcs.Task;
        }
        finally
        {
            await _subscriber.UnsubscribeAsync(channel);
        }
    }

    public async Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(registration, _jsonOptions);
        await _database.HashSetAsync(_keys.Workers, registration.WorkerIndex.ToString(), json)
            .ConfigureAwait(false);
        await SendHeartbeatAsync(registration.WorkerIndex, cancellationToken).ConfigureAwait(false);
    }

    public async Task SendHeartbeatAsync(int workerIndex, CancellationToken cancellationToken)
    {
        var serverTimeMilliseconds = await GetServerTimeMillisecondsAsync().ConfigureAwait(false);
        await _database.HashSetAsync(
            _keys.Workers,
            _keys.WorkerHeartbeatField(workerIndex),
            serverTimeMilliseconds).ConfigureAwait(false);
        await _database.KeyExpireAsync(_keys.Workers, _keyExpiration).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        var serverTimeMilliseconds = await GetServerTimeMillisecondsAsync().ConfigureAwait(false);
        var entries = await _database.HashGetAllAsync(_keys.Workers).ConfigureAwait(false);
        var oldestLiveHeartbeat = serverTimeMilliseconds - _workerTimeout.TotalMilliseconds;
        var heartbeats = entries
            .Where(entry => entry.Name.ToString().StartsWith("heartbeat:", StringComparison.Ordinal))
            .ToDictionary(
                entry => int.Parse(entry.Name.ToString()["heartbeat:".Length..]),
                entry => (long) entry.Value);
        var workers = new List<WorkerRegistration>(entries.Length);
        foreach (var entry in entries.Where(entry => int.TryParse(entry.Name.ToString(), out _)))
        {
            var registration = JsonSerializer.Deserialize<WorkerRegistration>(
                entry.Value.ToString(),
                _jsonOptions)!;
            if (registration.UnattributedCommandCount.HasValue
                || (heartbeats.TryGetValue(registration.WorkerIndex, out var heartbeat)
                    && heartbeat >= oldestLiveHeartbeat))
            {
                workers.Add(registration);
            }
        }

        return workers;
    }

    public async Task SignalCompletionAsync(CancellationToken cancellationToken)
    {
        await _database.StringSetAsync(_keys.CompletionFlag, "1");
        await _database.KeyExpireAsync(_keys.CompletionFlag, _keyExpiration);
        await _subscriber.PublishAsync(RedisChannel.Literal(_keys.CompletionChannel), "1");
    }

    public async Task BroadcastCancellationAsync(CancellationToken cancellationToken)
    {
        await _database.StringSetAsync(_keys.CancellationFlag, "1");
        await _database.KeyExpireAsync(_keys.CancellationFlag, _keyExpiration);
        await _subscriber.PublishAsync(RedisChannel.Literal(_keys.CancellationChannel), "1");
    }

    public async Task WaitForCancellationAsync(CancellationToken cancellationToken)
    {
        var existing = await _database.StringGetAsync(_keys.CancellationFlag);
        if (!existing.IsNullOrEmpty)
        {
            return;
        }

        var cancellationSignal = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var channel = RedisChannel.Literal(_keys.CancellationChannel);
        var subscription = await _subscriber.SubscribeAsync(channel);
        subscription.OnMessage(_ => cancellationSignal.TrySetResult());

        try
        {
            existing = await _database.StringGetAsync(_keys.CancellationFlag);
            if (!existing.IsNullOrEmpty)
            {
                cancellationSignal.TrySetResult();
            }

            if (!cancellationSignal.Task.IsCompleted)
            {
                _onWaitReady?.Invoke();
            }

            using var registration = cancellationToken.Register(() =>
                cancellationSignal.TrySetCanceled(cancellationToken));
            await cancellationSignal.Task;
        }
        finally
        {
            await subscription.UnsubscribeAsync().ConfigureAwait(false);
        }
    }

    private static readonly string ScanAndClaimScript = @"
local priority_band = 1000000000000
local items = redis.call('ZREVRANGE', KEYS[1], 0, -1, 'WITHSCORES')
local caps = cjson.decode(ARGV[1])
local worker_timeout = tonumber(ARGV[2])
local server_time = redis.call('TIME')
local now = (tonumber(server_time[1]) * 1000) + math.floor(tonumber(server_time[2]) / 1000)
local worker_entries = redis.call('HGETALL', KEYS[2])
local live_workers = {}

for i = 1, #worker_entries, 2 do
    local worker_index = tonumber(worker_entries[i])
    if worker_index ~= nil then
        local heartbeat = tonumber(redis.call('HGET', KEYS[2], 'heartbeat:' .. worker_index) or '0')
        if heartbeat >= now - worker_timeout then
            table.insert(live_workers, cjson.decode(worker_entries[i + 1]))
        end
    end
end

local function supports(required, available)
    for _, req in ipairs(required) do
        local found = false
        for _, cap in ipairs(available) do
            if string.lower(req) == string.lower(cap) then
                found = true
                break
            end
        end
        if not found then
            return false
        end
    end
    return true
end

local best_item = nil
local best_priority = -1
local best_eligible_workers = nil
local best_required_count = -1
local best_weight = -1
local best_assigned_at = nil

for i = 1, #items, 2 do
    local item = items[i]
    local score = tonumber(items[i + 1])
    local separator = string.find(item, '|', 1, true)
    local prefix = separator == 33 and string.sub(item, 1, 32) or nil
    local has_unique_prefix = prefix ~= nil and string.match(prefix, '^[0-9a-fA-F]+$') ~= nil
    local assignment_json = has_unique_prefix and string.sub(item, separator + 1) or item
    local assignment = cjson.decode(assignment_json)
    local required = assignment['RequiredCapabilities'] or {}
    if supports(required, caps) then
        local eligible_workers = 0
        for _, worker in ipairs(live_workers) do
            if supports(required, worker['Capabilities'] or {}) then
                eligible_workers = eligible_workers + 1
            end
        end

        local priority = math.floor(score / priority_band)
        local weight = score - (priority * priority_band)
        local required_count = #required
        local assigned_at = assignment['AssignedAt'] or ''
        local is_better = priority > best_priority
            or (priority == best_priority and (best_eligible_workers == nil or eligible_workers < best_eligible_workers))
            or (priority == best_priority and eligible_workers == best_eligible_workers and required_count > best_required_count)
            or (priority == best_priority and eligible_workers == best_eligible_workers and required_count == best_required_count and weight > best_weight)
            or (priority == best_priority and eligible_workers == best_eligible_workers and required_count == best_required_count and weight == best_weight and (best_assigned_at == nil or assigned_at < best_assigned_at))

        if is_better then
            best_item = item
            best_priority = priority
            best_eligible_workers = eligible_workers
            best_required_count = required_count
            best_weight = weight
            best_assigned_at = assigned_at
        end
    end
end

if best_item ~= nil then
    redis.call('ZREM', KEYS[1], best_item)
end
return best_item";

    internal static double GetQueueScore(ModuleAssignment assignment)
    {
        var criticalPathScore = Math.Clamp(
            assignment.CriticalPathWeight.TotalSeconds,
            0,
            MaximumCriticalPathScore);
        return ((int) assignment.Priority * PriorityScoreBand) + criticalPathScore;
    }

    private async Task<ModuleAssignment?> TryScanAndClaimAsync(IReadOnlySet<Capability> workerCapabilities)
    {
        var capsJson = JsonSerializer.Serialize(workerCapabilities.ToArray());
        var result = await _database.ScriptEvaluateAsync(
            ScanAndClaimScript,
            [(RedisKey) _keys.WorkQueue, (RedisKey) _keys.Workers],
            [capsJson, _workerTimeout.TotalMilliseconds]);

        if (result.IsNull)
        {
            return null;
        }

        var queueMember = result.ToString()!;
        var separatorIndex = queueMember.IndexOf(QueueMemberSeparator);
        var hasUniquePrefix = separatorIndex == 32
            && Guid.TryParseExact(queueMember.AsSpan(0, separatorIndex), "N", out _);
        var assignmentJson = hasUniquePrefix
            ? queueMember[(separatorIndex + 1)..]
            : queueMember;
        return JsonSerializer.Deserialize<ModuleAssignment>(assignmentJson, _jsonOptions);
    }

    private async Task<long> GetServerTimeMillisecondsAsync()
    {
        var result = await _database.ExecuteAsync(
            "TIME",
            Array.Empty<object>(),
            CommandFlags.None).ConfigureAwait(false);
        var parts = (RedisResult[]?) result;
        if (parts is not { Length: 2 })
        {
            throw new InvalidOperationException("Redis TIME returned an invalid response.");
        }

        return checked(((long) parts[0] * 1000) + ((long) parts[1] / 1000));
    }
}
