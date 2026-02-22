using System.Text.Json;
using ModularPipelines.Distributed.Redis.Configuration;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Redis-based implementation of <see cref="IDistributedCoordinator"/>.
/// All keys are isolated by run identifier to support concurrent pipeline runs.
/// </summary>
internal sealed class RedisDistributedCoordinator : IDistributedCoordinator
{
    private readonly IDatabase _database;
    private readonly ISubscriber _subscriber;
    private readonly RedisKeyBuilder _keys;
    private readonly TimeSpan _keyExpiration;
    private readonly int _dequeuePollDelay;
    private readonly JsonSerializerOptions _jsonOptions;

    public RedisDistributedCoordinator(
        IDatabase database,
        ISubscriber subscriber,
        RedisKeyBuilder keys,
        RedisDistributedOptions options)
    {
        _database = database;
        _subscriber = subscriber;
        _keys = keys;
        _keyExpiration = TimeSpan.FromSeconds(options.KeyExpirationSeconds);
        _dequeuePollDelay = options.DequeuePollDelayMilliseconds;
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
    }

    public async Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<string> workerCapabilities, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // Scan existing items for a capability match without consuming non-matching items
            var items = await _database.ListRangeAsync(_keys.WorkQueue);
            foreach (var item in items)
            {
                if (item.IsNullOrEmpty)
                {
                    continue;
                }

                var candidate = JsonSerializer.Deserialize<ModuleAssignment>(item.ToString(), _jsonOptions)!;

                if (candidate.RequiredCapabilities.Count == 0 ||
                    candidate.RequiredCapabilities.IsSubsetOf(workerCapabilities))
                {
                    // Atomically remove this specific item (first occurrence)
                    var removed = await _database.ListRemoveAsync(_keys.WorkQueue, item, count: 1);
                    if (removed > 0)
                    {
                        return candidate;
                    }

                    // Another worker took it; continue scanning
                }
            }

            // No matching item found — wait before polling again
            try
            {
                await Task.Delay(_dequeuePollDelay, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        return null;
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
    }

    public async Task SendHeartbeatAsync(int workerIndex, CancellationToken cancellationToken)
    {
        var heartbeat = new WorkerHeartbeat(workerIndex, DateTimeOffset.UtcNow, null);
        var heartbeatJson = JsonSerializer.Serialize(heartbeat, _jsonOptions);
        await _database.HashSetAsync(_keys.Heartbeats, workerIndex.ToString(), heartbeatJson);
        await _database.KeyExpireAsync(_keys.Heartbeats, _keyExpiration);

        // Update worker status from Connected to Active
        var workerJson = await _database.HashGetAsync(_keys.Workers, workerIndex.ToString());
        if (!workerJson.IsNullOrEmpty)
        {
            var worker = JsonSerializer.Deserialize<WorkerRegistration>(workerJson.ToString(), _jsonOptions)!;
            if (worker.Status == WorkerStatus.Connected)
            {
                var updated = worker with { Status = WorkerStatus.Active };
                await _database.HashSetAsync(_keys.Workers, workerIndex.ToString(), JsonSerializer.Serialize(updated, _jsonOptions));
            }
        }
    }

    public async Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        var entries = await _database.HashGetAllAsync(_keys.Workers);
        var workers = new List<WorkerRegistration>(entries.Length);
        foreach (var entry in entries)
        {
            workers.Add(JsonSerializer.Deserialize<WorkerRegistration>(entry.Value.ToString(), _jsonOptions)!);
        }

        return workers;
    }

    public async Task<WorkerHeartbeat?> GetLastHeartbeatAsync(int workerIndex, CancellationToken cancellationToken)
    {
        var value = await _database.HashGetAsync(_keys.Heartbeats, workerIndex.ToString());
        if (value.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<WorkerHeartbeat>(value.ToString(), _jsonOptions);
    }

    public async Task UpdateWorkerStatusAsync(int workerIndex, WorkerStatus status, CancellationToken cancellationToken)
    {
        var workerJson = await _database.HashGetAsync(_keys.Workers, workerIndex.ToString());
        if (!workerJson.IsNullOrEmpty)
        {
            var worker = JsonSerializer.Deserialize<WorkerRegistration>(workerJson.ToString(), _jsonOptions)!;
            var updated = worker with { Status = status };
            await _database.HashSetAsync(_keys.Workers, workerIndex.ToString(), JsonSerializer.Serialize(updated, _jsonOptions));
        }
    }

    public async Task BroadcastCancellationAsync(string reason, CancellationToken cancellationToken)
    {
        var signal = new CancellationSignal(reason, DateTimeOffset.UtcNow);
        var json = JsonSerializer.Serialize(signal, _jsonOptions);
        await _database.StringSetAsync(_keys.Cancellation, json, _keyExpiration);
        await _subscriber.PublishAsync(RedisChannel.Literal(_keys.CancellationChannel), json);
    }

    public async Task<CancellationSignal?> IsCancellationRequestedAsync(CancellationToken cancellationToken)
    {
        var value = await _database.StringGetAsync(_keys.Cancellation);
        if (value.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<CancellationSignal>(value.ToString(), _jsonOptions);
    }
}
