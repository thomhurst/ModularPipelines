# Enhanced Attribute System with Composition and Lifecycle Event Receivers

**Issue:** #1403
**Date:** 2025-12-22
**Status:** Design Complete

## Overview

A composable attribute system where attributes implement lifecycle interfaces to receive events and dynamically configure modules. Attributes become self-contained units of behavior that can be combined on modules.

## Goals

1. **Composable attributes** - Create custom attributes that combine behaviors (e.g., `[CriticalModule]` that adds retry + notification)
2. **Dynamic dependencies** - Add dependencies at registration time based on runtime conditions
3. **Per-module event handlers** - Attributes react to their own module's lifecycle events

## Design Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Relationship to existing hooks | Complement, not replace | Backward compatibility; global hooks for cross-cutting, attributes for per-module |
| Registration context scope | Full pipeline context | Maximum flexibility for attribute authors |
| Event context capabilities | Observation + control flow | Enable retry requests, skip dependents, fail pipeline |
| Attribute discovery | Attributes implement interfaces | Self-contained behavior; explicit about capabilities |
| Execution order | Declaration order | Intuitive; what you see is what you get |
| Error handling | Configurable per-attribute | `ContinueOnError` property; flexibility for critical vs best-effort |
| Existing behavior interfaces | Keep both forever | Interface-on-module for simple cases, attributes for composable behaviors |

## Event Receiver Interfaces

All interfaces live in `ModularPipelines.Attributes.Events` namespace.

### IModuleRegistrationEventReceiver

Invoked during DI registration. Use for dynamic dependency configuration.

```csharp
public interface IModuleRegistrationEventReceiver
{
    Task OnRegistrationAsync(IModuleRegistrationContext context);
}
```

### IModuleStartEventReceiver

Invoked immediately before a module starts executing.

```csharp
public interface IModuleStartEventReceiver
{
    bool ContinueOnError => false;
    Task OnModuleStartAsync(IModuleEventContext context);
}
```

### IModuleEndEventReceiver

Invoked after a module completes (success or failure).

```csharp
public interface IModuleEndEventReceiver
{
    bool ContinueOnError => false;
    Task OnModuleEndAsync(IModuleEventContext context, ModuleResult result);
}
```

### IModuleFailureEventReceiver

Invoked when a module fails. Called before `OnModuleEndAsync`.

```csharp
public interface IModuleFailureEventReceiver
{
    bool ContinueOnError => false;
    Task OnModuleFailureAsync(IModuleEventContext context, Exception exception);
}
```

### IModuleSkippedEventReceiver

Invoked when a module is skipped.

```csharp
public interface IModuleSkippedEventReceiver
{
    bool ContinueOnError => false;
    Task OnModuleSkippedAsync(IModuleEventContext context, SkipDecision reason);
}
```

## Context Interfaces

### IModuleRegistrationContext

Passed to `OnRegistrationAsync`. Provides full pipeline visibility and dependency manipulation.

```csharp
public interface IModuleRegistrationContext
{
    // Module Information
    Type ModuleType { get; }
    IReadOnlyList<Attribute> ModuleAttributes { get; }

    // Pipeline Configuration
    IConfiguration Configuration { get; }
    IHostEnvironment Environment { get; }

    // Registered Modules Discovery
    IReadOnlyList<Type> RegisteredModuleTypes { get; }
    bool IsModuleRegistered<TModule>() where TModule : IModule;
    bool IsModuleRegistered(Type moduleType);
    IEnumerable<Type> GetModulesAssignableTo<TBase>() where TBase : IModule;
    IEnumerable<Type> GetModulesWithAttribute<TAttribute>() where TAttribute : Attribute;

    // Dependency Manipulation
    void AddDependency<TModule>() where TModule : IModule;
    void AddDependency(Type moduleType);
    void AddDependencyOnAll<TBase>() where TBase : IModule;
    void AddDependencyOnAll(Func<Type, bool> predicate);
    void RemoveDependency<TModule>() where TModule : IModule;

    // Service Access
    IServiceCollection Services { get; }

    // Metadata
    void SetMetadata(string key, object value);
    T? GetMetadata<T>(string key);
}
```

### IModuleEventContext

Passed to runtime event receivers. Provides observation and control flow.

```csharp
public interface IModuleEventContext
{
    // Module Information
    Type ModuleType { get; }
    string ModuleName { get; }
    IModule ModuleInstance { get; }
    IReadOnlyList<Attribute> ModuleAttributes { get; }

    // Execution State
    DateTimeOffset StartTime { get; }
    TimeSpan ElapsedTime { get; }
    Status Status { get; }

    // Pipeline Context Access
    IPipelineContext PipelineContext { get; }
    CancellationToken CancellationToken { get; }

    // Service Resolution
    T GetService<T>() where T : notnull;
    T? GetServiceOrDefault<T>() where T : class;

    // Metadata (from registration phase)
    T? GetMetadata<T>(string key);

    // Control Flow
    void RequestRetry(TimeSpan? delay = null);
    void SkipDependentModules(string reason);
    void FailPipeline(string reason);
}
```

## Example Usage

### Notify on Failure

