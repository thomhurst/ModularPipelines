---
title: Module Execution Lifecycle
---

# Module execution lifecycle

A module combines execution policy, module-owned virtual hooks, opt-in attribute handlers,
and global event handlers.

## Execution phases

For a module that runs successfully, the phases are:

1. Dependencies become ready.
2. Global `IModuleEventHandler.OnModuleReadyAsync` handlers run sequentially by priority.
3. Attribute `IModuleReadyHandler` handlers run sequentially by priority.
4. Global `IModuleEventHandler.OnModuleStartAsync` handlers run sequentially by priority.
5. Attribute `IModuleStartHandler` handlers run sequentially by priority.
6. The module skip condition is evaluated.
7. `Module<T>.OnBeforeExecuteAsync` runs once.
8. `Module<T>.ExecuteAsync` runs through timeout handling and the configured resilience shield, which may compose retries with other resilience strategies.
9. `Module<T>.OnAfterExecuteAsync` runs once.
10. Global `IModuleEventHandler.OnModuleEndAsync` handlers run sequentially by priority.
11. Attribute `IModuleEndHandler` handlers run sequentially by priority.
12. The module result is published and dependants become eligible.

`OnBeforeExecuteAsync` and `OnAfterExecuteAsync` wrap the complete resilience shield, not each
individual attempt.

## Skipped modules

The skip condition is evaluated before the module-owned before hook. If it returns a skip
decision:

1. `Module<T>.OnSkippedAsync`
2. Attribute `IModuleSkippedHandler`
3. Global `IModuleEventHandler.OnModuleSkippedAsync`

`OnBeforeExecuteAsync`, `ExecuteAsync`, and `OnAfterExecuteAsync` do not run.

## Failed modules

When module execution throws:

1. `Module<T>.OnFailedAsync`
2. `Module<T>.OnAfterExecuteAsync`, with a failed `ModuleResult<T>`
3. Attribute `IModuleFailureHandler`
4. Global `IModuleEventHandler.OnModuleFailureAsync`

Retry attempts complete before this failure sequence. If the configured failure condition
ignores the failure, the resulting module status reflects that policy.

## Hook failures

- An exception from `OnBeforeExecuteAsync` prevents module execution. `OnFailedAsync` and the
  failure event handlers are notified, but `OnAfterExecuteAsync` does not run.
- Exceptions from `OnFailedAsync`, `OnSkippedAsync`, and `OnAfterExecuteAsync` are logged and do
  not replace the module outcome.
- Attribute and global handlers all run in ascending `Priority` order within their registration
  family, even after a handler fails. `ContinueOnError` controls failure propagation: `false`
  rethrows one recorded failure or aggregates multiple failures after dispatch; `true` suppresses
  that handler's failure.

## Choosing an extension point

Use module virtual hooks when behavior is part of one module. Use attribute handlers when
behavior should be explicitly attached to selected module types. Use `IModuleEventHandler`
when one service must observe every module in the pipeline.

See [Hooks](../how-to/hooks.md) for implementation examples.
