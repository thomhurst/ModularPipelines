# Direct Module-Level Hooks Design

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Issue:** #1868 - Direct Module-Level Hooks
**Date:** 2026-01-07
**Status:** Ready for Implementation

---

## Problem Statement

Module lifecycle events are currently driven through two mechanisms:
1. **IHookable interface** - Requires implementing an interface with both `OnBeforeExecute` and `OnAfterExecute`
2. **Attribute-based event system** - Uses reflection to discover attributes implementing receiver interfaces

Both approaches have drawbacks:
- **IHookable**: All-or-nothing (must implement both methods), interface-based composition
- **Attributes**: Indirection complexity, reflection overhead, hidden control flow, scattered behavior

Users want a simpler, more discoverable way to hook into module lifecycle events.

---

## Proposed Solution

Add **virtual methods** directly to the `Module<T>` base class that users can override:

```csharp
public class MyModule : Module<string>
{
    protected override async Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Runs before ExecuteAsync
        context.Logger.LogInformation("Starting execution...");
    }

    protected override async Task<ModuleResult<string>?> OnAfterExecuteAsync(
        IModuleContext context,
        ModuleResult<string> result,
        CancellationToken cancellationToken)
    {
        // Runs after ExecuteAsync completes (success or failure)
        context.Logger.LogInformation("Completed with status: {Status}", result.ModuleStatus);
        return result; // Can modify or replace result
    }

    protected override async Task OnSkippedAsync(
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken)
    {
        // Runs when module is skipped
        context.Logger.LogWarning("Skipped: {Reason}", skipDecision.Reason);
    }

    protected override async Task OnFailedAsync(
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Runs when module fails (before OnAfterExecuteAsync)
        await NotifyTeamAsync(exception);
    }

    public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return "Hello, World!";
    }
}
```

---

## Design Decisions

### 1. Virtual Methods vs Interface

**Choice: Virtual methods with default empty implementations**

Rationale:
- Users override only what they need (unlike IHookable requiring both methods)
- IntelliSense shows available hooks when typing `override`
- No interface to implement, no attributes to discover
- Clear execution order visible in base class

### 2. Hook Signatures

| Hook | Parameters | Return | Purpose |
|------|------------|--------|---------|
| `OnBeforeExecuteAsync` | `IModuleContext`, `CancellationToken` | `Task` | Setup, logging, validation |
| `OnAfterExecuteAsync` | `IModuleContext`, `ModuleResult<T>`, `CancellationToken` | `Task<ModuleResult<T>?>` | Cleanup, result transformation |
| `OnSkippedAsync` | `IModuleContext`, `SkipDecision`, `CancellationToken` | `Task` | Notification, cleanup on skip |
| `OnFailedAsync` | `IModuleContext`, `Exception`, `CancellationToken` | `Task` | Error handling, alerting |

**Key Design Choices:**
- `OnAfterExecuteAsync` can return a modified result (or null to keep original)
- `OnFailedAsync` runs BEFORE `OnAfterExecuteAsync` (matching attribute system behavior)
- All hooks receive `CancellationToken` for proper async patterns
- All hooks receive `IModuleContext` for access to logging, services, etc.

### 3. Execution Order

```
1. Skip check (ISkippable.ShouldSkip)
   |
   +-- If skipped --> OnSkippedAsync() --> End
   |
2. OnBeforeExecuteAsync()
3. ExecuteAsync()
   |
   +-- If failed --> OnFailedAsync()
   |
4. OnAfterExecuteAsync()
5. End
```

### 4. Interaction with Existing Systems

The three hook systems will coexist:

| System | Invocation Order | Use Case |
|--------|-----------------|----------|
| **Direct overrides** (new) | First | Module-specific behavior |
| **IHookable interface** | Second | Legacy compatibility |
| **Attribute receivers** | Third | Cross-cutting concerns, reusable behaviors |

All three are invoked at each lifecycle point. Direct overrides run first because they are "closest" to the module.

### 5. Error Handling

- Exceptions in `OnBeforeExecuteAsync` prevent `ExecuteAsync` from running
- Exceptions in `OnFailedAsync` are logged but don't prevent `OnAfterExecuteAsync`
- Exceptions in `OnAfterExecuteAsync` are logged but module result is preserved
- Exceptions in `OnSkippedAsync` are logged but skip proceeds

---

## Architecture Overview

### Current Module<T> Structure

