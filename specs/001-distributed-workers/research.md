# Research: Distributed Workers Mode

**Date**: 2026-02-22 | **Branch**: `001-distributed-workers`

## Decision 1: Integration Architecture

**Decision**: Introduce a new `ModularPipelines.Distributed` package that replaces the `IModuleExecutor` and `IModuleScheduler` implementations on master instances and provides a worker-mode executor. Use `InternalsVisibleTo` since these are internal interfaces.

**Rationale**: The existing `IModuleScheduler` (internal) already manages the dependency graph and ready-module channel. The `IModuleExecutor` consumes from that channel via `Parallel.ForEachAsync`. For distributed mode, the master replaces this local worker pool with coordination layer push/pull. Workers replace the full executor with a loop that polls for assignments.

**Alternatives considered**:
- Wrapping the existing executor in a decorator → rejected because the executor's `Parallel.ForEachAsync` is fundamentally local-only
- Creating an entirely new scheduler → rejected because 90% of the logic (dependency tracking, constraint evaluation, priority sorting) is reusable
- Using only public extension points (`IModuleResultRepository`) → rejected because it doesn't control scheduling or work distribution

## Decision 2: Module Identity Across Processes

**Decision**: Use `Type.FullName` (namespace-qualified) as the canonical module identifier for cross-process communication. The existing `ModuleName` property (simple class name) is insufficient for uniqueness.

**Rationale**: `Type.FullName` is deterministic, unique within an assembly, and available on both serialization and deserialization sides (since all instances run the same application). `AssemblyQualifiedName` is unnecessary because all instances share the same assemblies.

**Alternatives considered**:
- Simple class name (`Type.Name`) → rejected, not unique across namespaces
- `AssemblyQualifiedName` → rejected, overly verbose and includes version info that may differ

## Decision 3: Serialization Gaps

**Decision**: Address three serialization issues in `ModuleResult<T>`:

1. **Module type identification**: Add `ModuleTypeName` (string) as a `[JsonInclude]` property on `ModuleResult`, populated with `Type.FullName`.
2. **Null value handling**: Fix `ModuleResultJsonConverter<T>.Read()` to handle `null` Value for `Module<None>` or nullable result types.
3. **Type discriminator for T**: Transmit the `T` type name out-of-band in `ModuleAssignment`, so the receiver knows which generic deserializer to invoke.

**Rationale**: These are gaps in the existing serialization infrastructure identified by round-trip testing. The `JsonResultRepository` test already exercises serialization but only within a single process where type information is ambient.

**Alternatives considered**:
- Including the full serialized `Type` in JSON → rejected for security (type injection risk) and portability
- Using a registry of module type to result type mappings → decided as the approach, built automatically from the registered module list

## Decision 4: Coordination Layer Interface Design

**Decision**: A single `IDistributedCoordinator` interface with logically grouped methods. Not decomposed into multiple interfaces.

**Rationale**: SC-005 requires "implemented in under a day." One interface with ~12 methods organized by concern (work queue, results, health, cancellation) is simpler to understand and implement than 4+ separate interfaces. Users implement one class, register once.

**Alternatives considered**:
- Decomposed interfaces (`IWorkQueue`, `IResultBus`, `IWorkerRegistry`, `ICancellationSignal`) → rejected for implementation complexity; users would need to register 4 services
- Abstract base class with virtual methods → rejected because it prevents implementing with different transport backends per method group

## Decision 5: Master-Side Scheduling Strategy

**Decision**: Reuse the existing `ModuleScheduler` on the master. Intercept at the `ModuleExecutor` level — instead of running `Parallel.ForEachAsync` locally, the distributed executor reads from the scheduler's `ReadyModules` channel, publishes to the coordination layer, and listens for results to signal `CompletionSource` on local module proxies.

**Rationale**: The `ModuleScheduler` already handles dependency graph analysis, constraint evaluation (`[NotInParallel]`, `[ParallelLimiter]`), priority ordering, and the ready-module channel. Reusing it means global enforcement of all execution constraints on the master (as specified in FR-018) with zero duplication.

**Alternatives considered**:
- Reimplementing scheduling in the distributed package → rejected, would duplicate 500+ lines of well-tested constraint and dependency logic
- Having workers independently schedule → rejected, violates the master-controls-scheduling design

## Decision 6: Worker Execution Model

**Decision**: Workers run a simplified pipeline that registers all modules (for type resolution) but does NOT execute the normal scheduler. Instead, workers run a poll loop: dequeue assignment → create module scope → execute via standard `ModuleExecutionPipeline` → publish result → repeat.

**Rationale**: The `ModuleExecutionPipeline` (hooks, retry, timeout, skip) is fully reusable on workers. Only the scheduling and dependency-wait layers need to be replaced. Workers don't need dependency graph knowledge — the master handles all scheduling decisions.

**Alternatives considered**:
- Workers running a full pipeline with dependency filtering → rejected, too complex and duplicates scheduling logic
- Workers as thin process launchers → rejected, need full DI context for `IModuleContext` services

## Decision 7: Matrix Expansion Implementation

**Decision**: Matrix expansion occurs during pipeline initialization, after module registration but before the scheduler builds the dependency graph. A new initialization step scans for `[MatrixTarget]` attributes and generates N synthetic module registrations per matrix target, each wrapping the original module type with target-specific metadata.

**Rationale**: Expanding at registration time means the scheduler, dependency resolver, and constraint evaluator all see the expanded modules as first-class modules. No changes needed to the scheduling algorithm. The existing `ModuleAutoRegistrar.AutoRegisterMissingDependencies` already runs after registration, so expanded modules will have their dependencies auto-resolved.

**Alternatives considered**:
- Expanding inside `AddModule<T>()` → rejected, too early — doesn't have full service collection context
- Expanding in the scheduler → rejected, mixes scheduling with registration concerns
- Using `IModuleRegistrationEventReceiver` → rejected, attribute runs per-module but expansion needs post-registration global pass

## Decision 8: Capability System Design

**Decision**: Capabilities are implemented as:
- Workers: Set via `DistributedOptions.Capabilities` (list of strings) and auto-detected OS capability
- Modules: Declared via `[RequiresCapability("name")]` attribute, read at scheduler time
- Matching: Master checks `module.RequiredCapabilities.All(c => worker.Capabilities.Contains(c))` before enqueuing to a specific worker

**Rationale**: String-based capabilities align with the existing tag/category pattern (`[ModuleTag]`, `[ModuleCategory]`). Auto-detecting OS capability means cross-platform routing works out of the box.

**Alternatives considered**:
- Structured capability objects with versions → rejected, over-engineering for MVP
- Capability negotiation protocol → rejected, unnecessary when all instances run the same app
