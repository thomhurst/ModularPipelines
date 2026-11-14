---
title: Conditions and requirements
sidebar_position: 6
---

# Run conditions and requirements

## Run condition attributes

| V3 pattern | V4 replacement |
| --- | --- |
| `[RunIfAll<T1, ..., T4>]` | `[RunIf<T1, ..., T4>]` (all conditions must be true) |
| `[RunIfAny<T>]` with one condition | `[RunIf<T>]`; `[RunIfAny<T1, T2, ...>]` still takes two to four conditions |
| `[SkipIf<T>]` / `[SkipIf<T1, T2>]` | Unchanged: skips when any condition is true; V4 also accepts three or four conditions |
| `IsCI` / `IsLocal` condition classes | `OnCI` / `OnLocal` |
| `[RunOnLinuxOnly]` and similar single-platform conditions | `[RunIf<OnLinux>]`, `[RunIf<OnWindows>]`, or `[RunIf<OnMacOS>]` |
| Multiple alternative `RunOn*` attributes | One `[RunIfAny<OnLinux, OnMacOS>]`, preserving the original OR intent |

Fluent registration also accepts condition types and instances: `.WithRunIf<T>()`,
`.WithRunIf(IRunCondition)`, `.WithSkipIf<T>()`, and `.WithSkipIf(IRunCondition)`.

Keep conditions free of external side effects, because planning can evaluate them. See
[Run conditions](../how-to/run-conditions.md) and [Skipping](../how-to/skipping.md).

### How conditions combine

:::warning AND/OR grouping changed
In V3, all non-mandatory run-condition attributes on a module formed one OR set, whatever their
type. In V4, only attributes with the same `GroupKey` are alternatives. Separate groups must
**all** pass.
:::

For example, a module with `[RunIfBranch("main")]` and an OS attribute ran in V3 when *either*
matched. In V4 it needs *both*. To keep the OR, express the alternatives as `IRunCondition` types
in one `[RunIfAny<...>]`, or as one custom `RunConditionAttribute`.

### Git branch conditions

