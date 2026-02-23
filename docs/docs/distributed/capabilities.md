---
title: Capabilities and Routing
sidebar_position: 4
---

# Capabilities and Routing

Not every worker can execute every module. Some modules need Docker, others need a specific OS. The capability system controls how modules are routed to the right worker.

## Worker Capabilities

Workers advertise their capabilities when they register with the coordinator. Capabilities are simple strings that describe what the worker can do.

```csharp
builder.AddDistributedMode(o =>
{
    o.InstanceIndex = 1;
    o.TotalInstances = 4;
    o.Capabilities = new List<string> { "docker", "gpu" };
});
```

### Auto-Detected OS Capability

By default, `AutoDetectOsCapability` is `true`, which automatically adds the current operating system as a capability:

- Windows runners advertise `"windows"`
- Linux runners advertise `"linux"`
- macOS runners advertise `"macos"`

This means modules with `[RequiresCapability("linux")]` will only run on Linux workers without any extra configuration.

### Auto-Detected OS from RunOn*Only Attributes

When a module has a `[RunOnLinuxOnly]`, `[RunOnWindowsOnly]`, or `[RunOnMacOSOnly]` attribute, the framework automatically adds the corresponding OS capability requirement to its assignment. This keeps the attribute set DRY — you don't need to add both `[RunOnLinuxOnly]` and `[RequiresCapability("linux")]` to the same module.

```csharp
// The "linux" capability is auto-detected — no [RequiresCapability] needed
[RunOnLinuxOnly]
public class LinuxBuildModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Only executes on workers that have the "linux" capability
        return "built on linux";
    }
}
```

## RequiresCapability Attribute

Mark a module with `[RequiresCapability]` to restrict which workers can execute it. The module will only be assigned to workers that have **all** required capabilities.

```csharp
[RequiresCapability("docker")]
public class DockerBuildModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Only executes on workers that advertise "docker"
        await context.Docker().Build(new());
        return "built";
    }
}
```

### Multiple Capabilities

You can stack multiple attributes. The module will only run on a worker that has **all** of them:

```csharp
[RequiresCapability("linux")]
[RequiresCapability("docker")]
public class LinuxDockerModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Only runs on Linux workers that also have Docker
        return "done";
    }
}
```

### No Capabilities

Modules without `[RequiresCapability]` can run on any worker. They have no routing restrictions.

## MatrixTarget Attribute

The `[MatrixTarget]` attribute is designed for modules that need to run once per target value — for example, building for multiple operating systems or configurations.

```csharp
[MatrixTarget("windows", "linux", "macos")]
public class PlatformBuildModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        return "built";
    }
}
```

When the master encounters a matrix module, it expands it into one assignment per target. Each expanded instance gets a capability requirement matching its target value, so `"windows"` is sent to a worker with the `"windows"` capability, `"linux"` to a Linux worker, and so on.

## Capability Matching Rules

The matching logic is straightforward:

1. If a module has **no** required capabilities, it can run on **any** worker.
2. If a module has required capabilities, **all** of them must be present in the worker's capability set.
3. Capability matching is **case-insensitive**.
4. If no worker with the required capabilities is available, the module waits in the queue until one becomes available (up to `CapabilityTimeoutSeconds`).

## Example: Mixed Pipeline

```csharp
// Runs on any worker (including the master)
public class RestoreModule : Module<string> { ... }

// Only on Linux workers (auto-detected from [RunOnLinuxOnly])
[RunOnLinuxOnly]
[DependsOn<RestoreModule>]
public class LinuxBuildModule : Module<string> { ... }

// Only on Windows workers
[RequiresCapability("windows")]
[DependsOn<RestoreModule>]
public class WindowsBuildModule : Module<string> { ... }

// Aggregates results — runs on any available worker
[DependsOn<LinuxBuildModule>]
[DependsOn<WindowsBuildModule>]
public class PublishModule : Module<string> { ... }
```

In this pipeline:
1. `RestoreModule` is enqueued and any available worker (including the master) picks it up.
2. Once restore completes, `LinuxBuildModule` is enqueued for a Linux worker and `WindowsBuildModule` for a Windows worker. These run in parallel on different machines.
3. Once both builds complete, `PublishModule` is enqueued and any available worker picks it up.