```csharp
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class NotifyOnFailureAttribute : Attribute, IModuleFailureEventReceiver
{
    private readonly string _channel;

    public bool ContinueOnError => true;

    public NotifyOnFailureAttribute(string channel) => _channel = channel;

    public async Task OnModuleFailureAsync(IModuleEventContext context, Exception ex)
    {
        var notifier = context.GetService<ISlackNotifier>();
        await notifier.SendAsync(_channel, $"Module {context.ModuleName} failed: {ex.Message}");
    }
}

// Usage
[NotifyOnFailure("deployments")]
[NotifyOnFailure("oncall")]
public class ProductionDeployModule : Module<DeployResult> { }
```

### Conditional Dependencies

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class DependsOnTestsInCIAttribute : Attribute, IModuleRegistrationEventReceiver
{
    public Task OnRegistrationAsync(IModuleRegistrationContext context)
    {
        if (context.Configuration["CI"] == "true")
        {
            context.AddDependencyOnAll<ITestModule>();
        }
        return Task.CompletedTask;
    }
}

// Usage
[DependsOnTestsInCI]
public class PublishModule : Module<None> { }
```

### Combined Behaviors

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class CriticalModuleAttribute : Attribute,
    IModuleRegistrationEventReceiver,
    IModuleFailureEventReceiver,
    IModuleStartEventReceiver
{
    public bool ContinueOnError => false;

    public Task OnRegistrationAsync(IModuleRegistrationContext context)
    {
        context.SetMetadata("critical", true);
        return Task.CompletedTask;
    }

    public Task OnModuleStartAsync(IModuleEventContext context)
    {
        var logger = context.GetService<ILogger<CriticalModuleAttribute>>();
        logger.LogWarning("Starting critical module: {Module}", context.ModuleName);
        return Task.CompletedTask;
    }

    public async Task OnModuleFailureAsync(IModuleEventContext context, Exception ex)
    {
        context.FailPipeline($"Critical module {context.ModuleName} failed");

        var pager = context.GetService<IPagerDuty>();
        await pager.AlertAsync($"Critical failure: {context.ModuleName}");
    }
}
```

## Execution Order

1. Existing global hooks (`IPipelineGlobalHooks.OnStartAsync`)
2. For each module:
   - Existing `IPipelineModuleHooks.OnBeforeModuleStartAsync`
   - Existing `IHookable.OnBeforeExecute`
   - **New:** `IModuleStartEventReceiver.OnModuleStartAsync` (declaration order)
   - Module executes
   - **New:** `IModuleFailureEventReceiver` / `IModuleSkippedEventReceiver` (if applicable)
   - **New:** `IModuleEndEventReceiver.OnModuleEndAsync`
   - Existing `IHookable.OnAfterExecute`
   - Existing `IPipelineModuleHooks.OnAfterModuleEndAsync`
3. Existing global hooks (`IPipelineGlobalHooks.OnEndAsync`)

## Dynamic Dependency Resolution

### Registration Order

1. All modules registered in DI container (existing)
2. Static `[DependsOn<T>]` dependencies collected (existing)
3. **New:** `IModuleRegistrationEventReceiver.OnRegistrationAsync` invoked per module
4. Dynamic dependencies merged with static dependencies
5. Dependency graph validated (cycles, missing modules, self-references)
6. Execution order calculated

### Validation

- Cycle detection after dynamic dependencies added
- Missing dependency check (respects `IgnoreIfNotRegistered`)
- Self-dependency prevention

## Error Handling

| Scenario | ContinueOnError=false | ContinueOnError=true |
|----------|----------------------|---------------------|
| `OnRegistrationAsync` throws | Pipeline fails to start | Log warning, continue |
| `OnModuleStartAsync` throws | Module marked failed | Log warning, execute normally |
| `OnModuleEndAsync` throws | Pipeline fails | Log warning, continue |
| `OnModuleFailureAsync` throws | Both exceptions logged, fail | Log warning, continue |

### Control Flow Conflicts

- `RequestRetry`: Last one wins
- `SkipDependentModules`: Applied if any receiver calls it
- `FailPipeline`: Applied if any receiver calls it

## File Structure

```
src/ModularPipelines/
├── Attributes/
│   └── Events/
│       ├── IModuleRegistrationEventReceiver.cs
│       ├── IModuleStartEventReceiver.cs
│       ├── IModuleEndEventReceiver.cs
│       ├── IModuleFailureEventReceiver.cs
│       └── IModuleSkippedEventReceiver.cs
│
├── Context/
│   ├── IModuleRegistrationContext.cs
│   ├── IModuleEventContext.cs
│   ├── ModuleRegistrationContext.cs      (internal)
│   └── ModuleEventContext.cs             (internal)
│
├── Engine/
│   ├── Attributes/
│   │   ├── IModuleAttributeEventService.cs
│   │   ├── ModuleAttributeEventService.cs
│   │   └── AttributeEventInvoker.cs
│   │
│   └── Dependencies/
│       ├── ModuleDependencyRegistry.cs
│       └── ModuleRegistrationValidator.cs
```

## Backward Compatibility

- All existing attributes work unchanged
- All existing hooks work unchanged
- All existing behavior interfaces work unchanged
- New system complements existing; no deprecations

## Out of Scope

- Attribute-based replacements for `ISkippable`, `IRetryable<T>`, etc.
- Pipeline-level attribute receivers
- Removing or deprecating existing APIs
