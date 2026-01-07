---
title: Hooks
---

# Hooks

## Module Hooks

There are three ways to hook into module execution:
1. **Direct hooks** - Override virtual lifecycle methods in `Module<T>` (new in v4)
2. **IHookable interface** - Implement the interface for before/after execution
3. **Module hooks class** - Register global hooks invoked for every module

### Direct Module Hooks (Recommended)

The simplest way to add lifecycle hooks is to override the virtual methods directly in your module. These hooks run **before** IHookable hooks and provide access to the full module context and result.

```csharp
public class MyModule : Module<string>
{
    // Called before ExecuteAsync - ideal for setup, validation, or logging
    protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Setting up MyModule...");
        return Task.CompletedTask;
    }

    // Called after ExecuteAsync - can modify the result or add cleanup logic
    protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
        IModuleContext context,
        ModuleResult<string> result,
        CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("MyModule completed with: {Result}", result.Value);
        return Task.FromResult<ModuleResult<string>?>(null); // null = keep original result
    }

    // Called when the module is skipped (ShouldSkip returns true)
    protected override Task OnSkippedAsync(
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("MyModule was skipped: {Reason}", skipDecision.Reason);
        return Task.CompletedTask;
    }

    // Called when ExecuteAsync throws an exception (before OnAfterExecuteAsync)
    protected override Task OnFailedAsync(
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        context.Logger.LogError(exception, "MyModule failed!");
        // Send alert, cleanup resources, etc.
        return Task.CompletedTask;
    }

    public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return "Hello, World!";
    }
}
```

**Hook execution order:**
1. `OnBeforeExecuteAsync` (direct hook)
2. `IHookable.OnBeforeExecute` (if implemented)
3. `ExecuteAsync`
4. On failure: `OnFailedAsync` (direct hook)
5. `OnAfterExecuteAsync` (direct hook)
6. `IHookable.OnAfterExecute` (in finally block)

**Error handling:**
- `OnBeforeExecuteAsync` exceptions **prevent** execution and are propagated
- `OnAfterExecuteAsync` exceptions are **logged** but don't affect the result
- `OnSkippedAsync` exceptions are **logged** but don't affect skip behavior
- `OnFailedAsync` exceptions are **logged** but don't prevent `OnAfterExecuteAsync`

**Edge case: OnBeforeExecuteAsync throws**
If `OnBeforeExecuteAsync` throws an exception:
- `ExecuteAsync` will NOT be called
- `OnFailedAsync` will NOT be called (module never started)
- `OnAfterExecuteAsync` will NOT be called (before hooks didn't complete)
- `IHookable.OnAfterExecute` WILL still be called (in finally block)

### IHookable Interface

For backward compatibility, you can implement `IHookable` to add hooks:

```csharp
public class MyModule : Module<string>, IHookable
{
    public Task OnBeforeExecute(IPipelineContext context)
    {
        context.Logger.LogInformation("MyModule started!");
        return Task.CompletedTask;
    }

    public Task OnAfterExecute(IPipelineContext context)
    {
        context.Logger.LogInformation("MyModule ended!");
        return Task.CompletedTask;
    }

    public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return "Done";
    }
}
```

### Hooks class

If we want to have repeat behaviour for every module, we can register some 'Hook' classes during startup.

Pipeline Global Hooks will run once, before any modules have started, and/or after all modules have finished. Pipeline Module Hooks will run repeatedly, before every module, and/or after every module.

This can be useful if you want some repeated action for every module. E.g. standard logging behaviour.

To do so, simply create a class that implements `IPipelineModuleHooks`.

```csharp
collection.AddPipelineModuleHooks<MyModuleHooks>();
```

```csharp
public class MyModuleHooks : IPipelineModuleHooks
{
    public Task OnBeforeModuleStartAsync(IPipelineHookContext context, IModule module)
    {
        context.Logger.LogInformation("{Module} is starting", module.GetType().Name);
        return Task.CompletedTask;
    }

    public Task OnBeforeModuleEndAsync(IPipelineHookContext context, IModule module)
    {
        context.Logger.LogInformation("{Module} finished after {Elapsed}", module.GetType().Name, module.Duration);
        return Task.CompletedTask;
    }
}
```

## Global Hooks

Global hooks can be registered by creating a class that implements the `IPipelineGlobalHooks` interface. 

These are similar to the module hooks, but instead of running before and after EACH module, they run before and after ALL modules. So think of them more as a pipeline start up, and a pipeline finish hook.

The end hook also gives you access to a Pipeline summary object. Giving you information such as pass/fail, duration, start and end, etc.


```csharp
collection.AddPipelineGlobalHooks<MyGlobalHooks>();
```

```csharp
public class MyGlobalHooks : IPipelineGlobalHooks
{
    public Task OnStartAsync(IPipelineHookContext pipelineContext)
    {
        pipelineContext.Logger.LogInformation("Pipeline is starting");
        return Task.CompletedTask;
    }

    public Task OnEndAsync(IPipelineHookContext pipelineContext, PipelineSummary pipelineSummary)
    {
        pipelineContext.Logger.LogInformation("Pipeline is ending");
        return Task.CompletedTask;
    }
}
```