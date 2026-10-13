# Command Line

Pass the application's `args` to `Pipeline.CreateBuilder(args)` to enable the built-in pipeline command line:

```
var builder = Pipeline.CreateBuilder(args);



builder

    .AddModule<BuildModule>()

    .AddModule<TestModule>()

    .AddModule<DeployModule>();



await builder.RunAsync();
```

## Options[​](#options "Direct link to Options")

| Option                       | Behavior                                                                                                                                          |
| ---------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------- |
| `--help`, `-h`               | Prints built-in pipeline command-line usage without executing modules.                                                                            |
| `--dry-run`                  | Validates the pipeline and prints dependency-ordered execution waves, skip reasons, categories, and duration estimates without executing modules. |
| `--no-cache`                 | Disables module cache reads and writes for this run.                                                                                              |
| `--list-modules`             | Lists registered modules, categories, and direct dependencies without executing modules.                                                          |
| `--module <name>`            | Runs a module and its transitive dependency closure. Repeat the option or separate names with commas.                                             |
| `--skip-module <name>`       | Excludes a module. Repeat the option or separate names with commas.                                                                               |
| `--categories <name>`        | Runs modules in the specified categories. Repeat the option or separate names with commas.                                                        |
| `--ignore-categories <name>` | Excludes modules in the specified categories. Repeat the option or separate names with commas.                                                    |
| `--validate`                 | Validates pipeline configuration without executing modules.                                                                                       |

Module names are matched case-insensitively. A simple type name, full type name, or assembly-qualified type name can be used. If a simple name matches multiple registered modules, use a full type name.

For example:

```
dotnet run -- --module TestModule

dotnet run -- --module BuildModule,TestModule --skip-module SlowTestModule

dotnet run -- --categories Test --ignore-categories Integration

dotnet run -- --dry-run

dotnet run -- --no-cache

dotnet run -- --list-modules

dotnet run -- --validate
```

Explicit skips and category filters still apply to targeted dependency closures. If a required dependency is skipped, the existing dependency skip behavior also skips modules that require it.

Arguments that ModularPipelines does not recognize continue to the .NET host and configuration providers. Likely misspellings of pipeline options fail with a suggestion and usage text so that a typo cannot accidentally run the full pipeline. Put `--` before an intentional host argument that resembles a pipeline option; the separator itself is not forwarded.

```
dotnet run -- -- --dryrun=true
```

## Programmatic Selection[​](#programmatic-selection "Direct link to Programmatic Selection")

Use `PipelineOptions` when the selection does not come from command-line arguments:

```
builder.ConfigureOptions(options => options with

{

    TargetModules = [nameof(TestModule)],

    SkippedModules = [nameof(SlowTestModule)],

});
```

`TargetModules` includes each selected module's transitive dependency closure.

Set `DryRun = true` to make `RunAsync()` print the same plan and execute no modules:

```
builder.ConfigureOptions(options => options with

{

    DryRun = true,

});
```

## Programmatic Planning[​](#programmatic-planning "Direct link to Programmatic Planning")

Call `PlanAsync()` to inspect the plan without printing it:

```
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

The planner validates the dependency graph, evaluates module selection, category filters, attribute and fluent skip conditions, cascades required dependency skips, and obtains duration estimates. It does not invoke module bodies or execution lifecycle hooks. A wave estimate is the longest runnable module estimate in that wave. The plan estimate simulates eager dependency scheduling with the configured `MaxParallelism`, CPU/I/O execution limits, and `NotInParallel` constraints; waves are presentation layers, not execution barriers. Fluent conditions that await module results have an unknown skip decision because those results do not exist until execution.

## Disable Built-In Parsing[​](#disable-built-in-parsing "Direct link to Disable Built-In Parsing")

To forward every argument to host configuration, disable pipeline command-line options:

```
var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings

{

    Args = args,

    EnableCommandLineOptions = false,

});
```
