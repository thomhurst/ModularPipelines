---
title: Hooks
---

# Hooks

## Module Hooks

There are two ways to register hooks for a module:
- via a ModuleHooks class that is invoked before and after every module
- Override the Before/After methods within a module

### Overriding

Overriding in modules allows for specific behaviour per module, that won't affect other modules.
e.g.

```csharp
public class MyModule : Module
{
    protected override Task OnBeforeExecute(IPipelineContext context)
    {
        context.Logger.LogInformation("MyModule started!");
        return Task.CompletedTask;
    }

    protected override Task OnAfterExecute(IPipelineContext context)
    {
        context.Logger.LogInformation("MyModule ended!");
        return Task.CompletedTask;
    }

    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return NothingAsync();
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