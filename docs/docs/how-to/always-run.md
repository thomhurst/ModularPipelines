---
title: Always Run
---

# Always Run

## Module Run Types

There are two module run types.
- AlwaysRun,
- OnSuccessfulDependencies,

By default, modules use `OnSuccessfulDependencies`. This means that if a module is dependent on another module, and that other module fails, then the module waiting will not start and will abort.

If you switch to `AlwaysRun`, then regardless of if any dependencies failed, the module will still run. This can be useful when you want to clean up resources, regardless of whether the pipeline passed or failed.

## Example

```csharp
public class MyModule : Module
{
    public override ModuleRunType ModuleRunType => ModuleRunType.AlwaysRun;

    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Do something
    }
}
```