```csharp
public abstract class Module<T> : IModule
{
    public abstract Task<T?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken);
}
```

### Proposed Module<T> Structure

```csharp
public abstract class Module<T> : IModule
{
    // Existing
    public abstract Task<T?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken);

    // New lifecycle hooks
    protected virtual Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => Task.CompletedTask;

    protected virtual Task<ModuleResult<T>?> OnAfterExecuteAsync(
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken)
        => Task.FromResult<ModuleResult<T>?>(null); // null = keep original result

    protected virtual Task OnSkippedAsync(
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken)
        => Task.CompletedTask;

    protected virtual Task OnFailedAsync(
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken)
        => Task.CompletedTask;
}
```

### Integration Point: ModuleExecutionPipeline

The `ModuleExecutionPipeline` class orchestrates module execution. We need to:
1. Call `OnBeforeExecuteAsync` before `ExecuteAsync`
2. Call `OnFailedAsync` in catch blocks
3. Call `OnAfterExecuteAsync` in finally-like flow
4. Call `OnSkippedAsync` when skip decision is made

### Integration Point: ModuleRunner

The `ModuleRunner` handles skip detection. We need to:
1. Invoke `OnSkippedAsync` after skip is detected but before lifecycle events

---

## Implementation Plan

### Task 1: Add Virtual Hook Methods to Module<T>

**Files:**
- Modify: `src/ModularPipelines/Modules/Module.cs`

**Changes:**
```csharp
// Add to Module<T> class

/// <summary>
/// Called before the module executes. Override to add setup logic.
/// </summary>
/// <param name="context">The module context.</param>
/// <param name="cancellationToken">Cancellation token.</param>
/// <returns>A task representing the operation.</returns>
protected virtual Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    => Task.CompletedTask;

/// <summary>
/// Called after the module completes (success or failure). Override to add cleanup or result transformation.
/// </summary>
/// <param name="context">The module context.</param>
/// <param name="result">The module result.</param>
/// <param name="cancellationToken">Cancellation token.</param>
/// <returns>A modified result, or null to keep the original.</returns>
protected virtual Task<ModuleResult<T>?> OnAfterExecuteAsync(
    IModuleContext context,
    ModuleResult<T> result,
    CancellationToken cancellationToken)
    => Task.FromResult<ModuleResult<T>?>(null);

/// <summary>
/// Called when the module is skipped. Override to add skip notification logic.
/// </summary>
/// <param name="context">The module context.</param>
/// <param name="skipDecision">The skip decision with reason.</param>
/// <param name="cancellationToken">Cancellation token.</param>
/// <returns>A task representing the operation.</returns>
protected virtual Task OnSkippedAsync(
    IModuleContext context,
    SkipDecision skipDecision,
    CancellationToken cancellationToken)
    => Task.CompletedTask;

/// <summary>
/// Called when the module fails with an exception. Override to add error handling.
/// Called before OnAfterExecuteAsync.
/// </summary>
/// <param name="context">The module context.</param>
/// <param name="exception">The exception that caused the failure.</param>
/// <param name="cancellationToken">Cancellation token.</param>
/// <returns>A task representing the operation.</returns>
protected virtual Task OnFailedAsync(
    IModuleContext context,
    Exception exception,
    CancellationToken cancellationToken)
    => Task.CompletedTask;
```

**Verification:**
```bash
dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release
```

---

### Task 2: Create IDirectHookInvoker Internal Interface

**Files:**
- Create: `src/ModularPipelines/Engine/Execution/IDirectHookInvoker.cs`
- Create: `src/ModularPipelines/Engine/Execution/DirectHookInvoker.cs`

**Purpose:** Encapsulate the logic for invoking direct module hooks with proper error handling.

**Interface:**
```csharp
namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Invokes direct module lifecycle hooks (virtual method overrides).
/// </summary>
internal interface IDirectHookInvoker
{
    Task InvokeBeforeExecuteAsync<T>(
        Module<T> module,
        IModuleContext context,
        CancellationToken cancellationToken);

    Task<ModuleResult<T>?> InvokeAfterExecuteAsync<T>(
        Module<T> module,
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken);

    Task InvokeSkippedAsync<T>(
        Module<T> module,
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken);

    Task InvokeFailedAsync<T>(
        Module<T> module,
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken);
}
```

