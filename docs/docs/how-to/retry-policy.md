---
title: Retries and Resilience Shields
sidebar_position: 6
---

# Retries and Resilience Shields

When creating modules, you can configure retries per module using the `Configure()` method.
The standard API supports exponential backoff, jitter, and exception filtering without exposing
the underlying resilience library.

## Using ModuleConfiguration

### Simple Retries

The easiest way to add retries is with `WithRetry()`:

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithRetry(3)  // Retry up to 3 times with exponential backoff and jitter
        .Build();

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Do something that might fail transiently
    }
}
```

The default base delay is 100 milliseconds. Each retry uses equal jitter between half and all of
its exponential-backoff ceiling. You can set a different base delay and limit retries to selected
exceptions:

```csharp
protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
    .WithRetry(
        count: 5,
        baseDelay: TimeSpan.FromSeconds(1),
        shouldRetry: exception => exception is HttpRequestException)
    .Build();
```

### Advanced Resilience Shield

For resilience features outside the standard API, use a Kevlar `Shield` through the explicit `.Advanced` surface:

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .Advanced
        .WithShield(
            Shield.When<HttpRequestException>()
                .Retry(5, Backoff.Custom(i => TimeSpan.FromSeconds(i * i))))
        .Build();

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Do something
    }
}
```

### Context-Aware Resilience Shield

If you need access to the pipeline context when building your shield:

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .Advanced
        .WithShield(ctx =>
        {
            var retryCount = ctx.Environment.IsCI ? 5 : 2;
            return Shield.When<Exception>()
                .Retry(retryCount, Backoff.Custom(i => TimeSpan.FromSeconds(i)));
        })
        .Build();
}
```

## Combining with Other Behaviors

Retry configuration can be combined with other module behaviors:

```csharp
public class ResilientModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithRetry(3)
        .WithTimeout(TimeSpan.FromMinutes(10))
        .WithIgnoreFailures()  // Don't fail the pipeline even after all retries
        .Build();
}
```

## Default Retry Configuration

Retries are off by default. You can set a default retry count on the `PipelineOptions`:

```csharp
var builder = Pipeline.CreateBuilder(args);

builder
    .AddModule<Module1>()
    .AddModule<Module2>()
    .AddModule<Module3>();

builder.ConfigurePipelineOptions(options => options with
{
    DefaultRetryCount = 3,
});

await builder.ExecutePipelineAsync();
```

This applies to all modules that don't override their retry configuration. Modules can override this default by configuring retries in `Configure()`.
