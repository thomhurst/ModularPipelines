---
title: Run conditions
sidebar_position: 5
---

# Run conditions

Reusable run conditions implement `IRunCondition`:

Run conditions may be evaluated both during execution and by `PlanAsync()` or `--dry-run`.
Keep them side-effect-free: they must not mutate external state or rely on being evaluated
exactly once.

```csharp
public class ServiceIsAvailable : IRunCondition
{
    public async Task<bool> EvaluateAsync(IPipelineContext context)
    {
        var response = await context.Http.SendAsync("https://www.example.com/ping");
        return response.StatusCode == HttpStatusCode.OK;
    }
}
```

Apply the condition with an attribute that states its intent:

```csharp
[RunIfAll<ServiceIsAvailable>]
public class DeployModule : Module<None>
```

- `[SkipIf<T>]` skips when the condition is `true`.
- `[RunIfAll<T1, T2>]` runs only when every condition is `true`.
- `[RunIfAny<T1, T2>]` runs when at least one condition is `true`.

Multiple condition attributes are evaluated in this order: `SkipIf`, `RunIfAll`, then
`RunIfAny`. Attribute conditions and fluent `.WithSkipWhen(...)` conditions run in the same
execution pipeline after dependency waiting. Both invoke skipped hooks and lifecycle
notifications.

Fluent dependencies are validated before execution conditions are evaluated. Every dependency
declared with `DependsOn<T>()` must therefore be registered, even when an attribute condition
will skip the consuming module on the current platform or environment.

Built-in platform conditions include `OnLinux`, `OnWindows`, and `OnMacOS`:

```csharp
[RunIfAll<OnLinux>]
public class LinuxModule : Module<None>
```

One-off conditions can use `Configure().WithSkipWhen(...)`.
