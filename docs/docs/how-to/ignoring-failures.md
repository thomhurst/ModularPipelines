---
title: Ignoring Failures
---

# Ignoring Failures

Sometimes a module might throw an exception, but we simply don't care as it's not that important, or a specific error might be expected.

## Using ModuleConfiguration

### Always Ignore Failures

To always ignore failures from a module:

```csharp
public class OptionalModule : Module<CommandResult>
{
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithIgnoreFailures()
        ;

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // If this fails, the pipeline continues
    }
}
```

### Conditional Failure Ignoring

To ignore only specific types of failures:

```csharp
public class MyModule : Module<CommandResult>
{
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithIgnoreFailuresWhen((ctx, exception) => exception is ItemAlreadyExistsException)
        ;

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // ItemAlreadyExistsException will be ignored, other exceptions will fail the pipeline
    }
}
```

### Async Condition

For conditions that require async operations:

```csharp
public class MyModule : Module<CommandResult>
{
    protected override void Configure(ModuleConfigurationBuilder module) => module
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
        ;
}
```

## Combining with Other Behaviors

Ignoring failures can be combined with other module behaviors:

```csharp
public class ResilientModule : Module<CommandResult>
{
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithRetry(3)  // Try 3 times first
        .WithIgnoreFailuresWhen((ctx, ex) => ex is HttpRequestException)  // Then ignore HTTP errors
        .WithAlwaysRun()  // Run even if dependencies failed
        ;
}
```

## Checking Failure Status

Even when failures are ignored, you can check if a module failed from dependent modules:

```csharp
var myModule = await context.GetModule<MyOptionalModule>();

if (myModule.ExceptionOrDefault is ItemAlreadyExistsException)
{
    // Handle the expected failure case
    return None.Value;
}

return await DoSomethingAsync();
```
