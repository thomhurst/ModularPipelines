# Tasks: Distributed Workers Mode

**Input**: Design documents from `/specs/001-distributed-workers/`
**Prerequisites**: plan.md, spec.md, data-model.md, contracts/coordination-interfaces.md, research.md, quickstart.md

**Tests**: Test tasks are included per the implementation plan (Phase 7).

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the new project structure and configure build/test projects

- [x] T001 Create `ModularPipelines.Distributed` project with `src/ModularPipelines.Distributed/ModularPipelines.Distributed.csproj` targeting net10.0, referencing `ModularPipelines` core, with `InternalsVisibleTo` for test project
- [x] T002 Create `ModularPipelines.Distributed.UnitTests` test project with `test/ModularPipelines.Distributed.UnitTests/ModularPipelines.Distributed.UnitTests.csproj` using TUnit, referencing both `ModularPipelines` and `ModularPipelines.Distributed`
- [x] T003 Add both new projects to `ModularPipelines.sln` and verify `dotnet build ModularPipelines.sln -c Release` succeeds

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core abstractions, data types, interfaces, and attributes that ALL user stories depend on. These live in the `ModularPipelines` core package to avoid circular dependencies.

**CRITICAL**: No user story work can begin until this phase is complete

- [x] T004 [P] Create `DistributedRole` enum (Master, Worker) in `src/ModularPipelines/Distributed/DistributedRole.cs`
- [x] T005 [P] Create `WorkerStatus` enum (Connected, Active, Executing, Disconnected, TimedOut) in `src/ModularPipelines/Distributed/WorkerStatus.cs`
- [x] T006 [P] Create `DistributedOptions` class with all fields from data-model.md (Enabled, InstanceIndex, TotalInstances, Capabilities, HeartbeatIntervalSeconds, HeartbeatTimeoutSeconds, CapabilityTimeoutSeconds, AutoDetectOsCapability) in `src/ModularPipelines/Distributed/DistributedOptions.cs`
- [x] T007 [P] Create `ModuleAssignmentConfig` record (TimeoutSeconds, RetryCount, AlwaysRun) in `src/ModularPipelines/Distributed/ModuleAssignmentConfig.cs`
- [x] T008 [P] Create `ModuleAssignment` record (ModuleTypeName, ResultTypeName, RequiredCapabilities, MatrixTarget, AssignedAt, Configuration) in `src/ModularPipelines/Distributed/ModuleAssignment.cs`
- [x] T009 [P] Create `SerializedModuleResult` record (ModuleTypeName, ResultTypeName, WorkerIndex, SerializedJson, CompletedAt) in `src/ModularPipelines/Distributed/SerializedModuleResult.cs`
- [x] T010 [P] Create `WorkerRegistration` record (WorkerIndex, Capabilities, RegisteredAt, Status, CurrentModule) in `src/ModularPipelines/Distributed/WorkerRegistration.cs`
- [x] T011 [P] Create `WorkerHeartbeat` record (WorkerIndex, Timestamp, CurrentModule) in `src/ModularPipelines/Distributed/WorkerHeartbeat.cs`
- [x] T012 [P] Create `CancellationSignal` record (Reason, Timestamp) in `src/ModularPipelines/Distributed/CancellationSignal.cs`
- [x] T013 [P] Create `IDistributedCoordinator` interface with all 9 methods per contracts/coordination-interfaces.md in `src/ModularPipelines/Distributed/IDistributedCoordinator.cs`
- [x] T014 [P] Create `IDistributedCoordinatorFactory` interface with `CreateAsync(ct)` method in `src/ModularPipelines/Distributed/IDistributedCoordinatorFactory.cs`
- [x] T015 [P] Create `[RequiresCapability("name")]` attribute with `AttributeUsage(Class, AllowMultiple = true, Inherited = true)` in `src/ModularPipelines/Attributes/RequiresCapabilityAttribute.cs`
- [x] T016 [P] Create `[MatrixTarget("a", "b", "c")]` attribute with `AttributeUsage(Class, AllowMultiple = false, Inherited = true)` storing a `string[] Targets` property in `src/ModularPipelines/Attributes/MatrixTargetAttribute.cs`
- [x] T017 [P] Create `[PinToMaster]` attribute with `AttributeUsage(Class, AllowMultiple = false, Inherited = true)` in `src/ModularPipelines/Attributes/PinToMasterAttribute.cs`
- [x] T018 Add `ModuleTypeName` property (`[JsonInclude]`, populated with `Type.FullName`) to `ModuleResult` in `src/ModularPipelines/Models/ModuleResult.cs` — ensure null `Value` handling in `ModuleResultJsonConverter<T>`
- [x] T019 Add `GetMatrixTarget()` method returning `string?` to `IModuleContext` in `src/ModularPipelines/Context/IModuleContext.cs` and implement in the concrete context class
- [x] T020 Add builder extension methods `AddDistributedMode(Action<DistributedOptions>)`, `AddDistributedCoordinator<T>()`, and `AddDistributedCoordinatorFactory<T>()` in `src/ModularPipelines/Extensions/PipelineBuilderExtensions.cs`

