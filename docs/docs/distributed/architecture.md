---
title: Architecture
sidebar_position: 6
---

# Distributed Architecture

This page describes the internal architecture of distributed mode for contributors, coordinator authors,
and advanced users.

## Execution Flow

### Master Startup

1. `AddDistributedMode` enables distributed services and configures `DistributedOptions`.
2. While the pipeline is built, `PipelineBuilder` activates distributed mode whenever
   `AddDistributedMode` was called. `TotalInstances` describes topology; it is not a
   second activation switch. Building fails when more than one coordinator backend is registered.
3. `RoleDetector` honors an explicit `DistributedOptions.Role`. With the default `Auto`
   role, `InstanceIndex == 0` selects master and any other index selects worker. The
   master selects `DistributedModuleExecutor` as the execution backend. Selecting a backend
   fails when the distributed options are invalid, or when `TotalInstances` is above one but only
   the built-in process-local coordinator is registered.
4. A registered `IDistributedCoordinatorFactory` is wrapped in a deferred coordinator,
   so `CreateMasterAsync` or `CreateWorkerAsync` runs when that role's coordinator is
   first used. The wrapper forwards every operation and disposes the created coordinator
   with the host. Directly registered role-specific coordinators are used as-is.
5. Before scheduling work, the master registers module types for serialization. Dispatch
   starts immediately by default; `DistributedOptions.MinimumWorkerCount` can opt into a
   startup barrier. An assignment the master cannot run itself is published only once a
   matching worker has registered, or fails with a routing error after `WorkerRegistrationTimeout`.

### Worker Startup

1. `RoleDetector` selects `Worker` explicitly, or derives it from a non-zero
   `DistributedOptions.InstanceIndex` when `Role` is `Auto`. Workers select
   `WorkerModuleExecutor` as the execution backend. Each process identifies itself with the
   `WorkerId` `instance-{InstanceIndex}`, so every process must configure a distinct index;
   a second live registration with the same identifier is rejected.
2. The worker registers all available module types for serialization.
3. The worker builds its capability set from configured capabilities and the
   auto-detected operating-system capability.
4. The worker registers its identity, run ID, capabilities and maximum parallelism via
   `RegisterWorkerAsync`. Periodic `SendHeartbeatAsync` calls carry `WorkerStatus` with the
   modules the worker is executing, which renews their leases. During run-report finalization,
   a final status (`IsFinal`) adds command metrics without replacing the registration.
5. The worker starts a bounded execution pool. It acquires an execution slot before it claims
   an assignment, so it never holds a claimed lease it cannot start.

### Module Execution (Master Side)

```
Build dependency graph
        │
        ├──► Start master worker loop and lease maintenance (concurrent)
        │
        ▼
For each ready module:
        │
        ▼
Create ModuleAssignment ──► no capable worker? ──► wait, then routing error
        │
        ▼
Enqueue to coordinator
        │
        ▼
Wait for the final result; the backstop deadline starts when a worker claims it
        │
        ▼
Deserialize result, record its artifact references
Mark module complete/failed
Schedule dependents
```

The master runs the same bounded worker pool and competes with external workers for assignments. Its
lease maintenance loop runs every `WorkerHeartbeatInterval`: it renews the master's own leases, returns
expired leases to the queue with `RequeueExpiredLeasesAsync`, and starts the result deadline of every module
that `GetActiveLeasesAsync` reports as claimed.

When a module fails, the master immediately broadcasts `DistributedCancellationReason.PipelineFailed`,
withdraws queued non-AlwaysRun assignments with `WithdrawAssignmentAsync`, and records them as cancelled.
AlwaysRun modules keep running on the workers that hold them, and late AlwaysRun modules are still
dispatched. When the host stops, the master broadcasts `Stopped`. It always signals completion last.

### Module Execution (Worker Side)

```
Register with coordinator
        │
        ▼
   ┌──► Acquire execution slot
   │         │
   │    Claim compatible lease
   │         │
   │    Download consumed artifacts by reference, execute
   │         │
   │    Upload produced artifacts (a failed upload or missing required artifact fails the module)
   │         │
   │    Publish result and release the lease
   │         │
   └─────────┘ (loop)
```

