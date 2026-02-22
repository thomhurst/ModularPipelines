# Data Model: Distributed Workers Mode

**Date**: 2026-02-22 | **Branch**: `001-distributed-workers`

## Entities

### DistributedRole (enum)

Determines the instance's role in the distributed pipeline.

| Value  | Description                                    |
| ------ | ---------------------------------------------- |
| Master | Orchestrates scheduling, distributes work, aggregates results. Also executes modules locally. |
| Worker | Receives and executes module assignments from the master. |

**Lifecycle**: Set at startup, immutable for the pipeline run.

### WorkerRegistration

A record of a worker connecting to the master.

| Field           | Type               | Description                                        |
| --------------- | ------------------ | -------------------------------------------------- |
| WorkerIndex     | int                | Unique instance index (1..N for workers, 0 for master) |
| Capabilities    | IReadOnlySet<string> | Advertised capabilities (e.g., "linux", "docker") |
| RegisteredAt    | DateTimeOffset     | When the worker registered                         |
| Status          | WorkerStatus       | Current health status                              |
| CurrentModule   | string?            | FullName of the module currently being executed, or null |

**State transitions**:
```
Connected → Active → Executing → Active → ... → Disconnected
                                              → TimedOut
```

### WorkerStatus (enum)

| Value        | Description                                 |
| ------------ | ------------------------------------------- |
| Connected    | Registered but not yet executing             |
| Active       | Idle, ready to accept work                   |
| Executing    | Currently executing a module                 |
| Disconnected | Gracefully disconnected                      |
| TimedOut     | Heartbeat timeout exceeded                   |

### ModuleAssignment

A unit of work sent from the master to a worker.

| Field                | Type               | Description                                        |
| -------------------- | ------------------ | -------------------------------------------------- |
| ModuleTypeName       | string             | `Type.FullName` of the module to execute           |
| ResultTypeName       | string             | `Type.FullName` of `T` in `Module<T>` (for deserialization) |
| RequiredCapabilities | IReadOnlySet<string> | Capabilities required to execute this module     |
| MatrixTarget         | string?            | The matrix target value, if this is an expanded matrix module |
| AssignedAt           | DateTimeOffset     | When the assignment was published                  |
| Configuration        | ModuleAssignmentConfig | Serialized timeout, retry, and skip configuration |

### ModuleAssignmentConfig

Serialized module configuration for remote execution.

| Field           | Type      | Description                                 |
| --------------- | --------- | ------------------------------------------- |
| TimeoutSeconds  | double?   | Module timeout in seconds, or null for default |
| RetryCount      | int       | Number of retries allowed                    |
| AlwaysRun       | bool      | Whether module should run despite pipeline failure |

### SerializedModuleResult

The outcome of executing a module, in a transport-friendly format.

| Field           | Type           | Description                                      |
| --------------- | -------------- | ------------------------------------------------ |
| ModuleTypeName  | string         | `Type.FullName` of the module that produced this |
| ResultTypeName  | string         | `Type.FullName` of `T` in `ModuleResult<T>`     |
| WorkerIndex     | int            | Which worker executed the module                 |
| SerializedJson  | string         | JSON-serialized `ModuleResult<T>`                |
| CompletedAt     | DateTimeOffset | When execution completed                         |

**Note**: The `SerializedJson` field contains the full `ModuleResult<T>` with all timing data, status, and typed value/exception/skip decision. The receiver uses `ResultTypeName` to select the correct generic deserializer.

### WorkerHeartbeat

Periodic health signal from worker to master.

| Field       | Type           | Description                          |
| ----------- | -------------- | ------------------------------------ |
| WorkerIndex | int            | Which worker is reporting            |
| Timestamp   | DateTimeOffset | When the heartbeat was sent          |
| CurrentModule | string?      | Module being executed, or null if idle |

### CancellationSignal

Pipeline-wide cancellation broadcast.

| Field     | Type           | Description                    |
| --------- | -------------- | ------------------------------ |
| Reason    | string         | Why the pipeline was cancelled |
| Timestamp | DateTimeOffset | When cancellation was issued   |

### DistributedOptions

Configuration for distributed mode, set via `IOptions<DistributedOptions>`.

| Field                    | Type              | Description                                        |
| ------------------------ | ----------------- | -------------------------------------------------- |
| Enabled                  | bool              | Whether distributed mode is active (default: false) |
| InstanceIndex            | int               | This instance's index (0 = master)                 |
| TotalInstances           | int               | Total expected instances (informational)            |
| Capabilities             | IList<string>     | Capabilities to advertise (workers)                |
| HeartbeatIntervalSeconds | int               | Heartbeat frequency (default: 10)                  |
| HeartbeatTimeoutSeconds  | int               | Max time without heartbeat before timeout (default: 30) |
| CapabilityTimeoutSeconds | int               | Max wait for a capable worker before failing module (default: 300) |
| AutoDetectOsCapability   | bool              | Auto-add OS name as capability (default: true)     |

### MatrixModuleInstance

Runtime metadata for an expanded matrix module instance.

| Field           | Type   | Description                                        |
| --------------- | ------ | -------------------------------------------------- |
| OriginalType    | Type   | The original module type before expansion           |
| TargetValue     | string | The matrix target this instance was expanded for    |
| InstanceName    | string | Display name, e.g., `MyTests[linux]`               |
| CapabilityName  | string | The capability requirement applied to this instance |

## Entity Relationships

```
Master (DistributedRole.Master)
  ├── manages 0..N WorkerRegistration
  ├── publishes ModuleAssignment → coordination layer
  ├── receives SerializedModuleResult ← coordination layer
  ├── monitors WorkerHeartbeat
  └── broadcasts CancellationSignal

Worker (DistributedRole.Worker)
  ├── sends WorkerRegistration → coordination layer
  ├── receives ModuleAssignment ← coordination layer
  ├── publishes SerializedModuleResult → coordination layer
  ├── sends WorkerHeartbeat → coordination layer
  └── listens for CancellationSignal

ModuleAssignment 1 ←→ 1 SerializedModuleResult
  (each assignment produces exactly one result)

MatrixModuleInstance N ←→ 1 Original Module Type
  (one module type expands into N instances)
```