**Checkpoint**: All core abstractions defined — user story implementation can now begin

---

## Phase 3: User Story 1 - Run Pipeline in Distributed Mode (Priority: P1) MVP

**Goal**: Enable a pipeline to execute modules across a master and N worker instances, with the master scheduling work via a dynamic queue and workers executing assigned modules, producing the same pipeline summary as a single-machine run.

**Independent Test**: Run a pipeline with 3+ modules (some independent, some dependent) across a master and at least one simulated worker using the in-memory coordinator. Verify all modules execute with correct dependency ordering and the final summary matches a single-machine run.

### Implementation for User Story 1

- [x] T021 [P] [US1] Create `ModuleTypeRegistry` — maps `Type.FullName` to `(Type moduleType, Type resultType)`, built from registered module types at startup — in `src/ModularPipelines.Distributed/Serialization/ModuleTypeRegistry.cs`
- [x] T022 [P] [US1] Create `ModuleResultSerializer` — wraps `System.Text.Json` with type registry integration for `Serialize<T>(ModuleResult<T>)` and `Deserialize(SerializedModuleResult)` — in `src/ModularPipelines.Distributed/Serialization/ModuleResultSerializer.cs`
- [x] T023 [P] [US1] Create `InMemoryDistributedCoordinator` — thread-safe implementation using `Channel<ModuleAssignment>` for work queue and `ConcurrentDictionary` for results, workers, and cancellation — in `src/ModularPipelines.Distributed/Coordination/InMemoryDistributedCoordinator.cs`
- [x] T024 [P] [US1] Create `RoleDetector` — reads `DistributedOptions.InstanceIndex` to determine `DistributedRole` (0 = Master, >0 = Worker), with environment variable override support — in `src/ModularPipelines.Distributed/Configuration/RoleDetector.cs`
- [x] T025 [US1] Create `DistributedWorkPublisher` — reads from scheduler's `ReadyModules` channel, converts `ModuleState` to `ModuleAssignment`, publishes to coordinator via `EnqueueModuleAsync` — in `src/ModularPipelines.Distributed/Master/DistributedWorkPublisher.cs`
- [x] T026 [US1] Create `DistributedResultCollector` — background task that calls `WaitForResultAsync(moduleTypeName, ct)` for each distributed module, deserializes result, signals `CompletionSource` — in `src/ModularPipelines.Distributed/Master/DistributedResultCollector.cs`
- [x] T027 [US1] Create `DistributedModuleExecutor` — replaces `ModuleExecutor` on master, reuses `ModuleScheduler`, reads ready modules, checks `[PinToMaster]` for local execution vs enqueue, starts result collector — in `src/ModularPipelines.Distributed/Master/DistributedModuleExecutor.cs`
- [x] T028 [US1] Create `DistributedSummaryAggregator` — collects local + distributed results into a unified `PipelineSummary` matching single-machine format — in `src/ModularPipelines.Distributed/Master/DistributedSummaryAggregator.cs`
- [x] T029 [US1] Create `WorkerModuleExecutor` — worker-side execution loop: register → dequeue → resolve type → create DI scope → execute via `ModuleExecutionPipeline` → publish result → repeat — in `src/ModularPipelines.Distributed/Worker/WorkerModuleExecutor.cs`
- [x] T030 [US1] Create `WorkerHeartbeatService` as `IHostedService` — periodic `SendHeartbeatAsync()` at configured interval, reports current module — in `src/ModularPipelines.Distributed/Worker/WorkerHeartbeatService.cs`
- [x] T031 [US1] Create `WorkerCancellationMonitor` as `IHostedService` — polls `IsCancellationRequestedAsync()`, triggers `EngineCancellationToken.CancelWithReason(reason)` on signal — in `src/ModularPipelines.Distributed/Worker/WorkerCancellationMonitor.cs`
- [x] T032 [US1] Create `DistributedPipelinePlugin` implementing `IModularPipelinesPlugin` — registers master or worker services based on `DistributedOptions.InstanceIndex`, replaces `IModuleExecutor` accordingly, no-op when `!Enabled` — in `src/ModularPipelines.Distributed/Configuration/DistributedPipelinePlugin.cs`
- [x] T033 [US1] Create `DistributedPipelineBuilderExtensions` — `AddDistributedMode()` and `AddDistributedCoordinator<T>()` wiring for the distributed package — in `src/ModularPipelines.Distributed/Extensions/DistributedPipelineBuilderExtensions.cs`

