---
title: Run conditions
sidebar_position: 5
---

# Run conditions

A run condition is an `IRunCondition`: one asynchronous predicate that receives the pipeline context and
a cancellation token.

Run conditions may be evaluated both during execution and by `PlanAsync()` or `--dry-run`.
Keep them side-effect-free: they must not mutate external state or rely on being evaluated
exactly once.
Fluent conditions that await module results are reported as unknown in a dry-run plan because
planning never executes dependencies to produce those results.

```csharp
public class ServiceIsAvailable : IRunCondition
{
    public async Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var response = await context.Http.SendAsync(
            new HttpOptions(new HttpRequestMessage(HttpMethod.Get, "https://www.example.com/ping")),
            cancellationToken);
        return response.StatusCode == HttpStatusCode.OK;
    }
}
```

Pass the token to asynchronous work: it is canceled when the pipeline stops evaluating conditions.

## Applying conditions

Apply the condition with an attribute that states its intent:

```csharp
[RunIf<ServiceIsAvailable>]
public class DeployModule : Module<None>
```

- `[RunIf<T1, ..., T4>]` runs only when all of its one to four conditions are `true`.
- `[RunIfAny<T1, T2, ..., T4>]` runs when at least one condition is `true`.
- `[SkipIf<T1, ..., T4>]` skips when any condition is `true`.

Or register the condition in `Configure`, with a type or an instance:

```csharp
protected override void Configure(ModuleConfigurationBuilder module) => module
    .WithRunIf<ServiceIsAvailable>()
    .WithSkipIf(new IsDependabot());
```

`WithRunIf` and `WithSkipIf` compose with `.WithSkipWhen(...)` using OR-to-skip semantics: the module is
skipped when any configured condition says so.

## Reusable groups

Combine conditions once with `ConditionGroup`. `Logic` is `ConditionLogic.All` (AND) or `ConditionLogic.Any` (OR):

```csharp
public sealed class OnUnixPlatforms : ConditionGroup, IPlanningSafe
{
    public override IReadOnlyList<IRunCondition> Conditions => [new OnLinux(), new OnMacOS()];

    public override ConditionLogic Logic => ConditionLogic.Any;
}
```

## Stateful condition attributes

When a condition needs constructor state, derive an attribute from `RunConditionAttribute`. The base
constructor takes the intent: `ConditionIntent.Run` runs the module only when the condition is satisfied,
and `ConditionIntent.Skip` skips it when the condition is satisfied.

```csharp
public sealed class RunIfRegionAttribute(string region)
    : RunConditionAttribute(ConditionIntent.Run), IPlanningSafe
{
    public override string ConditionNames => $"RunIfRegion({region})";

    public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        Task.FromResult(context.Environment.Variables.Get("REGION") == region);
}

[RunIfRegion("eu-west-2")]
public class RegionalDeployModule : Module<None>
```

Each run attribute is a requirement of its own. To make several run attributes alternatives, give them the
same `GroupKey`: the module runs when any attribute in the group is satisfied, and every group must still hold.

```csharp
public sealed class RunOnBranchAttribute(string branch) : RunConditionAttribute(ConditionIntent.Run)
{
    public override Type? GroupKey => typeof(RunOnBranchAttribute);

    public override async Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var info = await context.Tools.Git.Information.GetInfoAsync(cancellationToken);
        return info?.BranchName == branch;
    }
}

// Runs on main or on release.
[RunOnBranch("main")]
[RunOnBranch("release")]
public class ReleaseModule : Module<None>
```

The group key is ignored for skip attributes, because any satisfied skip attribute already skips the module.

## Evaluation order

Skip attributes are evaluated first, then required run attributes, then alternative run attributes
(`RunIfAny` and grouped attributes). Attribute conditions and fluent conditions run in the same
execution pipeline after dependency waiting. Both invoke skipped hooks and lifecycle notifications.

Fluent dependencies are validated before execution conditions are evaluated. Every dependency
declared with `DependsOn<T>()` must therefore be registered, even when an attribute condition
will skip the consuming module on the current platform or environment.

## Planning-safe conditions

`PlanAsync()`, `--dry-run`, and dependency-graph export evaluate a condition only when it is marked with
`IPlanningSafe`. Implement the marker on conditions, condition attributes, and condition groups that are
free of side effects, blocking work, and remote I/O. Other conditions stay unresolved until the pipeline
runs. The same marker applies to dependency selectors derived from `DependsOnBaseAttribute` and to module
registration handlers.

## Built-in conditions

Built-in conditions include `OnCI`, `OnLocal`, `OnLinux`, `OnWindows`, `OnMacOS`, `OnFreeBSD`, and `OnUnix`.
`OnCI` and `OnLocal` use `context.Environment.BuildSystem.IsBuildServer`: a detected build system
(GitHub Actions, Azure Pipelines, TeamCity, and others) counts as CI, and otherwise a `CI`
environment variable set to anything other than `false` or `0` does.
All built-in conditions are planning-safe:

```csharp
[RunIf<OnLinux>]
public class LinuxModule : Module<None>
```

Parameterized built-ins cover environment variables. Combine platform conditions when a module
can run on alternative operating systems:

```csharp
[RunIfEnvironmentVariable("NUGET_API_KEY")]
[SkipIfEnvironmentVariable("CI", "true")]
[RunIfAny<OnLinux, OnMacOS>]
public class PublishModule : Module<None>
```

Use `RunIfEnvironmentVariableUnset` or `SkipIfEnvironmentVariableUnset` for the inverse
environment-variable check. The `ModularPipelines.Git` package also provides
`RunIfBranch`, `RunIfBranchStartsWith`, `RunIfChanged`, and `SkipIfBranch`; these stateful attributes
derive from `RunConditionAttribute`, and `RunIfBranch` and `RunIfBranchStartsWith` share a group key.

One-off conditions can use `Configure(ModuleConfigurationBuilder).WithSkipWhen(...)`.
