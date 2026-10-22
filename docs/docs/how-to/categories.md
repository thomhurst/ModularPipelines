---
title: Categories
---

# Categories
Sometimes we want to run only certain parts of a pipeline, or we might want to split a pipeline up into different targets. For instance, a test run, and then later on a deploy run. Categories can help achieve that.

## Attribute
Categories are applied to Modules by using the `[ModuleCategory]` attribute.

## PipelineBuilder
Categories to run or ignore are configured by replacing the immutable `PipelineOptions` snapshot with
`PipelineBuilder.ConfigureOptions(...)`.

## Run-Only Categories
If `RunOnlyCategories` has been set with some values, only Modules that have any of those categories will be run. If a module has none of those categories, it will not run.

## Ignore Categories
If "Ignore Categories" have been set with some values, if a Module has one of those categories, it will not run.

Category names are matched case-insensitively. Pipeline validation fails when a configured run or ignore category
does not match any registered module, including when a `RunOnlyCategories` filter would select zero modules.

Assigning `PipelineOptions.RunOnlyCategories` or `PipelineOptions.IgnoreCategories` replaces the previous
value, including values supplied through command-line options. Pass the complete filter when configuring
either property.


## Example of Running Specific Categories

```csharp
var builder = Pipeline.CreateBuilder(args);

builder
    .AddModule<Module1>()
    .AddModule<Module2>()
    .AddModule<Module3>()
    .AddModule<Module4>();

builder.ConfigureOptions(options => options with { RunOnlyCategories = ["UnitTest", "IntegrationTest"] });

await builder.RunAsync();
```


## Example of Ignoring Specific Categories

```csharp
var builder = Pipeline.CreateBuilder(args);

builder
    .AddModule<Module1>()
    .AddModule<Module2>()
    .AddModule<Module3>()
    .AddModule<Module4>();

builder.ConfigureOptions(options => options with { IgnoreCategories = ["Publish", "Deploy"] });

await builder.RunAsync();
```

## Tags

Tags let a module belong to multiple groups. Declare them with repeated
`[ModuleTag("name")]` attributes or fluent `WithTags("name", ...)` configuration,
as shown in [Defining Modules](defining-modules#tags-and-categories). Filters match
tags from both sources case-insensitively.

### Select or Exclude Tags

Use `--tags` to run modules matching **any** selected tag and `--ignore-tags` to
skip modules matching **any** excluded tag. Repeat either option or separate tags
with commas; `--option=value` syntax is also supported:

```shell
dotnet run -- --tags fast,critical --ignore-tags slow
dotnet run -- --tags fast --tags=critical --ignore-tags=slow
```

For the same behavior in code, use the builder methods. For example, assuming
the registered modules declare the `fast`, `critical`, and `slow` tags:

```csharp
var builder = Pipeline.CreateBuilder(args)
    .AddModule<BuildModule>()
    .AddModule<TestModule>()
    .RunOnlyTags("fast", "critical")
    .IgnoreTags("slow");

await builder.RunAsync();
```

Alternatively, set the corresponding immutable `PipelineOptions` properties:

```csharp
builder.ConfigureOptions(options => options with
{
    RunOnlyTags = ["fast", "critical"],
    IgnoreTags = ["slow"],
});
```

Each builder call or property assignment replaces that filter's previous values,
including values supplied on the command line. Pass the complete filter each
time. Calling `RunOnlyTags()` or `IgnoreTags()` with no arguments clears that
filter; assigning `null` or an empty list to its option does the same.

### Matching and Validation

- A nonempty `RunOnlyTags` filter skips modules that match none of its tags,
  including untagged modules. An empty filter does not restrict execution.
- `IgnoreTags` takes precedence over `RunOnlyTags`: a module with both an included
  tag and an excluded tag is skipped. An empty ignore filter excludes no modules,
  and tag exclusions alone leave untagged modules runnable.
- Category and module filters still apply. A module must satisfy both category
  and tag selection when both are configured. Selecting a module with `--module`
  does not bypass tag filters on that module or its dependencies. If a required
  dependency is filtered out, modules that require it are also skipped.
- Pipeline validation rejects any configured run or ignore tag that matches no
  registered module, even when another configured tag does match. If no
  `RunOnlyTags` value matches, validation reports that it would select zero
  registered modules. Include and ignore filters may overlap; exclusion wins.
- Both command-line options require a nonempty value. For example, `--tags`
  without a value or `--ignore-tags=` fails during argument parsing.

Planning commands such as `--dry-run` use the same tag filters as execution.
