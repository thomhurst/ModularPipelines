---
title: Parallelization
---

# Parallelization

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
