# Categories

Sometimes we want to run only certain parts of a pipeline, or we might want to split a pipeline up into different targets. For instance, a test run, and then later on a deploy run. Categories can help achieve that.

## Attribute[​](#attribute "Direct link to Attribute")

Categories are applied to Modules by using the `[ModuleCategory]` attribute.

## PipelineBuilder[​](#pipelinebuilder "Direct link to PipelineBuilder")

Categories to run or ignore are configured through the `PipelineBuilder` fluent methods or immutable `Options`.

## Run-Only Categories[​](#run-only-categories "Direct link to Run-Only Categories")

If `RunOnlyCategories` has been set with some values, only Modules that have any of those categories will be run. If a module has none of those categories, it will not run.

## Ignore Categories[​](#ignore-categories "Direct link to Ignore Categories")

If "Ignore Categories" have been set with some values, if a Module has one of those categories, it will not run.

Category names are matched case-insensitively. Pipeline validation fails when a configured run or ignore category does not match any registered module, including when a `RunOnlyCategories` filter would select zero modules.

The fluent `RunOnlyCategories(...)` and `IgnoreCategories(...)` methods replace any category filter configured by an earlier call or by command-line options. Pass the complete filter to each call.

## Example of Running Specific Categories[​](#example-of-running-specific-categories "Direct link to Example of Running Specific Categories")

```
var builder = Pipeline.CreateBuilder(args);



builder

    .AddModule<Module1>()

    .AddModule<Module2>()

    .AddModule<Module3>()

    .AddModule<Module4>();



builder.RunOnlyCategories("UnitTest", "IntegrationTest");



await builder.ExecutePipelineAsync();
```

## Example of Ignoring Specific Categories[​](#example-of-ignoring-specific-categories "Direct link to Example of Ignoring Specific Categories")

```
var builder = Pipeline.CreateBuilder(args);



builder

    .AddModule<Module1>()

    .AddModule<Module2>()

    .AddModule<Module3>()

    .AddModule<Module4>();



builder.IgnoreCategories("Publish", "Deploy");



await builder.ExecutePipelineAsync();
```
