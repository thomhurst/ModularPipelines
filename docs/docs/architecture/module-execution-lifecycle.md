---
title: Module Execution Lifecycle
sidebar_position: 1
---

# Module Execution Lifecycle

This document describes the complete execution lifecycle of a module in ModularPipelines, including the order of operations for hooks, skip checks, retries, timeouts, and how different behavior interfaces interact with each other.

## Overview

ModularPipelines modules can implement several behavior interfaces to customize their execution:

| Interface | Purpose |
|-----------|---------|
| `ISkippable` | Define conditions to skip module execution |
| `IHookable` | Add before/after execution hooks |
| `ITimeoutable` | Set execution timeout |
| `IRetryable<T>` | Configure retry policy |
| `IIgnoreFailures` | Allow failures without failing the pipeline |
| `IAlwaysRun` | Run even when pipeline fails |

## Execution Flow Diagram

The following diagram shows the complete module execution lifecycle:

```
                    Module Scheduled
                          |
                          v
              +------------------------+
              |  Wait for Dependencies |
              +------------------------+
                          |
                          v
              +------------------------+
              | Setup Cancellation     |
              | (IAlwaysRun check)     |
              +------------------------+
                          |
                          v
              +------------------------+
              | ISkippable.ShouldSkip  |-----> Skip? ---+
              +------------------------+               |
                          |                            |
                          | No                         | Yes
                          v                            v
              +------------------------+    +-------------------+
              | OnBeforeExecuteAsync   |    | OnSkippedAsync    |
              | (direct hook)          |    | (direct hook)     |
              +------------------------+    +-------------------+
                          |                            |
                          v                            v
              +------------------------+    +-------------------+
              | IHookable.OnBeforeExe  |    | Return Skipped    |
              +------------------------+    | Result            |
                          |                 +-------------------+
                          v
              +------------------------+
              |    Start Stopwatch     |
              |    Status=Processing   |
              +------------------------+
                          |
                          v
         +--------------------------------+
         |   ExecuteWithPolicies          |
         |   +-------------------------+  |
         |   | Apply Timeout           |  |
         |   | (ITimeoutable or 30min) |  |
         |   +-------------------------+  |
         |              |                 |
         |              v                 |
         |   +-------------------------+  |
         |   | Apply Retry Policy      |  |
         |   | (IRetryable or default) |  |
         |   +-------------------------+  |
         |              |                 |
         |              v                 |
         |   +-------------------------+  |
         |   |    ExecuteAsync         |  |
         |   +-------------------------+  |
         +--------------------------------+
                          |
            +-------------+-------------+
            |                           |
         Success                     Exception
            |                           |
            v                           v
  +------------------+       +--------------------+
  | Status=Successful|       | OnFailedAsync      |
  +------------------+       | (direct hook)      |
            |                +--------------------+
            |                           |
            |                           v
            |                +--------------------+
            |                | IIgnoreFailures?   |----> Yes: IgnoredFailure
            |                +--------------------+
            |                           |
            |                        No |
            |                           v
            |                +--------------------+
            |                | Status=Failed      |
            |                | Cancel Pipeline    |
            |                +--------------------+
            |                           |
            +-------------+-------------+
                          |
                          v
              +------------------------+
              | OnAfterExecuteAsync    |
              | (direct hook)          |
              +------------------------+
                          |
                          v
              +------------------------+
              | IHookable.OnAfterExe   |
              | (in finally block)     |
              +------------------------+
                          |
                          v
                    Module Complete
```

## Detailed Phase Descriptions

### Phase 1: Dependency Resolution

Before a module begins execution, it waits for all its dependencies to complete:

1. Dependencies declared via `[DependsOn<T>]` attributes are resolved
2. Dependencies declared via `DeclareDependencies()` method are resolved
3. The module waits until all dependencies have finished (success, failure, or skip)
4. If `[NotInParallel]` is specified, the module waits for exclusive execution

**Exception for AlwaysRun modules**: When a pipeline fails and AlwaysRun modules are executed late, they skip dependency waiting to prevent deadlocks.

### Phase 2: Cancellation Setup

The cancellation token is configured based on whether the module implements `IAlwaysRun`:

- **Normal modules**: Linked to the engine cancellation token (cancelled when any module fails)
- **AlwaysRun modules**: Not linked to engine cancellation (runs even after pipeline failure)

### Phase 3: Skip Check

If the module implements `ISkippable`:

1. `ShouldSkip(IModuleContext)` is called
2. If `SkipDecision.ShouldSkip` is true:
   - `OnSkippedAsync` (direct hook) is called
   - If a result repository is configured, historical results may be returned
   - The module is marked as `Skipped` and execution ends

