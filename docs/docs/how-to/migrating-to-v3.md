---
title: Migrating to V3
sidebar_position: 99
---

# Migrating from V2 to V3

ModularPipelines V3 is a major release that modernizes the API to follow ASP.NET Core minimal API patterns. This guide covers all breaking changes and how to migrate your existing pipelines.

## Quick Migration Checklist

- [ ] Replace `PipelineHostBuilder.Create()` with `Pipeline.CreateBuilder(args)`
- [ ] Replace callback-based configuration with direct property access
- [ ] Change `IPipelineContext` to `IModuleContext` in `ExecuteAsync` signatures
- [ ] Update `GetModule<T>()` calls to `context.GetModule<T>()` (method moved to context)
- [ ] Migrate virtual property overrides to `Configure()` builder
- [ ] Update result access patterns to use pattern matching or `ValueOrDefault`
- [ ] Move `WorkingDirectory`, `EnvironmentVariables`, etc. from tool options to `CommandExecutionOptions`
- [ ] Migrate `LogInput`/`LogOutput` to `CommandLoggingOptions` with `CommandLogVerbosity`

## Entry Point Changes

The pipeline entry point has been completely redesigned to match ASP.NET Core's minimal API pattern.

### Before (V2)

```csharp
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

### After (V3)

```csharp
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

### Key Differences

| V2 | V3 |
|----|-----|
| `PipelineHostBuilder.Create()` | `Pipeline.CreateBuilder(args)` |
| `.ConfigureAppConfiguration((ctx, builder) => ...)` | `builder.Configuration.Add...()` |
| `.ConfigureServices((ctx, collection) => ...)` | `builder.Services.Add...()` |
| `.ConfigurePipelineOptions((ctx, options) => ...)` | `builder.Options.Property = value` |
| `.AddModule<T>()` on builder | `builder.Services.AddModule<T>()` |
| `.ExecutePipelineAsync()` | `.Build().RunAsync()` |

### Compatibility Note

The `ExecutePipelineAsync()` extension method still exists for simpler migrations:

```csharp
// This still works in V3
await builder.ExecutePipelineAsync();
```

## Module Behavior Changes

V2 used virtual property and method overrides to configure module behavior. V3 consolidates these into a fluent `Configure()` builder.

### Before (V2)

```csharp
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

### After (V3)

```csharp
public class MyModule : Module<string>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithTimeout(TimeSpan.FromMinutes(5))
        .WithRetryCount(3)
        .WithSkipWhen(ctx => ctx.Git().Information.BranchName != "main"
            ? SkipDecision.Skip("Only runs on main branch")
            : SkipDecision.DoNotSkip)
        .WithIgnoreFailures()
        .WithAlwaysRun()
        .WithBeforeExecute(ctx => LogStartAsync(ctx))
        .WithAfterExecute(ctx => LogEndAsync(ctx))
        .Build();

    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Module logic
        return "result";
    }
}
```

### Migration Mapping

| V2 Override | V3 Configure() Method |
|-------------|----------------------|
| `TimeSpan Timeout` property | `.WithTimeout(TimeSpan)` |
| `AsyncRetryPolicy<T?> RetryPolicy` property | `.WithRetryCount(int)` or `.WithRetryPolicy(IAsyncPolicy)` |
| `Task<SkipDecision> ShouldSkip()` method | `.WithSkipWhen(...)` |
| `Task<bool> ShouldIgnoreFailures()` method | `.WithIgnoreFailures()` or `.WithIgnoreFailuresWhen(...)` |
| `ModuleRunType.AlwaysRun` | `.WithAlwaysRun()` |
| `Task OnBeforeExecute()` method | `.WithBeforeExecute(...)` |
| `Task OnAfterExecute()` method | `.WithAfterExecute(...)` |

### Alternative: Lifecycle Hook Overrides

V3 also supports lifecycle hooks as overridable methods on the module class:

```csharp
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

## Context Parameter Change

The `ExecuteAsync` method now receives `IModuleContext` instead of `IPipelineContext`.

### Before (V2)

```csharp
protected override async Task<string?> ExecuteAsync(
    IPipelineContext context, CancellationToken cancellationToken)
```

### After (V3)

```csharp
protected override async Task<string?> ExecuteAsync(
    IModuleContext context, CancellationToken cancellationToken)
```

`IModuleContext` extends the pipeline context with module-specific capabilities like `GetModule<TModule>()`.

## Command Execution Options

Execution-related properties have been separated from tool-specific options into a dedicated `CommandExecutionOptions` class. This provides cleaner separation between "what to run" and "how to run it".

### Before (V2)

```csharp
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

### After (V3)

```csharp
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

