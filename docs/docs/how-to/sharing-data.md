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
    protected override async Task<DeployResult> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Get the build module's result
        var buildResult = await context.GetModule<BuildModule>();

        // Access the required dependency value. This throws with module context
        // if the module failed, was skipped, or returned null.
        var artifact = buildResult.Value.ArtifactPath;

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
    ModuleResult<MyResult>.Failure { Exception: var ex }
        => HandleFailure(ex),
    ModuleResult<MyResult>.Skipped { Decision: var skip }
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

## Accessing Required Values

Use `Value` when the dependency must have produced a non-null value. It returns `T`
without a null-forgiveness operator. If the module failed, was skipped, or returned
`null`, it throws an `InvalidOperationException` that identifies the module and outcome:

```csharp
var result = await context.GetModule<MyModule>();
var value = result.Value;
```

## Safe Accessors

When an absent value is expected, inspect the union through its non-throwing accessors:

```csharp
var result = await context.GetModule<MyModule>();

if (result is ModuleResult<MyResult>.Success success)
{
    var value = success.Value;
    // Process value
}

if (result.ExceptionOrDefault is { } exception)
{
    // Handle error
}

if (result.SkipDecisionOrDefault is { } skipDecision)
{
    // Handle skip
}
```

## Checking type-erased results

`PipelineSummary.Results` and the `IModuleResult` parameter of `IModuleEndHandler.OnModuleEndAsync`
expose results without the module's value type. Use `Status`, `ExceptionOrDefault`, and
`SkipDecisionOrDefault` to inspect these results:

```csharp
var failed = summary.Results.Where(result => result.Status == ModuleStatus.Failed);

foreach (var result in summary.Results)
{
    if (result.ExceptionOrDefault is { } exception)
    {
        Console.Error.WriteLine($"{result.Name}: {exception.Message}");
    }

    if (result.SkipDecisionOrDefault is { } decision)
    {
        Console.WriteLine($"{result.Name} skipped: {decision.Reason}");
    }
}
```

`Status == ModuleStatus.Failed` selects that exact status. `ExceptionOrDefault` also exposes
exceptions for failure outcomes with other statuses, such as `FailureIgnored` or `TimedOut`.

The engine produces `ModuleResult<T>.Failure` and `ModuleResult<T>.Skipped`, including when
results are exposed through `IModuleResult`. These generic records do not inherit from
`ModuleResult.Failure` or `ModuleResult.Skipped`. Consequently, `result is ModuleResult.Failure`
and `summary.Results.OfType<ModuleResult.Skipped>()` miss those engine outcomes.
Analyzer [`MP0023`](../analyzers/MP0023.md) warns about these non-generic outcome tests on
`IModuleResult` or `ModuleResult` values and collections, including type patterns in switches.

When the value type is known, the generic patterns shown above remain valid. The non-generic
records remain public for explicit construction and implicit conversion to `ModuleResult<T>`.
If you intentionally store those constructed records in a type-erased collection, a
non-generic test can be valid; suppress MP0023 at that specific occurrence.

## Important: Declare Dependencies

Always declare dependencies using `[DependsOn<T>]` to ensure the dependent module has completed before you call `GetModule`:

```csharp
[DependsOn<BuildModule>]  // Ensures BuildModule completes first
[DependsOn<TestModule>]   // Ensures TestModule completes first
public class DeployModule : Module<DeployResult>
{
    protected override async Task<DeployResult> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Safe to call - dependencies are guaranteed to be complete
        var build = await context.GetModule<BuildModule>();
        var tests = await context.GetModule<TestModule>();

        // ...
    }
}
```