**Checkpoint**: At this point, distributed mode works end-to-end with the in-memory coordinator — master schedules, workers execute, results flow back, summary is correct.

---

## Phase 4: User Story 2 - Pluggable Coordination Layer (Priority: P2)

**Goal**: Ensure the coordination abstraction is clean and a custom implementation can be swapped in seamlessly via DI registration.

**Independent Test**: Implement a second trivial coordinator (e.g., a test double that wraps in-memory with logging) and verify it works with the same pipeline. Verify `AddDistributedCoordinatorFactory<T>()` async init path works.

### Implementation for User Story 2

- [x] T034 [US2] Implement `IDistributedCoordinatorFactory` async init path in `DistributedPipelinePlugin` — if factory is registered, call `CreateAsync` during pipeline startup and register the returned coordinator in DI — in `src/ModularPipelines.Distributed/Configuration/DistributedPipelinePlugin.cs`
- [x] T035 [US2] Add validation in `DistributedPipelinePlugin` — emit clear warning when using `InMemoryDistributedCoordinator` that it is only suitable for single-process testing — in `src/ModularPipelines.Distributed/Configuration/DistributedPipelinePlugin.cs`

**Checkpoint**: Custom coordination providers can be registered and used. Factory pattern supports async initialization.

---

## Phase 5: User Story 3 - Role Auto-Detection and Configuration (Priority: P2)

**Goal**: Enable role detection from CLI arguments (`--instance`, `--total`) and environment variables with zero boilerplate.

**Independent Test**: Launch pipeline with `--instance=0 --total=4` and verify master role; with `--instance=2 --total=4` and verify worker role. Launch without distributed config and verify single-machine mode.

### Implementation for User Story 3

- [x] T036 [US3] Add `IConfiguration` binding for `--instance` and `--total` CLI arguments to `DistributedOptions` — map via `IConfigurationSection` in `AddDistributedMode()` — in `src/ModularPipelines.Distributed/Extensions/DistributedPipelineBuilderExtensions.cs`
- [x] T037 [US3] Add environment variable support in `RoleDetector` — read `MODULAR_PIPELINES_INSTANCE` and `MODULAR_PIPELINES_TOTAL` as fallback when `DistributedOptions` values are not set — in `src/ModularPipelines.Distributed/Configuration/RoleDetector.cs`
- [x] T038 [US3] Ensure backward compatibility — verify in `DistributedPipelinePlugin` that when `DistributedOptions.Enabled` is false (default), no distributed services are registered and pipeline runs in standard single-machine mode — in `src/ModularPipelines.Distributed/Configuration/DistributedPipelinePlugin.cs`

