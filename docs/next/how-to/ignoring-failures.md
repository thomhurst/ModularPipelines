# Ignoring Failures

Sometimes a module might throw an exception, but we simply don't care as it's not that important, or a specific error might be expected.

## Using ModuleConfiguration[​](#using-moduleconfiguration "Direct link to Using ModuleConfiguration")

### Always Ignore Failures[​](#always-ignore-failures "Direct link to Always Ignore Failures")

To always ignore failures from a module:

```
public class OptionalModule : Module<CommandResult>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithIgnoreFailures()

        .Build();



    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)

    {

        // If this fails, the pipeline continues

    }

}
```

### Conditional Failure Ignoring[​](#conditional-failure-ignoring "Direct link to Conditional Failure Ignoring")

To ignore only specific types of failures:

```
public class MyModule : Module<CommandResult>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithIgnoreFailuresWhen((ctx, exception) => exception is ItemAlreadyExistsException)

        .Build();



    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)

    {

        // ItemAlreadyExistsException will be ignored, other exceptions will fail the pipeline

    }

}
```

### Async Condition[​](#async-condition "Direct link to Async Condition")

For conditions that require async operations:

```
public class MyModule : Module<CommandResult>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithIgnoreFailuresWhen(async (ctx, exception) =>

        {

            if (exception is HttpRequestException httpEx)

            {

                // Check if the service is in maintenance mode

                var isMaintenanceMode = await CheckMaintenanceModeAsync();

                return isMaintenanceMode;

            }

            return false;

        })

        .Build();

}
```

## Combining with Other Behaviors[​](#combining-with-other-behaviors "Direct link to Combining with Other Behaviors")

Ignoring failures can be combined with other module behaviors:

```
public class ResilientModule : Module<CommandResult>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithRetry(3)  // Try 3 times first

        .WithIgnoreFailuresWhen((ctx, ex) => ex is HttpRequestException)  // Then ignore HTTP errors

        .WithAlwaysRun()  // Run even if dependencies failed

        .Build();

}
```

## Checking Failure Status[​](#checking-failure-status "Direct link to Checking Failure Status")

Even when failures are ignored, you can check if a module failed from dependent modules:

```
var myModule = await context.GetModule<MyOptionalModule>();



if (myModule.ExceptionOrDefault is ItemAlreadyExistsException)

{

    // Handle the expected failure case

    return None.Value;

}



return await DoSomethingAsync();
```
