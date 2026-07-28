---
title: Sharing data across modules
sidebar_position: 4
---

# Sharing data across modules

Modules have been designed with data and sharing at its core.

When a module returns data in its `ExecuteAsync` method, that data is available to be seen by other modules.

Call `await context.GetModule<TModule>()` from within your module to access another module's result.

```csharp
[DependsOn<BuildModule>]
public class DeployModule : Module<DeployResult>
{
    protected override async Task<DeployResult?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Get the build module's result
        var buildResult = await context.GetModule<BuildModule>();

        // Access the value safely
        var artifact = buildResult.ValueOrDefault?.ArtifactPath;

        return await Deploy(artifact);
    }
}
```

## Handling Different Outcomes

Module results are a discriminated union with three possible states: Success, Failure, or Skipped. Use pattern matching to handle each case:

```csharp
var result = await context.GetModule<MyModule>();

// Pattern matching (recommended)
return result switch
{
    ModuleResult<MyResult>.Success { Value: var value }
        => await ProcessValue(value),
    ModuleResult.Failure { Exception: var ex }
        => HandleFailure(ex),
    ModuleResult.Skipped { Decision: var skip }
        => HandleSkipped(skip.Reason),
    _ => null
};
```

## Using Match Helper

For exhaustive handling, use the `Match` method:

```csharp
var result = await context.GetModule<MyModule>();

return result.Match(
    onSuccess: value => Process(value),
    onFailure: ex => HandleError(ex),
    onSkipped: skip => HandleSkip(skip)
);
```

## Convenience Properties

For simpler checks, use the convenience properties:

```csharp
var result = await context.GetModule<MyModule>();

// Quick status checks
if (result.IsSuccess)
{
    var value = result.ValueOrDefault;
    // Process value
}

if (result.IsFailure)
{
    var exception = result.ExceptionOrDefault;
    // Handle error
}

if (result.IsSkipped)
{
    var skipDecision = result.SkipDecisionOrDefault;
    // Handle skip
}
```

## Important: Declare Dependencies

Always declare dependencies using `[DependsOn<T>]` to ensure the dependent module has completed before you call `GetModule`:

```csharp
[DependsOn<BuildModule>]  // Ensures BuildModule completes first
[DependsOn<TestModule>]   // Ensures TestModule completes first
public class DeployModule : Module<DeployResult>
{
    protected override async Task<DeployResult?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Safe to call - dependencies are guaranteed to be complete
        var build = await context.GetModule<BuildModule>();
        var tests = await context.GetModule<TestModule>();

        // ...
    }
}
```
