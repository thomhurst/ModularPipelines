---
title: Architecture
sidebar_position: 6
---

# Distributed Architecture

This page describes the internal architecture of distributed mode for contributors and advanced users.

## Execution Flow

### Master Startup

1. `AddDistributedMode` enables distributed services and configures `DistributedOptions`.
2. While the pipeline is built, `PipelineBuilder` activates distributed mode when
   `Enabled` is `true` and `TotalInstances` is greater than one.
3. `RoleDetector` checks `MODULAR_PIPELINES_INSTANCE` first. A valid environment value
   of `0` selects the master even when `DistributedOptions.InstanceIndex` is non-zero;
   any other valid integer selects a worker. When the variable is absent or invalid,
   `InstanceIndex == 0` selects the master. The master replaces the default
   `IModuleExecutor` with `DistributedModuleExecutor`. The environment variable affects
   role selection only; it does not change `DistributedOptions.InstanceIndex`.
4. A registered `IDistributedCoordinatorFactory` is wrapped in a deferred coordinator,
   so its `CreateAsync` method runs when the coordinator is first used. A directly
   registered `IDistributedCoordinator` is used as-is.
5. Before scheduling work, the master registers module types for serialization and waits
   up to `DistributedOptions.CapabilityTimeout` for the configured workers to register.

### Worker Startup

1. `RoleDetector` checks `MODULAR_PIPELINES_INSTANCE` before
   `DistributedOptions.InstanceIndex`. A valid non-zero environment value selects a
   worker, while `0` selects the master even when `InstanceIndex` is non-zero. Without
   a valid override, every non-zero `InstanceIndex` is a worker. Workers replace the
   default `IModuleExecutor` with `WorkerModuleExecutor`. Registration and run-report
   metrics still use `DistributedOptions.InstanceIndex`, so every worker must configure
   a distinct non-zero index even when the environment variable selects its role.
2. The worker registers all available module types for serialization.
3. The worker builds its capability set from configured capabilities and, by default,
   the auto-detected operating-system capability.
4. The worker registers its capabilities with the coordinator via
   `RegisterWorkerAsync`. During run-report finalization it calls the method again to
   upsert its final command metrics.
5. The worker enters its dequeue/execute/publish loop.

### Module Execution (Master Side)

```
Build dependency graph
        │
        ├──► Start master worker loop (concurrent)
        │         │
        │    Dequeue from queue
        │    Execute locally
        │    Publish result
        │         │
        │         └──► (loop)
        │
        ▼
For each ready module:
        │
        ▼
Create ModuleAssignment
        │
        ▼
Enqueue to coordinator
        │
        ▼
Wait for result (from any worker, including master)
        │
        ▼
Deserialize result
Mark module complete/failed
Schedule dependents
```

The master runs a concurrent worker loop that competes with external workers for assignments. All modules go through the work queue — routing is purely capability-based.

### Module Execution (Worker Side)

```
Register with coordinator
        │
        ▼
   ┌──► Dequeue compatible assignment
   │         │
   │      Execute
   │         │
   │         ▼
   │  Serialize result
   │    │
   │    ▼
   │  Publish to coordinator
   │    │
   └────┘ (loop)
```

Workers loop until `DequeueModuleAsync` returns `null`. Coordinators return `null` after
the master calls `SignalCompletionAsync`, or when that worker's local cancellation token
is canceled.

## Coordinator Interface

The shipped `IDistributedCoordinator` interface defines seven methods across four concerns:

### Work Queue

| Method | Direction | Description |
|--------|-----------|-------------|
| `EnqueueModuleAsync` | Master → Queue | Pushes a module assignment onto the work queue. |
| `DequeueModuleAsync` | Queue → Worker | Waits for and claims an assignment compatible with the worker's capabilities, or returns `null` after completion. |

### Results

| Method | Direction | Description |
|--------|-----------|-------------|
| `PublishResultAsync` | Worker → Coordinator | Stores the serialized result and notifies waiters. |
| `WaitForResultAsync` | Master ← Coordinator | Blocks until a specific module's result is available. |

### Worker Management

