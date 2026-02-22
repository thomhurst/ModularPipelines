# Feature Specification: Distributed Workers Mode

**Feature Branch**: `001-distributed-workers`
**Created**: 2026-02-22
**Status**: Draft
**Input**: User description: "Develop a Distributed Workers mode for Modular Pipelines where one instance acts as a master and additional instances act as workers. On a GitHub matrix strategy, instance 0 becomes the master orchestrating work across N worker instances. Built generically so users can plug in their own coordination layer (Redis, HTTP, etc.)."

## Clarifications

### Session 2026-02-22

- Q: Should module distribution use static upfront assignment or a dynamic work queue? → A: Dynamic work queue. The master pushes modules to a ready queue as their dependencies are satisfied, and workers pull the next available module when idle. This ensures efficient load balancing across workers with uneven module execution times.
- Q: Should `[NotInParallel]` be enforced globally across all instances or locally per instance? → A: Global enforcement. Distributed mode must behave identically to single-machine mode — the master prevents conflicting modules from running simultaneously on any instance.
- Q: Must all workers register before execution begins, or can they join dynamically? → A: Dynamic join. The master starts executing immediately (it can run modules itself) and workers join as they become available, pulling from the work queue. This avoids blocking on slow runner provisioning in CI environments.
- Q: Should workers advertise capabilities and modules declare required capabilities? → A: Yes. Workers advertise their capabilities (installed tools, OS, custom tags) on registration. Modules can declare required capabilities in their configuration. The master only enqueues a module to workers that satisfy its capability requirements.
- Q: How should cross-platform module execution work (run same module on multiple OS targets)? → A: Matrix expansion at registration time. A module declares a matrix of targets via attribute. At startup, the framework expands it into N concrete module instances (one per target), each with a capability requirement automatically applied and a standard `ModuleResult<T>`. This works identically in both single-instance and distributed modes — no behavioral duality.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Run Pipeline in Distributed Mode (Priority: P1)

A pipeline author wants to split a large pipeline across multiple machines (e.g., GitHub Actions matrix runners) to reduce total execution time. They configure one instance as the master and the remaining instances as workers. The master maintains a dynamic work queue — as module dependencies are satisfied, newly ready modules are pushed to the queue, and idle workers pull and execute them. This ensures efficient load balancing regardless of individual module execution times. The master collects results and produces the same pipeline summary as a single-machine run.

**Why this priority**: This is the core value proposition. Without the ability to distribute and execute modules across instances and collect their results, no other feature in this spec matters.

**Independent Test**: Can be fully tested by running a pipeline with 3+ modules (some independent, some dependent) across a master and at least one worker instance, and verifying all modules execute successfully with correct dependency ordering and the final summary matches a single-machine run.

**Acceptance Scenarios**:

1. **Given** a pipeline with 10 modules where 6 are independent, **When** run in distributed mode with 1 master and 2 workers, **Then** the independent modules are distributed across all instances, dependent modules wait for their dependencies, and all 10 modules complete successfully.
2. **Given** a pipeline running in distributed mode, **When** all modules complete, **Then** the master produces a pipeline summary identical in structure and content to what a single-machine run would produce.
3. **Given** a pipeline with module A depending on module B, **When** module B is assigned to worker 1 and module A is assigned to worker 2, **Then** module A does not begin execution until module B's result has been communicated back through the coordination layer and made available to worker 2.

---

### User Story 2 - Pluggable Coordination Layer (Priority: P2)

A pipeline author wants to use their own preferred coordination mechanism (Redis, a message queue, shared filesystem, HTTP, GitHub Actions cache, etc.) rather than being locked into a single transport. They implement a well-defined set of interfaces and register their implementation via dependency injection, and the distributed mode uses it seamlessly.

**Why this priority**: Without extensibility, adoption is limited to users whose infrastructure matches the built-in transport. An abstraction layer is essential for real-world use across different CI systems and environments.

**Independent Test**: Can be tested by implementing a simple in-memory coordination provider (for testing in a single process with simulated workers) and verifying all distributed operations (work assignment, result collection, health signaling) function correctly through the abstraction.

**Acceptance Scenarios**:

