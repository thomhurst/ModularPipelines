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
