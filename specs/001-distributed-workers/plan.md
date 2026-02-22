# Implementation Plan: Distributed Workers Mode

**Branch**: `001-distributed-workers` | **Date**: 2026-02-22 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/001-distributed-workers/spec.md`

## Summary

Add a distributed execution mode to ModularPipelines where one instance acts as a master orchestrator and N instances act as workers. The master reuses the existing `ModuleScheduler` for dependency graph management and constraint enforcement, but replaces the local `ModuleExecutor` with a distributed variant that pushes ready modules to a pluggable coordination layer. Workers poll for assignments, execute modules using the standard `ModuleExecutionPipeline`, and report results back. A matrix expansion system enables cross-platform module execution by generating N module instances at registration time.

## Technical Context

**Language/Version**: C# / .NET 10.0 (matching existing project targets)
**Primary Dependencies**: Microsoft.Extensions.DependencyInjection, Microsoft.Extensions.Hosting, System.Text.Json, System.Threading.Channels (existing)
**Storage**: N/A (coordination layer is pluggable; in-memory provider for testing)
**Testing**: TUnit (matching existing test projects), in-memory coordinator for unit tests
**Target Platform**: Cross-platform (.NET 10.0) — Windows, Linux, macOS
**Project Type**: Library (NuGet package: `ModularPipelines.Distributed`)
**Performance Goals**: Coordination overhead < 5% of total pipeline time; result serialization < 100ms per module
**Constraints**: Zero behavioral changes when distributed mode is not enabled (FR-010); all existing attributes and constraints enforced globally (FR-018)
**Scale/Scope**: Support up to 20 concurrent workers; thousands of modules

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

Constitution file is an unfilled template — no project-specific gates defined. Proceeding with standard software engineering principles:
- [x] Feature is additive (new package, no breaking changes to core)
- [x] Existing tests unaffected
- [x] Public API follows existing patterns (`builder.Add*()` extensions, attributes, DI)
- [x] No new external dependencies in core package

## Project Structure

### Documentation (this feature)

```text
specs/001-distributed-workers/
├── plan.md              # This file
├── spec.md              # Feature specification
├── research.md          # Phase 0 research decisions
├── data-model.md        # Entity definitions
├── quickstart.md        # Usage guide
├── contracts/           # Interface contracts
│   └── coordination-interfaces.md
└── tasks.md             # Phase 2 output (created by /speckit.tasks)
```

### Source Code (repository root)

```text
src/
├── ModularPipelines/                           # Core framework (minimal changes)
│   ├── Attributes/
│   │   ├── RequiresCapabilityAttribute.cs       # NEW — capability requirement
│   │   ├── MatrixTargetAttribute.cs             # NEW — matrix expansion declaration
│   │   └── PinToMasterAttribute.cs              # NEW — pin to master instance
│   ├── Models/
│   │   └── ModuleResult.cs                      # MODIFIED — add ModuleTypeName for serialization
│   ├── Distributed/
│   │   ├── IDistributedCoordinator.cs           # NEW — coordination abstraction
│   │   ├── IDistributedCoordinatorFactory.cs    # NEW — async coordinator init
│   │   ├── DistributedOptions.cs                # NEW — distributed mode config
│   │   ├── DistributedRole.cs                   # NEW — Master/Worker enum
│   │   ├── ModuleAssignment.cs                  # NEW — work queue item
│   │   ├── SerializedModuleResult.cs            # NEW — result transport type
│   │   ├── WorkerRegistration.cs                # NEW — worker identity
│   │   ├── WorkerHeartbeat.cs                   # NEW — health signal
│   │   └── CancellationSignal.cs                # NEW — pipeline cancellation
│   ├── Context/
│   │   └── IModuleContext.cs                    # MODIFIED — add GetMatrixTarget()
│   └── Extensions/
│       └── PipelineBuilderExtensions.cs         # MODIFIED — add distributed builder methods
│
├── ModularPipelines.Distributed/                # NEW package — distributed engine
│   ├── ModularPipelines.Distributed.csproj
│   ├── Extensions/
│   │   └── DistributedPipelineBuilderExtensions.cs  # AddDistributedMode(), AddDistributedCoordinator<T>()
│   ├── Master/
│   │   ├── DistributedModuleExecutor.cs         # Replaces ModuleExecutor on master
│   │   ├── DistributedWorkPublisher.cs          # Reads scheduler channel, publishes to coordinator
│   │   ├── DistributedResultCollector.cs        # Listens for results, signals CompletionSource
│   │   ├── WorkerHealthMonitor.cs               # Heartbeat monitoring, failure detection
│   │   └── DistributedSummaryAggregator.cs      # Aggregates results across instances
│   ├── Worker/
│   │   ├── WorkerModuleExecutor.cs              # Worker-side execution loop
│   │   ├── WorkerHeartbeatService.cs            # Periodic heartbeat sender
│   │   └── WorkerCancellationMonitor.cs         # Polls for cancellation signals
│   ├── Matrix/
│   │   ├── MatrixModuleExpander.cs              # Registration-time module expansion
│   │   └── MatrixModuleInstance.cs              # Wrapper for expanded module instances
│   ├── Capabilities/
│   │   ├── CapabilityMatcher.cs                 # Capability requirement matching logic
│   │   └── OsCapabilityDetector.cs              # Auto-detect OS capability
│   ├── Coordination/
│   │   └── InMemoryDistributedCoordinator.cs    # In-memory provider for testing
│   ├── Serialization/
│   │   ├── ModuleResultSerializer.cs            # Serialize/deserialize ModuleResult<T> with type info
│   │   └── ModuleTypeRegistry.cs                # Maps Type.FullName ↔ (moduleType, resultType)
│   └── Configuration/
│       ├── DistributedPipelinePlugin.cs          # IModularPipelinesPlugin for DI setup
│       └── RoleDetector.cs                       # Determines Master/Worker from options
│
test/
└── ModularPipelines.Distributed.UnitTests/       # NEW test project
    ├── ModularPipelines.Distributed.UnitTests.csproj
    ├── Master/
    │   ├── DistributedModuleExecutorTests.cs
    │   ├── WorkerHealthMonitorTests.cs
    │   └── DistributedResultCollectorTests.cs
    ├── Worker/
    │   ├── WorkerModuleExecutorTests.cs
    │   └── WorkerCancellationMonitorTests.cs
    ├── Matrix/
    │   └── MatrixModuleExpanderTests.cs
    ├── Capabilities/
    │   └── CapabilityMatcherTests.cs
    ├── Serialization/
    │   ├── ModuleResultSerializerTests.cs
    │   └── ModuleTypeRegistryTests.cs
    ├── Coordination/
    │   └── InMemoryDistributedCoordinatorTests.cs
    └── Integration/
        ├── DistributedPipelineIntegrationTests.cs
        ├── MatrixExpansionIntegrationTests.cs
        └── CapabilityRoutingIntegrationTests.cs