1. **Given** a user has implemented the coordination interfaces using a custom transport, **When** they register their implementation via the pipeline host builder, **Then** the distributed mode uses their implementation for all inter-instance communication.
2. **Given** the framework ships with a default in-process/in-memory coordination provider (for testing and development), **When** a user runs distributed mode without registering a custom provider, **Then** the in-memory provider is used and a clear warning is emitted that this is only suitable for single-process testing.

---

### User Story 3 - Role Auto-Detection and Configuration (Priority: P2)

A pipeline author deploying on GitHub Actions (or similar CI) wants the master/worker role to be determined automatically based on environment variables or command-line arguments. Instance 0 becomes the master; instances 1..N become workers. Configuration is minimal — ideally just passing the instance index and total count.

**Why this priority**: Ease of setup directly impacts adoption. If configuring distributed mode requires significant boilerplate or manual orchestration, users won't use it.

**Independent Test**: Can be tested by launching the pipeline with `--instance=0 --total=4` and verifying it assumes the master role, and with `--instance=2 --total=4` and verifying it assumes the worker role.

**Acceptance Scenarios**:

1. **Given** a pipeline started with instance index 0, **When** distributed mode is enabled, **Then** the instance assumes the master role.
2. **Given** a pipeline started with instance index N (where N > 0), **When** distributed mode is enabled, **Then** the instance assumes the worker role.
3. **Given** a pipeline started without distributed mode configuration, **When** the pipeline runs, **Then** it operates in the existing single-machine mode with no behavioral changes (full backward compatibility).

---

### User Story 4 - Worker Capabilities and Module Affinity (Priority: P2)

A pipeline author has modules with different environmental requirements — some need a specific OS (e.g., Windows for code signing), some need specific tools installed (e.g., Docker, a database CLI), and some can run anywhere. Workers advertise their capabilities when they register with the master (e.g., "windows", "docker", "gpu"). Modules declare their required capabilities via configuration or attributes. The master only assigns a module to a worker that satisfies all of its required capabilities.

**Why this priority**: In real-world CI, runners are heterogeneous. Without capability matching, users would need to ensure every runner has every tool installed, which is impractical. This is essential for production adoption alongside the pluggable coordination layer.

**Independent Test**: Can be tested by setting up two workers — one advertising "docker" capability and one not — and a module requiring "docker". Verify the module is only assigned to the capable worker.

**Acceptance Scenarios**:

1. **Given** a worker registers with capabilities ["linux", "docker"] and a module requires capability "docker", **When** the master enqueues the module, **Then** only workers advertising "docker" are eligible to pull it.
2. **Given** a module requires capability "windows" and no registered worker advertises "windows", **When** the master attempts to schedule the module, **Then** the module is held in the queue until a capable worker joins, or fails with a clear error if no capable worker joins within a configurable timeout.
3. **Given** a module declares no required capabilities, **When** the master enqueues it, **Then** any available worker (including the master) can pull and execute it.
4. **Given** a worker advertises custom capabilities (e.g., "gpu", "high-memory"), **When** a module requires "gpu", **Then** the capability matching works for user-defined capability names, not just built-in ones.

---

### User Story 5 - Matrix Module Expansion (Priority: P2)

A pipeline author wants to run certain modules (e.g., test suites) across multiple platforms to verify cross-platform correctness. Rather than duplicating module classes for each OS, they declare a matrix of targets on a single module definition via attributes. At startup, the framework expands the definition into N concrete module instances — one per target — each with a capability requirement automatically applied. Each expanded instance is a real module in the dependency graph with its own standard `ModuleResult<T>`. This works identically in both single-instance and distributed modes.

**Why this priority**: Cross-platform testing is a primary use case for distributed runners. Without matrix expansion, users must manually duplicate module classes per OS, which is error-prone and defeats the purpose of the framework.

**Independent Test**: Can be tested by declaring a module with matrix targets ["windows", "linux", "macos"], verifying 3 concrete module instances are registered at startup, each with the correct capability requirement and its own independent result.

**Acceptance Scenarios**:

