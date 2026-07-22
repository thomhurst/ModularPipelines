---
title: Run conditions
sidebar_position: 5
---

# Run conditions

Reusable run conditions implement `IRunCondition`:

```csharp
public class ServiceIsAvailable : IRunCondition
{
    public async Task<bool> EvaluateAsync(IPipelineHookContext context)
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
`RunIfAny`. Attribute conditions run during module discovery and register a skipped result
directly. Fluent `.WithSkipWhen(...)` conditions run later in the execution pipeline and invoke
the skipped hooks and lifecycle notifications.

Built-in platform attributes remain available, including `[RunOnLinux]`, `[RunOnWindows]`,
`[RunOnMacOS]`, and their `*Only` variants.

## Legacy attributes

`RunConditionAttribute` and `MandatoryRunConditionAttribute` are deprecated. Existing modules
continue to work, but new reusable conditions should implement `IRunCondition` and use the
intent-specific attributes above. One-off conditions should use
`Configure().WithSkipWhen(...)`.
