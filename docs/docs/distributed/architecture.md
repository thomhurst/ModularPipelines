---
title: Architecture
sidebar_position: 6
---

# Distributed Architecture

This page describes the internal architecture of distributed mode for contributors and advanced users.

## Upgrading SignalR configuration for v4

`SignalRDistributedOptions.MaximumReceiveMessageSize` is renamed to
`SignalRDistributedOptions.MaxReceiveMessageSize`. Update property assignments and
configuration keys to the new name. The value remains a byte count, with a default
of `1024 * 1024` (1 MB).

```csharp
// Before
options.MaximumReceiveMessageSize = 4 * 1024 * 1024;

// v4
options.MaxReceiveMessageSize = 4 * 1024 * 1024;
```

## Execution Flow

### Master Startup

1. `AddDistributedMode` enables distributed services and configures `DistributedOptions`.
2. While the pipeline is built, `PipelineBuilder` activates distributed mode whenever
   `AddDistributedMode` was called. `TotalInstances` describes topology; it is not a
   second activation switch.
3. `RoleDetector` honors an explicit `DistributedOptions.Role`. With the default `Auto`
   role, `InstanceIndex == 0` selects master and any other index selects worker. The
   master selects `DistributedModuleExecutor` as the execution backend.
4. A registered `IDistributedCoordinatorFactory` is wrapped in a deferred coordinator,
   so `CreateMasterAsync` or `CreateWorkerAsync` runs when that role's coordinator is
   first used. Directly registered role-specific coordinators are used as-is.
5. Before scheduling work, the master registers module types for serialization. Dispatch
   starts immediately by default; `DistributedOptions.MinimumWorkerCount` can opt into a
   startup barrier. Capability-restricted assignments wait only until a matching worker
   registers or `CapabilityTimeout` expires.

### Worker Startup

1. `RoleDetector` selects `Worker` explicitly, or derives it from a non-zero
   `DistributedOptions.InstanceIndex` when `Role` is `Auto`. Workers select
   `WorkerModuleExecutor` as the execution backend. Registration and run-report metrics use
   `DistributedOptions.InstanceIndex`, so every worker must configure a distinct index.
2. The worker registers all available module types for serialization.
3. The worker builds its capability set from configured capabilities and, by default,
   the auto-detected operating-system capability.
4. The worker registers its identity, run ID, and capabilities via `RegisterWorkerAsync`.
   Periodic `SendHeartbeatAsync` calls carry `WorkerStatus`. During run-report
   finalization, a final status adds command metrics without replacing registration.
5. The worker starts a bounded execution pool, continuously dequeuing one assignment
   ahead while up to the configured number of modules execute concurrently.

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

The master runs the same bounded worker pool and competes with external workers for
assignments. All modules go through the work queue — routing is purely capability-based.

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

Workers execute up to the pipeline's global `Concurrency.MaxParallelism` setting. Each
node can lower its own limit with `DistributedOptions.MaxParallelism`; it cannot raise
the global limit. The pool keeps one dequeue pending while all execution slots are busy,
avoiding idle time between assignments. Workers stop when `DequeueModuleAsync` returns
`null`. Coordinators return `null` after the master calls `SignalCompletionAsync`, or
when that worker's local cancellation token is canceled.

`ExecutionHint` throttles and `[ParallelLimiter]` semaphores remain process-local. They
bound concurrency within each node, but do not coordinate a shared limit across nodes.

## Custom Execution Backends

`IExecutionBackend` is the public orchestration seam. It receives the planned modules,
their historical duration estimates keyed by module type (used to prioritise scheduling),
an `IExecutionBackendContext`, and the pipeline cancellation token. A backend supplies
results: it can submit modules to an external scheduler or remote processes and return the
results they produce, or apply results it computed by other means. The engine's own module
runner (dependency-injection scopes, hooks, artifact handling, retries and logging) is not
public, so a custom backend cannot drive a module through that lifecycle in-process; the
built-in local and distributed backends remain the only ones that execute modules
themselves. Set `OwnsEntirePlan` to `true` when the backend is responsible for completing
every planned module; before returning, such a backend must supply every result either in
its returned result list or through `IExecutionBackendContext.TryApplyResult`. A partial
backend sets `OwnsEntirePlan` to `false` and supplies only the results owned by that process.
Applying a result through the context immediately completes the local module awaitable,
allowing dependent work to observe remotely produced results.

Register a custom backend before building the pipeline:

```csharp
builder.AddExecutionBackend<MyExecutionBackend>();
```

Explicit custom registrations take precedence over automatic local, distributed-master,
and distributed-worker backend selection.

Capability-mismatch handling is transport-specific. The in-memory and Redis coordinators
scan their lists and leave incompatible assignments in place. The queue-backed SignalR
master coordinator dequeues each candidate, re-enqueues an incompatible assignment, and
continues scanning for work that the current worker can execute.

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
| `GetRegisteredWorkersAsync` | Master ← Coordinator | Returns registered workers for an optional startup barrier, capability-route validation, and post-execution worker metrics. |

### Completion

| Method | Direction | Description |
|--------|-----------|-------------|
| `SignalCompletionAsync` | Master → All | Tells waiting workers that the run has finished and no more assignments will arrive. |

