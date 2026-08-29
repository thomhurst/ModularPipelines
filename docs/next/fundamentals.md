# Fundamentals

## Pipeline Builder[​](#pipeline-builder "Direct link to Pipeline Builder")

Your pipeline is created using `Pipeline.CreateBuilder()`. This follows the ASP.NET Core minimal API pattern, providing direct access to `Configuration`, `Services`, and `Options`. Setup should feel familiar if you've used ASP.NET Core.

```
var builder = Pipeline.CreateBuilder(args);

builder.AddModule<MyModule>();

await builder.RunAsync();
```

## Modules[​](#modules "Direct link to Modules")

The building blocks of your pipelines are called Modules. Modules can be as big or as small as you decide, though it's recommended to make them as small as possible. That way we can speed up execution by utilizing parallelization and we are able to more clearly see what failed and where it failed.

> a self-contained unit or item, such as an assembly of electronic components and associated wiring or a segment of computer software, which itself performs a defined task and can be linked with other such units to form a larger system

Modules can retrieve other modules and access information from them.

## Tool Integrations[​](#tool-integrations "Direct link to Tool Integrations")

Installed tool integrations are available from the discoverable `context.Tools` surface:

```
await context.Tools.DotNet.BuildAsync(new DotNetBuildOptions

{

    ProjectSolution = "MyApp.sln"

}, cancellationToken: cancellationToken);
```

`context.Tools.DotNet` is the canonical API and does not require an integration extension namespace import. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.DotNet.Services.IDotNet>()`. Legacy `context.DotNet()` access is obsolete and hidden from IntelliSense.

## Strong Typing[​](#strong-typing "Direct link to Strong Typing")

Modules are strongly typed, so we can return clear, concrete objects, and other modules have direct access to those strong objects, without any need for casting or guessing the type, or guessing keys from a dictionary.

```
// Get a module's result

var myModule = await context.GetModule<MyFirstModule>();



// Access a required dependency value directly.

// This throws with module context if it failed, was skipped, or returned null.

var requiredValue = myModule.Value;

var firstString = requiredValue.MyFirstString;

var secondString = requiredValue.MySecondString;



// Or use pattern matching when every outcome needs different handling

if (myModule is ModuleResult<MyFirstModuleResult>.Success { Value: var successfulValue })

{

    Console.WriteLine(successfulValue.MyFirstString);

    Console.WriteLine(successfulValue.MySecondString);

}



// ValueOrDefault remains available when missing data is expected

var optionalFirstString = myModule.ValueOrDefault?.MyFirstString;

var optionalSecondString = myModule.ValueOrDefault?.MySecondString;
```

## Custom Types[​](#custom-types "Direct link to Custom Types")

A module isn't restricted to a pre-determined type either. You can pass the `Type` of object that you want to return when you inherit from the base `Module` class:

```
public class MyModule : Module<MyCustomClass>
```

```
public class PingApiModule : Module<HttpResponseMessage>
```

You'll then be instructed by the compiler to make sure the return type of your main `ExecuteAsync` method matches the `Type` you've set up:

```
protected override async Task<MyCustomClass> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
```

## Optional Data[​](#optional-data "Direct link to Optional Data")

You can use `IDictionary<string, object>` as a flexible return type:

```
public class MyModule : Module<IDictionary<string, object>>

{

    protected override async Task<IDictionary<string, object>> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        return new Dictionary<string, object>

        {

            ["key1"] = "value1",

            ["key2"] = 42

        };

    }

}
```

When a module has no meaningful result, use `None`:

```
public class PublishModule : Module<None>

{

    protected override async Task<None> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        await PublishAsync(cancellationToken);

        return None.Value;

    }

}
```

If `null` is meaningful data, make that explicit in both the module and method types:

```
public class OptionalLookupModule : Module<MyResult?>

{

    protected override async Task<MyResult?> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        return await TryFindResultAsync(cancellationToken);

    }

}
```

## Automatic Parallelisation and Explicit Dependencies[​](#automatic-parallelisation-and-explicit-dependencies "Direct link to Automatic Parallelisation and Explicit Dependencies")

Modules will all try to run in parallel if possible. But if a Module depends on another Module, it is smart enough to automatically wait for the dependent module to finish before executing.

Dependencies are configured by adding an attribute on your Module. This also makes it clear to navigate through your pipeline, as with your IDE/Intellisense, you can click through to other Modules with ease.

```
[DependsOn<MyOtherModule>]

public class MyModule : Module<string>

{

    protected override async Task<string> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        // MyOtherModule is guaranteed to have completed before this runs

        return "result";

    }

}
```

## Checking a Module's Status[​](#checking-a-modules-status "Direct link to Checking a Module's Status")

When you get another Module, you'll be passed a `ModuleResult<T>` that contains the data you returned, as well as information about its execution. Use pattern matching to handle different outcomes:

```
var myModule = await context.GetModule<MyOptionalModule>();



// Pattern matching (recommended)

return myModule switch

{

    ModuleResult<MyOptionalResult>.Success { Value: var result }

        => await ProcessResult(result),

    ModuleResult<MyOptionalResult>.Skipped { Decision: var skip }

        => null,  // Module was skipped

    ModuleResult<MyOptionalResult>.Failure { Exception: var ex }

        => throw new Exception("Dependency failed", ex),

    _ => null

};
```

Or use the safe accessors for simpler checks:

```
var myModule = await context.GetModule<MyOptionalModule>();



if (myModule.SkipDecisionOrDefault is not null)

{

    return None.Value;

}



if (myModule.ExceptionOrDefault is not null)

{

    // Check the exception

    if (myModule.ExceptionOrDefault is ItemAlreadyExistsException)

    {

        return None.Value;

    }

    throw new Exception("Unexpected failure", myModule.ExceptionOrDefault);

}



// Success case

return await DoSomethingAsync(myModule.Value);
```

You can also use the `Match` helper for exhaustive handling:

```
var myModule = await context.GetModule<MyOptionalModule>();



return await myModule.Match(

    onSuccess: result => ProcessResultAsync(result),

    onFailure: ex => HandleFailureAsync(ex),

    onSkipped: skip => Task.FromResult<MyResult?>(null)

);
```
