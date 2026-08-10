---
title: Timeouts
---

# Timeouts

Modules have a 30-minute timeout by default. Configure the pipeline default when your workloads need a different limit, or use `TimeSpan.Zero` to disable it:

```csharp
var builder = Pipeline.CreateBuilder();
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

`AlwaysRun` teardown has a separate 30-second scheduler-progress watchdog. This prevents a
constraint-deferred `AlwaysRun` module from waiting indefinitely for a hung active module, even
when ordinary module timeouts are disabled. Configure it independently when needed:

```csharp
builder.ConfigurePipelineOptions(options => options with
{
    AlwaysRunProgressTimeout = TimeSpan.FromMinutes(1),
});
```

Set `AlwaysRunProgressTimeout` to `TimeSpan.Zero` only when an unlimited teardown wait is intentional.
The timeout is one cumulative budget for the entire `AlwaysRun` teardown wait, not a fresh budget
for each retry, so increase it for pipelines whose blocking modules can legitimately run longer.

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

When a timeout occurs:

- The `CancellationToken` passed to `ExecuteAsync` will be cancelled
- The module will fail with a `ModuleTimeoutException`
- If retry policies are configured, the module may be retried
- If `WithIgnoreFailures()` is configured, the pipeline will continue despite the timeout
