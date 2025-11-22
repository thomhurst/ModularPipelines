# New Module Architecture (v3.0)

## Overview

This directory contains the new composition-based module architecture that replaces the inheritance-heavy `ModuleBase` system.

## Core Principles

- **Composition over Inheritance**: Behaviors are opt-in via interfaces, not forced via base class
- **Single Responsibility**: Each class/interface has one reason to change
- **Interface Segregation**: Modules only depend on what they use
- **Minimal by Default**: Base class provides only essential functionality (Id, Type, ExecuteAsync)

## File Structure

### Core Abstractions
- `IModule.cs` - Minimal module contract (Id, ModuleType)
- `IModule<T>` - Module with result type
- `ModuleNew<T>.cs` - Minimal base implementation (**will replace Module.cs after migration**)

### Behavior Interfaces (Opt-in)
Located in `Behaviors/`:
- `IModuleSkipLogic` - Custom skip conditions
- `IModuleLifecycle` - Before/after execution hooks
- `IModuleRetryPolicy` - Polly retry policies
- `IModuleTimeout` - Custom timeouts
- `IModuleErrorHandling` - Error handling decisions
- `IModuleSubModules` - Sub-module support

### Services (Extracted from Base Class)
Located in `../Services/`:
- `IModuleStateTracker` - Tracks status, timing, exceptions
- `IModuleDependencyResolver` - Resolves module dependencies
- `IModuleSubModuleService` - Executes sub-modules
- `IModuleExecutor` - Orchestrates module execution

## Example: Simple Module

```csharp
using ModularPipelines.Modules;
using ModularPipelines.Context;

[DependsOn<OtherModule>]
public class MyModule : ModuleNew<string>
{
    public override async Task<string?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken ct)
    {
        // Get dependency via context service
        var other = await context.GetModuleAsync<OtherModule>();
        var result = await other;

        return $"Processed: {result}";
    }
}
```

**Memory overhead**: ~32 bytes (just Guid + Type reference)

## Example: Module with Behaviors

```csharp
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.Context;
using ModularPipelines.Models;
using Polly;

[DependsOn<OtherModule>]
[Timeout(Minutes = 5)]
public class ComplexModule : ModuleNew<string>,
    IModuleSkipLogic,
    IModuleLifecycle,
    IModuleRetryPolicy
{
    public async Task<SkipDecision> ShouldSkipAsync(IPipelineContext context)
    {
        if (context.Environment.IsProduction)
        {
            return SkipDecision.Skip("Skipped in production");
        }

        return SkipDecision.DoNotSkip;
    }

    public async Task OnBeforeExecuteAsync(IPipelineContext context)
    {
        await context.Logger.LogInformationAsync("Starting complex module");
    }

    public async Task OnAfterExecuteAsync(IPipelineContext context, object? result, Exception? exception)
    {
        if (exception != null)
        {
            await context.Logger.LogErrorAsync("Module failed", exception);
        }
    }

    public IAsyncPolicy GetRetryPolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(i));
    }

    public override async Task<string?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken ct)
    {
        // Business logic here
        return "Success";
    }
}
```

**Capabilities are explicit and testable.**

## Migration Status

### ✅ Completed
- [x] Core interfaces (IModule, IModule<T>)
- [x] Minimal base class (ModuleNew<T>)
- [x] All behavior interfaces
- [x] All service interfaces

### ⏳ Remaining Work
- [ ] Implement service classes (ModuleStateTracker, ModuleDependencyResolver, etc.)
- [ ] Implement ModuleExecutor orchestration logic
- [ ] Extend IPipelineContext with GetModuleAsync methods
- [ ] Update PipelineHostBuilder to register new services
- [ ] Create/update declarative attributes (Timeout, Retry, etc.)
- [ ] Delete old ModuleBase, Module<T>, handler classes
- [ ] Migrate all existing modules (~50+ files)
- [ ] Update all unit tests
- [ ] Update documentation
- [ ] Version bump to 3.0.0

## Benefits

### Before (Inheritance)
- **Memory**: ~2-3 KB per module (6 handlers + state + locks)
- **Responsibilities**: 19 mixed concerns in base class
- **Testability**: Hard - tightly coupled
- **Extensibility**: Requires modifying base class

### After (Composition)
- **Memory**: ~32 bytes per module (just Guid + Type)
- **Responsibilities**: 1 per class/interface (SRP)
- **Testability**: Easy - independently testable concerns
- **Extensibility**: Add new interfaces without touching core

## Design Comparison

| Concern | Old (Inheritance) | New (Composition) |
|---------|------------------|-------------------|
| Core execution | Forced in base class | IModule<T> interface |
| Dependencies | GetModule<T>() inherited | context.GetModuleAsync<T>() |
| Skip logic | Virtual ShouldSkip() | IModuleSkipLogic interface |
| Lifecycle hooks | Virtual OnBefore/AfterExecute() | IModuleLifecycle interface |
| Retry | Virtual RetryPolicy property | IModuleRetryPolicy interface |
| Timeout | Virtual Timeout property | IModuleTimeout interface or [Timeout] attribute |
| Error handling | Virtual ShouldIgnoreFailures() | IModuleErrorHandling interface |
| Sub-modules | Inherited SubModule() methods | IModuleSubModules interface |
| State tracking | Properties on base class | IModuleStateTracker service |

## Notes

- `ModuleNew<T>` is temporary naming - will become `Module<T>` after old code is deleted
- All existing `[DependsOn<T>]` attribute syntax remains unchanged
- This is a **breaking change** requiring v3.0.0 major version bump