### Migration Mapping

| V2 (on tool options) | V3 (on `CommandExecutionOptions`) |
|----------------------|-----------------------------------|
| `WorkingDirectory` | `WorkingDirectory` |
| `EnvironmentVariables` | `EnvironmentVariables` |
| `ThrowOnNonZeroExitCode` | `ThrowOnNonZeroExitCode` |
| `CommandLineCredentials` | `CommandLineCredentials` |
| `LoggingSettings` | `LogSettings` |
| `InputLoggingManipulator` | `InputLoggingManipulator` |
| `OutputLoggingManipulator` | `OutputLoggingManipulator` |
| N/A | `ExecutionTimeout` (new) |
| N/A | `GracefulShutdownTimeout` (new) |
| N/A | `Sudo` (new) |

### Benefits

- **Cleaner API**: Tool options focus only on tool-specific arguments
- **Reusability**: Share `CommandExecutionOptions` across multiple commands
- **New features**: `ExecutionTimeout`, `GracefulShutdownTimeout`, and `Sudo` options

## Command Logging Configuration

V3 introduces a new `CommandLoggingOptions` system that replaces the previous logging configuration. The new system provides verbosity levels and fine-grained control over what gets logged.

### Before (V2)

```csharp
// Logging settings were on tool options with limited control
await context.DotNet().Build(new DotNetBuildOptions
{
    LogInput = true,
    LogOutput = false,
    // Limited options available
});
```

### After (V3)

```csharp
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

### Verbosity Levels

| Level | Description |
|-------|-------------|
| `Silent` | No output at all |
| `Minimal` | Only command input (no output/errors) |
| `Normal` | Input, output, and errors on failure (default) |
| `Detailed` | Above plus exit code and duration |
| `Diagnostic` | Everything including working directory and timestamps |

### Configuration Precedence

V3 uses a three-tier configuration system (highest to lowest priority):

1. **Per-Call**: `CommandExecutionOptions.LogSettings` on individual command calls
2. **Global Default**: `PipelineOptions.DefaultLoggingOptions` set at pipeline level
3. **System Default**: `CommandLoggingOptions.Default` (Normal verbosity)

### Setting Global Defaults

```csharp
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

### Using Presets

```csharp
// Silent - no command logging at all
new CommandExecutionOptions { LogSettings = CommandLoggingOptions.Silent }

// Diagnostic - maximum verbosity for debugging
new CommandExecutionOptions { LogSettings = CommandLoggingOptions.Diagnostic }

// Default - normal verbosity
new CommandExecutionOptions { LogSettings = CommandLoggingOptions.Default }
```

### Fine-Grained Control

Override individual settings regardless of verbosity level:

```csharp
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

### Output Manipulators

Transform logged content before it's written:

```csharp
new CommandExecutionOptions
{
    LogSettings = new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Normal },
    InputLoggingManipulator = input => input.Length > 100
        ? input.Substring(0, 100) + "..."
        : input,
    OutputLoggingManipulator = output => output.Replace("secret-value", "***")
}
```

### Migration Mapping

| V2 | V3 |
|----|-----|
| `LogInput = true/false` | `ShowCommandArguments = true/false` |
| `LogOutput = true/false` | `ShowStandardOutput = true/false` |
| `InputLoggingManipulator` on tool options | `InputLoggingManipulator` on `CommandExecutionOptions` |
| `OutputLoggingManipulator` on tool options | `OutputLoggingManipulator` on `CommandExecutionOptions` |
| N/A | `Verbosity` levels (Silent, Minimal, Normal, Detailed, Diagnostic) |
| N/A | `ShowExitCode`, `ShowExecutionTime`, `ShowWorkingDirectory` |
| N/A | `IncludeTimestamps` |
| N/A | Global defaults via `PipelineOptions.DefaultLoggingOptions` |

## Getting Module Results

The `GetModule` method has moved from the module base class to the context. The result access patterns have also changed to use a discriminated union.

### Before (V2)

```csharp
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

### After (V3)

```csharp
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
            ModuleResult.Skipped { Decision: var skip }
                => null,
            ModuleResult.Failure { Exception: var ex }
                => throw new InvalidOperationException("Build failed", ex),
            _ => throw new InvalidOperationException("Unexpected result type")
        };
    }
}
```

### Alternative Result Access Patterns

