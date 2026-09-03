using System.Text.Json;
using System.Threading;
using ModularPipelines.Distributed.Redis;
using ModularPipelines.Distributed.Serialization;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Redis-based implementation of <see cref="IDistributedMasterCoordinator"/>.
/// All keys are isolated by run identifier to support concurrent pipeline runs.
/// </summary>
internal sealed class RedisDistributedCoordinator : IDistributedMasterCoordinator
{
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
        _keyExpiration = TimeSpan.FromSeconds(options.KeyExpirationSeconds);
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
        await _database.ListLeftPushAsync(_keys.WorkQueue, json);
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

            // Wait for notifications — only LRANGE when a publish says work is available
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
        await _database.HashSetAsync(_keys.Workers, registration.WorkerIndex.ToString(), json);
        await _database.KeyExpireAsync(_keys.Workers, _keyExpiration);
        await SendHeartbeatAsync(registration.WorkerIndex, cancellationToken);
    }

    public async Task SendHeartbeatAsync(int workerIndex, CancellationToken cancellationToken)
    {
        await _database.StringSetAsync(
            _keys.WorkerHeartbeat(workerIndex),
            "1",
            _workerTimeout);
    }

    public async Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        var entries = await _database.HashGetAllAsync(_keys.Workers);
        var workers = new List<WorkerRegistration>(entries.Length);
        foreach (var entry in entries)
        {
            var registration = JsonSerializer.Deserialize<WorkerRegistration>(
                entry.Value.ToString(),
                _jsonOptions)!;
            if (registration.UnattributedCommandCount.HasValue
                || await _database.KeyExistsAsync(_keys.WorkerHeartbeat(registration.WorkerIndex))
                    .ConfigureAwait(false))
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
local items = redis.call('LRANGE', KEYS[1], 0, -1)
local caps = cjson.decode(ARGV[1])
for i, item in ipairs(items) do
    local assignment = cjson.decode(item)
    local required = assignment['RequiredCapabilities']
    if required == nil or #required == 0 then
        redis.call('LREM', KEYS[1], 1, item)
        return item
    end
    local matched = true
    for _, req in ipairs(required) do
        local found = false
        for _, cap in ipairs(caps) do
            if string.lower(req) == string.lower(cap) then
                found = true
                break
            end
        end
        if not found then
            matched = false
            break
        end
    end
    if matched then
        redis.call('LREM', KEYS[1], 1, item)
        return item
    end
end
return nil";

    private async Task<ModuleAssignment?> TryScanAndClaimAsync(IReadOnlySet<Capability> workerCapabilities)
    {
        var capsJson = JsonSerializer.Serialize(workerCapabilities.ToArray());
        var result = await _database.ScriptEvaluateAsync(
            ScanAndClaimScript,
            [(RedisKey) _keys.WorkQueue],
            [capsJson]);

        if (result.IsNull)
        {
            return null;
        }

        return JsonSerializer.Deserialize<ModuleAssignment>(result.ToString()!, _jsonOptions);
    }
}
