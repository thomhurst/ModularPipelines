# Sharing data across modules

Modules have been designed with data and sharing at its core.

When a module returns data in its `ExecuteAsync` method, that data is available to be seen by other modules.

Call `await context.GetModule<TModule>()` from within your module to access another module's result.

```
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

## Handling Different Outcomes[​](#handling-different-outcomes "Direct link to Handling Different Outcomes")

Module results are a discriminated union with three possible states: Success, Failure, or Skipped. Use pattern matching to handle each case:

```
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

## Using Match Helper[​](#using-match-helper "Direct link to Using Match Helper")

For exhaustive handling, use the `Match` method:

```
var result = await context.GetModule<MyModule>();



return result.Match(

    onSuccess: value => Process(value),

    onFailure: ex => HandleError(ex),

    onSkipped: skip => HandleSkip(skip)

);
```

## Accessing Required Values[​](#accessing-required-values "Direct link to Accessing Required Values")

Use `Value` when the dependency must have produced a non-null value. It returns `T` without a null-forgiveness operator. If the module failed, was skipped, or returned `null`, it throws an `InvalidOperationException` that identifies the module and outcome:

```
var result = await context.GetModule<MyModule>();

var value = result.Value;
```

## Safe Accessors[​](#safe-accessors "Direct link to Safe Accessors")

When an absent value is expected, inspect the union through its non-throwing accessors:

```
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

## Important: Declare Dependencies[​](#important-declare-dependencies "Direct link to Important: Declare Dependencies")

Always declare dependencies using `[DependsOn<T>]` to ensure the dependent module has completed before you call `GetModule`:

```
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