```csharp
var buildResult = await context.GetModule<BuildModule>();

// Option 1: Pattern matching (recommended - handles all cases)
return buildResult switch
{
    ModuleResult<BuildOutput>.Success { Value: var output } => Process(output),
    ModuleResult.Skipped => null,
    ModuleResult.Failure { Exception: var ex } => throw ex,
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

// Option 4: Quick status checks
if (buildResult.IsSuccess)
{
    var value = buildResult.ValueOrDefault;
}
if (buildResult.IsFailure)
{
    var ex = buildResult.ExceptionOrDefault;
}
if (buildResult.IsSkipped)
{
    var reason = buildResult.SkipDecisionOrDefault?.Reason;
}
```

### Key Change Summary

| V2 | V3 |
|----|-----|
| `await GetModule<T>()` (on module) | `await context.GetModule<T>()` (on context) |
| `result.Value` | `result.ValueOrDefault` or pattern match |
| `result.Exception` | `result.ExceptionOrDefault` or pattern match |
| `result.ModuleResultType == ModuleResultType.X` | `result.IsSuccess`, `result.IsFailure`, `result.IsSkipped` |

### Result Type Quick Reference

| Check | V2 | V3 |
|-------|-----|-----|
| Is success? | `result.ModuleResultType == ModuleResultType.Success` | `result.IsSuccess` or `result is ModuleResult<T>.Success` |
| Is failure? | `result.ModuleResultType == ModuleResultType.Failure` | `result.IsFailure` or `result is ModuleResult.Failure` |
| Is skipped? | `result.ModuleResultType == ModuleResultType.Skipped` | `result.IsSkipped` or `result is ModuleResult.Skipped` |
| Get value | `result.Value` | `result.ValueOrDefault` or pattern match |
| Get exception | `result.Exception` | `result.ExceptionOrDefault` or pattern match |

## Deleted Types and Members

The following have been removed in V3:

| Removed | Replacement |
|---------|-------------|
| `PipelineHostBuilder` class | `Pipeline.CreateBuilder()` returns `PipelineBuilder` |
| `ModuleBase` class | `Module<T>` (simplified hierarchy) |
| `ModuleBase<T>` class | `Module<T>` |
| `ShouldSkip()` method | `Configure().WithSkipWhen()` |
| `ShouldIgnoreFailures()` method | `Configure().WithIgnoreFailures()` |
| `ModuleRunType` property | `Configure().WithAlwaysRun()` |
| `Timeout` property | `Configure().WithTimeout()` |
| `RetryPolicy` property | `Configure().WithRetryCount()` or `.WithRetryPolicy()` |
| `GetModule<T>()` on module | `context.GetModule<TModule>()` |

## New Features in V3

### Non-Generic Module Classes

V3 introduces non-generic `Module` and `SyncModule` base classes for modules that perform actions without returning meaningful data. These use the `None` struct internally to represent the absence of a value.

```csharp
// Async module that doesn't return data
public class DeployModule : Module
{
    protected override async Task ExecuteModuleAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        await context.Shell.Command.ExecuteCommandLineTool(...);
        // No return statement needed
    }
}

// Sync module that doesn't return data
public class LoggingModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Pipeline executed at {Time}", DateTime.UtcNow);
        // No return statement needed
    }
}
```

The `None` struct represents "nothing" and is semantically equivalent to `null`:

```csharp
None value = None.Value;
value.Equals(null);     // true - None equals null
value == default;       // true
None? nullable = null;
nullable == value;      // true - None? and None are always equal
```

### Pipeline Validation

V3 introduces a validation API to catch configuration errors before execution:

```csharp
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

### Dynamic Dependencies

Declare dependencies programmatically at runtime:

```csharp
public class MyModule : Module<string>
{
    protected override void DeclareDependencies(IDependencyDeclaration deps)
    {
        // Always depend on this module
        deps.DependsOn<RequiredModule>();

        // Optional dependency (won't fail if not registered)
        deps.DependsOnOptional<OptionalModule>();

        // Conditional dependency
        deps.DependsOnIf<ProductionModule>(Environment.IsProduction);

        // Lazy dependency (evaluated later)
        deps.DependsOnLazy<HeavyModule>();
    }

    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // ...
    }
}
```

### New Dependency Attributes

```csharp
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

### Conditional Execution Attributes

```csharp
// Run only on specific platforms
[RunOnWindows]
public class WindowsModule : Module<string> { }

[RunOnLinux]
public class LinuxModule : Module<string> { }

[RunOnMacOS]
public class MacModule : Module<string> { }

// Run ONLY on specific platform (skip on others)
[RunOnWindowsOnly]
public class WindowsOnlyModule : Module<string> { }

// Skip based on custom condition
[SkipIf(typeof(IsNotMainBranchCondition))]
public class MainBranchModule : Module<string> { }

// Combine conditions
[RunIfAll(typeof(IsCI), typeof(IsMainBranch))]
public class CIMainModule : Module<string> { }

[RunIfAny(typeof(IsCI), typeof(ForceRun))]
public class FlexibleModule : Module<string> { }
```

