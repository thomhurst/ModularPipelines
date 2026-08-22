---
title: Hooks
---

# Hooks

Module lifecycle behavior has three extension points:

1. Override the virtual lifecycle methods on `Module<T>` for behavior owned by one module.
2. Implement the attribute interfaces in `ModularPipelines.Attributes.Events` for reusable,
   opt-in behavior attached to selected modules.
3. Implement `IModuleEventReceiver` for behavior that observes every module in a pipeline.

`ModuleConfiguration` controls execution policy only; it does not contain lifecycle hooks.

## Module virtual hooks

Override the virtual methods directly when the behavior belongs to the module:

```csharp
public class MyModule : Module<string>
{
    protected override Task OnBeforeExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Setting up MyModule");
        return Task.CompletedTask;
    }

    protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
        IModuleContext context,
        ModuleResult<string> result,
        CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("MyModule completed");
        return Task.FromResult<ModuleResult<string>?>(null);
    }

    protected override Task OnSkippedAsync(
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Skipped: {Reason}", skipDecision.Reason);
        return Task.CompletedTask;
    }

    protected override Task OnFailedAsync(
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        context.Logger.LogError(exception, "MyModule failed");
        return Task.CompletedTask;
    }

    protected internal override Task<string> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
        => Task.FromResult<string?>("Hello, World!");
}
```

`OnBeforeExecuteAsync` runs once before the first execution attempt.
`OnAfterExecuteAsync` runs once after the final attempt and can return a replacement result;
return `null` to retain the original result. `OnFailedAsync` runs before
`OnAfterExecuteAsync` when execution fails.

## Attribute event handlers

Implement an event-handler interface on an attribute, then apply it to selected modules:

```csharp
[AttributeUsage(AttributeTargets.Class)]
public sealed class AuditModuleAttribute : Attribute,
    IModuleStartHandler,
    IModuleEndHandler
{
    public Task OnModuleStartAsync(IModuleHookContext context)
    {
        context.Logger.LogInformation("{Module} started", context.ModuleName);
        return Task.CompletedTask;
    }

    public Task OnModuleEndAsync(IModuleHookContext context, IModuleResult result)
    {
        context.Logger.LogInformation("{Module} ended", context.ModuleName);
        return Task.CompletedTask;
    }
}

[AuditModule]
public class BuildModule : Module<string>
{
    // ...
}
```

Available interfaces are `IModuleReadyHandler`, `IModuleStartHandler`,
`IModuleEndHandler`, `IModuleFailureHandler`, and `IModuleSkippedHandler`.
Handlers can implement `IEventHandlerPriority`; lower values run first.

## Global module event receivers

Implement `IModuleEventReceiver` to observe every module, then register it once:

```csharp
public sealed class ModuleMetricsReceiver : IModuleEventReceiver
{
    public Task OnModuleStartAsync(IModuleHookContext context)
    {
        context.Logger.LogInformation("{Module} started", context.ModuleName);
        return Task.CompletedTask;
    }

    public Task OnModuleEndAsync(IModuleHookContext context)
    {
        context.Logger.LogInformation(
            "{Module} finished after {Elapsed}",
            context.ModuleName,
            context.ElapsedTime);
        return Task.CompletedTask;
    }
}

builder.AddModuleEventReceiver<ModuleMetricsReceiver>();
```

All registered global receivers are invoked concurrently for each event. Attribute handlers
run sequentially in priority order.

## Lifecycle ordering

The order for a successful module is:

1. Global `OnModuleReadyAsync`
2. Attribute `IModuleReadyHandler`
3. Global `OnModuleStartAsync`
4. Attribute `IModuleStartHandler`
5. Module `OnBeforeExecuteAsync`
6. Module `ExecuteAsync` through its configured resilience shield
7. Module `OnAfterExecuteAsync`
8. Global `OnModuleEndAsync`
9. Attribute `IModuleEndHandler`

For a failed execution, the completion portion is:

1. Module `OnFailedAsync`
2. Module `OnAfterExecuteAsync`
3. Attribute `IModuleFailureHandler`
4. Global `OnModuleFailureAsync`

For a skipped module, the completion portion is:

1. Module `OnSkippedAsync`
2. Attribute `IModuleSkippedHandler`
3. Global `OnModuleSkippedAsync`

If `OnBeforeExecuteAsync` throws, `ExecuteAsync` and `OnAfterExecuteAsync` do not run;
`OnFailedAsync` and the failure event receivers are still notified. Exceptions from
`OnAfterExecuteAsync`, `OnFailedAsync`, and `OnSkippedAsync` are logged without replacing
the module outcome.

## Pipeline hooks

`IPipelineGlobalHooks` observes the pipeline as a whole rather than individual modules:

```csharp
public sealed class PipelineLoggingHooks : IPipelineGlobalHooks
{
    public Task OnPipelineStartAsync(IPipelineContext context)
    {
        context.Logger.LogInformation("Pipeline started");
        return Task.CompletedTask;
    }

    public Task OnPipelineEndAsync(
        IPipelineContext context,
        PipelineSummary summary)
    {
        context.Logger.LogInformation("Pipeline ended");
        return Task.CompletedTask;
    }
}

builder.AddPipelineGlobalHooks<PipelineLoggingHooks>();
```
