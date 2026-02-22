---
title: Overview
sidebar_position: 1
---

# Distributed Mode

Distributed mode lets you split a ModularPipelines pipeline across multiple processes or machines. Instead of running every module in a single process, work is divided between a **master** that orchestrates and one or more **workers** that execute modules.

## Why Distributed?

Some pipelines have modules that must run on different operating systems, require specialized hardware, or simply take too long to run sequentially on a single machine. Distributed mode solves this by fanning work out across CI matrix runners, multiple containers, or separate machines, while keeping the same module code and dependency graph.

## Key Concepts

### Roles

Every pipeline instance runs in one of two roles:

| Role | Determined by | Responsibility |
|------|--------------|----------------|
| **Master** | `InstanceIndex == 0` | Builds the dependency graph, enqueues modules to the work queue, collects results, and produces the final pipeline summary. |
| **Worker** | `InstanceIndex > 0` | Registers with the coordinator, dequeues modules that match its capabilities, executes them, and publishes results back. |

The role is detected automatically from `DistributedOptions.InstanceIndex`. You can also override it with the `MODULAR_PIPELINES_INSTANCE` environment variable.

### Coordinator

The coordinator is the shared communication layer between master and workers. It handles work queuing, result publication, worker registration, heartbeats, and cancellation signals. ModularPipelines ships with two coordinator implementations:

- **InMemoryDistributedCoordinator** — for single-process testing only.
- **RedisDistributedCoordinator** — production-ready, uses Redis for cross-process coordination. Provided by the `ModularPipelines.Distributed.Redis` package.

### Capabilities

Workers advertise what they can do (e.g. `"linux"`, `"docker"`, `"gpu"`). Modules declare what they need via `[RequiresCapability]` attributes. The coordinator only assigns a module to a worker that has all required capabilities.

If `AutoDetectOsCapability` is enabled (the default), workers automatically advertise their operating system (`"windows"`, `"linux"`, or `"macos"`).

### Pin to Master

Some modules should never leave the master process — for example, modules that aggregate results or produce a final summary. Mark these with `[PinToMaster]` and they will execute locally on the master, skipping the work queue entirely.

## Architecture Diagram

```
┌─────────────────────────────────────────────────────┐
│                      Redis                          │
│  ┌──────────┐  ┌──────────┐  ┌───────────────────┐  │
│  │Work Queue│  │ Results  │  │ Workers/Heartbeats│  │
│  └────▲─────┘  └────┬─────┘  └───────────────────┘  │
│       │              │                               │
└───────┼──────────────┼───────────────────────────────┘
        │              │
   ┌────┴────┐    ┌────┴────┐
   │  Master │    │  Master │
   │ enqueue │    │ collect │
   └─────────┘    └─────────┘

   ┌─────────┐    ┌─────────┐    ┌─────────┐
   │Worker 1 │    │Worker 2 │    │Worker 3 │
   │ dequeue │    │ dequeue │    │ dequeue │
   │ execute │    │ execute │    │ execute │
   │ publish │    │ publish │    │ publish │
   └─────────┘    └─────────┘    └─────────┘
```

1. The **master** builds the module graph, then enqueues each module as a `ModuleAssignment` into the work queue.
2. **Workers** poll the queue, pick up assignments that match their capabilities, execute the module, and publish the serialized result.
3. The **master** waits for each result, deserializes it, and feeds it back into the dependency graph so downstream modules can proceed.
4. Workers send periodic **heartbeats** so the master can detect failures.
5. Either side can broadcast a **cancellation signal** to stop all instances.

## Packages

| Package | Purpose |
|---------|---------|
| `ModularPipelines.Distributed` | Core distributed abstractions, master/worker executors, capability system. Referenced automatically by the main `ModularPipelines` package. |
| `ModularPipelines.Distributed.Redis` | Redis-based coordinator implementation. Add this to your pipeline project. |

## Next Steps

- [Getting Started](./getting-started) — set up a distributed pipeline with Redis in minutes.
- [Configuration](./configuration) — all options for distributed mode and the Redis coordinator.
- [Capabilities and Routing](./capabilities) — control which workers execute which modules.
- [CI Example: GitHub Actions](./github-actions) — a complete matrix runner example.
