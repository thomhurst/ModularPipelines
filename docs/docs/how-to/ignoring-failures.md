---
title: Ignoring Failures
---

# Ignoring Failures

Sometimes a module might throw an exception, but we simply don't care as it's not that important, or a specific error might be expected.

We can hook into a `ShouldIgnoreFailures` method and plug in any conditional logic that means "that's okay, we can ignore that error."

## Example

```csharp
public class MyModule : Module
{
    protected override Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception)
    {
        return (exception is ItemAlreadyExistsException).AsTask();
    }

    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Do something
    }
}
```