**Checkpoint**: Role auto-detection works from CLI args, env vars, and programmatic config. Non-distributed mode is fully backward compatible.

---

## Phase 6: User Story 4 - Worker Capabilities and Module Affinity (Priority: P2)

**Goal**: Workers advertise capabilities on registration, modules declare required capabilities via `[RequiresCapability]`, and the master only assigns modules to workers satisfying all requirements.

**Independent Test**: Set up two simulated workers (one with "docker", one without) and a module requiring "docker". Verify the module only routes to the capable worker.

### Implementation for User Story 4

- [x] T039 [P] [US4] Create `CapabilityMatcher` — `bool CanExecute(ModuleAssignment assignment, WorkerRegistration worker)` checks `RequiredCapabilities` is subset of `worker.Capabilities` (case-insensitive) — in `src/ModularPipelines.Distributed/Capabilities/CapabilityMatcher.cs`
- [x] T040 [P] [US4] Create `OsCapabilityDetector` — auto-adds "windows", "linux", or "macos" to capabilities based on `RuntimeInformation.IsOSPlatform()` — in `src/ModularPipelines.Distributed/Capabilities/OsCapabilityDetector.cs`
- [x] T041 [US4] Integrate capability matching into `DistributedWorkPublisher` — read `[RequiresCapability]` attributes from module types, populate `ModuleAssignment.RequiredCapabilities`, implement hold-and-retry when no capable worker is registered, fail with clear error on `CapabilityTimeoutSeconds` expiry — in `src/ModularPipelines.Distributed/Master/DistributedWorkPublisher.cs`
- [x] T042 [US4] Integrate `OsCapabilityDetector` into worker registration — auto-detect OS capability when `DistributedOptions.AutoDetectOsCapability` is true (default), merge with user-configured capabilities — in `src/ModularPipelines.Distributed/Worker/WorkerModuleExecutor.cs`
- [x] T043 [US4] Integrate capability filtering into `InMemoryDistributedCoordinator.DequeueModuleAsync` — only return assignments whose `RequiredCapabilities` are a subset of the provided `workerCapabilities` — in `src/ModularPipelines.Distributed/Coordination/InMemoryDistributedCoordinator.cs`

**Checkpoint**: Capability-based routing works. Modules route only to capable workers. OS auto-detection works out of the box.

---

## Phase 7: User Story 5 - Matrix Module Expansion (Priority: P2)

**Goal**: A module with `[MatrixTarget("windows", "linux", "macos")]` expands into 3 concrete module instances at startup, each with its own capability requirement and `ModuleResult<T>`. Works identically in single-instance and distributed modes.

**Independent Test**: Declare a module with 3 matrix targets, verify 3 instances are registered with correct capability requirements. In single-instance mode, verify only the local-OS instance runs and others are skipped.

### Implementation for User Story 5

- [x] T044 [P] [US5] Create `MatrixModuleInstance` metadata class (OriginalType, TargetValue, InstanceName, CapabilityName) in `src/ModularPipelines.Distributed/Matrix/MatrixModuleInstance.cs`
- [x] T045 [US5] Create `MatrixModuleExpander` — runs during pipeline initialization after module registration: scans for `[MatrixTarget]`, generates N synthetic module registrations wrapping the original type with target-specific metadata and auto-applied `[RequiresCapability(target)]`, rewrites dependency graph so dependents of the base type depend on all expanded instances — in `src/ModularPipelines.Distributed/Matrix/MatrixModuleExpander.cs`
- [x] T046 [US5] Integrate `MatrixModuleExpander` into `DistributedPipelinePlugin.ConfigurePipeline` — run expansion after module auto-registration, before scheduler builds dependency graph — in `src/ModularPipelines.Distributed/Configuration/DistributedPipelinePlugin.cs`
- [x] T047 [US5] Implement `GetMatrixTarget()` in module context — return the target value from `MatrixModuleInstance` metadata if this is an expanded instance, or `null` if not — in `src/ModularPipelines/Context/` (concrete context class)
- [x] T048 [US5] Handle single-instance mode — modules with `[RequiresCapability]` that don't match the local machine's capabilities are skipped automatically, ensuring matrix expansion works without distributed mode — in `src/ModularPipelines.Distributed/Configuration/DistributedPipelinePlugin.cs`