1. **Given** a module declares matrix targets ["windows", "linux", "macos"], **When** the pipeline starts, **Then** the framework registers 3 concrete module instances (e.g., `MyTests[windows]`, `MyTests[linux]`, `MyTests[macos]`), each with a capability requirement matching its target.
2. **Given** 3 expanded module instances exist and the pipeline runs in distributed mode with 3 OS-specific workers, **When** modules are scheduled, **Then** each instance routes to its matching worker and produces its own `ModuleResult<T>`.
3. **Given** 3 expanded module instances exist and the pipeline runs in single-instance mode on Linux, **When** modules are scheduled, **Then** `MyTests[linux]` executes normally, while `MyTests[windows]` and `MyTests[macos]` are skipped because the local instance lacks those capabilities.
4. **Given** a downstream module depends on a matrix-expanded module, **When** declared as a dependency, **Then** it depends on all expanded instances — it waits for all of them to complete (or be skipped) before executing.
5. **Given** a module declares no matrix targets (the default), **When** the pipeline starts, **Then** it is registered as a single module with no expansion, behaving exactly as it does today.

---

### User Story 6 - Worker Failure Resilience (Priority: P3)

A pipeline author wants the distributed pipeline to handle worker failures gracefully. If a worker crashes or becomes unresponsive, the master detects the failure within a configurable timeout and either reassigns the work to another available worker or fails the affected modules with a clear error.

**Why this priority**: CI environments are inherently unreliable (runners can be preempted, network issues, etc.). Without resilience, distributed mode would be fragile and untrustworthy for production CI.

**Independent Test**: Can be tested by simulating a worker that stops responding after accepting a module assignment, and verifying the master detects the timeout, reassigns or fails the module, and the pipeline completes (with appropriate module failures reported).

**Acceptance Scenarios**:

1. **Given** a worker has been assigned a module and stops sending heartbeats, **When** the configured heartbeat timeout elapses, **Then** the master marks the module as failed or reassigns it to another worker (based on retry configuration).
2. **Given** a worker fails mid-execution, **When** the module has retry configured, **Then** the master reassigns the module to another available worker for retry.
3. **Given** all workers for a module have failed, **When** no more retries remain, **Then** the module is marked as failed and downstream dependent modules are cancelled, consistent with existing single-machine failure behavior.

---

### User Story 7 - Distributed Module Result Access (Priority: P3)

A pipeline author has modules that depend on results from other modules. When those modules execute on different machines, the dependent module must still be able to `await` its dependency and receive the typed result, exactly as it would in single-machine mode.

**Why this priority**: This is critical for maintaining the existing programming model. If distributed execution breaks `await GetModule<T>()`, users would need to rewrite their modules for distributed mode, which defeats the purpose.

**Independent Test**: Can be tested by creating module A (returning a typed result) on worker 1 and module B (depending on A) on worker 2, verifying B receives A's typed result through the coordination layer.

**Acceptance Scenarios**:

1. **Given** module B depends on module A, and A runs on a different instance, **When** module B calls `await context.GetModule<A>()`, **Then** it receives the same `ModuleResult<T>` that A produced, deserialized from the coordination layer.
2. **Given** module A produces a result containing complex typed data, **When** the result is serialized, transmitted, and deserialized across instances, **Then** the data is faithfully preserved and usable by the dependent module.

---

### Edge Cases

