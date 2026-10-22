---
title: Retries and Resilience Shields
sidebar_position: 6
---

# Retries and Resilience Shields

When creating modules, you can configure retries per module using the `Configure(ModuleConfigurationBuilder)` method.
The standard API supports exponential backoff, jitter, and exception filtering without exposing
the underlying resilience library.

## Using ModuleConfiguration

### Simple Retries

The easiest way to add retries is with `WithRetry()`:

```csharp
public class MyModule : Module<CommandResult>
{
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithRetry(3); // Retry up to 3 times with exponential backoff and jitter

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Do something that might fail transiently
    }
}
```

The default base delay is 100 milliseconds, and the default maximum delay is 30 seconds. Each retry
uses equal jitter between half and all of its exponential-backoff ceiling, capped at the maximum
delay before jitter is applied. You can set different delays and limit retries to selected exceptions:

```csharp
protected override void Configure(ModuleConfigurationBuilder module) => module
    .WithRetry(
        count: 5,
        baseDelay: TimeSpan.FromSeconds(1),
        shouldRetry: exception => exception is HttpRequestException,
        maxDelay: TimeSpan.FromSeconds(10));
```

Both delays must be non-negative. A maximum delay of zero disables backoff. A maximum delay below
the base delay also caps the first retry.

### Execution Attempt Number

Use `IModuleContext.Attempt` inside `ExecuteAsync` for per-attempt behavior. It is **1-based**:
the initial execution is attempt 1, the first retry is attempt 2, and so on. For example,
`WithRetry(3)` allows up to four executions, numbered 1 through 4.

```csharp
protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
{
    context.Logger.LogInformation("Downloading on attempt {Attempt}", context.Attempt);
    return await httpClient.GetStringAsync("https://example.com/data", cancellationToken);
}
```

Before execution starts, `Attempt` is 0. Between attempts and after completion, it holds the
most recent attempt number. Contexts that do not track execution attempts return 0.

### Asynchronous Retry Predicate

Use the `shouldRetryAsync` overload when deciding whether to retry requires asynchronous work,
such as recovering a connection. The predicate receives the module context, the exception,
the **1-based failed attempt number**, and the execution cancellation token:

```csharp
protected override void Configure(ModuleConfigurationBuilder module) => module
    .WithRetry(
        shouldRetryAsync: async (context, exception, attempt, cancellationToken) =>
        {
            if (exception is not HttpRequestException)
            {
                return false;
            }

            await RefreshConnectionAsync(cancellationToken);
            context.Logger.LogInformation("Retrying after failed attempt {Attempt}", attempt);
            return true;
        },
        count: 3,
        baseDelay: TimeSpan.FromSeconds(1),
        maxDelay: TimeSpan.FromSeconds(10));
```

`RefreshConnectionAsync` represents an application-provided asynchronous recovery method.
The predicate's `attempt` argument equals `context.Attempt`: both are 1 after the initial
execution fails. Returning `true` permits the next attempt; returning `false` propagates the
original exception.

The predicate runs only after a failure while retry budget remains, before the backoff delay.
It is not called after a successful execution or after the final allowed attempt fails.
If the predicate itself throws, its exception propagates without retrying. Pass the supplied
cancellation token to asynchronous recovery work so cancellation can interrupt it.

### Custom Resilience Shield

For resilience features outside the standard retry API, configure a Kevlar `Shield` directly:

```csharp
public class MyModule : Module<CommandResult>
{
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithShield(
            Shield.When<HttpRequestException>()
                .Retry(5, Backoff.Custom(i => TimeSpan.FromSeconds(i * i))));

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
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithShield(ctx =>
        {
            var retryCount = ctx.Environment.BuildSystem.IsCI ? 5 : 2;
            return Shield.When<Exception>()
                .Retry(retryCount, Backoff.Custom(i => TimeSpan.FromSeconds(i)));
        });
}
```

## Combining with Other Behaviors

Retry configuration can be combined with other module behaviors:

```csharp
public class ResilientModule : Module<CommandResult>
{
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithRetry(3)
        .WithTimeout(TimeSpan.FromMinutes(10))
        .WithIgnoreFailures(); // Don't fail the pipeline even after all retries
}
```

`WithTimeout` applies to each execution attempt, not to the whole retry chain. The asynchronous retry
predicate and backoff delays run outside that timeout. A timed-out attempt is passed to the resilience
shield as a `ModuleTimeoutException`, so exception filters still decide whether it should be retried. If the
attempt remains active after the cancellation grace period, the engine bypasses retries to avoid
running the same module instance concurrently with its abandoned attempt.

## Default Retry Configuration

Retries are off by default. You can set a default retry count on the `PipelineOptions`:

```csharp
var builder = Pipeline.CreateBuilder(args);

builder
    .AddModule<Module1>()
    .AddModule<Module2>()
    .AddModule<Module3>();

builder.ConfigureOptions(options => options with
{
    DefaultRetryCount = 3,
});

await builder.RunAsync();
```

This applies to all modules that don't override their retry configuration. Modules can override this default by configuring retries in `Configure(ModuleConfigurationBuilder)`.
The default retries use the same 100 millisecond base delay, 30 second maximum delay, and equal jitter.