**Checkpoint**: Matrix modules expand correctly, route to capable workers in distributed mode, and skip non-matching in single-instance mode.

---

## Phase 8: User Story 6 - Worker Failure Resilience (Priority: P3)

**Goal**: The master detects unresponsive workers via heartbeat timeouts and reassigns or fails their in-progress modules.

**Independent Test**: Simulate a worker that stops heartbeating after accepting a module. Verify the master detects the timeout within the configured period and either reassigns or fails the module.

### Implementation for User Story 6

- [x] T049 [US6] Create `WorkerHealthMonitor` — background task on master that periodically calls `GetRegisteredWorkersAsync()`, detects workers exceeding `HeartbeatTimeoutSeconds`, marks timed-out workers, reassigns their in-progress modules to the work queue (respecting retry config) or marks as failed — in `src/ModularPipelines.Distributed/Master/WorkerHealthMonitor.cs`
- [x] T050 [US6] Integrate `WorkerHealthMonitor` into `DistributedPipelinePlugin` — register and start on master instances — in `src/ModularPipelines.Distributed/Configuration/DistributedPipelinePlugin.cs`
- [x] T051 [US6] Handle reassignment logic in `DistributedModuleExecutor` — when a module is reassigned after worker failure, re-enqueue it to the coordinator and reset its `CompletionSource` — in `src/ModularPipelines.Distributed/Master/DistributedModuleExecutor.cs`

**Checkpoint**: Worker failures are detected and handled automatically. Modules are reassigned or failed gracefully.

---

## Phase 9: User Story 7 - Distributed Module Result Access (Priority: P3)

**Goal**: Modules depending on results from modules that executed on other instances receive the full typed `ModuleResult<T>` transparently.

**Independent Test**: Module A (typed result) runs on worker 1, Module B (depends on A) runs on worker 2. Verify B receives A's typed result correctly deserialized.

### Implementation for User Story 7

- [x] T052 [US7] Ensure `DistributedResultCollector` deserializes results via `ModuleResultSerializer` with full type fidelity and populates the local module's `CompletionSource` with the correct `ModuleResult<T>` — workers calling `await context.GetModule<A>()` receive the same typed result — in `src/ModularPipelines.Distributed/Master/DistributedResultCollector.cs`
- [x] T053 [US7] Handle complex/nested result types in `ModuleResultSerializer` — verify round-trip serialization for `ModuleResult<T>` where T is a custom class with nested objects, collections, and nullable properties — in `src/ModularPipelines.Distributed/Serialization/ModuleResultSerializer.cs`

**Checkpoint**: Cross-instance module result access works transparently. Dependent modules receive correct typed results regardless of which instance executed the dependency.

---

## Phase 10: Testing

**Purpose**: Comprehensive test coverage for all user stories using in-memory coordinator

### Unit Tests

