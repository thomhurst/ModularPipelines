# Retries and Resilience Shields

When creating modules, you can configure retries per module using the `Configure()` method. The standard API supports exponential backoff, jitter, and exception filtering without exposing the underlying resilience library.

## Using ModuleConfiguration[​](#using-moduleconfiguration "Direct link to Using ModuleConfiguration")

### Simple Retries[​](#simple-retries "Direct link to Simple Retries")

The easiest way to add retries is with `WithRetry()`:

```
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

The default base delay is 100 milliseconds. Each retry uses equal jitter between half and all of its exponential-backoff ceiling. You can set a different base delay and limit retries to selected exceptions:

```
protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

    .WithRetry(

        count: 5,

        baseDelay: TimeSpan.FromSeconds(1),

        shouldRetry: exception => exception is HttpRequestException)

    .Build();
```

### Custom Resilience Shield[​](#custom-resilience-shield "Direct link to Custom Resilience Shield")

For resilience features outside the standard retry API, configure a Kevlar `Shield` directly:

```
public class MyModule : Module<CommandResult>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

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

### Context-Aware Resilience Shield[​](#context-aware-resilience-shield "Direct link to Context-Aware Resilience Shield")

If you need access to the pipeline context when building your shield:

```
public class MyModule : Module<CommandResult>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithShield(ctx =>

        {

            var retryCount = ctx.Environment.IsCI ? 5 : 2;

            return Shield.When<Exception>()

                .Retry(retryCount, Backoff.Custom(i => TimeSpan.FromSeconds(i)));

        })

        .Build();

}
```

## Combining with Other Behaviors[​](#combining-with-other-behaviors "Direct link to Combining with Other Behaviors")

Retry configuration can be combined with other module behaviors:

```
public class ResilientModule : Module<CommandResult>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithRetry(3)

        .WithTimeout(TimeSpan.FromMinutes(10))

        .WithIgnoreFailures()  // Don't fail the pipeline even after all retries

        .Build();

}
```

`WithTimeout` applies to each execution attempt, not to the whole retry chain. Backoff delays run outside that timeout. A timed-out attempt is passed to the resilience shield as a `ModuleTimeoutException`, so exception filters still decide whether it should be retried. If the attempt remains active after the cancellation grace period, the engine bypasses retries to avoid running the same module instance concurrently with its abandoned attempt.

## Default Retry Configuration[​](#default-retry-configuration "Direct link to Default Retry Configuration")

Retries are off by default. You can set a default retry count on the `PipelineOptions`:

```
var builder = Pipeline.CreateBuilder(args);



builder

    .AddModule<Module1>()

    .AddModule<Module2>()

    .AddModule<Module3>();



builder.ConfigurePipelineOptions(options => options with

{

    DefaultRetryCount = 3,

});



await builder.RunAsync();
```

This applies to all modules that don't override their retry configuration. Modules can override this default by configuring retries in `Configure()`.
