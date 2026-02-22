# Interface Contracts: Distributed Coordination Layer

**Date**: 2026-02-22 | **Branch**: `001-distributed-workers`

## Overview

These are the public interfaces that users implement to provide a custom coordination transport (Redis, HTTP, shared filesystem, etc.). The framework ships with an `InMemoryDistributedCoordinator` for testing/development.

## IDistributedCoordinator

The primary interface. Users implement this and register it via DI.

```
IDistributedCoordinator
├── Work Queue
│   ├── EnqueueModuleAsync(assignment, ct) → Task
│   └── DequeueModuleAsync(capabilities, ct) → Task<ModuleAssignment?>
├── Results
│   ├── PublishResultAsync(result, ct) → Task
│   └── WaitForResultAsync(moduleTypeName, ct) → Task<SerializedModuleResult>
├── Worker Management
│   ├── RegisterWorkerAsync(registration, ct) → Task
│   ├── SendHeartbeatAsync(workerIndex, ct) → Task
│   └── GetRegisteredWorkersAsync(ct) → Task<IReadOnlyList<WorkerRegistration>>
└── Cancellation
    ├── BroadcastCancellationAsync(reason, ct) → Task
    └── IsCancellationRequestedAsync(ct) → Task<CancellationSignal?>
```

### Method Contracts

#### Work Queue

**`EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken ct)`**
- Called by: Master only
- Behavior: Publish a module assignment to the work queue. The assignment includes required capabilities. The implementation MUST make this available to `DequeueModuleAsync` callers that match the capabilities.
- Ordering: FIFO within capability-matched items is preferred but not required.
- Idempotency: Not required — each call represents a unique assignment.

**`DequeueModuleAsync(IReadOnlySet<string> workerCapabilities, CancellationToken ct)`**
- Called by: Workers only
- Behavior: Atomically dequeue and return the next assignment whose `RequiredCapabilities` are a subset of `workerCapabilities`. Returns `null` if no matching assignment is available (non-blocking) OR blocks until one is available (implementation choice — the framework handles both via polling).
- Thread safety: MUST be safe for concurrent callers. Each assignment MUST be dequeued by exactly one worker.

#### Results

**`PublishResultAsync(SerializedModuleResult result, CancellationToken ct)`**
- Called by: Workers (and master for locally-executed modules)
- Behavior: Publish a completed module result. The master will call `WaitForResultAsync` for each module it distributed.

**`WaitForResultAsync(string moduleTypeName, CancellationToken ct)`**
- Called by: Master only
- Behavior: Wait for and return the result for the specified module. MUST block until the result is available or the CancellationToken is cancelled. The implementation may use polling, pub/sub, or any mechanism.
- Uniqueness: Each `moduleTypeName` will have exactly one result published.

#### Worker Management

**`RegisterWorkerAsync(WorkerRegistration registration, CancellationToken ct)`**
- Called by: Workers, once at startup
- Behavior: Register this worker with the coordination layer. The master polls `GetRegisteredWorkersAsync` to discover workers.

**`SendHeartbeatAsync(int workerIndex, CancellationToken ct)`**
- Called by: Workers, periodically (default every 10 seconds)
- Behavior: Update the worker's last-seen timestamp. The master uses this to detect unresponsive workers.

**`GetRegisteredWorkersAsync(CancellationToken ct)`**
- Called by: Master, periodically
- Behavior: Return all registered workers with their current status and last heartbeat time.

#### Cancellation

**`BroadcastCancellationAsync(string reason, CancellationToken ct)`**
- Called by: Master only
- Behavior: Signal all workers to stop executing. Workers poll `IsCancellationRequestedAsync`.

**`IsCancellationRequestedAsync(CancellationToken ct)`**
- Called by: Workers, periodically
- Behavior: Return the cancellation signal if one has been broadcast, or `null` if pipeline is still running.

## IDistributedCoordinatorFactory (optional)

For coordination providers that need async initialization (connecting to a server, creating queues, etc.).

```
IDistributedCoordinatorFactory
└── CreateAsync(ct) → Task<IDistributedCoordinator>
```

If registered, the framework calls `CreateAsync` during pipeline startup and uses the returned coordinator. If not registered, the framework uses the directly-registered `IDistributedCoordinator`.

## Registration Pattern

```
// In Program.cs or pipeline setup:
builder.AddDistributedMode(options => {
    options.InstanceIndex = int.Parse(args["--instance"]);
    options.TotalInstances = int.Parse(args["--total"]);
});

// Register custom coordination provider:
builder.AddDistributedCoordinator<RedisDistributedCoordinator>();

// Or with factory for async init:
builder.AddDistributedCoordinatorFactory<RedisCoordinatorFactory>();
```

## Attribute Contracts

### RequiresCapabilityAttribute

```
[RequiresCapability("docker")]
[RequiresCapability("linux")]
public class DockerBuildModule : Module<CommandResult> { }
```

- Multiple attributes = AND logic (all capabilities required)
- Read at scheduler time, stored in module metadata
- Capabilities are case-insensitive string comparisons

### MatrixTargetAttribute

```
[MatrixTarget("windows", "linux", "macos")]
public class CrossPlatformTests : Module<TestResult> { }
```

- Triggers registration-time expansion into N modules
- Each expanded instance gets `[RequiresCapability(target)]` auto-applied
- Module code accesses its target via `context.GetMatrixTarget()` (returns `string?`)
- Dependencies on a matrix module depend on all expanded instances

### PinToMasterAttribute

```
[PinToMaster]
public class PublishResultsModule : Module<None> { }
```

- Module is only executed on the master instance
- Master never enqueues this module to the work queue
- In non-distributed mode, has no effect (all modules run locally)
