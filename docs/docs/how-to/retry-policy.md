---
title: Retry Policies
sidebar_position: 6
---

# Retry Policies

When creating modules, you can set a retry policy per module using the `Configure()` method. The retry policy uses Polly, so if you've used Polly before you should be familiar with how to use it.

## Using ModuleConfiguration

### Simple Retry Count

The easiest way to add retries is with `WithRetryCount()`:

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithRetryCount(3)  // Retry up to 3 times with exponential backoff
        .Build();

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Do something that might fail transiently
    }
}
```

### Custom Polly Policy

For more control, you can provide a custom Polly policy:

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithRetryPolicy(
            Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(5, i => TimeSpan.FromSeconds(i * i)))
        .Build();

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Do something
    }
}
```

### Context-Aware Retry Policy

If you need access to the pipeline context when building your policy:

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithRetryPolicy(ctx =>
        {
            var retryCount = ctx.Environment.IsCI ? 5 : 2;
            return Policy.Handle<Exception>()
                .WaitAndRetryAsync(retryCount, i => TimeSpan.FromSeconds(i));
        })
        .Build();
}
```

## Combining with Other Behaviors

Retry policies can be combined with other module behaviors:

```csharp
public class ResilientModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithRetryCount(3)
        .WithTimeout(TimeSpan.FromMinutes(10))
        .WithIgnoreFailures()  // Don't fail the pipeline even after all retries
        .Build();
}
```

## Default Retry Policy

Retry policies are off by default. You can set a default retry count on the `PipelineOptions` when using the `PipelineHostBuilder`:

```csharp
await PipelineHostBuilder.Create()
    .AddModule<Module1>()
    .AddModule<Module2>()
    .AddModule<Module3>()
    .ConfigurePipelineOptions((context, options) =>
    {
        options.DefaultRetryCount = 3;
    })
    .ExecutePipelineAsync();
```

This applies to all modules that don't override their retry policy. Modules can override this default by configuring their own retry policy in `Configure()`.
