# Retry Policies

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

### Advanced Polly Policy[​](#advanced-polly-policy "Direct link to Advanced Polly Policy")

For policy features outside the standard API, use the explicit `.Advanced` surface:

```
public class MyModule : Module<CommandResult>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .Advanced

        .WithRetryPolicy(

            Policy.Handle<HttpRequestException>()

                .WaitAndRetryAsync(5, i => TimeSpan.FromSeconds(i * i)))

        .Build();



    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)

    {

        // Do something

    }

}
```

### Context-Aware Retry Policy[​](#context-aware-retry-policy "Direct link to Context-Aware Retry Policy")

If you need access to the pipeline context when building your policy:

```
public class MyModule : Module<CommandResult>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .Advanced

        .WithRetryPolicy(ctx =>

        {

            var retryCount = ctx.Environment.IsCI ? 5 : 2;

            return Policy.Handle<Exception>()

                .WaitAndRetryAsync(retryCount, i => TimeSpan.FromSeconds(i));

        })

        .Build();

}
```

## Combining with Other Behaviors[​](#combining-with-other-behaviors "Direct link to Combining with Other Behaviors")

Retry policies can be combined with other module behaviors:

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

## Default Retry Policy[​](#default-retry-policy "Direct link to Default Retry Policy")

Retry policies are off by default. You can set a default retry count on the `PipelineOptions`:

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



await builder.ExecutePipelineAsync();
```

This applies to all modules that don't override their retry policy. Modules can override this default by configuring their own retry policy in `Configure()`.
