# Interface Hierarchy

This document describes the interface hierarchy in ModularPipelines and provides guidance on which interfaces to use in different scenarios.

## Overview

ModularPipelines organizes its interfaces into three categories:

1. **User-Facing Interfaces** - For module developers to use
2. **Extension Point Interfaces** - For implementing hooks, requirements, and validators
3. **Internal Engine Interfaces** - Implementation details (not part of public API)

## User-Facing Interfaces

### IModuleContext (Primary Interface)

`IModuleContext` is the **primary interface** for module developers. It provides access to all pipeline capabilities:

```csharp
public class MyModule : Module<string>
{
    protected override async Task<string> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        // Access all capabilities through context
        context.Logger.LogInformation("Starting...");
        var config = context.Configuration["MyKey"];
        var service = context.GetService<IMyService>();

        return "result";
    }
}
```

**Capabilities available through IModuleContext:**

| Property/Method | Description |
|----------------|-------------|
| `ServiceProvider` | DI container |
| `Get<T>()` | Get service from DI |
| `Configuration` | App configuration |
| `PipelineOptions` | Pipeline settings |
| `Logger` | Module-scoped logger |
| `Command` | Run external commands |
| `Powershell` | Run PowerShell |
| `Bash` | Run Bash scripts |
| `Http` | HTTP client |
| `Downloader` | Download files |
| `Installer` | Install applications |
| `Json`, `Xml`, `Yaml` | Serialization |
| `Hex`, `Base64`, `Hasher` | Encoding/hashing |
| `FileSystem` | File operations |
| `Zip` | Compression |
| `Checksum` | File checksums |
| `Environment` | Environment info |
| `BuildSystemDetector` | CI system detection |
| `GetModule<TModule>()` | Get another module's result |
| `GetModuleIfRegistered<TModule>()` | Get another module if registered (returns null if not) |
| `SubModule()` | Track sub-operations |

### Extension Methods

Helper extension methods provide simplified access to common operations:

```csharp
// DI helpers
var service = context.GetService<IMyService>(); // Throws if not registered
var optional = context.TryGetService<IMyService>(); // Returns null if not registered

// Configuration helpers
var value = context.GetConfigValue("key"); // Returns null if missing
var required = context.GetRequiredConfigValue("key"); // Throws if missing

// Build system detection
if (context.IsRunningInCI()) { /* ... */ }
if (context.IsRunningLocally()) { /* ... */ }
if (context.IsRunningIn(BuildSystem.GitHubActions)) { /* ... */ }
```

### IPipelineHookContext

`IPipelineHookContext` is the base interface for hook implementations. It provides the same capabilities as `IModuleContext` but without module-specific methods.

Use `IPipelineHookContext` when implementing:
- `IPipelineGlobalHooks`
- `IPipelineModuleHooks`
- `IPipelineRequirement`

### Deprecated: IPipelineContext

`IPipelineContext` is **deprecated**. Use `IModuleContext` instead.

```csharp
// DEPRECATED - Don't use
public interface IPipelineContext : IPipelineHookContext { }

// RECOMMENDED - Use this
public interface IModuleContext : IPipelineContext { }
```

## Extension Point Interfaces

These interfaces are designed to be implemented by users to extend pipeline behavior.

### IPipelineGlobalHooks

Implement to receive callbacks for pipeline lifecycle events:

```csharp
public class MyHooks : IPipelineGlobalHooks
{
    public Task OnStartAsync(IPipelineHookContext context)
    {
        context.Logger.LogInformation("Pipeline starting...");
        return Task.CompletedTask;
    }

    public Task OnEndAsync(IPipelineHookContext context, PipelineSummary summary)
    {
        context.Logger.LogInformation("Pipeline finished: {TotalModules} modules", summary.TotalModules);
        return Task.CompletedTask;
    }
}
```

### IPipelineModuleHooks

Implement to receive callbacks for individual module events:

```csharp
public class MyModuleHooks : IPipelineModuleHooks
{
    public Task OnBeforeModuleStartAsync(IPipelineHookContext context, IModule module)
    {
        context.Logger.LogInformation("Module {ModuleName} starting...", module.GetType().Name);
        return Task.CompletedTask;
    }

    public Task OnAfterModuleEndAsync(IPipelineHookContext context, IModule module)
    {
        context.Logger.LogInformation("Module {ModuleName} finished", module.GetType().Name);
        return Task.CompletedTask;
    }
}
```

### IPipelineRequirement

Implement to define requirements that must be satisfied before pipeline execution:

```csharp
public class DockerRequirement : IPipelineRequirement
{
    public int Order => 100;

    public async Task<RequirementDecision> MustAsync(IPipelineHookContext context)
    {
        var result = await context.Command.ExecuteCommandLineTool(
            new("docker", "version"));

        return result.ExitCode == 0
            ? RequirementDecision.Met
            : RequirementDecision.NotMet("Docker is not installed");
    }
}
```

### IPipelineValidator

Implement to add custom validation logic that runs during pipeline initialization:

```csharp
public class MyValidator : IPipelineValidator
{
    public int Order => 100;

    public ValidationResult Validate(IServiceProvider services)
    {
        var errors = new List<string>();

        // Check for required services
        if (services.GetService<IMyRequiredService>() == null)
        {
            errors.Add("IMyRequiredService is not registered");
        }

        return new ValidationResult(errors);
    }
}
```

### IPipelineHost

The host interface for running pipelines. Typically used internally by `PipelineHostBuilder`.

## Internal Engine Interfaces

The following interfaces are **internal** implementation details and should not be used directly:

- `IPipelineExecutor` - Pipeline execution orchestration
- `IPipelineInitializer` - Pipeline initialization
- `IPipelineOutputCoordinator` - Output coordination
- `IPipelineOutputScope` - Output lifecycle
- `IPipelineSummaryFactory` - Summary creation
- `IPipelineContextProvider` - Context provisioning
- `IPipelineFileWriter` - File writing
- `IPipelineSetupExecutor` - Setup execution
- `IPipelineValidationService` - Validation orchestration
- `IPipelineServiceContainerWrapper` - DI wrapper

## Interface Hierarchy Diagram

```
IModuleContext (primary for modules)
  └── IPipelineContext (deprecated)
        └── IPipelineHookContext (base for hooks)
              ├── IPipelineServices
              ├── IPipelineLogging
              ├── IPipelineTools
              ├── IPipelineEncoding
              ├── IPipelineFileSystem
              └── IPipelineEnvironment
```

## Migration Guide

### From IPipelineContext to IModuleContext

If you're using `IPipelineContext` in module code, migrate to `IModuleContext`:

```csharp
// Before
protected override async Task<string> ExecuteAsync(
    IPipelineContext context,  // DEPRECATED
    CancellationToken ct) { }

// After
protected override async Task<string> ExecuteAsync(
    IModuleContext context,    // RECOMMENDED
    CancellationToken ct) { }
```

Note: The `Module<T>` base class already uses `IModuleContext` in its signature, so this migration is typically automatic.

## Best Practices

1. **Use IModuleContext** for module implementations
2. **Use IPipelineHookContext** for hook implementations
3. **Use extension methods** for common operations (GetService, GetConfigValue, etc.)
4. **Don't depend on internal interfaces** - they may change without notice
5. **Implement IPipelineValidator** for custom validation, not the validation service