### Module Tags

Tag modules for organization and dependency management:

```csharp
// Via attributes
[ModuleTag("critical")]
[ModuleTag("deployment")]
[ModuleCategory("Infrastructure")]
public class DeployModule : Module<DeployResult> { }

// Via property override
public class MyModule : Module<string>
{
    public override IReadOnlySet<string> Tags => new HashSet<string> { "critical", "fast" };
    public override string? Category => "Build";
}
```

### Plugin System

Create reusable pipeline extensions:

```csharp
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

## Complete Migration Example

### Before (V2)

```csharp
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

### After (V3)

```csharp
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
        .WithSkipWhen(ctx => ctx.Git().Information.BranchName != "main"
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

## Breaking API Reference

| V2 API | V3 API | Notes |
|--------|--------|-------|
| `PipelineHostBuilder.Create()` | `Pipeline.CreateBuilder(args)` | Pass command-line args |
| `.ExecutePipelineAsync()` | `.Build().RunAsync()` | Two-step, or use extension |
| `.ConfigureAppConfiguration(...)` | `builder.Configuration` | Direct access |
| `.ConfigureServices(...)` | `builder.Services` | Direct access |
| `.ConfigurePipelineOptions(...)` | `builder.Options` | Direct access |
| `IPipelineContext` | `IModuleContext` | In ExecuteAsync signature |
| `GetModule<T>()` | `context.GetModule<T>()` | Method moved to context |
| `result.Value` | `result.ValueOrDefault` | Or use pattern matching |
| `result.ModuleResultType` | `result.IsSuccess/IsFailure/IsSkipped` | Or pattern match |
| `ShouldSkip()` override | `Configure().WithSkipWhen()` | Fluent builder |
| `ShouldIgnoreFailures()` override | `Configure().WithIgnoreFailures()` | Fluent builder |
| `Timeout` property override | `Configure().WithTimeout()` | Fluent builder |
| `RetryPolicy` property override | `Configure().WithRetryCount()` | Fluent builder |
| `ModuleRunType` override | `Configure().WithAlwaysRun()` | Fluent builder |
| `OnBeforeExecute()` override | `Configure().WithBeforeExecute()` or `OnBeforeExecuteAsync()` | Either approach |
| `OnAfterExecute()` override | `Configure().WithAfterExecute()` or `OnAfterExecuteAsync()` | Either approach |
| `options.WorkingDirectory` | `CommandExecutionOptions.WorkingDirectory` | Separate parameter |
| `options.EnvironmentVariables` | `CommandExecutionOptions.EnvironmentVariables` | Separate parameter |
| `options.ThrowOnNonZeroExitCode` | `CommandExecutionOptions.ThrowOnNonZeroExitCode` | Separate parameter |

## LLM/AI Migration Reference

This section provides structured data optimized for AI assistants helping with code migration.

### Complete API Transformation Map

```yaml
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

- old: "result.Value"
  new: "result.ValueOrDefault"

- old: "result.Exception"
  new: "result.ExceptionOrDefault"

- old: "result.ModuleResultType == ModuleResultType.Success"
  new: "result.IsSuccess"

- old: "result.ModuleResultType == ModuleResultType.Failure"
  new: "result.IsFailure"

- old: "result.ModuleResultType == ModuleResultType.Skipped"
  new: "result.IsSkipped"

# Module Configuration (property overrides → fluent builder)
- old: "protected internal override TimeSpan Timeout => ..."
  new: "Configure().WithTimeout(TimeSpan)"

- old: "protected override AsyncRetryPolicy<T?> RetryPolicy => ..."
  new: "Configure().WithRetryCount(int)"

- old: "protected internal override Task<SkipDecision> ShouldSkip(...)"
  new: "Configure().WithSkipWhen(Func<IModuleContext, SkipDecision>)"

- old: "protected internal override Task<bool> ShouldIgnoreFailures(...)"
  new: "Configure().WithIgnoreFailures()"

- old: "public override ModuleRunType ModuleRunType => ModuleRunType.AlwaysRun"
  new: "Configure().WithAlwaysRun()"

- old: "protected internal override Task OnBeforeExecute(IPipelineContext context)"
  new: "Configure().WithBeforeExecute(...) or OnBeforeExecuteAsync(...)"

- old: "protected internal override Task OnAfterExecute(IPipelineContext context)"
  new: "Configure().WithAfterExecute(...) or OnAfterExecuteAsync(...)"

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

