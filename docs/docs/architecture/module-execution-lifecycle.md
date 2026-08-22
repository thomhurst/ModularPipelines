---
title: Module Execution Lifecycle
---

# Module execution lifecycle

A module combines execution policy, module-owned virtual hooks, opt-in attribute handlers,
and global event receivers.

## Execution phases

For a module that runs successfully, the phases are:

1. Dependencies become ready.
2. Global `IModuleEventReceiver.OnModuleReadyAsync` receivers run concurrently.
3. Attribute `IModuleReadyHandler` handlers run sequentially by priority.
4. Global `IModuleEventReceiver.OnModuleStartAsync` receivers run concurrently.
5. Attribute `IModuleStartHandler` handlers run sequentially by priority.
6. The module skip condition is evaluated.
7. `Module<T>.OnBeforeExecuteAsync` runs once.
8. `Module<T>.ExecuteAsync` runs through timeout and retry policies.
9. `Module<T>.OnAfterExecuteAsync` runs once.
10. Global `IModuleEventReceiver.OnModuleEndAsync` receivers run concurrently.
11. Attribute `IModuleEndHandler` handlers run sequentially by priority.
12. The module result is published and dependants become eligible.

`OnBeforeExecuteAsync` and `OnAfterExecuteAsync` wrap the complete resilience shield, not each
individual attempt.

## Skipped modules

The skip condition is evaluated before the module-owned before hook. If it returns a skip
decision:

1. `Module<T>.OnSkippedAsync`
2. Attribute `IModuleSkippedHandler`
3. Global `IModuleEventReceiver.OnModuleSkippedAsync`

`OnBeforeExecuteAsync`, `ExecuteAsync`, and `OnAfterExecuteAsync` do not run.

## Failed modules

When module execution throws:

1. `Module<T>.OnFailedAsync`
2. `Module<T>.OnAfterExecuteAsync`, with a failed `ModuleResult<T>`
3. Attribute `IModuleFailureHandler`
4. Global `IModuleEventReceiver.OnModuleFailureAsync`

Retry attempts complete before this failure sequence. If the configured failure condition
ignores the failure, the resulting module status reflects that policy.

## Hook failures

- An exception from `OnBeforeExecuteAsync` prevents module execution. `OnFailedAsync` and the
  failure event receivers are notified, but `OnAfterExecuteAsync` does not run.
- Exceptions from `OnFailedAsync`, `OnSkippedAsync`, and `OnAfterExecuteAsync` are logged and do
  not replace the module outcome.
- Attribute handlers propagate by default. Set their `ContinueOnError` property to continue after
  a handler failure.
- Exceptions from global event receivers propagate from the lifecycle event.

## Choosing an extension point

Use module virtual hooks when behavior is part of one module. Use attribute handlers when
behavior should be explicitly attached to selected module types. Use `IModuleEventReceiver`
when one service must observe every module in the pipeline.

See [Hooks](../how-to/hooks.md) for implementation examples.