Workers execute up to the pipeline's global `Concurrency.MaxParallelism` setting. Each
node can lower its own limit with `DistributedOptions.MaxParallelism`; it cannot raise
the global limit. Workers stop when `DequeueModuleAsync` returns `null`, which coordinators
do after the master calls `SignalCompletionAsync` or broadcasts `Stopped`.

After a `PipelineFailed` broadcast, a worker cancels the non-AlwaysRun modules it is executing
and keeps claiming, because coordinators then return only AlwaysRun leases. After `Stopped`, it
cancels everything.

`ExecutionHint` throttles and `[ParallelLimiter]` semaphores remain process-local. They
bound concurrency within each node, but do not coordinate a shared limit across nodes.

### Timeouts

The worker enforces each module's per-attempt timeout exactly as standalone execution does. The master only
keeps a backstop for a stalled worker: when a worker claims the module, the master allows every configured
attempt at the module's timeout (or `PipelineOptions.DefaultModuleTimeout`) plus
`DistributedOptions.ModuleResultTimeout`. Queued, unclaimed work does not time out; it waits for capacity.
On expiry the master withdraws the assignment, records `TimedOut`, and publishes that failure as the final
result, so a late worker result is ignored.

### Artifacts

A producer's result lists the `ArtifactReference` values it uploaded. The master records the references of
every accepted result and includes the producers' references in each consumer's assignment, and workers record
the references of the dependency results they fetch. Consumers download exactly those references instead of
the newest upload in the store, whose timestamps come from different machines. The master also tells each
producer which of its artifacts planned consumers without run conditions require; the worker fails the module
when one of them is not produced, matching standalone execution.

## Custom Execution Backends

`IExecutionBackend` is the public orchestration seam. It receives one `ExecutionBackendRequest` with the
planned modules, their historical duration estimates keyed by `ModuleId` (used to prioritise scheduling),
and an `IExecutionBackendContext`, plus the pipeline cancellation token. A backend can submit
modules to external processes and apply their results, or call
`context.ExecuteModuleAsync(module, cancellationToken)` to execute a planned module in
this process. The public method uses the same engine runner as the built-in local backend,
including dependency waits, concurrency constraints, dependency-injection scopes, hooks,
retries, artifact handling, logging, and secret masking.

Set `OwnsEntirePlan` to `true` when the backend is responsible for completing
every planned module; before returning, such a backend must supply every result either in
its returned result list or through `IExecutionBackendContext.TryApplyResult`. A partial
backend sets `OwnsEntirePlan` to `false` and supplies only the results owned by that process.
Applying a result through the context immediately completes the local module awaitable,
allowing dependent work to observe remotely produced results.

An in-process backend can request all planned modules concurrently. The engine waits for
their dependencies and enforces module constraints and `Concurrency.MaxParallelism`.
Dependency waits do not occupy execution slots:

```csharp
public sealed class MyExecutionBackend : IExecutionBackend
{
    public bool OwnsEntirePlan => true;

    public async Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
        ExecutionBackendRequest request,
        CancellationToken cancellationToken)
    {
        return await Task.WhenAll(request.Modules.Select(module =>
            request.Context.ExecuteModuleAsync(module, cancellationToken)));
    }
}
```

Use the exact module instances and context supplied to the backend. Request execution of
each dependency, or apply its remote result, before awaiting a dependent module alone.
Concurrent requests for one module share its execution; the first request's cancellation
token controls the execution, and later callers can cancel their own waits. Await all
requests before returning. A completed request includes module hooks and scope disposal,
and failures follow the pipeline's configured failure policy. Do not apply a remote result
to a module whose local execution is still in progress.

When an `AlwaysRun` request uses the pipeline token supplied to the backend, unrelated
failures and failed dependencies do not cancel that request. User cancellation still
applies. Other request tokens, including tokens linked by the backend, are honored
directly. Use a separate request token when you need to cancel an individual module.

Register a custom backend before building the pipeline:

```csharp
builder.AddExecutionBackend<MyExecutionBackend>();
```

Explicit custom registrations take precedence over automatic local, distributed-master,
and distributed-worker backend selection.

## Coordinator Contract

`IDistributedWorkerCoordinator` defines the six operations available to workers.
`IDistributedMasterCoordinator` inherits that contract and adds the master operations,
so the master can also execute modules locally. Every built-in backend runs the shared
`DistributedCoordinatorContract` tests, and custom coordinators should satisfy the same behaviour.