**Important**: When a module is skipped, neither `OnBeforeExecuteAsync` nor `IHookable.OnBeforeExecute` are called.

### Phase 4: Before Hooks

Hooks execute in this order:

1. **OnBeforeExecuteAsync** (direct virtual method override)
   - Exceptions here **prevent** execution and are propagated as module failure
   - Neither `OnFailedAsync` nor `OnAfterExecuteAsync` will be called

2. **IHookable.OnBeforeExecute** (interface implementation)
   - Runs after the direct hook
   - Exceptions are propagated

### Phase 5: Execution with Policies

The actual `ExecuteAsync` method is wrapped with timeout and retry policies:

#### Timeout Application

1. If `ITimeoutable` is implemented, use the specified `Timeout` property
2. Otherwise, use the default timeout of 30 minutes
3. Use `TimeSpan.Zero` to disable timeout (not recommended)

#### Retry Policy Application

1. If `IRetryable<T>` is implemented, use the custom retry policy
2. Otherwise, if `PipelineOptions.DefaultRetryCount > 0`, use default retry policy
3. Otherwise, no retries are attempted

**The timeout wraps the entire retry operation**, meaning the timeout applies to the total execution time including all retries.

### Phase 6: Failure Handling

When `ExecuteAsync` throws an exception:

1. **OnFailedAsync** (direct hook) is called first
2. If `IIgnoreFailures` is implemented:
   - `ShouldIgnoreFailures(context, exception)` is called
   - If true, status is set to `IgnoredFailure` and pipeline continues
3. If failure is not ignored, the pipeline is cancelled

### Phase 7: After Hooks

Regardless of success or failure, after hooks run:

1. **OnAfterExecuteAsync** (direct hook)
   - Only runs if `OnBeforeExecuteAsync` completed successfully
   - Receives the `ModuleResult<T>` (success or failure)
   - Can modify and return a new result
   - Exceptions are logged but don't affect the result

2. **IHookable.OnAfterExecute** (in finally block)
   - Always runs if before hooks executed
   - Runs even if `OnAfterExecuteAsync` threw an exception
   - Exceptions are logged but don't affect the result

## Behavior Interaction Matrix

This table shows how different behavior combinations interact:

| Combination | Behavior |
|-------------|----------|
| **IHookable + ISkippable** | `OnBeforeExecute` does NOT run if module is skipped. Skip check happens before hooks. |
| **IRetryable + ITimeoutable** | Timeout applies to TOTAL execution time (all retries combined), not per-retry. |
| **IHookable + IRetryable** | `OnBeforeExecute` runs ONCE before any retries. `OnAfterExecute` runs ONCE after all retries complete. |
| **IAlwaysRun + ISkippable** | Skip check still runs. AlwaysRun only affects cancellation, not skip logic. Both can be combined. |
| **IIgnoreFailures + IRetryable** | Retries happen first. If all retries fail, then `IIgnoreFailures` is checked. |
| **IAlwaysRun + IIgnoreFailures** | Both apply. AlwaysRun prevents cancellation from other failures; IIgnoreFailures prevents this module's failure from cancelling others. |
| **ITimeoutable + IAlwaysRun** | Timeout still applies. AlwaysRun only affects pipeline cancellation, not module timeout. |

## Detailed Behavior Interaction Examples

### IHookable + ISkippable

```csharp
public class MyModule : Module<string>, IHookable, ISkippable
{
    public Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        // This runs FIRST
        return SkipDecision.Skip("Not needed").AsTask();
    }

    public Task OnBeforeExecute(IPipelineContext context)
    {
        // This does NOT run when skipped
        return Task.CompletedTask;
    }

    public Task OnAfterExecute(IPipelineContext context)
    {
        // This does NOT run when skipped
        return Task.CompletedTask;
    }
}
```

**Result**: When `ShouldSkip` returns true, neither `OnBeforeExecute` nor `OnAfterExecute` are called.

### IRetryable + ITimeoutable

```csharp
public class MyModule : Module<string>, IRetryable<string>, ITimeoutable
{
    public TimeSpan Timeout => TimeSpan.FromSeconds(30);

    public AsyncRetryPolicy<string?> GetRetryPolicy(IPipelineContext context)
    {
        return Policy<string?>
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(i));
    }
}
```

**Result**: The 30-second timeout applies to the TOTAL execution time. If each retry takes 10 seconds with 3-second waits between retries, you could timeout during the second retry attempt.

```
Timeline:
0s  - Attempt 1 starts
10s - Attempt 1 fails, wait 1s
11s - Attempt 2 starts
21s - Attempt 2 fails, wait 2s
23s - Attempt 3 starts
30s - TIMEOUT! (even though attempt 3 might succeed at 33s)
```

### IHookable + IRetryable