| Method | Direction | Description |
|--------|-----------|-------------|
| `RegisterWorkerAsync` | Worker → Coordinator | Upserts a worker's index, capabilities, registration time, and run identifier; workers call it again after execution with final command metrics. |
| `GetRegisteredWorkersAsync` | Master ← Coordinator | Returns registered workers during the startup wait and again after execution while the master aggregates final worker metrics. |

### Completion

| Method | Direction | Description |
|--------|-----------|-------------|
| `SignalCompletionAsync` | Master → All | Tells waiting workers that the run has finished and no more assignments will arrive. |

## Redis Implementation Details

The `RedisDistributedCoordinator` maps each method to Redis operations:

| Method | Redis Operations |
|--------|-----------------|
| `EnqueueModuleAsync` | `LPUSH` to the work queue + `EXPIRE` + `PUBLISH` on the work-available channel |
| `DequeueModuleAsync` | Subscribe to work/completion channels; atomically scan `LRANGE` and claim a capability-compatible item with `LREM` |
| `PublishResultAsync` | `HSET` on results hash + `EXPIRE` + `PUBLISH` on the module result channel |
| `WaitForResultAsync` | `HGET` results hash (check first), then `SUBSCRIBE` result channel, then `HGET` again (close race window), await message |
| `RegisterWorkerAsync` | `HSET` on workers hash + `EXPIRE` |
| `GetRegisteredWorkersAsync` | `HGETALL` on workers hash |
| `SignalCompletionAsync` | `SET` completion key with TTL + `PUBLISH` completion channel |

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
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Extensions;

public sealed class MyCustomCoordinator : IDistributedCoordinator
{
    public Task EnqueueModuleAsync(
        ModuleAssignment assignment,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<ModuleAssignment?> DequeueModuleAsync(
        IReadOnlySet<string> workerCapabilities,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task PublishResultAsync(
        SerializedModuleResult result,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<SerializedModuleResult> WaitForResultAsync(
        string moduleTypeName,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task RegisterWorkerAsync(
        WorkerRegistration registration,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task SignalCompletionAsync(CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
```

Register it directly:

```csharp
builder.AddDistributedCoordinator<MyCustomCoordinator>();
```

Or via a factory for async initialization:

```csharp
public sealed class MyCoordinatorFactory : IDistributedCoordinatorFactory
{
    public Task<IDistributedCoordinator> CreateAsync(
        CancellationToken cancellationToken) =>
        Task.FromResult<IDistributedCoordinator>(new MyCustomCoordinator());
}

builder.AddDistributedCoordinatorFactory<MyCoordinatorFactory>();
```

## Current Liveness Limitations

Worker registration is not a heartbeat. Workers upsert an initial capability record and later
upsert final command metrics during run-report finalization. The master reads registrations
while waiting for the expected worker count and polls them again after execution to aggregate
those final metrics. A custom coordinator must therefore retain registration state for the
whole run and support repeated upserts and post-execution reads.

The shipped coordinator contract still has no heartbeat, unregister, or worker-health member.
After `CapabilityTimeout`, the master proceeds with the workers that registered; the later
metrics polling reports completion data but does not provide continuous liveness detection.

If a worker disappears after claiming an assignment, the master can wait until
`ModuleResultTimeout` (45 minutes by default) for that assignment's result. SignalR can react
to connection state internally, but that behavior is not part of the shared coordinator
contract. First-class liveness is tracked by
[#4373](https://github.com/thomhurst/ModularPipelines/issues/4373).

## Cancellation and Completion

Cancellation tokens stop work only in the process where cancellation is requested. The
shipped coordinator contract does not broadcast cancellation between the master and workers.

`SignalCompletionAsync` is different from cancellation. When the master receives at least
one runnable module, it calls this method in a `finally` block after distributed execution
ends. Coordinators use that signal to wake workers blocked in `DequeueModuleAsync` and let
their execution loops exit normally. If the runnable set is empty, the master currently
returns before sending the signal, so external workers remain blocked until their local
cancellation tokens are canceled. First-class distributed cancellation is also tracked by
[#4373](https://github.com/thomhurst/ModularPipelines/issues/4373).
