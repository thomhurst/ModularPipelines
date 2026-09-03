# Distributed Mode

Distributed mode lets you split a ModularPipelines pipeline across multiple processes or machines. Instead of running every module in a single process, work is divided between a **master** that orchestrates and one or more **workers** that execute modules.

## Why Distributed?[​](#why-distributed "Direct link to Why Distributed?")

Some pipelines have modules that must run on different operating systems, require specialized hardware, or simply take too long to run sequentially on a single machine. Distributed mode solves this by fanning work out across CI matrix runners, multiple containers, or separate machines, while keeping the same module code and dependency graph.

## Key Concepts[​](#key-concepts "Direct link to Key Concepts")

### Roles[​](#roles "Direct link to Roles")

Every pipeline instance runs in one of two roles:

| Role       | Determined by                                                | Responsibility                                                                                                                                                                                                  |
| ---------- | ------------------------------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Master** | `Role == Master`, or `Role == Auto` and `InstanceIndex == 0` | Builds the dependency graph, enqueues modules to the work queue, collects results, and produces the final pipeline summary. Also participates as a worker, dequeuing and executing modules from the same queue. |
| **Worker** | `Role == Worker`, or `Role == Auto` and `InstanceIndex > 0`  | Registers with the coordinator, dequeues modules that match its capabilities, executes them, and publishes results back.                                                                                        |

`DistributedOptions.Role` defaults to `Auto`, which derives the role from `InstanceIndex`. Set it to `Master` or `Worker` when the process role must be explicit.

### Coordinator[​](#coordinator "Direct link to Coordinator")

The coordinator is the shared communication layer between master and workers. It handles work queuing, result publication, worker registration, heartbeats, and cancellation signals. ModularPipelines ships with two coordinator implementations:

* **InMemoryDistributedCoordinator** — for single-process testing only.
* **RedisDistributedCoordinator** — production-ready, uses Redis for cross-process coordination. Provided by the `ModularPipelines.Distributed.Redis` package.

### Capabilities[​](#capabilities "Direct link to Capabilities")

Workers advertise what they can do through typed values such as `Capability.Linux`, `Capability.Docker`, and `Capability.Gpu`. Modules declare what they need via `[RequiresCapability]` attributes using compile-time `Capability.Names` constants. The coordinator only assigns a module to a worker that has all required capabilities.

If `AutoDetectOsCapability` is enabled (the default), workers automatically advertise their operating system through the matching well-known capability.

## Architecture Diagram[​](#architecture-diagram "Direct link to Architecture Diagram")

```
┌─────────────────────────────────────────────────────┐

│                      Redis                          │

│  ┌──────────┐  ┌──────────┐  ┌───────────────────┐  │

│  │Work Queue│  │ Results  │  │ Workers/Heartbeats│  │

│  └────▲─────┘  └────┬─────┘  └───────────────────┘  │

│       │              │                               │

└───────┼──────────────┼───────────────────────────────┘

        │              │

   ┌────┴──────────────┴────┐

   │         Master         │

   │  enqueue ─── collect   │

   │  dequeue ─── execute   │

   └────────────────────────┘



   ┌─────────┐    ┌─────────┐    ┌─────────┐

   │Worker 1 │    │Worker 2 │    │Worker 3 │

   │ dequeue │    │ dequeue │    │ dequeue │

   │ execute │    │ execute │    │ execute │

   │ publish │    │ publish │    │ publish │

   └─────────┘    └─────────┘    └─────────┘
```

1. The **master** builds the module graph, then enqueues each module as a `ModuleAssignment` into the work queue.
2. **All instances** (master and workers) poll the queue, pick up assignments that match their capabilities, execute the module, and publish the serialized result. The master participates as a worker alongside external workers.
3. The **master** waits for each result, deserializes it, and feeds it back into the dependency graph so downstream modules can proceed.
4. Workers send periodic **heartbeats** so the master can detect failures.
5. Either side can broadcast a **cancellation signal** to stop all instances.

## Packages[​](#packages "Direct link to Packages")

| Package                              | Purpose                                                                                                                                     |
| ------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------- |
| `ModularPipelines.Distributed`       | Core distributed abstractions, master/worker executors, capability system. Referenced automatically by the main `ModularPipelines` package. |
| `ModularPipelines.Distributed.Redis` | Redis-based coordinator implementation. Add this to your pipeline project.                                                                  |

## Next Steps[​](#next-steps "Direct link to Next Steps")

* [Getting Started](/ModularPipelines/docs/next/distributed/getting-started.md) — set up a distributed pipeline with Redis in minutes.
* [Configuration](/ModularPipelines/docs/next/distributed/configuration.md) — all options for distributed mode and the Redis coordinator.
* [Capabilities and Routing](/ModularPipelines/docs/next/distributed/capabilities.md) — control which workers execute which modules.
* [CI Example: GitHub Actions](/ModularPipelines/docs/next/distributed/github-actions.md) — a complete matrix runner example.