**Implementation:**
```csharp
namespace ModularPipelines.Engine.Execution;

internal class DirectHookInvoker : IDirectHookInvoker
{
    private readonly ILogger<DirectHookInvoker> _logger;

    public DirectHookInvoker(ILogger<DirectHookInvoker> logger)
    {
        _logger = logger;
    }

    public async Task InvokeBeforeExecuteAsync<T>(
        Module<T> module,
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        // Call the protected virtual method via reflection or internal accessor
        await module.InvokeOnBeforeExecuteAsync(context, cancellationToken);
    }

    // ... other methods with error handling and logging
}
```

**Note:** We need internal accessor methods on Module<T> since protected methods aren't directly callable from other classes.

---

### Task 3: Add Internal Accessor Methods to Module<T>

**Files:**
- Modify: `src/ModularPipelines/Modules/Module.cs`

**Purpose:** Expose protected virtual methods to internal engine code.

**Changes:**
```csharp
// Add internal accessors that call the protected virtual methods

internal Task InvokeOnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    => OnBeforeExecuteAsync(context, cancellationToken);

internal Task<ModuleResult<T>?> InvokeOnAfterExecuteAsync(
    IModuleContext context,
    ModuleResult<T> result,
    CancellationToken cancellationToken)
    => OnAfterExecuteAsync(context, result, cancellationToken);

internal Task InvokeOnSkippedAsync(
    IModuleContext context,
    SkipDecision skipDecision,
    CancellationToken cancellationToken)
    => OnSkippedAsync(context, skipDecision, cancellationToken);

internal Task InvokeOnFailedAsync(
    IModuleContext context,
    Exception exception,
    CancellationToken cancellationToken)
    => OnFailedAsync(context, exception, cancellationToken);
```

---

### Task 4: Integrate Hooks into ModuleExecutionPipeline

**Files:**
- Modify: `src/ModularPipelines/Engine/ModuleExecutionPipeline.cs`

**Changes:**

1. Add `IDirectHookInvoker` dependency
2. Call `InvokeBeforeExecuteAsync` before execution
3. Call `InvokeFailedAsync` in exception handler
4. Call `InvokeAfterExecuteAsync` after execution
5. Allow result transformation from `OnAfterExecuteAsync`

**Execution Flow:**
```csharp
public async Task<ModuleResult<T>> ExecuteAsync<T>(...)
{
    try
    {
        // Skip check first
        if (module is ISkippable skippable)
        {
            var skipDecision = await skippable.ShouldSkip(moduleContext);
            if (skipDecision.ShouldSkip)
            {
                // NEW: Invoke direct hook
                await _directHookInvoker.InvokeSkippedAsync(module, moduleContext, skipDecision, ct);
                return await HandleSkipped(...);
            }
        }

        // NEW: Before hook (direct override - runs first)
        await _directHookInvoker.InvokeBeforeExecuteAsync(module, moduleContext, ct);

        // Existing: IHookable.OnBeforeExecute
        if (module is IHookable hookable)
        {
            await hookable.OnBeforeExecute(moduleContext);
        }

        // Execute the module
        var result = await ExecuteWithPolicies(module, executionContext, moduleContext);
        var moduleResult = new ModuleResult<T>(result, executionContext);

        // NEW: After hook (direct override)
        var modifiedResult = await _directHookInvoker.InvokeAfterExecuteAsync(
            module, moduleContext, moduleResult, ct);
        if (modifiedResult != null)
        {
            moduleResult = modifiedResult;
        }

        return moduleResult;
    }
    catch (Exception exception)
    {
        // NEW: Failed hook (direct override)
        await _directHookInvoker.InvokeFailedAsync(module, moduleContext, exception, ct);

        return await HandleException(...);
    }
    finally
    {
        // Existing: IHookable.OnAfterExecute
        if (module is IHookable hookable)
        {
            await hookable.OnAfterExecute(moduleContext);
        }
    }
}
```

---

### Task 5: Register DirectHookInvoker in DI

**Files:**
- Modify: `src/ModularPipelines/DependencyInjection/DependencyInjectionSetup.cs`

**Changes:**
```csharp
// Add to singletons section
.AddSingleton<IDirectHookInvoker, DirectHookInvoker>()
```

---

### Task 6: Add Unit Tests for Direct Hooks

**Files:**
- Create: `test/ModularPipelines.UnitTests/Hooks/DirectModuleHooksTests.cs`