- [x] T054 [P] Write `ModuleTypeRegistryTests` — type resolution by `FullName`, missing type handling — in `test/ModularPipelines.Distributed.UnitTests/Serialization/ModuleTypeRegistryTests.cs`
- [x] T055 [P] Write `ModuleResultSerializerTests` — round-trip all result variants (success, failure, skip, null value, complex types) — in `test/ModularPipelines.Distributed.UnitTests/Serialization/ModuleResultSerializerTests.cs`
- [x] T056 [P] Write `InMemoryDistributedCoordinatorTests` — all coordinator methods, thread safety, concurrent dequeue, capability filtering — in `test/ModularPipelines.Distributed.UnitTests/Coordination/InMemoryDistributedCoordinatorTests.cs`
- [x] T057 [P] Write `CapabilityMatcherTests` — subset matching, case insensitivity, empty requirements, disjoint capabilities — in `test/ModularPipelines.Distributed.UnitTests/Capabilities/CapabilityMatcherTests.cs`
- [x] T058 [P] Write `MatrixModuleExpanderTests` — expansion count, capability assignment, dependency rewiring, no-expansion default — in `test/ModularPipelines.Distributed.UnitTests/Matrix/MatrixModuleExpanderTests.cs`
- [x] T059 [P] Write `WorkerHealthMonitorTests` — timeout detection, reassignment trigger, graceful disconnection handling — in `test/ModularPipelines.Distributed.UnitTests/Master/WorkerHealthMonitorTests.cs`
- [x] T060 [P] Write `DistributedModuleExecutorTests` — ready module dispatch, PinToMaster local execution, result collection signaling — in `test/ModularPipelines.Distributed.UnitTests/Master/DistributedModuleExecutorTests.cs`
- [x] T061 [P] Write `WorkerModuleExecutorTests` — dequeue-execute-publish loop, error handling, cancellation — in `test/ModularPipelines.Distributed.UnitTests/Worker/WorkerModuleExecutorTests.cs`
- [x] T062 [P] Write `WorkerCancellationMonitorTests` — cancellation signal detection, engine token cancellation — in `test/ModularPipelines.Distributed.UnitTests/Worker/WorkerCancellationMonitorTests.cs`
- [x] T063 [P] Write `DistributedResultCollectorTests` — result deserialization, CompletionSource signaling, timeout handling — in `test/ModularPipelines.Distributed.UnitTests/Master/DistributedResultCollectorTests.cs`

### Integration Tests

- [x] T064 Write `DistributedPipelineIntegrationTests` — pipeline with independent and dependent modules across simulated master + workers → correct summary. Tests: dependency ordering across instances, `[NotInParallel]` global enforcement, pipeline cancellation propagation — in `test/ModularPipelines.Distributed.UnitTests/Integration/DistributedPipelineIntegrationTests.cs`
- [x] T065 Write `MatrixExpansionIntegrationTests` — matrix module expands to N instances, each gets correct capability, single-instance skips non-matching, downstream waits for all expanded instances — in `test/ModularPipelines.Distributed.UnitTests/Integration/MatrixExpansionIntegrationTests.cs`
- [x] T066 Write `CapabilityRoutingIntegrationTests` — module routes to capable worker only, no capable worker → timeout and failure, late-joining capable worker picks up work — in `test/ModularPipelines.Distributed.UnitTests/Integration/CapabilityRoutingIntegrationTests.cs`

### Backward Compatibility

- [x] T067 Verify existing test suite passes unchanged when distributed mode is not enabled — run `dotnet build ModularPipelines.sln -c Release` and existing test projects

**Checkpoint**: All tests pass. Distributed mode is fully tested with in-memory coordinator.

---

## Phase 11: Polish & Cross-Cutting Concerns

**Purpose**: Final quality improvements that affect multiple user stories

- [x] T068 [P] Add distributed operation logging (assignments, results, failures, reassignments) throughout master and worker components per FR-019
- [x] T069 [P] Verify `dotnet format` compliance across all new files
- [x] T070 Run quickstart.md validation — ensure all code examples in `specs/001-distributed-workers/quickstart.md` compile and represent correct API usage
- [x] T071 Final build verification — `dotnet build ModularPipelines.sln -c Release` with all new projects included, no warnings

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies — can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion — BLOCKS all user stories
- **US1 (Phase 3)**: Depends on Foundational (Phase 2) — this is the MVP
- **US2 (Phase 4)**: Depends on US1 (Phase 3) — extends coordination layer
- **US3 (Phase 5)**: Depends on US1 (Phase 3) — adds CLI/env var config
- **US4 (Phase 6)**: Depends on US1 (Phase 3) — adds capability matching
- **US5 (Phase 7)**: Depends on US4 (Phase 6) — matrix expansion uses capability system
- **US6 (Phase 8)**: Depends on US1 (Phase 3) — adds failure resilience
- **US7 (Phase 9)**: Depends on US1 (Phase 3) — validates result serialization
- **Testing (Phase 10)**: Can start unit tests after their corresponding user story; integration tests after US4+US5
- **Polish (Phase 11)**: Depends on all user stories being complete