### Worker Operations

| Method | Contract |
|--------|----------|
| `RegisterWorkerAsync` | Registers a worker session. Repeating the same `WorkerId` and `RegisteredAt` is a reconnect; a different `RegisteredAt` for a live `WorkerId` throws `InvalidOperationException`. |
| `DequeueModuleAsync` | Claims the best compatible assignment for a `WorkerId` and returns its `ModuleLease`. Returns `null` after completion or a `Stopped` broadcast, only AlwaysRun leases after `PipelineFailed`, and throws `OperationCanceledException` when cancelled. |
| `PublishResultAsync` | Stores a result and releases its lease. The first result for a module is final; later ones are ignored. |
| `WaitForResultAsync` | Waits for a module's final result; workers use it to load dependency results. |
| `SendHeartbeatAsync` | Records `WorkerStatus` and renews the leases on its `InFlightModules` that belong to that worker. |
| `WaitForCancellationAsync` | Waits for a cancellation broadcast and returns its `DistributedCancellationReason`. |

### Additional Master Operations

| Method | Contract |
|--------|----------|
| `EnqueueModuleAsync` | Adds an assignment to the queue. |
| `WithdrawAssignmentAsync` | Removes a queued, unclaimed assignment; returns whether one was removed. |
| `GetActiveLeasesAsync` | Returns the leases workers hold. |
| `RequeueExpiredLeasesAsync` | Queues again every assignment whose lease was not renewed for `WorkerTimeout` and has no result. |
| `GetRegisteredWorkersAsync` | Returns workers with live heartbeats or a final status. |
| `GetWorkerStatusesAsync` | Returns the latest status for each worker, including final command metrics. |
| `SignalCompletionAsync` | Tells workers that no more assignments will arrive. |
| `BroadcastCancellationAsync` | Broadcasts a cancellation reason. The first reason is durable; workers that start waiting later observe it. |

Claims prefer, in order: higher `ModulePriority`, assignments fewer live workers can run, more specific
capability requirements, longer critical paths, and earlier enqueue order.

### Evolution Policy

Coordinator, discovery and artifact-store interfaces evolve through default interface members. A member
added in a 4.x minor release ships with a default implementation, so existing implementations keep compiling
and working; members are only removed or changed in a major release. Wire types (`ModuleAssignment`,
`ModuleLease`, `SerializedModuleResult`, `WorkerRegistration`, `WorkerStatus`, `ArtifactDescriptor`,
`ArtifactReference`, `DependencyResultReference`, `MasterEndpoint`) are records with `required` and `init`
properties, so later versions can add optional members without breaking payloads or callers.

Coordinators that hold connections should implement `IAsyncDisposable`: the pipeline disposes the coordinator
it created, directly or through a factory, when the host shuts down.

## Redis Implementation Details

The `RedisDistributedCoordinator` keeps the queue in a sorted set and leases in a hash, and runs each
multi-step operation as one Lua script:

| Method | Redis operations |
|--------|-----------------|
| `EnqueueModuleAsync` | `ZADD` to the queue, refresh key expiry, `PUBLISH` work-available |
| `DequeueModuleAsync` | Claim script: check completion and cancellation, drop copies that already have a result or lease, pick the best compatible item, `ZREM` it and `HSET` its lease. Waits on per-call pub/sub queues with a two-second bound between claims. |
| `PublishResultAsync` | `HSETNX` the result and `HDEL` the lease in one script, refresh expiry, `PUBLISH` the result channel |
| `WaitForResultAsync` | `HGET`, then subscribe and re-read the hash after every notification or two-second poll |
| `RegisterWorkerAsync` | Script: reject a live duplicate session, store registration, status and heartbeat |
| `SendHeartbeatAsync` | Store status, then a script refreshes the heartbeat and extends the worker's own leases |
| `WithdrawAssignmentAsync` | Script: `ZREM` queued copies of the module |
| `RequeueExpiredLeasesAsync` | Script: remove expired leases and `ZADD` their assignments unless a result exists, then refresh key expiry |
| `SignalCompletionAsync` / `BroadcastCancellationAsync` | `SET` the flag (the cancellation reason with `NX`) and `PUBLISH` |

Every call observes its cancellation token; because Redis cannot cancel a sent command, waits are bounded
with `WaitAsync` and later commands are not issued.

