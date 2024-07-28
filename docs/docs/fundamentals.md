---
title: Fundamentals
sidebar_position: 1
---

# Fundamentals

The building blocks of your pipelines are called Modules. Modules can be as big or as small as you decide, though it's recommended to make them as small as possible. That way we can speed up execution by utilizing parallelization and we are able to more clearly see what failed and where it failed.

> a self-contained unit or item, such as an assembly of electronic components and associated wiring or a segment of computer software, which itself performs a defined task and can be linked with other such units to form a larger system

Modules can retrieve other modules, and access information from them.

## Strong Typing

Modules are strongly typed, so we can return clear, concrete objects, and other modules have direct access to those strong objects, without any need for casting or guessing the type, or guessing keys from a dictionary.

```csharp
var myModule = await GetModule<MyFirstModule>();
var string1 = myModule.Value!.MyFirstString;
var string2 = myModule.Value!.MySecondString;
```

## Custom Types

A module isn't restricted to a pre-determined type either. You can pass the `Type` of object that you want to return when you inherit from the base `Module` class

```csharp
public class MyModule : Module<MyCustomClass>
```

```csharp
public class PingApiModule : Module<HttpResponseMessage>
```

You'll then be instructed by the compiler to make sure the return type of your main `ExecuteAsync` method matches the `Type` you've set up.

```csharp
protected override async Task<MyCustomClass?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
```

## Optional Data

You can choose not to set a `Type` and the default will be an `IDictionary<string, object>`.

```csharp
public class MyModule : Module
{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
}
```

Returning an object isn't mandatory either. You can return `null` or use the method `NothingAsync();`.

## Automatic Parallelisation and Explicit Dependencies

Modules will all try to run in parallel if possible. But if a Module depends on another Module, it is smart enough to automatically wait for the dependent module to finish before executing.

Dependencies are configured by adding an attribute on your Module. This also makes it clear to navigate through your pipeline, as with your IDE/Intellisense, you can click through to other Modules with ease.

```
[DependsOn<MyOtherModule>]
public class MyModule : Module
```

## Checking a Module's status

When you get another Module, you'll be passed an object that has the data you returned, as well as some information about its execution. So you can have logic in your pipeline for if another module was skipped for example.

```csharp
var myModule = await GetModule<MyOptionalModule>();

if (myModule.ModuleResultType == ModuleResultType.Skipped)
{
    return null;
}

return await DoSomethingAsync();
```

or if a Module failed, but it was configured to not stop the pipeline, you could check its Exception.

```csharp
var myModule = await GetModule<MyOptionalModule>();

if (gitModule.Exception is ItemAlreadyExistsException)
{
    return null;
}

return await DoSomethingAsync();
```