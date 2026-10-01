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
| **Master** | `Role == Master`, or `Role == Auto` and `InstanceIndex == 0` | Builds the dependency graph, enqueues modules to the work queue, collects results, and produces the final pipeline summary. Also participates as a worker, dequeuing and executing modules from the same queue. |
| **Worker** | `Role == Worker`, or `Role == Auto` and `InstanceIndex > 0` | Registers with the coordinator, dequeues modules that match its capabilities, executes them, and publishes results back. |

`DistributedOptions.Role` defaults to `Auto`, which derives the role from `InstanceIndex`. Set it to `Master` or `Worker` when the process role must be explicit.

### Coordinator

The coordinator is the shared communication layer between master and workers. It handles work queuing, leases, result publication, worker registration, heartbeats, and cancellation signals. Choose one backend:

- **Redis** — `AddRedisDistributedCoordinator` from the `ModularPipelines.Distributed.Redis` package. Every process talks to a shared Redis server.
- **SignalR** — `AddSignalRDistributedCoordinator` from the `ModularPipelines.Distributed.SignalR` package. The master hosts a SignalR hub and workers connect to it directly, optionally through a cloudflared tunnel and Redis-based discovery.
- **Custom** — implement `IDistributedMasterCoordinator` and register it with `AddDistributedCoordinator<T>()` or `AddDistributedCoordinatorFactory<T>()`.

Register exactly one backend; building the pipeline fails when several are registered. A run with `TotalInstances` greater than one fails at startup when no backend is registered, instead of leaving workers waiting forever. Single-process runs use a built-in process-local coordinator automatically.

### Leases

A worker claims each module under a lease tied to its `WorkerId`. Heartbeats list the modules the worker is executing and renew their leases. When a worker crashes or loses its connection for longer than `WorkerTimeout`, its leases expire and the master returns those modules to the queue, so a lost worker costs seconds rather than the module result timeout. The first result published for a module is final.

Workers also watch the master. When the master exits, fails or crashes without signalling completion, each worker cancels its in-flight modules and fails rather than waiting for work that will never come. SignalR workers react when their connection to the master closes for good; Redis workers react when the master's heartbeat is older than `MasterTimeout`.

### Capabilities

Workers advertise what they can do through typed values such as `Capability.Linux`, `Capability.Docker`, and `Capability.Gpu`. Modules declare what they need via `[RequiresCapability]` and `[RequiresAnyCapability]` attributes using compile-time `Capability.Names` constants, or through run conditions such as `[RunIf<OnLinux>]`. The coordinator only assigns a module to a worker that satisfies its `CapabilityRequirement`.

Every instance automatically advertises its operating system through the matching well-known capability. Register an `ICapabilityProvider` to detect more.

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
4. Workers send periodic **heartbeats** that renew the leases on their in-flight modules; the master requeues modules whose leases expire.
5. When a module fails, the master immediately broadcasts a `PipelineFailed` cancellation: workers cancel non-AlwaysRun modules and only claim AlwaysRun work, and the master withdraws queued non-AlwaysRun assignments. A user stop broadcasts `Stopped`, which cancels everything.

## Packages

| Package | Purpose |
|---------|---------|
| `ModularPipelines` | Core distributed abstractions, master/worker executors, and the capability system. |
| `ModularPipelines.Distributed.Redis` | Redis coordinator, artifact store, and module cache. |
| `ModularPipelines.Distributed.SignalR` | SignalR coordinator: the master hosts the hub that workers connect to. |
| `ModularPipelines.Distributed.Discovery.Redis` | Advertises the SignalR master's endpoint and access token through Redis. |
| `ModularPipelines.Distributed.Artifacts.S3` | S3-compatible artifact store. |

## Next Steps

- [Getting Started](./getting-started) — set up a distributed pipeline with Redis in minutes.
- [Configuration](./configuration) — all options for distributed mode and the Redis coordinator.
- [Capabilities and Routing](./capabilities) — control which workers execute which modules.
- [CI Example: GitHub Actions](./github-actions) — a complete matrix runner example.
