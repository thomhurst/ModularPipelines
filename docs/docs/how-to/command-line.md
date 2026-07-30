---
title: Command Line
sidebar_position: 2
---

# Command Line

Pass the application's `args` to `Pipeline.CreateBuilder(args)` to enable the
built-in pipeline command line:

```csharp
using var builder = Pipeline.CreateBuilder(args);

builder
    .AddModule<BuildModule>()
    .AddModule<TestModule>()
    .AddModule<DeployModule>();

await builder.ExecutePipelineAsync();
```

## Options

| Option | Behavior |
| --- | --- |
| `--list-modules` | Lists registered modules, categories, and direct dependencies without executing modules. |
| `--module <name>` | Runs a module and its transitive dependency closure. Repeat the option or separate names with commas. |
| `--skip-module <name>` | Excludes a module. Repeat the option or separate names with commas. |
| `--categories <name>` | Runs modules in the specified categories. Repeat the option or separate names with commas. |
| `--ignore-categories <name>` | Excludes modules in the specified categories. Repeat the option or separate names with commas. |
| `--validate` | Validates pipeline configuration without executing modules. |

Module names are matched case-insensitively. A simple type name, full type name,
or assembly-qualified type name can be used. If a simple name matches multiple
registered modules, use a full type name.

For example:

```shell
dotnet run -- --module TestModule
dotnet run -- --module BuildModule,TestModule --skip-module SlowTestModule
dotnet run -- --categories Test --ignore-categories Integration
dotnet run -- --list-modules
dotnet run -- --validate
```

Explicit skips and category filters still apply to targeted dependency closures.
If a required dependency is skipped, the existing dependency skip behavior also
skips modules that require it.

Arguments that ModularPipelines does not recognize continue to the .NET host and
configuration providers.

## Programmatic Selection

Use `PipelineOptions` when the selection does not come from command-line
arguments:

```csharp
builder.ConfigurePipelineOptions(options => options with
{
    TargetModules = [nameof(TestModule)],
    SkippedModules = [nameof(SlowTestModule)],
});
```

`TargetModules` includes each selected module's transitive dependency closure.

## Disable Built-In Parsing

To forward every argument to host configuration, disable pipeline command-line
options:

```csharp
using var builder = Pipeline.CreateBuilder(new PipelineBuilderOptions
{
    Args = args,
    EnableCommandLineOptions = false,
});
```