```

**Structure Decision**: New `ModularPipelines.Distributed` package follows the existing pattern of tool-specific packages (`ModularPipelines.Git`, `ModularPipelines.Docker`). Core abstractions (interfaces, attributes, data types) live in the main `ModularPipelines` package to avoid a circular dependency — modules in any package need to use `[RequiresCapability]` and `[MatrixTarget]` attributes. The distributed engine implementation lives in the separate package.

## Implementation Phases

### Phase 1: Core Abstractions & Serialization (Foundation)

**Goal**: Establish the public API surface, data types, and serialization infrastructure.

1. **Core data types** in `ModularPipelines/Distributed/`:
   - `DistributedRole`, `DistributedOptions`, `ModuleAssignment`, `SerializedModuleResult`, `WorkerRegistration`, `WorkerHeartbeat`, `CancellationSignal`
   - All must be JSON-serializable via `System.Text.Json`

2. **Coordination interface** in `ModularPipelines/Distributed/`:
   - `IDistributedCoordinator` with work queue, result, health, and cancellation methods
   - `IDistributedCoordinatorFactory` for async init

3. **New attributes** in `ModularPipelines/Attributes/`:
   - `[RequiresCapability("name")]` — `AttributeUsage(Class, AllowMultiple = true, Inherited = true)`
   - `[MatrixTarget("a", "b", "c")]` — `AttributeUsage(Class, AllowMultiple = false, Inherited = true)`
   - `[PinToMaster]` — `AttributeUsage(Class, AllowMultiple = false, Inherited = true)`

4. **Serialization enhancements** in `ModularPipelines/Models/ModuleResult.cs`:
   - Add `ModuleTypeName` property (`[JsonInclude]`, populated with `Type.FullName`)
   - Fix null `Value` handling in `ModuleResultJsonConverter<T>`

5. **Builder extensions** in `ModularPipelines/Extensions/`:
   - `AddDistributedMode(Action<DistributedOptions>)` — configures options
   - `AddDistributedCoordinator<T>()` — registers coordinator
   - `AddDistributedCoordinatorFactory<T>()` — registers factory

6. **Module context extension**:
   - `GetMatrixTarget()` method on `IModuleContext` — returns `string?`

### Phase 2: In-Memory Coordinator & Module Type Registry

**Goal**: Provide a testable coordination implementation and type mapping.

1. **`InMemoryDistributedCoordinator`** — thread-safe, single-process implementation using `Channel<T>` and `ConcurrentDictionary`:
   - Work queue: `Channel<ModuleAssignment>` with capability filtering
   - Results: `ConcurrentDictionary<string, TaskCompletionSource<SerializedModuleResult>>`
   - Workers: `ConcurrentDictionary<int, WorkerRegistration>`
   - Cancellation: `CancellationTokenSource` wrapper

2. **`ModuleTypeRegistry`** — built from registered module types at startup:
   - Maps `Type.FullName` → `(Type moduleType, Type resultType)`
   - Used for deserialization: given a `SerializedModuleResult.ResultTypeName`, invoke the correct `JsonSerializer.Deserialize<ModuleResult<T>>()` via compiled expression

3. **`ModuleResultSerializer`** — wraps `System.Text.Json` with type registry integration:
   - `Serialize<T>(ModuleResult<T>)` → `SerializedModuleResult`
   - `Deserialize(SerializedModuleResult)` → `IModuleResult` (type-erased, with correct runtime type)

### Phase 3: Matrix Expansion & Capability System

**Goal**: Implement module expansion and capability matching.

1. **`MatrixModuleExpander`** — runs during pipeline initialization:
   - Scans registered modules for `[MatrixTarget]`
   - For each target value, creates a synthetic module registration wrapping the original type
   - Each synthetic module gets `[RequiresCapability(target)]` auto-applied
   - Rewrites dependency graph: modules depending on the matrix base type depend on all expanded instances
   - In non-distributed mode: capabilities are checked against the local machine; non-matching instances are skipped

2. **`CapabilityMatcher`** — matching logic:
   - `bool CanExecute(ModuleAssignment assignment, WorkerRegistration worker)` — checks `assignment.RequiredCapabilities ⊆ worker.Capabilities`

3. **`OsCapabilityDetector`** — auto-detects OS:
   - Adds `"windows"`, `"linux"`, or `"macos"` to capabilities based on `RuntimeInformation.IsOSPlatform()`

4. **Capability integration with scheduler**:
   - In non-distributed mode: modules with `[RequiresCapability]` that don't match the local machine's capabilities are skipped (similar to `[RunIfAll<OnLinux>]` but using the capability system)

### Phase 4: Master-Side Distributed Executor

**Goal**: Replace the local module executor with distributed work distribution on master instances.

1. **`DistributedModuleExecutor`** — replaces `ModuleExecutor` on master:
   - Creates the standard `ModuleScheduler` (reuses all dependency/constraint logic)
   - Runs the scheduler loop normally
   - Instead of `Parallel.ForEachAsync` on local threads:
     - Reads from `scheduler.ReadyModules` channel
     - For each ready module: check `[PinToMaster]` → execute locally OR `EnqueueModuleAsync` to coordinator
     - Starts `DistributedResultCollector` to listen for results
     - When a result arrives: signal the local `Module<T>.CompletionSource` → unblocks dependent modules in the scheduler
   - Also runs a local execution pool for master-pinned modules and modules from the work queue (master is also a worker)

2. **`DistributedWorkPublisher`** — reads from scheduler channel:
   - Converts `ModuleState` → `ModuleAssignment` (with serialized config)
   - Publishes to coordinator's work queue
   - Handles capability-based routing (only enqueues if at least one registered worker has matching capabilities; otherwise holds and retries when new workers join)

3. **`DistributedResultCollector`** — background task:
   - For each distributed module, calls `WaitForResultAsync(moduleTypeName, ct)`
   - On result received: deserializes `ModuleResult<T>`, signals `CompletionSource`
   - Calls `scheduler.MarkModuleCompleted(moduleType, success)` to unlock dependents

4. **`WorkerHealthMonitor`** — background task:
   - Periodically calls `GetRegisteredWorkersAsync()`
   - Detects workers exceeding heartbeat timeout
   - For timed-out workers: reassigns their in-progress modules to the work queue (respecting retry config) or marks modules as failed

5. **`DistributedSummaryAggregator`**:
   - Collects all results (local + distributed) into a unified `PipelineSummary`
   - Same structure as single-machine summary

### Phase 5: Worker-Side Executor

**Goal**: Implement the worker execution loop.

1. **`WorkerModuleExecutor`** — replaces `ModuleExecutor` on worker instances:
   - On startup: registers with coordinator (`RegisterWorkerAsync`)
   - Main loop: `while (!cancelled)`:
     - `DequeueModuleAsync(capabilities, ct)` — blocks/polls until assignment available
     - Resolve module type from `ModuleTypeRegistry`
     - Create DI scope, resolve `IModuleContext`
     - Execute via standard `ModuleExecutionPipeline.ExecuteAsync<T>()`
     - Serialize result → `PublishResultAsync(result, ct)`
   - Handles module-level errors (catch, serialize as failure result, continue loop)
   - Handles coordinator errors (log, retry with backoff, fail if persistent)

2. **`WorkerHeartbeatService`** — `IHostedService`:
   - Periodic `SendHeartbeatAsync()` at configured interval
   - Reports current module being executed

3. **`WorkerCancellationMonitor`** — `IHostedService`:
   - Periodically polls `IsCancellationRequestedAsync()`
   - On cancellation: triggers `EngineCancellationToken.CancelWithReason(reason)`

### Phase 6: Plugin Integration & Configuration

**Goal**: Wire everything together via the plugin system.

1. **`DistributedPipelinePlugin`** — `IModularPipelinesPlugin`:
   - `ConfigureServices`: Registers distributed services based on `DistributedOptions`
     - If `Enabled && InstanceIndex == 0`: Register master-side services (replace `IModuleExecutor`)
     - If `Enabled && InstanceIndex > 0`: Register worker-side services (replace `IModuleExecutor`)
     - If `!Enabled`: No-op (backward compatible)
   - `ConfigurePipeline`: Runs matrix expansion, builds capability metadata

2. **`RoleDetector`**:
   - Reads `DistributedOptions.InstanceIndex` → determines `DistributedRole`
   - Supports override via environment variables (e.g., `MODULAR_PIPELINES_INSTANCE`)

3. **CLI argument integration**:
   - Map `--instance` and `--total` CLI args to `DistributedOptions` via `IConfiguration`

### Phase 7: Testing

**Goal**: Comprehensive test coverage using in-memory coordinator.

1. **Unit tests**:
   - `ModuleResultSerializerTests` — round-trip all result variants
   - `ModuleTypeRegistryTests` — type resolution by FullName
   - `CapabilityMatcherTests` — subset matching, edge cases
   - `MatrixModuleExpanderTests` — expansion count, capability assignment, dependency rewiring
   - `InMemoryDistributedCoordinatorTests` — all coordinator methods, thread safety
   - `WorkerHealthMonitorTests` — timeout detection, reassignment

2. **Integration tests** (using in-memory coordinator, simulated workers as tasks):
   - `DistributedPipelineIntegrationTests`:
     - Pipeline with independent modules → distributed across simulated workers → correct summary
     - Pipeline with dependencies across workers → correct ordering
     - Pipeline with `[NotInParallel]` → global enforcement
     - Worker failure → module reassignment or failure
     - Pipeline cancellation → all workers stop
   - `MatrixExpansionIntegrationTests`:
     - Matrix module expands to N instances
     - Each instance gets correct capability
     - Single-instance mode skips non-matching instances
     - Downstream dependencies wait for all expanded instances
   - `CapabilityRoutingIntegrationTests`:
     - Module routes to capable worker only
     - No capable worker → timeout and failure
     - Late-joining capable worker → picks up work

3. **Backward compatibility tests**:
   - Existing test suite runs unchanged when distributed mode is not enabled

## Key Technical Risks

| Risk | Impact | Mitigation |
| ---- | ------ | ---------- |
| `ModuleResult<T>` serialization edge cases (null values, custom types) | Modules fail on remote execution | Comprehensive serializer tests; fail-fast with clear error on serialization failure |
| `IModuleScheduler` is internal — changes may break across versions | Distributed package breaks on core updates | Use `InternalsVisibleTo`; add integration tests against the scheduler contract |
| Coordination layer latency dominates for fast modules | No speedup for pipelines with many small modules | Document minimum module execution time for distributed benefit; batch small modules |
| Worker registration race conditions | Duplicate assignments or missed modules | Coordinator contract requires atomic dequeue; in-memory implementation uses channels |
| Matrix expansion interacts with auto-registrar | Dependency resolution breaks for expanded modules | Expansion runs after auto-registrar; expanded modules inherit original dependencies |

## Complexity Tracking

No constitution violations to justify — the feature is additive and the architecture reuses existing components (scheduler, execution pipeline, DI, plugin system).
