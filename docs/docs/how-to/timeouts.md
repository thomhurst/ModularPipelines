---
title: Timeouts
---

# Timeouts

When creating modules, you can set a timeout per module using the `Configure()` method. You can set this to any timespan you like. Just bear in mind some build runners, like GitHub Actions, have their own timeouts, so extending past these won't help.

## Using ModuleConfiguration

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithTimeout(TimeSpan.FromSeconds(120))
        .Build();

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Do something - will be cancelled after 120 seconds
    }
}
```

## Combining with Other Behaviors

Timeouts can be combined with other module behaviors:

```csharp
public class ResilientModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithTimeout(TimeSpan.FromMinutes(5))
        .WithRetryCount(3)  // Retry if timeout or other failure occurs
        .WithIgnoreFailures()  // Don't fail the pipeline if module times out
        .Build();

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Long-running operation with timeout protection
    }
}
```

## Timeout Behavior

When a timeout occurs:
- The `CancellationToken` passed to `ExecuteAsync` will be cancelled
- The module will fail with a `TimeoutException`
- If retry policies are configured, the module may be retried
- If `WithIgnoreFailures()` is configured, the pipeline will continue despite the timeout
