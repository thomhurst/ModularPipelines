---
title: Architecture
sidebar_position: 6
---

# Distributed Architecture

This page describes the internal architecture of distributed mode for contributors and advanced users.

## Execution Flow

### Master Startup

1. The `DistributedPipelinePlugin` detects role based on `InstanceIndex` and registers master services.
2. The default `IModuleExecutor` is replaced with `DistributedModuleExecutor`.
3. All module types are registered in the `ModuleTypeRegistry` for serialization.
4. The coordinator is initialized (via `IDistributedCoordinatorFactory.CreateAsync()` or directly).
5. The `WorkerHealthMonitor` background service starts monitoring worker heartbeats.

### Worker Startup

1. The plugin detects the worker role and registers worker services.
2. The default `IModuleExecutor` is replaced with `WorkerModuleExecutor`.
3. The worker builds its capability set (configured capabilities + auto-detected OS).
4. The worker registers with the coordinator via `RegisterWorkerAsync`.
5. Two background services start: `WorkerHeartbeatService` (periodic heartbeats) and `WorkerCancellationMonitor` (polls for cancellation).

### Module Execution (Master Side)

```
Build dependency graph
        │
        ▼
For each ready module:
        │
   ┌────┴─────────────────┐
   │ [PinToMaster]?       │
   │                      │
   ▼ Yes                  ▼ No
Execute locally     Create ModuleAssignment
   │                      │
   │                      ▼
   │                Enqueue to coordinator
   │                      │
   │                      ▼
   │                Wait for result
   │                      │
   └──────────┬───────────┘
              ▼
   Deserialize result
   Mark module complete/failed
   Schedule dependents
```

### Module Execution (Worker Side)

```
Register with coordinator
        │
        ▼
   ┌──► Dequeue assignment ◄─────┐
   │         │                   │
   │    ┌────┴────┐              │
   │    │ Match?  │              │
   │    ▼ Yes     ▼ No          │
   │  Execute   Re-enqueue      │
   │    │         │              │
   │    ▼         └──────────────┘
   │  Serialize result
   │    │
   │    ▼
   │  Publish to coordinator
   │    │
   └────┘ (loop)
```

Workers loop until the queue is empty and no more work is expected, or until cancellation is requested.

## Coordinator Interface

The `IDistributedCoordinator` interface defines nine methods across four concerns:

### Work Queue

| Method | Direction | Description |
|--------|-----------|-------------|
| `EnqueueModuleAsync` | Master → Queue | Pushes a module assignment onto the work queue. |
| `DequeueModuleAsync` | Queue → Worker | Pops an assignment from the queue, checking capability match. Re-enqueues if the worker can't handle it. |

### Results

| Method | Direction | Description |
|--------|-----------|-------------|
| `PublishResultAsync` | Worker → Coordinator | Stores the serialized result and notifies waiters. |
| `WaitForResultAsync` | Master ← Coordinator | Blocks until a specific module's result is available. Uses Pub/Sub to avoid polling. |

### Worker Management

| Method | Direction | Description |
|--------|-----------|-------------|
| `RegisterWorkerAsync` | Worker → Coordinator | Registers a worker with its capabilities and status. |
| `SendHeartbeatAsync` | Worker → Coordinator | Updates the heartbeat timestamp. Transitions status from Connected to Active. |
| `GetRegisteredWorkersAsync` | Master ← Coordinator | Returns all registered workers (for health monitoring). |

### Cancellation

| Method | Direction | Description |
|--------|-----------|-------------|
| `BroadcastCancellationAsync` | Any → All | Stores a cancellation signal and notifies all instances. |
| `IsCancellationRequestedAsync` | Any ← Coordinator | Checks whether cancellation has been requested. |

## Redis Implementation Details

The `RedisDistributedCoordinator` maps each method to Redis operations:

