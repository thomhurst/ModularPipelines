---
title: Command Line
sidebar_position: 2
---

# Command Line

Pass the application's `args` to `Pipeline.CreateBuilder(args)` to enable the
built-in pipeline command line:

```csharp
var builder = Pipeline.CreateBuilder(args);

builder
    .AddModule<BuildModule>()
    .AddModule<TestModule>()
    .AddModule<DeployModule>();

await builder.RunAsync();
```

## Options

| Option | Behavior |
| --- | --- |
| `--help`, `-h` | Prints built-in pipeline command-line usage without executing modules. |
| `--dry-run` | Validates the pipeline and prints dependency-ordered execution waves, skip reasons, categories, and duration estimates without executing modules. |
| `--no-cache` | Disables module cache reads and writes, and result reuse with `--rerun-failed`. |
| `--rerun-failed` | Reruns unsuccessful modules and their transitive dependents, reusing compatible stored successful results. |
| `--list-modules` | Lists registered modules, categories, and direct dependencies without executing modules. |
| `--module <name>` | Runs a module and its transitive dependency closure. Repeat the option or separate names with commas. |
| `--skip-module <name>` | Excludes a module. Repeat the option or separate names with commas. |
| `--categories <name>` | Runs modules in the specified categories. Repeat the option or separate names with commas. |
| `--ignore-categories <name>` | Excludes modules in the specified categories. Repeat the option or separate names with commas. |
| `--validate` | Validates pipeline configuration without executing modules. |
| `--graph <format> [path]` | Exports the dependency graph as `mermaid`, `dot`, or `json` without executing modules. |
| `--graph-path <path>` | Sets the graph output path. Requires `--graph` and cannot be combined with its positional path. |

Module names are matched case-insensitively. A simple type name, full type name,
or assembly-qualified type name can be used. If a simple name matches multiple
registered modules, use a full type name.

For example:

```shell
dotnet run -- --module TestModule
dotnet run -- --module BuildModule,TestModule --skip-module SlowTestModule
dotnet run -- --categories Test --ignore-categories Integration
dotnet run -- --dry-run
dotnet run -- --no-cache
dotnet run -- --list-modules
dotnet run -- --validate
dotnet run -- --graph mermaid
dotnet run -- --graph dot artifacts/pipeline.dot
dotnet run -- --graph=json --graph-path=artifacts/pipeline.json
```

Without an explicit path, graph export writes `dependency-graph.mmd`,
`dependency-graph.dot`, or `dependency-graph.json` in the current directory, depending
on the format. Both graph options accept `--option=value` syntax. Specify the format
and output path only once. `--graph` cannot be combined with another pipeline command,
such as `--dry-run`, `--validate`, or `--list-modules`.

Explicit skips and category filters still apply to targeted dependency closures.
If a required dependency is skipped, the existing dependency skip behavior also
skips modules that require it.

Arguments that ModularPipelines does not recognize continue to the .NET host and
configuration providers. Likely misspellings of pipeline options fail with a suggestion
and usage text so that a typo cannot accidentally run the full pipeline. Put `--` before
an intentional host argument that resembles a pipeline option; the separator itself is
not forwarded.

```shell
dotnet run -- -- --dryrun=true
```

## Rerun Failed Modules

```shell
dotnet run -- --rerun-failed
```

The pipeline selects the newest compatible retained run for the same pipeline
identity (`RunReport.PipelineIdentity`, or the identity derived from registered
module types). Compatibility requires the same module types, compiled module
assemblies, result types, and dependency edges. Reports from older schema versions,
changed graphs, and corrupt files are not used. If no compatible history exists,
the selected modules execute normally.

Run reports contain status and telemetry, not typed result values. Configure an
`IModuleResultRepository` with `AddResultsRepository<TRepository>()` to reuse
successful results. The repository must return the actual successful typed result,
including its original module identity and start/end timestamps. Results must match
the retained report. Missing, stale, failed, disabled, or unreadable results cause
the module and its transitive dependents to execute. A missing result never becomes
a default-valued success. The repository remains responsible for validating inputs
and external state that are not represented in the module graph.

Failed, cancelled, skipped, and unknown modules execute again, along with their
transitive dependents. Successful modules can be reused across repeated reruns,
including legitimate null or default result values stored as successful results.
Module and category selection, explicit skips, and module skip conditions still
apply, including their existing historical-result behavior. Rerun execution bypasses
fingerprint cache reads and writes so a failed module or dependent really executes.
Combine with `--no-cache` to disable successful result reuse by the rerun plan as
well. Planning commands (`--dry-run`, `--list-modules`, `--validate`, and graph
export) retain their normal behavior without loading a rerun plan.

This option currently supports in-process pipelines. Distributed master/worker
execution rejects it with an explicit error.

Set `PipelineOptions.RerunFailed = true` for the same behavior programmatically.

## Programmatic Selection

Use `PipelineOptions` when the selection does not come from command-line
arguments:

```csharp
builder.ConfigureOptions(options => options with
{
    TargetModules = [nameof(TestModule)],
    SkippedModules = [nameof(SlowTestModule)],
});
```

`TargetModules` includes each selected module's transitive dependency closure.

Set `DryRun = true` to make `RunAsync()` print the same plan and execute no modules:

```csharp
builder.ConfigureOptions(options => options with
{
    DryRun = true,
});
```

## Programmatic Planning

Call `PlanAsync()` to inspect the plan without printing it:

```csharp
await using var pipeline = await builder.BuildAsync();
PipelinePlan plan = await pipeline.PlanAsync();

foreach (var wave in plan.Waves)
{
    foreach (var module in wave.Modules)
    {
        var decision = module.SkipDecision?.Reason
            ?? (module.IsSkipDecisionKnown ? "Run" : "Unknown: requires module results");
        Console.WriteLine($"{wave.Number}: {module.ModuleName} - {decision}");
    }
}
```

The planner validates the dependency graph, evaluates module selection, category filters,
attribute and fluent skip conditions, cascades required dependency skips, and obtains duration
estimates. It does not invoke module bodies or execution lifecycle hooks. A wave estimate is the
longest runnable module estimate in that wave. The plan estimate simulates eager dependency
scheduling with the configured `MaxParallelism`, CPU/I/O execution limits, and `NotInParallel`
constraints; waves are presentation layers, not execution barriers. Fluent conditions that await
module results have an unknown skip decision because those results do not exist until execution.

## Disable Built-In Parsing

To forward every argument to host configuration, disable pipeline command-line
options:

```csharp
var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
{
    Args = args,
    EnableCommandLineOptions = false,
});
```