## SignalR Implementation Details

The SignalR master hosts the in-memory coordinator behind `DistributedPipelineHub`. Workers invoke hub
methods for every operation: `RegisterWorker`, `Heartbeat`, `DequeueModule`, `PublishResult`,
`WaitForResult` and `WaitForCancellation`. The master never pushes to workers, so a worker that reconnects
re-registers its session and retries its interrupted invocations; its heartbeats renew the leases it still
holds, and leases of a worker that does not come back expire and are requeued. When the connection closes for
good, `DequeueModuleAsync` returns `null` and the worker stops.

Security: when the master is reachable beyond the machine, connections must present the access token as a
bearer token (or the `access_token` query parameter used by WebSockets); the master compares it in constant
time and rejects the request with `401` otherwise. Registrations must carry the master's `RunId`, and only
the registered worker may publish its results and heartbeats.

## Serialization

Module results are serialized via `ModuleResultSerializer` using `System.Text.Json`. The `ModuleTypeRegistry`
maintains a mapping from module identifiers to their concrete .NET types, so results can be deserialized back
to the correct `ModuleResult<T>`.

Exceptions carry their type name, message, stack trace, inner exception chain and, for `AggregateException`,
every inner exception. Deserialization only activates types from the `System` namespaces and an allowlist of
ModularPipelines exceptions whose data travels with them (`ModuleTimeoutException`, `ModuleFailedException`,
`PipelineException`, `RequirementNotMetException`, `PipelineCanceledException`); any other type becomes a
`RemoteModuleException` that keeps the original diagnostics and the worker's `WorkerId`.

`SerializedModuleResult.Payload` contains the serialized result. Assignments carry
`DependencyResultReference` entries; workers fetch those results from the coordinator's
result store rather than embedding dependency payloads in each assignment. Condition groups the master already
evaluated travel as version-independent type names (`Namespace.Type, AssemblyName`).

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
    public Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<bool> WithdrawAssignmentAsync(ModuleId moduleId, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<IReadOnlyList<ModuleLease>> GetActiveLeasesAsync(CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<IReadOnlyList<ModuleId>> RequeueExpiredLeasesAsync(CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<ModuleLease?> DequeueModuleAsync(
        WorkerId workerId,
        IReadOnlySet<Capability> workerCapabilities,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task PublishResultAsync(
        SerializedModuleResult result,
        ModuleLease? lease,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<SerializedModuleResult> WaitForResultAsync(ModuleId moduleId, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<IReadOnlyList<WorkerStatus>> GetWorkerStatusesAsync(CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task SignalCompletionAsync(CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task BroadcastCancellationAsync(
        DistributedCancellationReason reason,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<DistributedCancellationReason> WaitForCancellationAsync(CancellationToken cancellationToken) =>
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
    public Task<IDistributedMasterCoordinator> CreateMasterAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IDistributedMasterCoordinator>(new MyCustomCoordinator());

    public Task<IDistributedWorkerCoordinator> CreateWorkerAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IDistributedWorkerCoordinator>(new MyCustomCoordinator());
}

builder.AddDistributedCoordinatorFactory<MyCoordinatorFactory>();
```

## Worker Liveness and Final Metrics

`WorkerRegistration` contains the worker's session identity, run ID, capabilities and parallelism. Liveness and
command metrics arrive through `SendHeartbeatAsync(WorkerStatus, ...)`; the master reads the latest statuses
with `GetWorkerStatusesAsync`. A status with `IsFinal` carries the worker's final command metrics and keeps its
registration visible after its heartbeat expires, so the master can collect metrics from workers that have
already stopped. Custom coordinators must keep heartbeat timing separate from the registration timestamp.

## Cancellation and Completion

`BroadcastCancellationAsync` signals cancellation to distributed workers, which observe it through
`WaitForCancellationAsync`. `PipelineFailed` cancels non-AlwaysRun work only; `Stopped` cancels everything.
Local cancellation tokens still bound individual operations.

`SignalCompletionAsync` is different from cancellation. The master calls it in a `finally` block after
distributed execution ends, and coordinators then return `null` from pending and later `DequeueModuleAsync`
calls, so workers exit even when they missed a notification. If the runnable set is empty, the master returns
before sending the signal, so external workers remain blocked until their local cancellation tokens are
canceled.
