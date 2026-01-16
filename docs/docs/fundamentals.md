---
title: Fundamentals
sidebar_position: 1
---

# Fundamentals

## Pipeline Builder

Your pipeline is created using `Pipeline.CreateBuilder()`. This follows the ASP.NET Core minimal API pattern, providing direct access to `Configuration`, `Services`, and `Options`. Setup should feel familiar if you've used ASP.NET Core.

```csharp
var builder = Pipeline.CreateBuilder(args);
builder.Services.AddModule<MyModule>();
await builder.Build().RunAsync();
```

## Modules

The building blocks of your pipelines are called Modules. Modules can be as big or as small as you decide, though it's recommended to make them as small as possible. That way we can speed up execution by utilizing parallelization and we are able to more clearly see what failed and where it failed.

> a self-contained unit or item, such as an assembly of electronic components and associated wiring or a segment of computer software, which itself performs a defined task and can be linked with other such units to form a larger system

Modules can retrieve other modules and access information from them.

## Strong Typing

Modules are strongly typed, so we can return clear, concrete objects, and other modules have direct access to those strong objects, without any need for casting or guessing the type, or guessing keys from a dictionary.

```csharp
// Get a module's result
var myModule = await context.GetModule<MyFirstModule>();

// Access the value using pattern matching (recommended)
if (myModule is ModuleResult<MyFirstModuleResult>.Success { Value: var result })
{
    var string1 = result.MyFirstString;
    var string2 = result.MySecondString;
}

// Or use ValueOrDefault for simpler access
var string1 = myModule.ValueOrDefault?.MyFirstString;
var string2 = myModule.ValueOrDefault?.MySecondString;
```

## Custom Types

A module isn't restricted to a pre-determined type either. You can pass the `Type` of object that you want to return when you inherit from the base `Module` class:

```csharp
public class MyModule : Module<MyCustomClass>
```

```csharp
public class PingApiModule : Module<HttpResponseMessage>
```

You'll then be instructed by the compiler to make sure the return type of your main `ExecuteAsync` method matches the `Type` you've set up:

```csharp
protected override async Task<MyCustomClass?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
```

## Optional Data

You can use `IDictionary<string, object>` as a flexible return type:

```csharp
public class MyModule : Module<IDictionary<string, object>>
{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(
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

Returning an object isn't mandatory either. You can return `null`:

```csharp
protected override async Task<MyResult?> ExecuteAsync(
    IModuleContext context, CancellationToken cancellationToken)
{
    await DoSomethingAsync();
    return null;
}
```

## Automatic Parallelisation and Explicit Dependencies

Modules will all try to run in parallel if possible. But if a Module depends on another Module, it is smart enough to automatically wait for the dependent module to finish before executing.

Dependencies are configured by adding an attribute on your Module. This also makes it clear to navigate through your pipeline, as with your IDE/Intellisense, you can click through to other Modules with ease.

```csharp
[DependsOn<MyOtherModule>]
public class MyModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // MyOtherModule is guaranteed to have completed before this runs
        return "result";
    }
}
```

## Checking a Module's Status

When you get another Module, you'll be passed a `ModuleResult<T>` that contains the data you returned, as well as information about its execution. Use pattern matching to handle different outcomes:

```csharp
var myModule = await context.GetModule<MyOptionalModule>();

// Pattern matching (recommended)
return myModule switch
{
    ModuleResult<MyOptionalResult>.Success { Value: var result }
        => await ProcessResult(result),
    ModuleResult.Skipped { Decision: var skip }
        => null,  // Module was skipped
    ModuleResult.Failure { Exception: var ex }
        => throw new Exception("Dependency failed", ex),
    _ => null
};
```

Or use the convenience properties for simpler checks:

```csharp
var myModule = await context.GetModule<MyOptionalModule>();

if (myModule.IsSkipped)
{
    return null;
}

if (myModule.IsFailure)
{
    // Check the exception
    if (myModule.ExceptionOrDefault is ItemAlreadyExistsException)
    {
        return null;
    }
    throw new Exception("Unexpected failure", myModule.ExceptionOrDefault);
}

// Success case
return await DoSomethingAsync(myModule.ValueOrDefault);
```

You can also use the `Match` helper for exhaustive handling:

```csharp
var myModule = await context.GetModule<MyOptionalModule>();

return await myModule.Match(
    onSuccess: result => ProcessResultAsync(result),
    onFailure: ex => HandleFailureAsync(ex),
    onSkipped: skip => Task.FromResult<MyResult?>(null)
);
```
