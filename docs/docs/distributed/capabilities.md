---
title: Capabilities and Routing
sidebar_position: 4
---

# Capabilities and Routing

Not every worker can execute every module. Some modules need Docker, others need a specific OS. The capability system controls how modules are routed to the right worker.

`Capability`, `CapabilityRequirement`, `ICapabilityProvider`, and the capability builder extensions live in the root `ModularPipelines` namespace. Local pipelines can use them with only `using ModularPipelines;`; distributed execution is optional.

## Worker Capabilities

Workers advertise typed `Capability` values when they register with the coordinator. Built-in values provide discoverable names; create custom capabilities explicitly with `new Capability("name")` or an explicit `(Capability) "name"` cast. The conversion is explicit because it validates the name and throws for an empty or whitespace name.

```csharp
using ModularPipelines;

builder.AddCapabilities(Capability.Docker, Capability.Gpu, new Capability("high-memory"));
```

`AddCapabilities` works with or without distributed mode. `DistributedOptions.Capabilities` is equivalent when you already configure `AddDistributedMode`.

### Auto-Detected OS Capability

Every instance always advertises its current operating system; no configuration is needed:

- Windows runners advertise `Capability.Windows`
- Linux runners advertise `Capability.Linux`
- macOS runners advertise `Capability.MacOS`
- FreeBSD runners advertise `Capability.FreeBSD`

Attribute arguments must be compile-time constants, so use the corresponding `Capability.Names` values. For example, modules with `[RequiresCapability(Capability.Names.Linux)]` only run on Linux workers without extra configuration.

### Detecting Custom Capabilities

The OS capability comes from a built-in `ICapabilityProvider`. Register your own providers to detect other capabilities at startup. Each process's capabilities are the union of all providers, `AddCapabilities`, and `DistributedOptions.Capabilities`. Providers run once per process:

```csharp
public sealed class GpuCapabilityProvider : ICapabilityProvider
{
    public Task<IEnumerable<Capability>> GetCapabilitiesAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IEnumerable<Capability>>(
            File.Exists("/dev/nvidia0") ? [Capability.Gpu] : []);
}

builder.AddCapabilityProvider<GpuCapabilityProvider>();
```

Docker and GPU support are not detected automatically, because the presence of a binary or device does not prove the capability is usable. Advertise them explicitly or with a provider.

### Routing from Run Conditions

`OnLinux`, `OnWindows`, `OnMacOS`, and `OnFreeBSD` implement `ICapabilityCondition`. When a module uses one of them in `[RunIf]`, `[RunIfAny]`, or a `ConditionGroup`, the framework translates the condition into a capability requirement instead of evaluating it on the master. This keeps the attribute set DRY — you don't need to add both `[RunIf<OnLinux>]` and `[RequiresCapability(Capability.Names.Linux)]` to the same module.

- `[RunIf<OnLinux>]` requires `linux`.
- `[RunIfAny<OnLinux, OnMacOS>]` and `[RunIf<OnUnix>]` require `linux` **or** `macos`.
- `[RunIf<OnLinux, OnGpu>]` requires `linux` **and** the custom condition's capability.

Implement `ICapabilityCondition` to make your own conditions routable:

```csharp
public sealed class OnGpu : ICapabilityCondition
{
    public Capability Capability => Capability.Gpu;
}
```

The default `IRunCondition.EvaluateAsync` implementation checks the executing process's declared
capabilities. Declare GPU support with `builder.AddCapabilities(Capability.Gpu)`, or register an
`ICapabilityProvider` to detect it. The provider's result is shared by routing and local evaluation;
put hardware detection there instead of duplicating it in the condition.

An explicit `EvaluateAsync` implementation is called during local or worker execution and can
reject a module even on a worker with the required capability. It is **not** called by the
master when constructing a route: the master reads `Capability` and preserves the condition's
AND/OR composition. Use an ordinary `IRunCondition` for a runtime predicate that should not
also constrain worker capabilities.

```csharp
// The "linux" capability is auto-detected — no [RequiresCapability] needed
[RunIf<OnLinux>]
public class LinuxBuildModule : Module<string>
{
    protected override async Task<string> ExecuteAsync(
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
[RequiresCapability(Capability.Names.Docker)]
public class DockerBuildModule : Module<string>
{
    protected override async Task<string> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Only executes on workers that advertise "docker"
        await context.Tools.Docker.BuildAsync(new());
        return "built";
    }
}
```

### Multiple Capabilities

Pass multiple names to one attribute or stack attributes. Both forms require **all** declared capabilities:

```csharp
[RequiresCapability(Capability.Names.Linux, Capability.Names.Docker)]
public class LinuxDockerModule : Module<string>
{
    protected override async Task<string> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Only runs on Linux workers that also have Docker
        return "done";
    }
}
```

### Alternative Capabilities

Use `[RequiresAnyCapability]` when any one of several capabilities is enough. Each attribute adds one group of alternatives, and every group must be satisfied:

```csharp
// Runs on a Linux or macOS worker that also has Docker
[RequiresAnyCapability(Capability.Names.Linux, Capability.Names.MacOS)]
[RequiresCapability(Capability.Names.Docker)]
public class UnixDockerModule : Module<string> { ... }
```

### Running Without Distributed Mode

Capability requirements apply everywhere, just like `[RunIf<OnLinux>]`. When a module would run locally (a normal single-machine run, a worker, or the master running its own share of the work), and this machine's capabilities do not satisfy the module's `[RequiresCapability]` or `[RequiresAnyCapability]` requirement, the module is **skipped** and the skip reason names the missing capabilities. In distributed mode the master instead routes the module to a worker that satisfies it.

```csharp
// Skipped on a machine that has not declared docker
[RequiresCapability(Capability.Names.Docker)]
public class DockerBuildModule : Module<string> { ... }

// Runs locally once docker is declared
builder.AddCapabilities(Capability.Docker);
```

### No Capabilities

Modules without `[RequiresCapability]` can run on any worker. They have no routing restrictions.

## Capability Matching Rules

The matching logic is straightforward:

1. If a module has **no** required capabilities, it can run on **any** worker.
2. A module's requirement is a `CapabilityRequirement`: a list of clauses. Every clause must be satisfied, and a worker satisfies a clause when it advertises **at least one** of the clause's capabilities. For example, `docker & (linux | macos)`.
3. Capability matching is **case-insensitive**.
4. A worker runs one operating system, so a module whose requirements need two different operating systems is skipped as impossible. This applies whether the conflict comes from run conditions (`[RunIf<OnLinux>]` with `[RunIf<OnWindows>]`) or declared capabilities (`[RequiresCapability(Capability.Names.Linux, Capability.Names.Windows)]`).
5. If no registered worker has the required capabilities, only that module waits, and it is not queued until a capable worker registers. After `WorkerRegistrationTimeout`, it fails with a routing error that lists the missing route instead of waiting for the module-result timeout.

## Example: Mixed Pipeline

```csharp
// Runs on any worker (including the master)
public class RestoreModule : Module<string> { ... }

// Only on Linux workers (auto-detected from [RunIf<OnLinux>])
[RunIf<OnLinux>]
[DependsOn<RestoreModule>]
public class LinuxBuildModule : Module<string> { ... }

// Only on Windows workers (auto-detected from [RunIf<OnWindows>])
[RunIf<OnWindows>]
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
