# Migrating from V2 to V3

ModularPipelines V3 is a major release that modernizes the API to follow ASP.NET Core minimal API patterns. This guide covers all breaking changes and how to migrate your existing pipelines.

> **Using V4?** This historical guide shows the V3 API. V4 registers modules directly on `PipelineBuilder`, removes synchronous `Build()`, and validates before execution. See the [current pipeline builder guide](/ModularPipelines/docs/next/how-to/pipeline-host.md).

> **TL;DR - The 6 biggest changes:**
>
> 1. `PipelineHostBuilder.Create()` → `Pipeline.CreateBuilder(args)`
> 2. `IPipelineContext` → `IModuleContext` in ExecuteAsync
> 3. `GetModule<T>()` and `SubModule()` → now on `context` instead of module class
> 4. Virtual overrides (Timeout, ShouldSkip) → `Configure()` fluent builder
> 5. `WorkingDirectory`, `EnvironmentVariables` → now on `CommandExecutionOptions`
> 6. Tool options constructors removed → use property initializers

## Quick Migration Checklist[​](#quick-migration-checklist "Direct link to Quick Migration Checklist")

* <!-- -->
  Replace `PipelineHostBuilder.Create()` with `Pipeline.CreateBuilder(args)`
* <!-- -->
  Replace callback-based configuration with direct property access
* <!-- -->
  Change `IPipelineContext` to `IModuleContext` in `ExecuteAsync` signatures
* <!-- -->
  Update `GetModule<T>()` calls to `context.GetModule<T>()` (method moved to context)
* <!-- -->
  Update `SubModule()` calls to `context.SubModule()` (method moved to context)
* <!-- -->
  Migrate virtual property overrides to `Configure()` builder
* <!-- -->
  Update result access patterns to use pattern matching or `ValueOrDefault`
* <!-- -->
  Move `WorkingDirectory`, `EnvironmentVariables`, etc. from tool options to `CommandExecutionOptions`
* <!-- -->
  Migrate `LogInput`/`LogOutput` to `CommandLoggingOptions` with `CommandLogVerbosity`
* <!-- -->
  Update `context.Command` to `context.Shell.Command`
* <!-- -->
  Replace tool options constructors with property initializers (e.g., `new DotNetNewOptions { TemplateShortName = "x" }`)
* <!-- -->
  Add `token:` named parameter when calling tool commands without `CommandExecutionOptions`

## Table of Contents[​](#table-of-contents "Direct link to Table of Contents")

**What Still Works:**