## Redis Implementation Details

The `RedisDistributedCoordinator` maps each method to Redis operations:

| Method | Redis Operations |
|--------|-----------------|
| `EnqueueModuleAsync` | `LPUSH` to the work queue + `EXPIRE` + `PUBLISH` on the work-available channel |
| `DequeueModuleAsync` | `GET` completion flag (check first), `SUBSCRIBE` to work/completion channels, atomically scan `LRANGE` and claim a capability-compatible item with `LREM`, then `GET` completion again (close race window) |
| `PublishResultAsync` | `HSET` on results hash + `EXPIRE` + `PUBLISH` on the module result channel |
| `WaitForResultAsync` | `HGET` results hash (check first), then `SUBSCRIBE` result channel, then `HGET` again (close race window), await message |
| `RegisterWorkerAsync` | `HSET` on workers hash + `EXPIRE` |
| `GetRegisteredWorkersAsync` | `HGETALL` on workers hash |
| `SignalCompletionAsync` | `SET` completion key, separate `EXPIRE`, then `PUBLISH` completion channel |

### WaitForResultAsync Race Condition Handling

The `WaitForResultAsync` method uses a check-subscribe-recheck pattern to avoid a race where a result is published between the initial check and the subscription:

1. `HGET` the results hash — if the result already exists, return immediately.
2. `SUBSCRIBE` to the result channel.
3. `HGET` again — if the result arrived between step 1 and 2, return it.
4. Await the Pub/Sub message.

This guarantees no result is missed regardless of timing.

## Serialization

Module results are serialized via `ModuleResultSerializer` using `System.Text.Json`. The `ModuleTypeRegistry` maintains a mapping from module type names to their concrete .NET types, so results can be deserialized back to the correct `ModuleResult<T>`.

Capability collections on `ModuleAssignment` and `WorkerRegistration` are lists, so the default JSON serializer writes them as plain string arrays on the wire.

`SerializedModuleResult.Payload` contains the serialized result. Assignments carry
`DependencyResultReference` entries; workers fetch those results from the coordinator's
result store rather than embedding dependency payloads in each assignment.

## Implementing a Custom Coordinator

To implement a different transport (HTTP, shared filesystem, message queue, etc.), implement
`IDistributedMasterCoordinator` and optionally `IDistributedCoordinatorFactory`.
The master interface includes `IDistributedWorkerCoordinator` because the master also
executes assignments. A factory can return a separate worker-only implementation.

```csharp
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Distributed;

public sealed class MyCustomCoordinator : IDistributedMasterCoordinator
{
    public Task EnqueueModuleAsync(
        ModuleAssignment assignment,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<ModuleAssignment?> DequeueModuleAsync(
        IReadOnlySet<Capability> workerCapabilities,
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

    public Task SendHeartbeatAsync(
        WorkerStatus status,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<IReadOnlyList<WorkerStatus>> GetWorkerStatusesAsync(
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task SignalCompletionAsync(CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task BroadcastCancellationAsync(CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task WaitForCancellationAsync(CancellationToken cancellationToken) =>
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
    public Task<IDistributedMasterCoordinator> CreateMasterAsync(
        CancellationToken cancellationToken) =>
        Task.FromResult<IDistributedMasterCoordinator>(new MyCustomCoordinator());

    public Task<IDistributedWorkerCoordinator> CreateWorkerAsync(
        CancellationToken cancellationToken) =>
        Task.FromResult<IDistributedWorkerCoordinator>(new MyCustomCoordinator());
}

builder.AddDistributedCoordinatorFactory<MyCoordinatorFactory>();
```

## Worker Liveness and Final Metrics

`WorkerRegistration` contains immutable identity, run ID, and capabilities. Liveness and
command metrics arrive through `SendHeartbeatAsync(WorkerStatus, ...)`; the master reads
the latest statuses with `GetWorkerStatusesAsync`. Coordinators retain final metrics for
post-execution reads, including after a worker disconnects or its heartbeat expires.

`GetRegisteredWorkersAsync` returns workers with live heartbeats or retained final metrics.
Scheduling considers live workers when checking capability routes. After `CapabilityTimeout`,
the master fails a queued assignment if neither a suitable worker nor the master can execute it.
Custom coordinators must keep heartbeat timing separate from the worker's registration timestamp.

If a worker disappears after claiming an assignment, the master can wait until
`ModuleResultTimeout` (45 minutes by default) for that assignment's result. SignalR can react
to connection state internally, but that behavior is not part of the shared coordinator
contract.

## Cancellation and Completion

`BroadcastCancellationAsync` signals cancellation to distributed workers, which observe it
through `WaitForCancellationAsync`. Local cancellation tokens still bound individual operations.

`SignalCompletionAsync` is different from cancellation. When the master receives at least
one runnable module, it calls this method in a `finally` block after distributed execution
ends. Coordinators use that signal to wake workers blocked in `DequeueModuleAsync` and let
their execution loops exit normally. If the runnable set is empty, the master currently
returns before sending the signal, so external workers remain blocked until their local
cancellation tokens are canceled.
