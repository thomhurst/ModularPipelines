# Module execution lifecycle

A module combines execution policy, module-owned virtual hooks, opt-in attribute handlers, and global event handlers.

## Execution phases[​](#execution-phases "Direct link to Execution phases")

For a module that runs successfully, the phases are:

1. Dependencies become ready.
2. Global `IModuleEventHandler.OnModuleReadyAsync` handlers run sequentially in ascending `Order`.
3. Attribute `IModuleReadyHandler` handlers run sequentially in ascending `Order`.
4. Global `IModuleEventHandler.OnModuleStartAsync` handlers run sequentially in ascending `Order`.
5. Attribute `IModuleStartHandler` handlers run sequentially in ascending `Order`.
6. The module skip condition is evaluated.
7. `Module<T>.OnBeforeExecuteAsync` runs once.
8. `Module<T>.ExecuteAsync` runs through timeout handling and the configured resilience shield, which may compose retries with other resilience strategies.
9. `Module<T>.OnAfterExecuteAsync` runs once.
10. Global `IModuleEventHandler.OnModuleEndAsync` handlers run sequentially in ascending `Order`.
11. Attribute `IModuleEndHandler` handlers run sequentially in ascending `Order`.
12. The module result is published and dependants become eligible.

`OnBeforeExecuteAsync` and `OnAfterExecuteAsync` wrap the complete resilience shield, not each individual attempt.

## Skipped modules[​](#skipped-modules "Direct link to Skipped modules")

The skip condition is evaluated before the module-owned before hook. If it returns a skip decision:

1. `Module<T>.OnSkippedAsync`
2. Attribute `IModuleSkippedHandler`
3. Global `IModuleEventHandler.OnModuleSkippedAsync`

`OnBeforeExecuteAsync`, `ExecuteAsync`, and `OnAfterExecuteAsync` do not run.

## Failed modules[​](#failed-modules "Direct link to Failed modules")

When module execution throws:

1. `Module<T>.OnFailedAsync`
2. `Module<T>.OnAfterExecuteAsync`, with a failed `ModuleResult<T>`
3. Attribute `IModuleFailureHandler`
4. Global `IModuleEventHandler.OnModuleFailureAsync`

Retry attempts complete before this failure sequence. If the configured failure condition ignores the failure, the resulting module status reflects that policy.

## Hook failures[​](#hook-failures "Direct link to Hook failures")

* An exception from `OnBeforeExecuteAsync` prevents module execution. `OnFailedAsync` and the failure event handlers are notified, but `OnAfterExecuteAsync` does not run.
* Exceptions from `OnFailedAsync`, `OnSkippedAsync`, and `OnAfterExecuteAsync` are logged and do not replace the module outcome.
* Attribute and global handlers all run in ascending `Order` within their registration family, even after a handler fails. `ContinueOnError` controls failure propagation: `false` records the failure (one exception, or an aggregate when several handlers fail); `true` only logs a warning.
* Ready and Start handler failures fail the module through the normal failure path: the module does not execute, the failure handlers are notified, and the module status is `Failed`. Global and attribute handlers behave the same way.
* End, Failure, and Skipped handlers observe the outcome and never change it. Each family runs in isolation, so a failing attribute family does not stop the global family (or the reverse). The failure is logged and recorded as a secondary pipeline error, surfaced with the pipeline's other errors.
* A failing `OnPipelineEndAsync` handler fails the pipeline, but never replaces an exception thrown by execution; in that case it is recorded as a secondary pipeline error.
* Estimated-time reads and writes are best-effort: provider exceptions are logged and never change the module outcome.

## Choosing an extension point[​](#choosing-an-extension-point "Direct link to Choosing an extension point")

Use module virtual hooks when behavior is part of one module. Use attribute handlers when behavior should be explicitly attached to selected module types. Use `IModuleEventHandler` when one service must observe every module in the pipeline.

See [Hooks](/ModularPipelines/docs/next/how-to/hooks.md) for implementation examples.