- What happens when the master instance fails? The workers should detect master unavailability and terminate gracefully with a clear error, rather than hanging indefinitely.
- What happens when a worker receives a module assignment but the module type is not available in its assembly? The worker should report a clear error back to the master for that module.
- What happens when the coordination layer itself becomes unavailable mid-execution? Instances should detect communication failures and fail fast with a descriptive error rather than retrying silently forever.
- What happens when two instances claim the same instance index? The master should detect the conflict during registration and reject the duplicate, failing the pipeline with a clear error.
- What happens when no workers ever connect? The master executes all modules itself (it is also a worker). The pipeline completes successfully, just without the benefit of distributed parallelism.
- What happens when a module's result is too large to serialize through the coordination layer? The coordination layer should surface a clear error from its transport, and the module should be marked as failed.
- What happens when modules use non-serializable services from `IModuleContext` (e.g., local file system operations)? Modules that require local resources should be pinnable to the master instance via configuration or attributes.
- What happens when a capable worker joins after a module requiring its capabilities has already timed out and been marked as failed? The module remains failed — the master does not retroactively retry timed-out modules when new workers appear.
- What happens when one matrix-expanded instance succeeds and another fails? Each is an independent module with its own result. Downstream modules depending on the matrix group will not execute because not all instances succeeded, consistent with standard dependency failure behavior.
- What happens when a matrix-expanded module is run in single-instance (non-distributed) mode? Instances whose capability requirements are not satisfied locally are skipped automatically. Only the instance matching the local environment executes. This is identical behavior to any module with a `[RequiresCapability]` that the local instance cannot satisfy.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST support two operational roles: master (orchestrator) and worker (executor), determined at startup.
- **FR-002**: The system MUST allow role selection via command-line arguments (instance index and total count) or programmatic configuration.
- **FR-003**: The master MUST use a dynamic work queue model: it analyzes the module dependency graph, pushes modules to the ready queue as their dependencies are satisfied, and workers pull the next available module when idle. This ensures load balancing across workers with uneven module execution times.
- **FR-004**: The master MUST track the execution state of all modules across all instances and enforce dependency ordering across instance boundaries.
- **FR-005**: Workers MUST receive module assignments from the master, execute them, and report results (success, failure, or skip) back through the coordination layer.
- **FR-006**: Module results transmitted between instances MUST preserve the full `ModuleResult<T>` structure, including typed values, exceptions, skip decisions, timing data, and module metadata.
- **FR-007**: The system MUST define a set of coordination interfaces that abstract all inter-instance communication (work assignment, result reporting, health monitoring, cancellation signaling).
- **FR-008**: Users MUST be able to register custom coordination layer implementations via the existing dependency injection system.
- **FR-009**: The system MUST ship with an in-memory coordination provider suitable for testing and development.
- **FR-010**: The system MUST maintain full backward compatibility — pipelines that do not opt into distributed mode MUST behave identically to the current single-machine execution.
- **FR-011**: Workers MUST send periodic health signals (heartbeats) to the master so the master can detect unresponsive workers.
- **FR-012**: The master MUST start executing modules immediately upon startup (using itself as a worker) and MUST allow workers to join dynamically at any point during execution. The master MUST support configurable timeouts for worker heartbeats (detecting failures). Late-joining workers immediately begin pulling from the work queue.
- **FR-013**: When a worker fails, the master MUST either reassign the affected module(s) to another worker or mark them as failed, respecting the module's retry configuration.
- **FR-014**: Pipeline-wide cancellation signals MUST propagate across all instances — if the master cancels the pipeline (e.g., due to a critical module failure), all workers MUST receive the cancellation and stop executing.
- **FR-015**: The master MUST produce a unified pipeline summary at completion, aggregating results from all instances, with the same structure as the existing single-machine summary.
- **FR-016**: Users MUST be able to pin specific modules to the master instance (e.g., modules requiring local filesystem access or non-serializable context) via configuration or attributes.
- **FR-017**: The master MUST also execute modules itself (not only delegate) — the master is both an orchestrator and a worker for its assigned modules.
- **FR-018**: The distributed mode MUST respect existing module execution constraints globally across all instances — `[NotInParallel]`, `[ParallelLimiter]`, `[Priority]`, skip conditions, and timeout/retry policies MUST behave identically to single-machine mode. The master enforces these constraints when enqueuing modules, ensuring no two conflicting modules run simultaneously on any instance.
- **FR-019**: The system MUST log all distributed operations (assignments, results, failures, reassignments) with sufficient detail for debugging coordination issues.
- **FR-020**: Workers MUST advertise their capabilities (e.g., installed tools, operating system, custom tags) when registering with the master.
- **FR-021**: Modules MUST be able to declare required capabilities via configuration or attributes. A module with required capabilities MUST only be assigned to workers that advertise all of those capabilities.
- **FR-022**: Capability names MUST be user-defined strings — the framework MUST NOT restrict capabilities to a fixed set. Both built-in (e.g., OS detection) and custom capability names MUST be supported.
- **FR-023**: If no currently registered worker satisfies a module's required capabilities, the master MUST hold the module in the queue until a capable worker joins. If no capable worker joins before a configurable timeout, the module MUST be marked as failed with a clear error identifying the unsatisfied capabilities.
- **FR-024**: Modules MUST be able to declare a matrix of targets via attributes. At startup, the framework MUST expand the module definition into N concrete module instances — one per target — each automatically assigned a capability requirement matching its target.
- **FR-025**: Each expanded matrix module instance MUST be a first-class module in the dependency graph with its own standard `ModuleResult<T>`. No special aggregate result type is introduced.
- **FR-026**: Matrix expansion MUST behave identically in single-instance and distributed modes. In single-instance mode, expanded instances whose capability requirements are not satisfied locally MUST be skipped. In distributed mode, they route to capable workers.
- **FR-027**: When a downstream module depends on a matrix-expanded module, it MUST depend on all expanded instances — waiting for all to complete (or be skipped) before executing.
- **FR-028**: The module code within a matrix-expanded module MUST have access to which target it was expanded for, so it can adapt behavior if needed (e.g., OS-specific test configuration).