**Test Cases:**

1. `OnBeforeExecuteAsync_CalledBeforeExecuteAsync`
2. `OnAfterExecuteAsync_CalledAfterExecuteAsync`
3. `OnAfterExecuteAsync_CanModifyResult`
4. `OnSkippedAsync_CalledWhenModuleSkipped`
5. `OnFailedAsync_CalledWhenModuleFails`
6. `OnFailedAsync_CalledBeforeOnAfterExecuteAsync`
7. `DirectHooks_CalledBeforeIHookable`
8. `DirectHooks_CalledBeforeAttributeReceivers`
9. `OnBeforeExecuteAsync_ExceptionPreventsExecution`
10. `OnAfterExecuteAsync_ExceptionLogged_ResultPreserved`

---

### Task 7: Add Integration Tests

**Files:**
- Create: `test/ModularPipelines.UnitTests/Hooks/DirectModuleHooksIntegrationTests.cs`

**Test Cases:**

1. Full pipeline execution with all hooks
2. Hook ordering verification (direct -> IHookable -> attributes)
3. Result transformation in OnAfterExecuteAsync
4. Error handling in hooks

---

### Task 8: Update Documentation

**Files:**
- Update: `docs/docs/hooks.md` (if exists) or create new

**Content:**
- Explain the three hook systems
- When to use each
- Examples for each hook method
- Execution order diagram

---

## Task Summary

| Task | Description | Files | Effort |
|------|-------------|-------|--------|
| 1 | Add virtual hook methods to Module<T> | Module.cs | Small |
| 2 | Create IDirectHookInvoker interface and implementation | New files | Medium |
| 3 | Add internal accessor methods to Module<T> | Module.cs | Small |
| 4 | Integrate hooks into ModuleExecutionPipeline | ModuleExecutionPipeline.cs | Medium |
| 5 | Register DirectHookInvoker in DI | DependencyInjectionSetup.cs | Small |
| 6 | Add unit tests | New test file | Medium |
| 7 | Add integration tests | New test file | Medium |
| 8 | Update documentation | docs/ | Small |

---

## Success Criteria

1. Users can override `OnBeforeExecuteAsync`, `OnAfterExecuteAsync`, `OnSkippedAsync`, `OnFailedAsync`
2. Hooks are called in correct order relative to `ExecuteAsync`
3. Direct hooks run before IHookable and attribute-based hooks
4. All existing tests pass
5. New tests cover all hook scenarios
6. Build succeeds with no warnings

---

## Future Considerations

1. **OnTimeoutAsync** - Could be added for timeout-specific handling
2. **OnRetryAsync** - Could be added for retry-specific logic
3. **Analyzers** - Could add Roslyn analyzers to warn about common mistakes (e.g., not calling base)

---

## Appendix: Full Module<T> After Changes

```csharp
public abstract class Module<T> : IModule
{
    internal TaskCompletionSource<ModuleResult<T?>> CompletionSource { get; } = new();

    Type IModule.ResultType => typeof(T);

    protected virtual void DeclareDependencies(IDependencyDeclaration deps) { }

    internal IReadOnlyList<DeclaredDependency> GetDeclaredDependencies() { ... }

    // Core execution method
    public abstract Task<T?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken);

    // NEW: Lifecycle hooks
    protected virtual Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => Task.CompletedTask;

    protected virtual Task<ModuleResult<T>?> OnAfterExecuteAsync(
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken)
        => Task.FromResult<ModuleResult<T>?>(null);

    protected virtual Task OnSkippedAsync(
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken)
        => Task.CompletedTask;

    protected virtual Task OnFailedAsync(
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken)
        => Task.CompletedTask;

    // NEW: Internal accessors for engine
    internal Task InvokeOnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => OnBeforeExecuteAsync(context, cancellationToken);

    internal Task<ModuleResult<T>?> InvokeOnAfterExecuteAsync(
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken)
        => OnAfterExecuteAsync(context, result, cancellationToken);

    internal Task InvokeOnSkippedAsync(
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken)
        => OnSkippedAsync(context, skipDecision, cancellationToken);

    internal Task InvokeOnFailedAsync(
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken)
        => OnFailedAsync(context, exception, cancellationToken);

    public TaskAwaiter<ModuleResult<T?>> GetAwaiter() => CompletionSource.Task.GetAwaiter();
}
```