Import `ModularPipelines.Git` for these attributes. See [Git](./integrations.md#git).

| Attributes | Behavior |
| --- | --- |
| `RunIfBranch`, `RunIfBranchStartsWith` | OR alternatives that share one group. Other attributes cannot join that group |
| `RunOnlyOnBranch`, `RunOnlyIfBranchStartsWith` | Independent AND requirements. Use them when a branch restriction must apply even if another run alternative matches |

Branch prefixes now use case-sensitive ordinal matching instead of culture-sensitive matching.

### Custom conditions

| V3 pattern | V4 replacement |
| --- | --- |
| `MandatoryRunConditionAttribute` subclass | `RunConditionAttribute` subclass calling `base(ConditionIntent.Run)` |
| Non-mandatory `RunConditionAttribute` subclass (OR-ed alternatives) | `RunConditionAttribute` subclass with `ConditionIntent.Run` that overrides `GroupKey` with a shared key |
| `RunConditionAttribute.Condition(IPipelineHookContext)` | `EvaluateAsync(IPipelineContext, CancellationToken)` |
| Custom `IConditionAttribute` implementations, `ConditionLogic.Skip` | `RunConditionAttribute` subclass; use `ConditionIntent.Skip` for skip semantics |
| `IRunCondition.EvaluateAsync(IPipelineHookContext)` | `EvaluateAsync(IPipelineContext, CancellationToken)` |
| `ConditionGroup.Conditions` (`IRunCondition[]`), `ConditionGroup.EvaluateAsync(context)` | `IReadOnlyList<IRunCondition>`; `EvaluateAsync(context, cancellationToken)` |
| Subclasses of Git/GitHub condition attributes (`RunOnlyOnBranch`, `SkipIfBranch`, `SkipIfDependabot`, ...) | These now derive from `RunConditionAttribute`; override `EvaluateAsync` instead of `Condition` |

## Skip decisions

| V3 pattern | V4 replacement |
| --- | --- |
| `SkipDecision.Of(condition, reason)` | `SkipDecision.When(condition, reason)` |
| Implicit bool/string/task skip decisions | Explicit `SkipDecision.Skip(reason)`, `.DoNotSkip`, or `.When(...)` |
| `value.AsSkipDecisionIfTrue(reason)` | `SkipDecision.When(condition, reason)` |

## `WithSkipWhen`

**Every callback now takes the context.** The overloads without a context parameter are removed.

```csharp
// V3
.WithSkipWhen(() => !publishEnabled)

// V4: true still means skip.
.WithSkipWhen(_ => !publishEnabled)
.WithSkipWhen(_ => !publishEnabled, "Publishing is disabled") // With a reason.
```

A callback that returns `bool` can omit the reason. The module then reports "Skip condition was
met". Add a reason when you want the summary to explain the skip.

**Asynchronous callbacks also take a cancellation token** and return `ValueTask`. An `async` lambda
targets the new overload directly:

```csharp
.WithSkipWhen(async (context, cancellationToken) =>
    await CheckAsync(context, cancellationToken)) // CheckAsync returns Task<SkipDecision>.
```

For an existing check with no cancellation parameter, adapt it with
`async (context, _) => await CheckAsync(context)`. Add cancellation support when practical.

Asynchronous `WithIgnoreFailuresWhen` callbacks change the same way. They take
`(context, exception, cancellationToken)` and return `ValueTask<bool>`. The synchronous
`(context, exception)` overload is unchanged.

:::warning Repeated `WithSkipWhen` calls now combine with OR
The module skips when **any** condition is true. In V3, the last call replaced the earlier ones.

- Remove earlier calls that V3 silently ignored.
- Use `WithSkipWhenAll` when every condition must be true.

Analyzer [`MP0020`](../analyzers/MP0020.md) (Warning) flags the second and later `WithSkipWhen`
calls on the same builder. Projects that treat warnings as errors can fail their build on it.
:::

## Requirements

Replace `MustAsync` and synchronous `Must` overrides with the cancellation-aware `EvaluateAsync`:

```csharp
public sealed class TokenRequirement : PipelineRequirement
{
    public override Task<RequirementDecision> EvaluateAsync(
        IPipelineContext context, CancellationToken cancellationToken)
        => Task.FromResult(
            string.IsNullOrEmpty(context.Environment.Variables.Get("NUGET_API_KEY"))
                ? RequirementDecision.Failed("NUGET_API_KEY is required")
                : RequirementDecision.Passed);
}
```

`PipelineRequirement.EvaluateAsync` is abstract, so every subclass must override it.

| V3 | V4 |
| --- | --- |
| `MustAsync` / `Must` overrides | `EvaluateAsync(IPipelineContext, CancellationToken)` |
| `RequirementDecision.Of`, implicit string/task conversions | Removed. Use `RequirementDecision.Passed` / `.Failed(reason)`. The bool conversion remains |
| `decision.Success` | `decision.IsSatisfied` |
| `Require.CIEnvironment(...)` | `Require.Ci(...)` |
| `Require.ThatAsync(async context => ...)` | `Require.ThatAsync(async (context, cancellationToken) => ...)`; forward the token into the check |
| Registering `WindowsRequirement` and the other built-in classes | Factories: `builder.AddRequirement(Require.Windows())`, `Require.Linux()`, `Require.MacOS()`, `Require.WindowsAdmin()` |
| `DelegateRequirement` | Internal. Keep creating delegate requirements with `Require.That(...)` or `Require.ThatAsync(...)`, and type them as `IPipelineRequirement` |
| `FailedRequirementsException` | `RequirementNotMetException`; unmet requirements are reported together in one exception |

The protected `Pass()`, `Fail(reason)`, and `When(condition, failureReason)` helpers are still
available in `PipelineRequirement` subclasses. Keep existing calls to them. `RequirementDecision.Passed`
/ `.Failed(reason)` are alternatives, and are what classes that implement `IPipelineRequirement`
directly use.

See [Requirements](../how-to/requirements.md).
