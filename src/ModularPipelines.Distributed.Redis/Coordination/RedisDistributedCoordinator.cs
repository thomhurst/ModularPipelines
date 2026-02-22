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

    public async Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<string> workerCapabilities, CancellationToken cancellationToken)
    {
        // Check if completion was already signalled before subscribing
        var completionFlag = await _database.StringGetAsync(_keys.CompletionFlag);
        if (!completionFlag.IsNullOrEmpty)
        {
            return null;
        }

        // Subscribe to work-available and completion notifications
        using var signal = new SemaphoreSlim(0);
        var completed = false;
        var workChannel = RedisChannel.Literal(_keys.WorkAvailableChannel);
        var completionChannel = RedisChannel.Literal(_keys.CompletionChannel);

        await _subscriber.SubscribeAsync(workChannel, (_, _) => signal.Release());
        await _subscriber.SubscribeAsync(completionChannel, (_, _) =>
        {
            completed = true;
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

                if (completed)
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

    public async Task SignalCompletionAsync(CancellationToken cancellationToken)
    {
        await _database.StringSetAsync(_keys.CompletionFlag, "1");
        await _database.KeyExpireAsync(_keys.CompletionFlag, _keyExpiration);
        await _subscriber.PublishAsync(RedisChannel.Literal(_keys.CompletionChannel), "1");
    }

    private async Task<ModuleAssignment?> TryScanAndClaimAsync(IReadOnlySet<string> workerCapabilities)
    {
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

        return null;
    }
}
