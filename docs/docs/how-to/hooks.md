---
title: Hooks
---

# Hooks

Module lifecycle behavior has three extension points:

1. Override the virtual lifecycle methods on `Module<T>` for behavior owned by one module.
2. Implement the attribute interfaces in `ModularPipelines.Events` for reusable,
   opt-in behavior attached to selected modules.
3. Implement `IModuleEventHandler` for behavior that observes every module in a pipeline.

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
    public Task OnModuleStartAsync(IModuleHookContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("{Module} started", context.ModuleName);
        return Task.CompletedTask;
    }

    public Task OnModuleEndAsync(
        IModuleHookContext context,
        IModuleResult result,
        CancellationToken cancellationToken)
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
All handlers inherit `IEventHandler`. Set `Order` to control the order within a handler
family (ascending, default `0`), or `ContinueOnError` to log a handler failure as a warning
and continue. `IModuleHookContext` is read-only: hooks observe modules, while retries,
skips, and failure policy are configured on the module.

Registration attributes implement `IModuleRegistrationHandler`. Also implement
`IPlanningSafe` only for deterministic, idempotent handlers
without external side effects; those handlers may run while exporting a resolved
dependency graph.

## Global module event handlers

Implement `IModuleEventHandler` to observe every module, then register it once:

```csharp
public sealed class ModuleMetricsHandler : IModuleEventHandler
{
    public Task OnModuleStartAsync(IModuleHookContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("{Module} started", context.ModuleName);
        return Task.CompletedTask;
    }

    public Task OnModuleEndAsync(
        IModuleHookContext context,
        IModuleResult result,
        CancellationToken cancellationToken)
    {
        context.Logger.LogInformation(
            "{Module} finished after {Elapsed}",
            context.ModuleName,
            context.ElapsedTime);
        return Task.CompletedTask;
    }
}

builder.AddModuleEventHandler<ModuleMetricsHandler>();
```

Global and attribute handlers use the same callback signatures and shared error/order
properties. Global handlers run sequentially in ascending `Order` for each event. A class that
implements both `IModuleEventHandler` and `IPipelineEventHandler` and is registered with both
`AddModuleEventHandler<T>()` and `AddPipelineEventHandler<T>()` is one shared singleton.

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
`OnFailedAsync` and the failure event handlers are still notified. Exceptions from
`OnAfterExecuteAsync`, `OnFailedAsync`, and `OnSkippedAsync` are logged without replacing
the module outcome.

## Handler failures

Ready and Start handlers are gates. When one throws (and `ContinueOnError` is `false`), the
module fails with that exception, the module does not execute, and failure handlers are
notified. Global and attribute handlers take the same path.

End, Failure, and Skipped handlers are observers. They cannot change the outcome they observe:
a handler exception is logged, recorded as a pipeline error, and the next handler family still
runs. A module that succeeded stays succeeded, and a failed module keeps its original exception.
Recorded handler errors are thrown with the pipeline's other errors when the pipeline throws on
failure. A failing `IPipelineEventHandler.OnPipelineEndAsync` fails the pipeline, but never replaces
an exception that already failed it; in that case it is recorded as an additional pipeline error.

## Cancellation

Every callback receives a `CancellationToken`. Ready and Start handlers receive the module's
execution token. End, Failure, Skipped, and pipeline end handlers receive a token that is
cancelled only when the user or host cancels the pipeline, so they still run after a module
failure. Extension points use `Task` rather than `ValueTask`.

## Pipeline event handlers

`IPipelineEventHandler` observes the pipeline as a whole rather than individual modules:

```csharp
public sealed class PipelineLoggingHandler : IPipelineEventHandler
{
    public Task OnPipelineStartAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Pipeline started");
        return Task.CompletedTask;
    }

    public Task OnPipelineEndAsync(
        IPipelineContext context,
        PipelineSummary summary,
        CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Pipeline ended");
        return Task.CompletedTask;
    }
}

builder.AddPipelineEventHandler<PipelineLoggingHandler>();
```

Pipeline handlers also inherit `IEventHandler` and run in ascending `Order`.

## Extension point conventions

Handlers, requirements, validators, and providers follow the same conventions:

- **Ordering.** Extension points that run in sequence expose an `Order` property, ascending (lower
  values run first), defaulting to `0`. This applies to event handlers, requirements
  (`IPipelineRequirement.Order`), and validators (`IPipelineValidator.Order`). Plugins apply in the
  order they are added. Module scheduling priority (`ModulePriority`, `[Priority]`) is a different
  concept: it decides which ready module starts first, and higher priorities start first.
- **Async shape.** Asynchronous members return `Task` or `Task<T>` (not `ValueTask`) and take a
  `CancellationToken` as their last parameter.
- **Evolution.** Interfaces that are likely to grow give new members default implementations when
  a sensible default exists (for example the sub-module members of `IModuleEstimatedTimeProvider`
  or `IModuleResultRepository.IsEnabled`); otherwise they are exposed as abstract base classes.
