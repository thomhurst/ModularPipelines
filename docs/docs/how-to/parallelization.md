---
title: Parallelization
---

# Parallelization Disabling

As mentioned, modules will try to run in parallel by default, waiting on any dependencies if they need to.

However, sometimes modules don't have any dependencies, but also it isn't a good idea trying to run them in parallel.

As example of this could be installing applications. Windows for instance, doesn't like you trying to install multiple applications at the same time.

So if you want any modules to be run without parallelisation, there is the `NotInParallel` attribute.

These modules will attempt to run first, before all other modules that can be run in parallel.
If these have any dependencies, they will be triggered too.

## Example

```csharp
[NotInParallel]
public class MyModule : Module
{
    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Do something
    }
}
```

Not `NotInParallel` attribute can also take a `ConstraintKey` parameter.
If this is set, then a module will not run in parallel with other modules containing the same constraint key.
If another module has a different constraint key, these will still run in parallel.

## Example

```csharp
[NotInParallel(ConstraintKey = "Install")]
public class InstallModule1 : Module
{
    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Do something
    }
}

[NotInParallel(ConstraintKey = "Install")]
public class InstallModule2 : Module
{
    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Do something
    }
}

[NotInParallel(ConstraintKey = "Build")]
public class BuildProjectModule : Module
{
    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Do something
    }
}
```

In the above example, `InstallModule1` and `InstallModule2` will not run at the same time. However, either of them could run at the same time as `BuildProjectModule`.

# Parallel Limiter

ModularPipelines allows the user to control the parallel limit on specific modules.

To do this, we add a `[ParallelLimiter<>]` attribute.

You'll notice this has a generic type argument - You must give it a type that implements `IParallelLimit` and has a public empty constructor. That interface requires you to define what the limit is for those modules.

If a module doesn't have a parallel limit defined, it'll try and eagerly run when the .NET thread pool allows it to do so.

If it does have a parallel limit defined, be aware that that parallel limit is shared for any modules with that same `Type` of parallel limit. 

In the example below, `MyParallelLimit` has a limit of `2`. Now any module that has this parallel limit attribute applied to it, will only be processed 2 at a time. 

Other modules without this attribute may run alongside them still. 

And other modules with a different `Type` of parallel limit may also run alongside them still, but limited amongst themselves by their shared `Type` and limit.

So be aware that limits are only shared among modules with that same `IParallelLimit` `Type`.

## Example

```csharp
using TUnit.Core;

namespace MyTestProject;

[ParallelLimiter<MyParallelLimit>]
public class InstallModule1 : Module
{
    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Do something
    }
}

[ParallelLimiter<MyParallelLimit>]
public class InstallModule2 : Module
{
    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Do something
    }
}

[ParallelLimiter<MyParallelLimit>]
public class BuildProjectModule : Module
{
    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Do something
    }
}

public record MyParallelLimit : IParallelLimit
{
    public int Limit => 2;
}
```

## Caveats
If a test uses `[DependsOn(nameof(OtherTest))]` and the other test has its own different parallel limit, this isn't guaranteed to be honoured.