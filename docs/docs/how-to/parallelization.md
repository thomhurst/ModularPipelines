---
title: Parallelization
---

# Parallelization Disabling

During normal scheduling, modules run in parallel by default once their dependencies have completed, subject to the pipeline's concurrency limits.

Use `NotInParallel` when modules access a resource that cannot safely be shared. For example, application installers may need exclusive access to the machine.

Without constraint keys, `[NotInParallel]` prevents a module from running alongside any other module. Its dependencies must still complete before it can run during normal scheduling.

`AlwaysRun` modules can start during failure cleanup without waiting for pending dependencies that may never complete. Do not rely on those dependencies having completed when writing cleanup work.

## Example

```csharp
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines;

[NotInParallel]
public class MyModule : Module
{
    protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Perform work that must not overlap with any other module.
        return Task.CompletedTask;
    }
}
```

Pass constraint keys to the `NotInParallel` constructor to restrict exclusion to modules with a shared key.
Modules with different keys can run in parallel, unless another constraint prevents it.

## Example

```csharp
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines;

[NotInParallel("Install")]
public class InstallModule1 : Module
{
    protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Install the first application here.
        return Task.CompletedTask;
    }
}

[NotInParallel("Install")]
public class InstallModule2 : Module
{
    protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Install the second application here.
        return Task.CompletedTask;
    }
}

[NotInParallel("Build")]
public class BuildProjectModule : Module
{
    protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Build the project here.
        return Task.CompletedTask;
    }
}
```

`InstallModule1` and `InstallModule2` cannot run at the same time. Either can run alongside `BuildProjectModule`.
You can also pass multiple keys, such as `[NotInParallel("Install", "Build")]`, to exclude modules sharing either key.

# Parallel Limiter

Use `[ParallelLimiter<TParallelLimit>]` to cap the number of modules that can run at the same time with a shared limit type.
The type must implement `IParallelLimit`, which declares `static abstract int Limit { get; }`.
Implement it with a public static `int Limit` property that returns a positive limit. No instance or public parameterless constructor is required.

The limit is shared by modules using the same limit type within a pipeline. Modules without a limiter and modules using a different limit type can run alongside them, subject to their own constraints and the pipeline's concurrency limits.

The attribute applies only to module classes and is inherited by derived module classes.
You can configure the same limit fluently:

```csharp
protected override void Configure(ModuleConfigurationBuilder module) => module
    .WithParallelLimiter<MyParallelLimit>();
```

Fluent configuration overrides a limiter attribute, including an inherited attribute. Repeated
`WithParallelLimiter<TParallelLimit>()` calls use the last limit type. Attribute and fluent
configuration share the same semaphore for the same limit type, and planning uses that same limit.
The effective type is available through `IModule.Configuration.ParallelLimiterType`.

## Example

This is a complete `Program.cs` for a .NET console application with a reference to `ModularPipelines`.
It registers three modules that share `MyParallelLimit`, so at most two of them execute at once.
Replace the delays with your module work.

```csharp
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines;
using ModularPipelines.Attributes;

var builder = Pipeline.CreateBuilder(args);
builder.AddModule<InstallModule1>();
builder.AddModule<InstallModule2>();
builder.AddModule<BuildProjectModule>();

await builder.RunAsync();

[ParallelLimiter<MyParallelLimit>]
public class InstallModule1 : Module
{
    protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return Task.Delay(1000, cancellationToken);
    }
}

[ParallelLimiter<MyParallelLimit>]
public class InstallModule2 : Module
{
    protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return Task.Delay(1000, cancellationToken);
    }
}

[ParallelLimiter<MyParallelLimit>]
public class BuildProjectModule : Module
{
    protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return Task.Delay(1000, cancellationToken);
    }
}

public sealed class MyParallelLimit : IParallelLimit
{
    public static int Limit => 2;
}
```