```csharp
public class MyModule : Module<string>, IHookable, IRetryable<string>
{
    private int _beforeCount = 0;
    private int _afterCount = 0;

    public Task OnBeforeExecute(IPipelineContext context)
    {
        _beforeCount++; // Called ONCE, regardless of retries
        return Task.CompletedTask;
    }

    public Task OnAfterExecute(IPipelineContext context)
    {
        _afterCount++; // Called ONCE, after all retries complete
        return Task.CompletedTask;
    }

    public AsyncRetryPolicy<string?> GetRetryPolicy(IPipelineContext context)
    {
        return Policy<string?>.Handle<Exception>().RetryAsync(3);
    }
}
```

**Result**: Hooks run once, not per-retry:
- `OnBeforeExecute`: Called once before first attempt
- `ExecuteAsync`: May be called 1-4 times (initial + 3 retries)
- `OnAfterExecute`: Called once after final attempt (success or failure)

### IAlwaysRun + ISkippable

```csharp
public class CleanupModule : Module<string>, IAlwaysRun, ISkippable
{
    public Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        // Skip logic still applies to AlwaysRun modules
        if (context.Environment.IsDevelopment())
        {
            return SkipDecision.Skip("No cleanup in dev").AsTask();
        }
        return SkipDecision.DoNotSkip.AsTask();
    }
}
```

**Result**: Both behaviors apply independently:
- `IAlwaysRun`: Module won't be cancelled when other modules fail
- `ISkippable`: Module can still be skipped based on custom logic

### IIgnoreFailures + IRetryable

```csharp
public class ResilientModule : Module<string>, IRetryable<string>, IIgnoreFailures
{
    public AsyncRetryPolicy<string?> GetRetryPolicy(IPipelineContext context)
    {
        return Policy<string?>.Handle<Exception>().RetryAsync(3);
    }

    public Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception)
    {
        // This is only called AFTER all retries are exhausted
        return Task.FromResult(true);
    }
}
```

**Result**: Order of operations:
1. Attempt execution
2. On failure, retry up to 3 times
3. If all 4 attempts fail, check `ShouldIgnoreFailures`
4. If ignored, status = `IgnoredFailure`, pipeline continues

## Hook Execution Order Summary

For a module that implements all behaviors and runs successfully:

```
1. Wait for dependencies
2. Setup cancellation (check IAlwaysRun)
3. ISkippable.ShouldSkip() [if implemented]
4. OnBeforeExecuteAsync() [direct hook]
5. IHookable.OnBeforeExecute() [if implemented]
6. Start timing
7. Apply timeout + retry wrapper
8. ExecuteAsync() [may retry multiple times]
9. Stop timing
10. OnAfterExecuteAsync() [direct hook]
11. IHookable.OnAfterExecute() [in finally block]
```

For a module that fails:

```
1-7. Same as above
8. ExecuteAsync() fails after retries
9. OnFailedAsync() [direct hook]
10. IIgnoreFailures.ShouldIgnoreFailures() [if implemented]
11. If not ignored: Cancel pipeline
12. OnAfterExecuteAsync() [direct hook]
13. IHookable.OnAfterExecute() [in finally block]
```

For a module that is skipped:

```
1. Wait for dependencies
2. Setup cancellation
3. ISkippable.ShouldSkip() returns true
4. OnSkippedAsync() [direct hook]
5. Check result repository for historical data
6. Return skipped result
```

## Edge Cases

### OnBeforeExecuteAsync Throws Exception

If `OnBeforeExecuteAsync` throws:
- `ExecuteAsync` is NOT called
- `OnFailedAsync` is NOT called (module never started)
- `OnAfterExecuteAsync` is NOT called (before hooks didn't complete)
- `IHookable.OnAfterExecute` IS still called (in finally block)

### AlwaysRun Module with Failed Dependencies

When the pipeline fails and an AlwaysRun module hasn't started yet:
- The module is started without waiting for dependencies
- This prevents deadlocks where dependencies will never complete
- The module receives a fresh `CancellationToken.None`

### Timeout vs Cancellation

- **Timeout**: Module-specific limit on execution time
- **Cancellation**: Pipeline-wide signal when any module fails

AlwaysRun modules ignore pipeline cancellation but still respect their own timeout.

## Best Practices

1. **Use ISkippable for conditional execution**, not OnBeforeExecuteAsync
2. **Set reasonable timeouts** to prevent runaway modules
3. **Use IIgnoreFailures for non-critical modules** like notifications
4. **Combine IAlwaysRun with IIgnoreFailures** for cleanup modules
5. **Keep hooks lightweight** - heavy logic belongs in ExecuteAsync
6. **Log in OnFailedAsync** for debugging failed modules
7. **Consider total time when using IRetryable + ITimeoutable** together