# Non-generic modules (V3 addition)
- old: "public class MyModule : Module<IDictionary<string, object>>"
  new: "public class MyModule : Module"
  note: "For modules that don't return data"

- old: "protected override async Task<IDictionary<string, object>?> ExecuteAsync(...)"
  new: "protected override async Task ExecuteModuleAsync(...)"
  note: "No return statement needed"
```

### Common Compiler Errors and Fixes

| Error | Cause | Fix |
|-------|-------|-----|
| `CS0246: 'PipelineHostBuilder' could not be found` | Class renamed | Change to `Pipeline.CreateBuilder(args)` |
| `CS0246: 'IPipelineContext' could not be found` | Interface renamed | Change to `IModuleContext` |
| `CS1061: 'Module' does not contain 'GetModule'` | Method moved | Change `GetModule<T>()` to `context.GetModule<T>()` |
| `CS0117: 'ModuleResult' does not contain 'Value'` | Property renamed | Change `.Value` to `.ValueOrDefault` |
| `CS0117: 'ModuleResult' does not contain 'Exception'` | Property renamed | Change `.Exception` to `.ExceptionOrDefault` |
| `CS0115: 'ShouldSkip': no suitable method found to override` | Method removed | Use `Configure().WithSkipWhen()` instead |
| `CS0115: 'Timeout': no suitable method found to override` | Property removed | Use `Configure().WithTimeout()` instead |
| `CS0115: 'RetryPolicy': no suitable method found to override` | Property removed | Use `Configure().WithRetryCount()` instead |
| `CS1061: 'DotNetBuildOptions' does not contain 'WorkingDirectory'` | Property moved | Pass `CommandExecutionOptions` as second parameter |
| `CS1061: 'DotNetBuildOptions' does not contain 'LogInput'` | Property moved | Use `CommandExecutionOptions.LogSettings` with `CommandLoggingOptions` |
| `CS0246: 'CommandLoggingOptions' could not be found` | Missing using | Add `using ModularPipelines.Options;` |

### Regex Patterns for Automated Migration

```regex
# Entry point
s/PipelineHostBuilder\.Create\(\)/Pipeline.CreateBuilder(args)/g

# Context parameter in ExecuteAsync
s/IPipelineContext\s+context/IModuleContext context/g

# GetModule calls
s/await\s+GetModule<(\w+)>\(\)/await context.GetModule<$1>()/g
s/GetModule<(\w+)>\(\)/context.GetModule<$1>()/g

# Result property access
s/\.Value(?![a-zA-Z])/\.ValueOrDefault/g
s/\.Exception(?![a-zA-Z])/\.ExceptionOrDefault/g

# Result type checks
s/\.ModuleResultType\s*==\s*ModuleResultType\.Success/\.IsSuccess/g
s/\.ModuleResultType\s*==\s*ModuleResultType\.Failure/\.IsFailure/g
s/\.ModuleResultType\s*==\s*ModuleResultType\.Skipped/\.IsSkipped/g
```

### V3 Module Template

```csharp
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
public class MyActionModule : Module
{
    protected override async Task ExecuteModuleAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Implementation - no return needed
    }
}

// Sync module WITHOUT return value
public class MySyncModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Implementation - no return needed
    }
}
```

### V3 Result Handling Patterns

```csharp
// Pattern 1: Pattern matching (recommended)
var result = await context.GetModule<BuildModule>();
return result switch
{
    ModuleResult<BuildOutput>.Success { Value: var output } => Process(output),
    ModuleResult.Skipped => null,
    ModuleResult.Failure { Exception: var ex } => throw ex,
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
if (result.IsSuccess)
{
    var value = result.ValueOrDefault;
}
```

### V3 Command Execution Pattern

```csharp
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

### Keywords for Search

ModularPipelines, V3 migration, PipelineHostBuilder, Pipeline.CreateBuilder, IPipelineContext, IModuleContext, GetModule, ModuleResult, ValueOrDefault, ExceptionOrDefault, IsSuccess, IsFailure, IsSkipped, ModuleConfiguration, Configure, WithTimeout, WithRetryCount, WithSkipWhen, WithIgnoreFailures, WithAlwaysRun, CommandExecutionOptions, WorkingDirectory, EnvironmentVariables, Module non-generic, SyncModule, None struct, ExecuteModuleAsync, ExecuteModule

## Getting Help

If you encounter issues migrating to V3:

1. Check the [GitHub Issues](https://github.com/thomhurst/ModularPipelines/issues) for known migration problems
2. Review the [Examples](https://github.com/thomhurst/ModularPipelines/tree/main/src/ModularPipelines.Examples) for V3 patterns
3. Open a new issue with the `migration` label if you're stuck
