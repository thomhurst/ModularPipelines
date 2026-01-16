---
title: Categories
---

# Categories
Sometimes we want to run only certain parts of a pipeline, or we might want to split a pipeline up into different targets. For instance, a test run, and then later on a deploy run. Categories can help achieve that.

## Attribute
Categories are applied to Modules by using the `[ModuleCategory]` attribute.

## PipelineBuilder
Categories to run or ignore are configured on the `PipelineBuilder` via `Options`.

## Run Categories
If "Run Categories" have been set with some values, only Modules that have any of those categories will be run. If a module has none of those categories, it will not run.

## Ignore Categories
If "Ignore Categories" have been set with some values, if a Module has one of those categories, it will not run.


## Example of Running Specific Categories

```csharp
var builder = Pipeline.CreateBuilder(args);

builder.Services
    .AddModule<Module1>()
    .AddModule<Module2>()
    .AddModule<Module3>()
    .AddModule<Module4>();

builder.Options.RunOnlyCategories = ["UnitTest", "IntegrationTest"];

await builder.Build().RunAsync();
```


## Example of Ignoring Specific Categories

```csharp
var builder = Pipeline.CreateBuilder(args);

builder.Services
    .AddModule<Module1>()
    .AddModule<Module2>()
    .AddModule<Module3>()
    .AddModule<Module4>();

builder.Options.IgnoreCategories = ["Publish", "Deploy"];

await builder.Build().RunAsync();
```