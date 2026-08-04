---
title: Timeouts
---

# Timeouts

Modules have a 30-minute timeout by default. Configure the pipeline default when your workloads need a different limit, or use `TimeSpan.Zero` to disable it:

```csharp
using var builder = Pipeline.CreateBuilder();
builder.ConfigurePipelineOptions(options => options with
{
    DefaultModuleTimeout = TimeSpan.FromHours(2),
});

// Disable the default. Per-module timeouts still apply.
builder.ConfigurePipelineOptions(options => options with
{
    DefaultModuleTimeout = TimeSpan.Zero,
});
```

You can override the pipeline default for one module using `Configure()`. Bear in mind some build runners, like GitHub Actions, have their own timeouts, so extending past these won't help.

## Using ModuleConfiguration

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithTimeout(TimeSpan.FromSeconds(120))
        .Build();

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
        .WithRetry(3)  // Retry if timeout or other failure occurs
        .WithIgnoreFailures()  // Don't fail the pipeline if module times out
        .Build();

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Long-running operation with timeout protection
    }
}
```

## Timeout Behavior

Timeouts apply to each execution attempt. With retries enabled, every attempt receives the full
timeout and retry backoff delays do not consume it. A module configured with a five-minute timeout
and three retries can therefore spend up to five minutes in each of its four attempts, plus retry
delays.

When a timeout occurs:

- The `CancellationToken` passed to `ExecuteAsync` will be cancelled
- The module will fail with a `ModuleTimeoutException`
- If retry policies are configured and the attempt stops within the cancellation grace period,
  the module may be retried. An attempt that remains active after the grace period is never retried,
  preventing concurrent executions of the same module instance.
- If `WithIgnoreFailures()` is configured, the pipeline will continue despite the timeout