| Method | Redis Operations |
|--------|-----------------|
| `EnqueueModuleAsync` | `LPUSH` to work queue list + `EXPIRE` |
| `DequeueModuleAsync` | `RPOP` from work queue (FIFO via LPUSH/RPOP), poll loop with configurable delay |
| `PublishResultAsync` | `HSET` on results hash + `PUBLISH` on result channel |
| `WaitForResultAsync` | `HGET` results hash (check first), then `SUBSCRIBE` result channel, then `HGET` again (close race window), await message |
| `RegisterWorkerAsync` | `HSET` on workers hash + `EXPIRE` |
| `SendHeartbeatAsync` | `HSET` on heartbeats hash + update worker status in workers hash |
| `GetRegisteredWorkersAsync` | `HGETALL` on workers hash |
| `BroadcastCancellationAsync` | `SET` cancellation key with TTL + `PUBLISH` cancellation channel |
| `IsCancellationRequestedAsync` | `GET` cancellation key |

### WaitForResultAsync Race Condition Handling

The `WaitForResultAsync` method uses a check-subscribe-recheck pattern to avoid a race where a result is published between the initial check and the subscription:

1. `HGET` the results hash — if the result already exists, return immediately.
2. `SUBSCRIBE` to the result channel.
3. `HGET` again — if the result arrived between step 1 and 2, return it.
4. Await the Pub/Sub message.

This guarantees no result is missed regardless of timing.

## Serialization

Module results are serialized via `ModuleResultSerializer` using `System.Text.Json`. The `ModuleTypeRegistry` maintains a mapping from module type names to their concrete .NET types, so results can be deserialized back to the correct `ModuleResult<T>`.

The `ReadOnlySetJsonConverter` handles `IReadOnlySet<string>` fields (used in `ModuleAssignment.RequiredCapabilities` and `WorkerRegistration.Capabilities`), which `System.Text.Json` cannot deserialize by default.

## Implementing a Custom Coordinator

To implement a different transport (HTTP, shared filesystem, message queue, etc.), implement `IDistributedCoordinator` and optionally `IDistributedCoordinatorFactory`:

```csharp
public class MyCustomCoordinator : IDistributedCoordinator
{
    public Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken ct) { ... }
    public Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<string> capabilities, CancellationToken ct) { ... }
    public Task PublishResultAsync(SerializedModuleResult result, CancellationToken ct) { ... }
    public Task<SerializedModuleResult> WaitForResultAsync(string moduleTypeName, CancellationToken ct) { ... }
    public Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken ct) { ... }
    public Task SendHeartbeatAsync(int workerIndex, CancellationToken ct) { ... }
    public Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken ct) { ... }
    public Task BroadcastCancellationAsync(string reason, CancellationToken ct) { ... }
    public Task<CancellationSignal?> IsCancellationRequestedAsync(CancellationToken ct) { ... }
}
```

Register it directly:

```csharp
builder.AddDistributedCoordinator<MyCustomCoordinator>();
```

Or via a factory for async initialization:

```csharp
public class MyCoordinatorFactory : IDistributedCoordinatorFactory
{
    public async Task<IDistributedCoordinator> CreateAsync(CancellationToken ct)
    {
        // Connect to your backend...
        return new MyCustomCoordinator(connection);
    }
}

builder.AddDistributedCoordinatorFactory<MyCoordinatorFactory>();
```

## Health Monitoring

The master runs a `WorkerHealthMonitor` background service that periodically checks worker heartbeats via `GetRegisteredWorkersAsync`. If a worker hasn't sent a heartbeat within `HeartbeatTimeoutSeconds`, the master considers it unresponsive.

Workers send heartbeats via the `WorkerHeartbeatService` at the interval configured in `HeartbeatIntervalSeconds`. The first heartbeat transitions a worker's status from `Connected` to `Active`.

## Cancellation

Either the master or a worker can broadcast a cancellation signal. The `WorkerCancellationMonitor` background service polls `IsCancellationRequestedAsync` every 2 seconds on each worker. When a signal is detected, it triggers the pipeline's `CancellationToken`, causing all in-progress modules to receive cancellation.