### User Story Dependencies

- **US1 (P1)**: Can start after Foundational — No dependencies on other stories (MVP)
- **US2 (P2)**: Depends on US1 — extends the coordinator registration path
- **US3 (P2)**: Depends on US1 — enhances role detection already built in US1
- **US4 (P2)**: Depends on US1 — adds capability dimension to work distribution
- **US5 (P2)**: Depends on US4 — matrix expansion auto-applies capability requirements
- **US6 (P3)**: Depends on US1 — adds resilience on top of base distributed mode
- **US7 (P3)**: Depends on US1 — validates serialization already built in US1

### Within Each User Story

- Models/data types before services
- Services before executors
- Executors before plugin integration
- Core implementation before integration tests

### Parallel Opportunities

- All Foundational tasks T004–T020 marked [P] can run in parallel (independent files)
- Within US1: T021–T024 can run in parallel (independent files), then T025–T033 are sequential
- US2, US3, US6, US7 can run in parallel after US1 completes (independent concerns)
- US5 must follow US4 (depends on capability system)
- All unit tests (T054–T063) can run in parallel
- All integration tests can run in parallel after their prerequisites

---

## Parallel Example: User Story 1

```bash
# Launch independent foundation components together:
Task: "Create ModuleTypeRegistry in src/ModularPipelines.Distributed/Serialization/ModuleTypeRegistry.cs"
Task: "Create ModuleResultSerializer in src/ModularPipelines.Distributed/Serialization/ModuleResultSerializer.cs"
Task: "Create InMemoryDistributedCoordinator in src/ModularPipelines.Distributed/Coordination/InMemoryDistributedCoordinator.cs"
Task: "Create RoleDetector in src/ModularPipelines.Distributed/Configuration/RoleDetector.cs"

# Then launch master components (depend on above):
Task: "Create DistributedWorkPublisher in src/ModularPipelines.Distributed/Master/DistributedWorkPublisher.cs"
Task: "Create DistributedResultCollector in src/ModularPipelines.Distributed/Master/DistributedResultCollector.cs"
```

## Parallel Example: Testing Phase

```bash
# Launch all unit tests in parallel:
Task: "Write ModuleTypeRegistryTests"
Task: "Write ModuleResultSerializerTests"
Task: "Write InMemoryDistributedCoordinatorTests"
Task: "Write CapabilityMatcherTests"
Task: "Write MatrixModuleExpanderTests"
Task: "Write WorkerHealthMonitorTests"
Task: "Write DistributedModuleExecutorTests"
Task: "Write WorkerModuleExecutorTests"
Task: "Write WorkerCancellationMonitorTests"
Task: "Write DistributedResultCollectorTests"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRITICAL — blocks all stories)
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Test distributed mode end-to-end with in-memory coordinator
5. Verify backward compatibility (existing tests still pass)

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Distributed mode works end-to-end (MVP!)
3. Add User Story 2 → Custom coordinators can be plugged in
4. Add User Story 3 → Zero-config role detection from CLI/env
5. Add User Story 4 → Capability-based module routing
6. Add User Story 5 → Cross-platform matrix testing
7. Add User Stories 6 + 7 → Resilience and result fidelity
8. Testing Phase → Full test coverage
9. Polish → Logging, formatting, quickstart validation

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1 (MVP — critical path)
3. Once US1 is done:
   - Developer A: User Story 4 (Capabilities) → User Story 5 (Matrix)
   - Developer B: User Story 2 (Coordination) + User Story 3 (Config)
   - Developer C: User Story 6 (Resilience) + User Story 7 (Results)
4. All developers: Testing Phase (unit tests are highly parallelizable)
5. Polish together

---

## Notes

- [P] tasks = different files, no dependencies on incomplete tasks
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- The in-memory coordinator is the primary test vehicle — real coordination providers (Redis, etc.) are user-supplied
- `InternalsVisibleTo` is needed since `IModuleScheduler` and `IModuleExecutor` are internal interfaces
- All data types must be JSON-serializable via `System.Text.Json`