* [Unchanged Features](#unchanged-features)

**Breaking Changes (in reading order):**

1. [Entry Point Changes](#entry-point-changes) - `Pipeline.CreateBuilder(args)`
2. [Module Behavior Changes](#module-behavior-changes) - `Configure()` builder
3. [Context Parameter Change](#context-parameter-change) - `IModuleContext`
4. [Command Execution Options](#command-execution-options) - Separated from tool options
5. [Command Logging Configuration](#command-logging-configuration) - Verbosity levels
6. [Getting Module Results](#getting-module-results) - Pattern matching
7. [Sub-Module Changes](#sub-module-changes) - Moved to context
8. [Tool Options Classes Regenerated](#tool-options-classes-regenerated) - No constructors
9. [Shell and Command Execution](#shell-and-command-execution) - `context.Shell.Command`
10. [Git and Tool Command Signatures](#git-and-tool-command-signatures) - Optional parameters
11. [Async Configuration Methods](#async-configuration-methods) - Unified sync/async
12. [Deleted Types and Members](#deleted-types-and-members)

**Reference:**

* [New Features in V3](#new-features-in-v3)
* [Complete Migration Example](#complete-migration-example)
* [Breaking API Reference](#breaking-api-reference) - Summary table
* [LLM/AI Migration Reference](#llmai-migration-reference) - For AI assistants

## Unchanged Features[​](#unchanged-features "Direct link to Unchanged Features")

The following features work the same in V3 as they did in V2. **These have NOT been removed:**

* **Hooks**: Global pipeline hooks via `IHook<T>` interfaces remain unchanged
* **Requirements**: `IPipelineRequirement` for validating prerequisites
* **Secrets**: Secret registration and obfuscation via `AddSecret()`
* **Categories and Tags**: Module organization with `[ModuleCategory]` and `[ModuleTag]`
* **DependsOn Attributes**: `[DependsOn<TModule>]` for declaring dependencies
* **Module Logger**: `context.Logger` for logging within modules
* **File System Operations**: `context.FileSystem` for file operations
* **Git Information**: `context.Git().Information` for repository info
* **Sub-Modules**: Still fully supported, but API changed (see [Sub-Module Changes](#sub-module-changes) below)

## Entry Point Changes[​](#entry-point-changes "Direct link to Entry Point Changes")

The pipeline entry point has been completely redesigned to match ASP.NET Core's minimal API pattern.

### Before (V2)[​](#before-v2 "Direct link to Before (V2)")

```
await PipelineHostBuilder.Create()

    .ConfigureAppConfiguration((context, builder) =>

    {

        builder.AddJsonFile("appsettings.json")

            .AddUserSecrets<Program>()

            .AddEnvironmentVariables();

    })

    .ConfigureServices((context, collection) =>

    {

        collection.Configure<MySettings>(context.Configuration.GetSection("MySettings"));



        if (context.HostingEnvironment.IsDevelopment())

        {

            collection.AddModule<DevModule>();

        }



        collection.AddModule<BuildModule>();

    })

    .ConfigurePipelineOptions((context, options) =>

    {

        options.ExecutionMode = ExecutionMode.StopOnFirstException;

    })

    .AddModule<TestModule>()

    .AddModule<DeployModule>()

    .ExecutePipelineAsync();
```

### After (V3)[​](#after-v3 "Direct link to After (V3)")

```
var builder = Pipeline.CreateBuilder(args);



// Direct property access instead of callbacks

builder.Configuration

    .AddJsonFile("appsettings.json")

    .AddUserSecrets<Program>()

    .AddEnvironmentVariables();



// Configure services directly

builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));



if (builder.Environment.IsDevelopment())

{

    builder.Services.AddModule<DevModule>();

}



builder.Services

    .AddModule<BuildModule>()

    .AddModule<TestModule>()

    .AddModule<DeployModule>();



// Configure options directly

builder.Options.ExecutionMode = ExecutionMode.StopOnFirstException;



// Two-step build and run

await builder.Build().RunAsync();
```

### Key Differences[​](#key-differences "Direct link to Key Differences")

| V2                                                  | V3                                 |
| --------------------------------------------------- | ---------------------------------- |
| `PipelineHostBuilder.Create()`                      | `Pipeline.CreateBuilder(args)`     |
| `.ConfigureAppConfiguration((ctx, builder) => ...)` | `builder.Configuration.Add...()`   |
| `.ConfigureServices((ctx, collection) => ...)`      | `builder.Services.Add...()`        |
| `.ConfigurePipelineOptions((ctx, options) => ...)`  | `builder.Options.Property = value` |
| `.AddModule<T>()` on builder                        | `builder.Services.AddModule<T>()`  |
| `.ExecutePipelineAsync()`                           | `.Build().RunAsync()`              |

### Compatibility Note[​](#compatibility-note "Direct link to Compatibility Note")

The `ExecutePipelineAsync()` extension method still exists for simpler migrations:

```
// This still works in V3

await builder.ExecutePipelineAsync();
```

## Module Behavior Changes[​](#module-behavior-changes "Direct link to Module Behavior Changes")

V2 used virtual property and method overrides to configure module behavior. V3 consolidates these into a fluent `Configure()` builder.

### Before (V2)[​](#before-v2-1 "Direct link to Before (V2)")

```
public class MyModule : Module<string>

{

    // Timeout override

    protected internal override TimeSpan Timeout => TimeSpan.FromMinutes(5);



    // Retry policy override

    protected override AsyncRetryPolicy<string?> RetryPolicy =>

        Policy<string?>.Handle<Exception>()

            .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(i * i));



    // Skip logic override

    protected internal override Task<SkipDecision> ShouldSkip(IPipelineContext context)

    {

        if (context.Git().Information.BranchName != "main")

            return Task.FromResult(SkipDecision.Skip("Only runs on main branch"));

        return Task.FromResult(SkipDecision.DoNotSkip);

    }



    // Ignore failures override

    protected internal override Task<bool> ShouldIgnoreFailures(

        IPipelineContext context, Exception exception) => Task.FromResult(true);



    // Always run override

    public override ModuleRunType ModuleRunType => ModuleRunType.AlwaysRun;



    // Lifecycle hooks

    protected internal override Task OnBeforeExecute(IPipelineContext context)

    {

        // Pre-execution logic

        return Task.CompletedTask;

    }



    protected internal override Task OnAfterExecute(IPipelineContext context)

    {

        // Post-execution logic

        return Task.CompletedTask;

    }



    protected override async Task<string?> ExecuteAsync(

        IPipelineContext context, CancellationToken cancellationToken)

    {

        // Module logic

        return "result";

    }

}
```

### After (V3)[​](#after-v3-1 "Direct link to After (V3)")

```
public class MyModule : Module<string>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithTimeout(TimeSpan.FromMinutes(5))

        .WithRetry(3)

        .WithSkipWhen(async ctx => (await ctx.Git().Information.GetInfoAsync())?.BranchName != "main"

            ? SkipDecision.Skip("Only runs on main branch")

            : SkipDecision.DoNotSkip)

        .WithIgnoreFailures()

        .WithAlwaysRun()

        .Build();



    protected override async Task<string?> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        // Module logic

        return "result";

    }

}
```

### Migration Mapping[​](#migration-mapping "Direct link to Migration Mapping")

| V2 Override                                 | V3 Configure() Method                                               |
| ------------------------------------------- | ------------------------------------------------------------------- |
| `TimeSpan Timeout` property                 | `.WithTimeout(TimeSpan)`                                            |
| `AsyncRetryPolicy<T?> RetryPolicy` property | `.WithRetry(int, ...)` or `.Advanced.WithRetryPolicy(IAsyncPolicy)` |
| `Task<SkipDecision> ShouldSkip()` method    | `.WithSkipWhen(...)`                                                |
| `Task<bool> ShouldIgnoreFailures()` method  | `.WithIgnoreFailures()` or `.WithIgnoreFailuresWhen(...)`           |
| `ModuleRunType.AlwaysRun`                   | `.WithAlwaysRun()`                                                  |
| `Task OnBeforeExecute()` method             | `OnBeforeExecuteAsync(...)`                                         |
| `Task OnAfterExecute()` method              | `OnAfterExecuteAsync(...)`                                          |

### Alternative: Lifecycle Hook Overrides[​](#alternative-lifecycle-hook-overrides "Direct link to Alternative: Lifecycle Hook Overrides")

V3 also supports lifecycle hooks as overridable methods on the module class:

```
public class MyModule : Module<string>

{

    protected override Task OnBeforeExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        // Runs before ExecuteAsync

        return Task.CompletedTask;

    }



    protected override Task OnAfterExecuteAsync(

        IModuleContext context,

        ModuleResult<string> result,

        CancellationToken cancellationToken)

    {

        // Runs after ExecuteAsync (success or failure)

        return Task.FromResult<ModuleResult<string>?>(null);

    }



    protected override Task OnSkippedAsync(

        IModuleContext context,

        SkipDecision skipDecision,

        CancellationToken cancellationToken)

    {

        // Runs when module is skipped

        return Task.CompletedTask;

    }



    protected override Task OnFailedAsync(

        IModuleContext context,

        Exception exception,

        CancellationToken cancellationToken)

    {

        // Runs when module fails (before OnAfterExecuteAsync)

        return Task.CompletedTask;

    }



    protected override async Task<string?> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        return "result";

    }

}
```

## Context Parameter Change[​](#context-parameter-change "Direct link to Context Parameter Change")

The `ExecuteAsync` method now receives `IModuleContext` instead of `IPipelineContext`.

### Before (V2)[​](#before-v2-2 "Direct link to Before (V2)")

```
protected override async Task<string?> ExecuteAsync(

    IPipelineContext context, CancellationToken cancellationToken)
```

### After (V3)[​](#after-v3-2 "Direct link to After (V3)")

```
protected override async Task<string?> ExecuteAsync(

    IModuleContext context, CancellationToken cancellationToken)
```

`IModuleContext` extends the pipeline context with module-specific capabilities like `GetModule<TModule>()`.

## Command Execution Options[​](#command-execution-options "Direct link to Command Execution Options")

Execution-related properties have been separated from tool-specific options into a dedicated `CommandExecutionOptions` class. This provides cleaner separation between "what to run" and "how to run it".

### Before (V2)[​](#before-v2-3 "Direct link to Before (V2)")

```
// Execution options were mixed with tool options

await context.DotNet().Build(new DotNetBuildOptions

{

    Project = "MySolution.sln",

    Configuration = Configuration.Release,

    WorkingDirectory = "/path/to/project",        // Was on tool options

    EnvironmentVariables = new Dictionary<string, string?>

    {

        ["CI"] = "true"

    },

    ThrowOnNonZeroExitCode = false

});
```

### After (V3)[​](#after-v3-3 "Direct link to After (V3)")

```
// Tool options only contain tool-specific arguments

await context.DotNet().Build(

    new DotNetBuildOptions

    {

        ProjectSolution = "MySolution.sln",

        Configuration = Configuration.Release,

    },

    new CommandExecutionOptions                    // Execution options are separate

    {

        WorkingDirectory = "/path/to/project",

        EnvironmentVariables = new Dictionary<string, string?>

        {

            ["CI"] = "true"

        },

        ThrowOnNonZeroExitCode = false

    });
```

### Migration Mapping[​](#migration-mapping-1 "Direct link to Migration Mapping")

| V2 (on tool options)       | V3 (on `CommandExecutionOptions`) |
| -------------------------- | --------------------------------- |
| `WorkingDirectory`         | `WorkingDirectory`                |
| `EnvironmentVariables`     | `EnvironmentVariables`            |
| `ThrowOnNonZeroExitCode`   | `ThrowOnNonZeroExitCode`          |
| `CommandLineCredentials`   | `CommandLineCredentials`          |
| `LoggingSettings`          | `LogSettings`                     |
| `InputLoggingManipulator`  | `InputLoggingManipulator`         |
| `OutputLoggingManipulator` | `OutputLoggingManipulator`        |
| N/A                        | `ExecutionTimeout` (new)          |
| N/A                        | `GracefulShutdownTimeout` (new)   |
| N/A                        | `Sudo` (new)                      |

### Benefits[​](#benefits "Direct link to Benefits")

* **Cleaner API**: Tool options focus only on tool-specific arguments
* **Reusability**: Share `CommandExecutionOptions` across multiple commands
* **New features**: `ExecutionTimeout`, `GracefulShutdownTimeout`, and `Sudo` options

## Command Logging Configuration[​](#command-logging-configuration "Direct link to Command Logging Configuration")

V3 introduces a new `CommandLoggingOptions` system that replaces the previous logging configuration. The new system provides verbosity levels and fine-grained control over what gets logged.

### Before (V2)[​](#before-v2-4 "Direct link to Before (V2)")

```
// Logging settings were on tool options with limited control

await context.DotNet().Build(new DotNetBuildOptions

{

    LogInput = true,

    LogOutput = false,

    // Limited options available

});
```

### After (V3)[​](#after-v3-4 "Direct link to After (V3)")

```
// Rich logging configuration via CommandLoggingOptions

await context.DotNet().Build(

    new DotNetBuildOptions { Configuration = "Release" },

    new CommandExecutionOptions

    {

        LogSettings = new CommandLoggingOptions

        {

            Verbosity = CommandLogVerbosity.Detailed,

            ShowCommandArguments = true,

            ShowStandardOutput = true,

            ShowStandardError = true,

            ShowExitCode = true,

            ShowExecutionTime = true

        }

    });
```

### Verbosity Levels[​](#verbosity-levels "Direct link to Verbosity Levels")

| Level        | Description                                           |
| ------------ | ----------------------------------------------------- |
| `Silent`     | No output at all                                      |
| `Minimal`    | Only command input (no output/errors)                 |
| `Normal`     | Input, output, and errors on failure (default)        |
| `Detailed`   | Above plus exit code and duration                     |
| `Diagnostic` | Everything including working directory and timestamps |

### Configuration Precedence[​](#configuration-precedence "Direct link to Configuration Precedence")

V3 uses a three-tier configuration system (highest to lowest priority):

1. **Per-Call**: `CommandExecutionOptions.LogSettings` on individual command calls
2. **Global Default**: `PipelineOptions.DefaultLoggingOptions` set at pipeline level
3. **System Default**: `CommandLoggingOptions.Default` (Normal verbosity)

### Setting Global Defaults[​](#setting-global-defaults "Direct link to Setting Global Defaults")

```
var builder = Pipeline.CreateBuilder(args);



// Set global default for all commands

builder.Options.DefaultLoggingOptions = new CommandLoggingOptions

{

    Verbosity = CommandLogVerbosity.Minimal

};



// Or use presets

builder.Options.DefaultLoggingOptions = CommandLoggingOptions.Silent;

builder.Options.DefaultLoggingOptions = CommandLoggingOptions.Diagnostic;



await builder.Build().RunAsync();
```

### Using Presets[​](#using-presets "Direct link to Using Presets")

```
// Silent - no command logging at all

new CommandExecutionOptions { LogSettings = CommandLoggingOptions.Silent }



// Diagnostic - maximum verbosity for debugging

new CommandExecutionOptions { LogSettings = CommandLoggingOptions.Diagnostic }



// Default - normal verbosity

new CommandExecutionOptions { LogSettings = CommandLoggingOptions.Default }
```

### Fine-Grained Control[​](#fine-grained-control "Direct link to Fine-Grained Control")

Override individual settings regardless of verbosity level:

```
new CommandLoggingOptions

{

    Verbosity = CommandLogVerbosity.Normal,

    ShowCommandArguments = true,

    ShowStandardOutput = true,

    ShowStandardError = true,

    ShowExitCode = true,           // Show even at Normal verbosity

    ShowExecutionTime = true,      // Show even at Normal verbosity

    ShowWorkingDirectory = false,

    IncludeTimestamps = false

}
```

### Output Manipulators[​](#output-manipulators "Direct link to Output Manipulators")

Transform logged content before it's written:

```
new CommandExecutionOptions

{

    LogSettings = new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Normal },

    InputLoggingManipulator = input => input.Length > 100

        ? input.Substring(0, 100) + "..."

        : input,

    OutputLoggingManipulator = output => output.Replace("secret-value", "***")

}
```

### Migration Mapping[​](#migration-mapping-2 "Direct link to Migration Mapping")

| V2                                         | V3                                                                 |
| ------------------------------------------ | ------------------------------------------------------------------ |
| `LogInput = true/false`                    | `ShowCommandArguments = true/false`                                |
| `LogOutput = true/false`                   | `ShowStandardOutput = true/false`                                  |
| `InputLoggingManipulator` on tool options  | `InputLoggingManipulator` on `CommandExecutionOptions`             |
| `OutputLoggingManipulator` on tool options | `OutputLoggingManipulator` on `CommandExecutionOptions`            |
| N/A                                        | `Verbosity` levels (Silent, Minimal, Normal, Detailed, Diagnostic) |
| N/A                                        | `ShowExitCode`, `ShowExecutionTime`, `ShowWorkingDirectory`        |
| N/A                                        | `IncludeTimestamps`                                                |
| N/A                                        | Global defaults via `PipelineOptions.DefaultLoggingOptions`        |

## Getting Module Results[​](#getting-module-results "Direct link to Getting Module Results")

The `GetModule` method has moved from the module base class to the context. The result access patterns have also changed to use a discriminated union.

### Before (V2)[​](#before-v2-5 "Direct link to Before (V2)")

```
[DependsOn<BuildModule>]

public class DeployModule : Module<DeployResult>

{

    protected override async Task<DeployResult?> ExecuteAsync(

        IPipelineContext context, CancellationToken cancellationToken)

    {

        // Method on module base class

        var buildResult = await GetModule<BuildModule>();



        // Direct value access

        var artifact = buildResult.Value!.ArtifactPath;



        // Enum-based status check

        if (buildResult.ModuleResultType == ModuleResultType.Skipped)

        {

            return null;

        }



        if (buildResult.ModuleResultType == ModuleResultType.Failure)

        {

            throw new Exception("Build failed", buildResult.Exception);

        }



        return await Deploy(artifact);

    }

}
```

### After (V3)[​](#after-v3-5 "Direct link to After (V3)")

```
[DependsOn<BuildModule>]

public class DeployModule : Module<DeployResult>

{

    protected override async Task<DeployResult?> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        // Method moved to context

        var buildResult = await context.GetModule<BuildModule>();



        // Option 1: Pattern matching (recommended)

        return buildResult switch

        {

            ModuleResult<BuildOutput>.Success { Value: var output }

                => await Deploy(output.ArtifactPath),

            ModuleResult<BuildOutput>.Skipped { Decision: var skip }

                => null,

            ModuleResult<BuildOutput>.Failure { Exception: var ex }

                => throw new InvalidOperationException("Build failed", ex),

            _ => throw new InvalidOperationException("Unexpected result type")

        };

    }

}
```

### Alternative Result Access Patterns[​](#alternative-result-access-patterns "Direct link to Alternative Result Access Patterns")

```
var buildResult = await context.GetModule<BuildModule>();



// Option 1: Pattern matching (recommended - handles all cases)

return buildResult switch

{

    ModuleResult<BuildOutput>.Success { Value: var output } => Process(output),

    ModuleResult<BuildOutput>.Skipped => null,

    ModuleResult<BuildOutput>.Failure { Exception: var ex } => throw ex,

    _ => null

};



// Option 2: Match helper method (functional style)

return buildResult.Match(

    onSuccess: output => Process(output),

    onFailure: ex => throw new InvalidOperationException("Failed", ex),

    onSkipped: skip => null

);



// Option 3: Safe accessor (simplest migration path)

var artifact = buildResult.ValueOrDefault?.ArtifactPath;

if (artifact == null) return null;

return await Deploy(artifact);



// Option 4: Safe accessors and the Success union case

if (buildResult is ModuleResult<BuildOutput>.Success)

{

    var value = buildResult.ValueOrDefault;

}

if (buildResult.ExceptionOrDefault is { } ex)

{

}

if (buildResult.SkipDecisionOrDefault is { } skip)

{

    var reason = skip.Reason;

}
```

### Key Change Summary[​](#key-change-summary "Direct link to Key Change Summary")

| V2                                 | V3                                                                      |
| ---------------------------------- | ----------------------------------------------------------------------- |
| `await GetModule<T>()` (on module) | `await context.GetModule<T>()` (on context)                             |
| `result.Value`                     | `result.ValueOrDefault` or pattern match                                |
| `result.Exception`                 | `result.ExceptionOrDefault` or pattern match                            |
| Legacy result enum check           | Pattern match or inspect `ExceptionOrDefault` / `SkipDecisionOrDefault` |

### Result Type Quick Reference[​](#result-type-quick-reference "Direct link to Result Type Quick Reference")

| Check         | V2                       | V3                                           |
| ------------- | ------------------------ | -------------------------------------------- |
| Is success?   | Legacy result enum check | `result is ModuleResult<T>.Success`          |
| Is failure?   | Legacy result enum check | `result.ExceptionOrDefault is not null`      |
| Is skipped?   | Legacy result enum check | `result.SkipDecisionOrDefault is not null`   |
| Get value     | `result.Value`           | `result.ValueOrDefault` or pattern match     |
| Get exception | `result.Exception`       | `result.ExceptionOrDefault` or pattern match |

## Sub-Module Changes[​](#sub-module-changes "Direct link to Sub-Module Changes")

Sub-modules have moved from being a protected method on the module class to being on the context, similar to `GetModule`.

### Before (V2)[​](#before-v2-6 "Direct link to Before (V2)")

```
public class PackageModule : Module<PackageResult>

{

    protected override async Task<PackageResult?> ExecuteAsync(

        IPipelineContext context, CancellationToken cancellationToken)

    {

        var packages = new[] { "Package1", "Package2", "Package3" };



        foreach (var package in packages)

        {

            // Protected method on module class

            await SubModule(package, async () =>

            {

                await context.DotNet().Pack(new DotNetPackOptions { Project = package });

            });

        }



        return new PackageResult(packages.Length);

    }

}
```

### After (V3)[​](#after-v3-6 "Direct link to After (V3)")

```
public class PackageModule : Module<PackageResult>

{

    protected override async Task<PackageResult?> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        var packages = new[] { "Package1", "Package2", "Package3" };



        foreach (var package in packages)

        {

            // Method moved to context

            await context.SubModule(package, async () =>

            {

                await context.DotNet().Pack(new DotNetPackOptions { Project = package });

            });

        }



        return new PackageResult(packages.Length);

    }

}
```

### With Return Value[​](#with-return-value "Direct link to With Return Value")

```
// V2: await SubModule<T>(name, func)

// V3: await context.SubModule<T>(name, func)



var result = await context.SubModule<string>("ProcessItem", async () =>

{

    // Process and return value

    return "processed";

});
```

### Key Change[​](#key-change "Direct link to Key Change")

| V2                                                  | V3                                       |
| --------------------------------------------------- | ---------------------------------------- |
| `await SubModule(name, action)` (protected method)  | `await context.SubModule(name, action)`  |
| `await SubModule<T>(name, func)` (protected method) | `await context.SubModule<T>(name, func)` |

## Tool Options Classes Regenerated[​](#tool-options-classes-regenerated "Direct link to Tool Options Classes Regenerated")

**Important**: All tool options classes (DotNet, Git, Docker, Azure, etc.) have been regenerated using CLI scraping. This means:

* **No constructors**: Options classes no longer accept constructor arguments. Use property initializers instead.
* **Properties may have changed**: Some properties may have been renamed, removed, or added based on current CLI help output.
* **Use property initializers**: Always use object initializer syntax.

### Before (V2)[​](#before-v2-7 "Direct link to Before (V2)")

```
// Constructor arguments - NO LONGER WORKS

var newOptions = new DotNetNewOptions("console");

var packOptions = new DotNetPackOptions(projectPath);

var pushOptions = new DotNetNugetPushOptions(packagePath);
```

### After (V3)[​](#after-v3-7 "Direct link to After (V3)")

```
// Use property initializers instead

var newOptions = new DotNetNewOptions { TemplateShortName = "console" };

var packOptions = new DotNetPackOptions { TargetPath = projectPath };

var pushOptions = new DotNetNugetPushOptions { PackagePath = packagePath };
```

### Finding the Right Property Names[​](#finding-the-right-property-names "Direct link to Finding the Right Property Names")

If you're unsure which property to use:

1. **IntelliSense**: Type `new DotNetXxxOptions { }` and explore available properties
2. **Source code**: Options are in `ModularPipelines.<Tool>/Options/` directories
3. **CLI help**: Property names typically match CLI flag names (e.g., `--output` → `Output`)

## Shell and Command Execution[​](#shell-and-command-execution "Direct link to Shell and Command Execution")

The command execution API has moved from `context.Command` to `context.Shell.Command`.

### Before (V2)[​](#before-v2-8 "Direct link to Before (V2)")

```
await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("mytool")

{

    Arguments = new[] { "arg1", "arg2" }

});
```

### After (V3)[​](#after-v3-8 "Direct link to After (V3)")

```
await context.Shell.Command.ExecuteCommandLineTool(

    new CommandLineToolOptions("mytool")

    {

        Arguments = new[] { "arg1", "arg2" }

    },

    new CommandExecutionOptions

    {

        WorkingDirectory = "/path/to/dir"

    });
```

### Shell Context Structure[​](#shell-context-structure "Direct link to Shell Context Structure")

```
context.Shell.Command    // General CLI tool execution (ICommandContext)

context.Shell.Bash       // Bash script execution

context.Shell.PowerShell // PowerShell script execution
```

## Git and Tool Command Signatures[​](#git-and-tool-command-signatures "Direct link to Git and Tool Command Signatures")

All tool commands (Git, DotNet, Docker, etc.) now accept an optional `CommandExecutionOptions` parameter. This is **optional** - you don't need to provide it if using defaults.

### Before (V2)[​](#before-v2-9 "Direct link to Before (V2)")

```
await context.Git().Tag(new GitTagOptions

{

    TagName = "v1.0.0",

    Message = "Release v1.0.0"

}, cancellationToken);
```

### After (V3)[​](#after-v3-9 "Direct link to After (V3)")

```
// Without execution options (most common)

await context.Git().Tag(new GitTagOptions

{

    TagName = "v1.0.0",

    Message = "Release v1.0.0"

}, token: cancellationToken);



// With execution options (when needed)

await context.Git().Tag(

    new GitTagOptions

    {

        TagName = "v1.0.0",

        Message = "Release v1.0.0"

    },

    new CommandExecutionOptions { WorkingDirectory = repoPath },

    cancellationToken);
```

### Note on Parameter Names[​](#note-on-parameter-names "Direct link to Note on Parameter Names")

When calling without `CommandExecutionOptions`, use named parameter `token:` for the cancellation token to avoid ambiguity:

```
// Correct - named parameter

await context.Git().Push(options, token: cancellationToken);



// May be ambiguous without named parameter

await context.Git().Push(options, cancellationToken); // Could fail
```

## Async Configuration Methods[​](#async-configuration-methods "Direct link to Async Configuration Methods")

The `WithSkipWhen` and `WithIgnoreFailuresWhen` methods accept both sync and async lambdas. There are **no separate `Async` versions** - the same method handles both:

```
// Sync lambda

.WithSkipWhen(() => someCondition)



// Async lambda - same method name

.WithSkipWhen(async () => await CheckConditionAsync())



// With context - async repository lookup

.WithSkipWhen(async ctx =>

    (await ctx.Git().Information.GetInfoAsync())?.BranchName != "main")



// With context - async

.WithSkipWhen(async ctx => await ctx.SomeAsyncCheck())



// Returning SkipDecision

.WithSkipWhen(async ctx =>

    (await ctx.Git().Information.GetInfoAsync())?.BranchName != "main"

    ? SkipDecision.Skip("Not main branch")

    : SkipDecision.DoNotSkip)
```

The same applies to `WithIgnoreFailuresWhen`:

```
// Sync

.WithIgnoreFailuresWhen((ctx, ex) => ex is TimeoutException)



// Async

.WithIgnoreFailuresWhen(async (ctx, ex) => await ShouldIgnoreAsync(ex))
```

## Deleted Types and Members[​](#deleted-types-and-members "Direct link to Deleted Types and Members")

The following have been removed in V3:

| Removed                         | Replacement                                                |
| ------------------------------- | ---------------------------------------------------------- |
| `PipelineHostBuilder` class     | `Pipeline.CreateBuilder()` returns `PipelineBuilder`       |
| `ModuleBase` class              | `Module<T>` (simplified hierarchy)                         |
| `ModuleBase<T>` class           | `Module<T>`                                                |
| `ShouldSkip()` method           | `Configure().WithSkipWhen()`                               |
| `ShouldIgnoreFailures()` method | `Configure().WithIgnoreFailures()`                         |
| `ModuleRunType` property        | `Configure().WithAlwaysRun()`                              |
| `Timeout` property              | `Configure().WithTimeout()`                                |
| `RetryPolicy` property          | `Configure().WithRetry()` or `.Advanced.WithRetryPolicy()` |
| `GetModule<T>()` on module      | `context.GetModule<TModule>()`                             |

## New Features in V3[​](#new-features-in-v3 "Direct link to New Features in V3")

### Modules Without Results[​](#modules-without-results "Direct link to Modules Without Results")

V3 introduced non-generic convenience bases for modules without results. V4 removes those bases so execution consistently uses `ExecuteAsync` or `Execute`. Current code uses `Module<None>` and `SyncModule<None>` instead.

```
// Async module that doesn't return data

public class DeployModule : Module<None>

{

    protected override async Task<None> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        await context.Shell.Command.ExecuteCommandLineTool(...);

        return None.Value;

    }

}



// Sync module that doesn't return data

public class LoggingModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context, CancellationToken cancellationToken)

    {

        context.Logger.LogInformation("Pipeline executed at {Time}", DateTime.UtcNow);

        return None.Value;

    }

}
```

The `None` struct represents "nothing" and is semantically equivalent to `null`:

```
None value = None.Value;

value.Equals(null);     // true - None equals null

value == default;       // true

None? nullable = null;

nullable == value;      // true - None? and None are always equal
```

### Pipeline Validation[​](#pipeline-validation "Direct link to Pipeline Validation")

V3 introduces a validation API to catch configuration errors before execution:

```
var builder = Pipeline.CreateBuilder(args);

builder.Services.AddModule<MyModule>();



// Option 1: Validate without running

var validation = await builder.ValidateAsync();

if (validation.HasErrors)

{

    foreach (var error in validation.Errors)

    {

        Console.WriteLine($"[{error.Category}] {error.Message}");

    }

    Environment.Exit(1);

}



// Option 2: BuildAsync validates and throws on error

try

{

    var pipeline = await builder.BuildAsync();

    await pipeline.RunAsync();

}

catch (PipelineValidationException ex)

{

    foreach (var error in ex.ValidationResult.Errors)

    {

        Console.WriteLine($"[{error.Category}] {error.Message}");

    }

}
```

### Fluent Dependencies[​](#fluent-dependencies "Direct link to Fluent Dependencies")

Declare dependencies through module configuration:

```
public class MyModule : Module<string>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .DependsOn<RequiredModule>()

        .DependsOnOptional<OptionalModule>()

        .DependsOnIf<ProductionModule>(Environment.IsProduction)

        .Build();



    protected override async Task<string?> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        // ...

    }

}
```

Two redundant dependency APIs have been removed:

* Replace `DependsOnLazy<TModule>()` with `DependsOnOptional<TModule>()`. The removed API was only an optional-dependency marker; it did not defer module execution.
* Replace `DependsOnIf<TModule>(Func<bool>)` with `DependsOnIf<TModule>(bool)` by evaluating the predicate before the call, for example `.DependsOnIf<HeavyModule>(ShouldRunHeavyProcessing())`.

The same replacements apply to the overloads that accept a module `Type`.

### New Dependency Attributes[​](#new-dependency-attributes "Direct link to New Dependency Attributes")

```
// Depend on all modules in a category

[DependsOnModulesInCategory("Build")]

public class TestModule : Module<TestResults> { }



// Depend on all modules with a specific tag

[DependsOnModulesWithTag("database")]

public class MigrationModule : Module<bool> { }



// Depend on all modules with a specific attribute

[DependsOnModulesWithAttribute(typeof(CriticalAttribute))]

public class ValidationModule : Module<bool> { }
```

### Conditional Execution Attributes[​](#conditional-execution-attributes "Direct link to Conditional Execution Attributes")

```
// Run only on specific platforms

[RunIfAll<OnWindows>]

public class WindowsModule : Module<string> { }



[RunIfAll<OnLinux>]

public class LinuxModule : Module<string> { }



[RunIfAll<OnMacOS>]

public class MacModule : Module<string> { }



// Skip based on custom condition

[SkipIf<IsNotMainBranchCondition>]

public class MainBranchModule : Module<string> { }



// Combine conditions

[RunIfAll<IsCI, IsMainBranch>]

public class CIMainModule : Module<string> { }



[RunIfAny<IsCI, ForceRun>]

public class FlexibleModule : Module<string> { }
```

### Module Tags[​](#module-tags "Direct link to Module Tags")

Tag modules for organization and dependency management:

```
// Via attributes

[ModuleTag("critical")]

[ModuleTag("deployment")]

[ModuleCategory("Infrastructure")]

public class DeployModule : Module<DeployResult> { }



// Via module configuration

public class MyModule : Module<string>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithTags("critical", "fast")

        .WithCategory("Build")

        .Build();

}
```

### Plugin System[​](#plugin-system "Direct link to Plugin System")

Create reusable pipeline extensions:

```
public class MyPlugin : IModularPipelinesPlugin

{

    public string Name => "MyPlugin";

    public int Priority => 0; // Lower runs first



    public void ConfigureServices(IServiceCollection services)

    {

        services.AddSingleton<IMyService, MyService>();

    }



    public void ConfigurePipeline(PipelineBuilder builder)

    {

        builder.Services.AddModule<PluginModule>();

        builder.Options.PrintLogo = false;

    }

}



// Register plugin via attribute on assembly

[assembly: ModularPipelinesPlugin(typeof(MyPlugin))]
```

## Complete Migration Example[​](#complete-migration-example "Direct link to Complete Migration Example")

### Before (V2)[​](#before-v2-10 "Direct link to Before (V2)")

```
// Program.cs

await PipelineHostBuilder.Create()

    .ConfigureAppConfiguration((ctx, builder) =>

    {

        builder.AddJsonFile("appsettings.json");

    })

    .ConfigureServices((ctx, services) =>

    {

        services.AddModule<BuildModule>()

            .AddModule<TestModule>()

            .AddModule<DeployModule>();

    })

    .ExecutePipelineAsync();



// BuildModule.cs

public class BuildModule : Module<BuildOutput>

{

    protected internal override TimeSpan Timeout => TimeSpan.FromMinutes(10);



    protected override async Task<BuildOutput?> ExecuteAsync(

        IPipelineContext context, CancellationToken cancellationToken)

    {

        var result = await context.DotNet().Build(new DotNetBuildOptions());

        return new BuildOutput(result.StandardOutput);

    }

}



// DeployModule.cs

[DependsOn<BuildModule>]

[DependsOn<TestModule>]

public class DeployModule : Module<bool>

{

    protected internal override Task<SkipDecision> ShouldSkip(IPipelineContext context)

    {

        if (context.Git().Information.BranchName != "main")

            return Task.FromResult(SkipDecision.Skip("Not main branch"));

        return Task.FromResult(SkipDecision.DoNotSkip);

    }



    protected override async Task<bool> ExecuteAsync(

        IPipelineContext context, CancellationToken cancellationToken)

    {

        var buildResult = await GetModule<BuildModule>();



        if (buildResult.ModuleResultType != ModuleResultType.Success)

            return false;



        // Deploy using buildResult.Value

        return true;

    }

}
```

### After (V3)[​](#after-v3-10 "Direct link to After (V3)")

```
// Program.cs

var builder = Pipeline.CreateBuilder(args);



builder.Configuration.AddJsonFile("appsettings.json");



builder.Services

    .AddModule<BuildModule>()

    .AddModule<TestModule>()

    .AddModule<DeployModule>();



await builder.Build().RunAsync();



// BuildModule.cs

public class BuildModule : Module<BuildOutput>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithTimeout(TimeSpan.FromMinutes(10))

        .Build();



    protected override async Task<BuildOutput?> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        var result = await context.DotNet().Build(new DotNetBuildOptions());

        return new BuildOutput(result.StandardOutput);

    }

}



// DeployModule.cs

[DependsOn<BuildModule>]

[DependsOn<TestModule>]

public class DeployModule : Module<bool>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithSkipWhen(async ctx => (await ctx.Git().Information.GetInfoAsync())?.BranchName != "main"

            ? SkipDecision.Skip("Not main branch")

            : SkipDecision.DoNotSkip)

        .Build();



    protected override async Task<bool> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        var buildResult = await context.GetModule<BuildModule>();



        if (buildResult is not ModuleResult<BuildOutput>.Success { Value: var output })

            return false;



        // Deploy using output

        return true;

    }

}
```

## Breaking API Reference[​](#breaking-api-reference "Direct link to Breaking API Reference")

| V2 API                            | V3 API                                              | Notes                       |
| --------------------------------- | --------------------------------------------------- | --------------------------- |
| `PipelineHostBuilder.Create()`    | `Pipeline.CreateBuilder(args)`                      | Pass command-line args      |
| `.ExecutePipelineAsync()`         | `.Build().RunAsync()`                               | Two-step, or use extension  |
| `.ConfigureAppConfiguration(...)` | `builder.Configuration`                             | Direct access               |
| `.ConfigureServices(...)`         | `builder.Services`                                  | Direct access               |
| `.ConfigurePipelineOptions(...)`  | `builder.Options`                                   | Direct access               |
| `IPipelineContext`                | `IModuleContext`                                    | In ExecuteAsync signature   |
| `GetModule<T>()`                  | `context.GetModule<T>()`                            | Method moved to context     |
| `SubModule()`                     | `context.SubModule()`                               | Method moved to context     |
| `result.Value`                    | `result.ValueOrDefault`                             | Or use pattern matching     |
| Legacy result enum                | `ModuleStatus`, safe accessors, or pattern matching | One result model            |
| `ShouldSkip()` override           | `Configure().WithSkipWhen()`                        | Fluent builder              |
| `ShouldIgnoreFailures()` override | `Configure().WithIgnoreFailures()`                  | Fluent builder              |
| `Timeout` property override       | `Configure().WithTimeout()`                         | Fluent builder              |
| `RetryPolicy` property override   | `Configure().WithRetry()`                           | Fluent builder              |
| `ModuleRunType` override          | `Configure().WithAlwaysRun()`                       | Fluent builder              |
| `OnBeforeExecute()` override      | `OnBeforeExecuteAsync()`                            | Override the module virtual |
| `OnAfterExecute()` override       | `OnAfterExecuteAsync()`                             | Override the module virtual |
| `options.WorkingDirectory`        | `CommandExecutionOptions.WorkingDirectory`          | Separate parameter          |
| `options.EnvironmentVariables`    | `CommandExecutionOptions.EnvironmentVariables`      | Separate parameter          |
| `options.ThrowOnNonZeroExitCode`  | `CommandExecutionOptions.ThrowOnNonZeroExitCode`    | Separate parameter          |

## LLM/AI Migration Reference[​](#llmai-migration-reference "Direct link to LLM/AI Migration Reference")

This section provides structured data optimized for AI assistants helping with code migration.

> **IMPORTANT FOR AI ASSISTANTS:**
>
> * **SubModule API changed**: `SubModule()` moved from module class to `context.SubModule()`
> * **GetModule API changed**: `GetModule<T>()` moved from module class to `context.GetModule<T>()`
>
> **Unchanged features** (still work the same):
>
> * `[DependsOn<TModule>]` attributes
> * `IHook<T>` interfaces
> * `IPipelineRequirement`
> * `context.Logger`
> * `context.FileSystem`
> * `context.Git().Information`

### Complete API Transformation Map[​](#complete-api-transformation-map "Direct link to Complete API Transformation Map")

```
# Entry Point Changes

- old: "PipelineHostBuilder.Create()"

  new: "Pipeline.CreateBuilder(args)"



- old: ".ExecutePipelineAsync()"

  new: ".Build().RunAsync()"



- old: ".ConfigureAppConfiguration((context, builder) => { ... })"

  new: "builder.Configuration.Add...()"



- old: ".ConfigureServices((context, collection) => { ... })"

  new: "builder.Services.Add...()"



- old: ".ConfigurePipelineOptions((context, options) => { ... })"

  new: "builder.Options.PropertyName = value"



# Context Parameter

- old: "IPipelineContext context"

  new: "IModuleContext context"

  scope: "ExecuteAsync method signature"



# Module Result Access

- old: "await GetModule<TModule>()"

  new: "await context.GetModule<TModule>()"



# Sub-Module Access

- old: "await SubModule(name, action)"

  new: "await context.SubModule(name, action)"

  note: "Protected method moved to context"



- old: "await SubModule<T>(name, func)"

  new: "await context.SubModule<T>(name, func)"

  note: "Protected method moved to context"



- old: "result.Value"

  new: "result.ValueOrDefault"



- old: "result.Exception"

  new: "result.ExceptionOrDefault"



- old: "legacy success enum check"

  new: "result is ModuleResult<T>.Success"



- old: "legacy failure enum check"

  new: "result.ExceptionOrDefault is not null"



- old: "legacy skipped enum check"

  new: "result.SkipDecisionOrDefault is not null"



# Module Configuration (property overrides → fluent builder)

- old: "protected internal override TimeSpan Timeout => ..."

  new: "Configure().WithTimeout(TimeSpan)"



- old: "protected override AsyncRetryPolicy<T?> RetryPolicy => ..."

  new: "Configure().WithRetry(int)"



- old: "protected internal override Task<SkipDecision> ShouldSkip(...)"

  new: "Configure().WithSkipWhen(Func<IModuleContext, SkipDecision>)"



- old: "protected internal override Task<bool> ShouldIgnoreFailures(...)"

  new: "Configure().WithIgnoreFailures()"



- old: "public override ModuleRunType ModuleRunType => ModuleRunType.AlwaysRun"

  new: "Configure().WithAlwaysRun()"



- old: "protected internal override Task OnBeforeExecute(IPipelineContext context)"

  new: "OnBeforeExecuteAsync(...)"



- old: "protected internal override Task OnAfterExecute(IPipelineContext context)"

  new: "OnAfterExecuteAsync(...)"



# Command Execution Options (moved from tool options to separate parameter)

- old: "new DotNetBuildOptions { WorkingDirectory = path }"

  new: "new DotNetBuildOptions { }, new CommandExecutionOptions { WorkingDirectory = path }"



- old: "new DotNetBuildOptions { EnvironmentVariables = dict }"

  new: "new DotNetBuildOptions { }, new CommandExecutionOptions { EnvironmentVariables = dict }"



- old: "new DotNetBuildOptions { ThrowOnNonZeroExitCode = false }"

  new: "new DotNetBuildOptions { }, new CommandExecutionOptions { ThrowOnNonZeroExitCode = false }"



# Command Logging (new system in V3)

- old: "new DotNetBuildOptions { LogInput = true, LogOutput = false }"

  new: "new CommandExecutionOptions { LogSettings = new CommandLoggingOptions { ShowCommandArguments = true, ShowStandardOutput = false } }"



- old: "InputLoggingManipulator on tool options"

  new: "InputLoggingManipulator on CommandExecutionOptions"



- old: "OutputLoggingManipulator on tool options"

  new: "OutputLoggingManipulator on CommandExecutionOptions"



- new_only: "CommandLogVerbosity.Silent/Minimal/Normal/Detailed/Diagnostic"

  note: "Use verbosity levels for quick configuration"



- new_only: "builder.Options.DefaultLoggingOptions = CommandLoggingOptions.Silent"

  note: "Set global defaults at pipeline level"



- new_only: "CommandLoggingOptions.Silent / .Diagnostic / .Default presets"

  note: "Pre-configured logging options"



# Shell/Command Execution (API restructured)

- old: "context.Command.ExecuteCommandLineTool(...)"

  new: "context.Shell.Command.ExecuteCommandLineTool(...)"



- old: "context.Bash.ExecuteCommand(...)"

  new: "context.Shell.Bash.ExecuteCommand(...)"



- old: "context.Powershell.ExecuteCommand(...)"

  new: "context.Shell.PowerShell.ExecuteCommand(...)"



# Tool Options Constructors (removed - use property initializers)

- old: "new DotNetNewOptions(\"console\")"

  new: "new DotNetNewOptions { TemplateShortName = \"console\" }"

  note: "All tool options constructors removed - use property initializers"



- old: "new DotNetPackOptions(projectPath)"

  new: "new DotNetPackOptions { TargetPath = projectPath }"



- old: "new GitTagOptions(\"v1.0.0\")"

  new: "new GitTagOptions { TagName = \"v1.0.0\" }"



# Async Configuration (no separate Async methods)

- old: "WithSkipWhenAsync(async () => ...)"

  new: "WithSkipWhen(async () => ...)"

  note: "Same method accepts both sync and async lambdas"



- old: "WithIgnoreFailuresWhenAsync(...)"

  new: "WithIgnoreFailuresWhen(...)"

  note: "Same method accepts both sync and async lambdas"



# Modules without results

- old: "public class MyModule : Module<IDictionary<string, object>>"

  new: "public class MyModule : Module<None>"

  note: "Use None when the module does not return data"



- old: "protected override async Task<IDictionary<string, object>?> ExecuteAsync(...)"

  new: "protected override async Task<None> ExecuteAsync(...)"

  note: "Return None.Value"
```

### Common Compiler Errors and Fixes[​](#common-compiler-errors-and-fixes "Direct link to Common Compiler Errors and Fixes")

| Error                                                                              | Cause                | Fix                                                                                     |
| ---------------------------------------------------------------------------------- | -------------------- | --------------------------------------------------------------------------------------- |
| `CS0246: 'PipelineHostBuilder' could not be found`                                 | Class renamed        | Change to `Pipeline.CreateBuilder(args)`                                                |
| `CS0246: 'IPipelineContext' could not be found`                                    | Interface renamed    | Change to `IModuleContext`                                                              |
| `CS1061: 'Module' does not contain 'GetModule'`                                    | Method moved         | Change `GetModule<T>()` to `context.GetModule<T>()`                                     |
| `CS1061: 'Module' does not contain 'SubModule'`                                    | Method moved         | Change `SubModule()` to `context.SubModule()`                                           |
| `CS0117: 'ModuleResult' does not contain 'Value'`                                  | Property renamed     | Change `.Value` to `.ValueOrDefault`                                                    |
| `CS0117: 'ModuleResult' does not contain 'Exception'`                              | Property renamed     | Change `.Exception` to `.ExceptionOrDefault`                                            |
| `CS0115: 'ShouldSkip': no suitable method found to override`                       | Method removed       | Use `Configure().WithSkipWhen()` instead                                                |
| `CS0115: 'Timeout': no suitable method found to override`                          | Property removed     | Use `Configure().WithTimeout()` instead                                                 |
| `CS0115: 'RetryPolicy': no suitable method found to override`                      | Property removed     | Use `Configure().WithRetry()` instead                                                   |
| `CS1061: 'DotNetBuildOptions' does not contain 'WorkingDirectory'`                 | Property moved       | Pass `CommandExecutionOptions` as second parameter                                      |
| `CS1061: 'DotNetBuildOptions' does not contain 'LogInput'`                         | Property moved       | Use `CommandExecutionOptions.LogSettings` with `CommandLoggingOptions`                  |
| `CS0246: 'CommandLoggingOptions' could not be found`                               | Missing using        | Add `using ModularPipelines.Options;`                                                   |
| `CS1729: 'DotNetNewOptions' does not contain a constructor that takes 1 arguments` | Constructors removed | Use property initializer: `new DotNetNewOptions { TemplateShortName = "template" }`     |
| `CS1061: 'IModuleContext' does not contain 'Command'`                              | API restructured     | Use `context.Shell.Command.ExecuteCommandLineTool()`                                    |
| `CS0117: 'SkipDecision' does not contain 'WithSkipWhenAsync'`                      | No async version     | Use `WithSkipWhen()` with async lambda: `.WithSkipWhen(async () => await CheckAsync())` |

### Regex Patterns for Automated Migration[​](#regex-patterns-for-automated-migration "Direct link to Regex Patterns for Automated Migration")

```
# Entry point

s/PipelineHostBuilder\.Create\(\)/Pipeline.CreateBuilder(args)/g



# Context parameter in ExecuteAsync

s/IPipelineContext\s+context/IModuleContext context/g



# GetModule calls

s/await\s+GetModule<(\w+)>\(\)/await context.GetModule<$1>()/g

s/GetModule<(\w+)>\(\)/context.GetModule<$1>()/g



# SubModule calls

s/await\s+SubModule<(\w+)>\(/await context.SubModule<$1>(/g

s/await\s+SubModule\(/await context.SubModule(/g

s/SubModule<(\w+)>\(/context.SubModule<$1>(/g

s/SubModule\(/context.SubModule(/g



# Result property access

s/\.Value(?![a-zA-Z])/\.ValueOrDefault/g

s/\.Exception(?![a-zA-Z])/\.ExceptionOrDefault/g



# Result checks should be migrated manually to pattern matching or safe accessors.
```

### V3 Module Template[​](#v3-module-template "Direct link to V3 Module Template")

```
// Async module WITH return value

public class MyModule : Module<MyResult>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithTimeout(TimeSpan.FromMinutes(5))

        // Add other configuration as needed

        .Build();



    protected override async Task<MyResult?> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        // Implementation

        return new MyResult();

    }

}



// Async module WITHOUT return value

public class MyActionModule : Module<None>

{

    protected override async Task<None> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        // Implementation

        return None.Value;

    }

}



// Sync module WITHOUT return value

public class MySyncModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context, CancellationToken cancellationToken)

    {

        // Implementation

        return None.Value;

    }

}
```

### V3 Result Handling Patterns[​](#v3-result-handling-patterns "Direct link to V3 Result Handling Patterns")

```
// Pattern 1: Pattern matching (recommended)

var result = await context.GetModule<BuildModule>();

return result switch

{

    ModuleResult<BuildOutput>.Success { Value: var output } => Process(output),

    ModuleResult<BuildOutput>.Skipped => null,

    ModuleResult<BuildOutput>.Failure { Exception: var ex } => throw ex,

    _ => null

};



// Pattern 2: Match helper

var result = await context.GetModule<BuildModule>();

return result.Match(

    onSuccess: output => Process(output),

    onFailure: ex => throw ex,

    onSkipped: skip => null

);



// Pattern 3: Simple property access (easiest migration)

var result = await context.GetModule<BuildModule>();

if (result is ModuleResult<BuildOutput>.Success)

{

    var value = result.ValueOrDefault;

}
```

### V3 Command Execution Pattern[​](#v3-command-execution-pattern "Direct link to V3 Command Execution Pattern")

```
// Tool-specific options separate from execution options

await context.DotNet().Build(

    new DotNetBuildOptions

    {

        ProjectSolution = "MySolution.sln",

        Configuration = "Release",

    },

    new CommandExecutionOptions

    {

        WorkingDirectory = "/path/to/project",

        EnvironmentVariables = new Dictionary<string, string?>

        {

            ["CI"] = "true"

        },

        ThrowOnNonZeroExitCode = false,

        ExecutionTimeout = TimeSpan.FromMinutes(10)

    });
```

### Keywords for Search[​](#keywords-for-search "Direct link to Keywords for Search")

ModularPipelines, V3 migration, PipelineHostBuilder, Pipeline.CreateBuilder, IPipelineContext, IModuleContext, GetModule, ModuleResult, ValueOrDefault, ExceptionOrDefault, IsSuccess, IsFailure, IsSkipped, ModuleConfiguration, Configure, WithTimeout, WithRetry, WithSkipWhen, WithIgnoreFailures, WithAlwaysRun, CommandExecutionOptions, WorkingDirectory, EnvironmentVariables, Module non-generic, SyncModule, None struct, ExecuteModuleAsync, ExecuteModule, SubModule, context.SubModule, CommandLoggingOptions, CommandLogVerbosity, context.Shell.Command, context.Shell.Bash, context.Shell.PowerShell, DotNetNewOptions, DotNetPackOptions, GitTagOptions, TemplateShortName, property initializer, constructor removed, token parameter

## Getting Help[​](#getting-help "Direct link to Getting Help")

If you encounter issues migrating to V3:

1. Check the [GitHub Issues](https://github.com/thomhurst/ModularPipelines/issues) for known migration problems
2. Review the [Examples](https://github.com/thomhurst/ModularPipelines/tree/main/src/ModularPipelines.Examples) for V3 patterns
3. Open a new issue with the `migration` label if you're stuck