### Key Entities

- **Distributed Pipeline Role**: Whether an instance is operating as a master or worker. Determined at startup, immutable for the lifetime of the run.
- **Module Assignment**: A unit of work assigned by the master to a worker. Contains the module identity, its dependencies, and any configuration needed for execution.
- **Module Execution Result**: The outcome of executing a module on any instance. Contains the typed result value (or failure/skip information), timing data, and module metadata. Must be serializable.
- **Worker Registration**: A record of a worker connecting to the master. Contains the worker's instance index, advertised capabilities (e.g., "linux", "docker", "gpu", custom tags), and health status. Workers may register at any point during pipeline execution (dynamic join); the master does not wait for all workers before starting.
- **Worker Capability**: A string tag advertised by a worker describing what it can do or what environment it provides. Capabilities are user-defined and open-ended. The master uses capabilities to match modules to eligible workers.
- **Module Capability Requirement**: A set of capability strings declared by a module. The master only assigns the module to workers that advertise all required capabilities. Modules with no requirements can run on any worker.
- **Coordination Provider**: The pluggable transport layer responsible for all inter-instance communication. Implementations handle the mechanics of message delivery, discovery, and health checking.
- **Matrix Module**: A module definition with declared matrix targets. At startup, the framework expands it into N concrete module instances, one per target. Each instance is a standard module with its own `ModuleResult<T>` and an automatically applied capability requirement.
- **Work Queue**: The dynamic queue of modules ready for execution, managed by the master. Modules are enqueued as their dependencies are satisfied and dequeued by idle workers. Respects execution constraints, pinning rules, and capability/target routing.

## Assumptions

- All instances run the same version of the pipeline application with the same set of registered modules. The master does not need to handle heterogeneous module registrations across workers.
- Module result types (`T` in `Module<T>`) are JSON-serializable. The existing `ModuleResultJsonConverterFactory` already supports this. Users with non-serializable result types must pin those modules to a single instance.
- The coordination layer is responsible for its own authentication and security. The framework provides the abstraction but does not enforce transport-level security.
- Workers have access to the same external resources (source code, environment variables, secrets) as they would in a non-distributed run. The framework does not handle distributing source code or secrets to workers.
- The master performs module scheduling decisions. Workers do not make independent scheduling decisions — they execute what they are assigned.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: A pipeline with N independent modules running on M workers (M > 1) completes in less time than the same pipeline running on a single machine, with wall-clock speedup proportional to the number of workers for CPU-bound modules.
- **SC-002**: Users can enable distributed mode by adding fewer than 20 lines of configuration code to an existing pipeline, excluding the coordination provider implementation.
- **SC-003**: The distributed pipeline produces a summary report that is structurally identical to the single-machine summary — all module results, timings, and statuses are present and accurate.
- **SC-004**: When a worker fails mid-execution, the master detects the failure within 30 seconds (configurable) and takes corrective action (reassign or fail) without manual intervention.
- **SC-005**: A custom coordination provider can be implemented and integrated by a developer familiar with the framework in under a day of effort, guided by clear interface contracts and documentation.
- **SC-006**: Existing pipelines that do not opt into distributed mode experience zero behavioral or performance changes.
- **SC-007**: Module dependencies across instance boundaries are respected — no module ever executes before all its dependencies have completed and their results are available, regardless of which instance they ran on.
