# Capabilities and Routing

Not every worker can execute every module. Some modules need Docker, others need a specific OS. The capability system controls how modules are routed to the right worker.

## Worker Capabilities[​](#worker-capabilities "Direct link to Worker Capabilities")

Workers advertise typed `Capability` values when they register with the coordinator. Built-in values provide discoverable names, while implicit string conversion still supports custom capabilities.

```
builder.AddDistributedMode(o =>

{

    o.InstanceIndex = 1;

    o.TotalInstances = 4;

    o.Capabilities = [Capability.Docker, Capability.Gpu, "high-memory"];

});
```

### Auto-Detected OS Capability[​](#auto-detected-os-capability "Direct link to Auto-Detected OS Capability")

By default, `AutoDetectOsCapability` is `true`, which automatically adds the current operating system as a capability:

* Windows runners advertise `Capability.Windows`
* Linux runners advertise `Capability.Linux`
* macOS runners advertise `Capability.MacOS`
* FreeBSD runners advertise `Capability.FreeBSD`

Attribute arguments must be compile-time constants, so use the corresponding `Capability.Names` values. For example, modules with `[RequiresCapability(Capability.Names.Linux)]` only run on Linux workers without extra configuration.

### Auto-Detected OS from Platform Conditions[​](#auto-detected-os-from-platform-conditions "Direct link to Auto-Detected OS from Platform Conditions")

When a module has a `[RunIf<OnLinux>]`, `[RunIf<OnWindows>]`, `[RunIf<OnMacOS>]`, or `[RunIf<OnFreeBSD>]` attribute, the framework automatically adds the corresponding OS capability requirement to its assignment. This keeps the attribute set DRY — you don't need to add both `[RunIf<OnLinux>]` and `[RequiresCapability(Capability.Names.Linux)]` to the same module.

```
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

## RequiresCapability Attribute[​](#requirescapability-attribute "Direct link to RequiresCapability Attribute")

Mark a module with `[RequiresCapability]` to restrict which workers can execute it. The module will only be assigned to workers that have **all** required capabilities.

```
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

### Multiple Capabilities[​](#multiple-capabilities "Direct link to Multiple Capabilities")

Pass multiple names to one attribute or stack attributes. Both forms require **all** declared capabilities:

```
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

### No Capabilities[​](#no-capabilities "Direct link to No Capabilities")

Modules without `[RequiresCapability]` can run on any worker. They have no routing restrictions.

## Capability Matching Rules[​](#capability-matching-rules "Direct link to Capability Matching Rules")

The matching logic is straightforward:

1. If a module has **no** required capabilities, it can run on **any** worker.
2. If a module has required capabilities, **all** of them must be present in the worker's capability set.
3. Capability matching is **case-insensitive**.
4. If no worker with the required capabilities is available, the module waits in the queue until one becomes available (up to `CapabilityTimeout`).

## Example: Mixed Pipeline[​](#example-mixed-pipeline "Direct link to Example: Mixed Pipeline")

```
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
