# Run conditions

Reusable run conditions implement `IRunCondition`:

Run conditions may be evaluated both during execution and by `PlanAsync()` or `--dry-run`. Keep them side-effect-free: they must not mutate external state or rely on being evaluated exactly once. Fluent conditions that await module results are reported as unknown in a dry-run plan because planning never executes dependencies to produce those results.

```
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

```
[RunIf<ServiceIsAvailable>]

public class DeployModule : Module
```

* `[RunIf<T>]` runs when its condition is `true`.
* `[SkipIf<T1, ..., T4>]` skips when any condition is `true`.
* `[RunIfAll<T1, T2, ..., T4>]` runs only when all two to four conditions are `true`.
* `[RunIfAny<T1, T2, ..., T4>]` runs when at least one condition is `true`.

When one required condition needs constructor state, derive an attribute from `RunIfAttribute`. Use `SkipIfAttribute`, `RunIfAllAttribute`, or `RunIfAnyAttribute` for the other condition logics:

```
public sealed class RunIfRegionAttribute(string region) : RunIfAttribute

{

    public override Task<bool> EvaluateAsync(IPipelineContext context) =>

        Task.FromResult(

            context.Environment.Variables.Get("REGION") == region);

}



[RunIfRegion("eu-west-2")]

public class RegionalDeployModule : Module<None>
```

The base class also provides a cancellation-aware overload. Override it when the condition performs cancellable asynchronous work.

Multiple condition attributes are evaluated in this order: `SkipIf`, `RunIf`/`RunIfAll`, then `RunIfAny`. Attribute conditions and fluent `.WithSkipWhen(...)` conditions run in the same execution pipeline after dependency waiting. Both invoke skipped hooks and lifecycle notifications.

Fluent dependencies are validated before execution conditions are evaluated. Every dependency declared with `DependsOn<T>()` must therefore be registered, even when an attribute condition will skip the consuming module on the current platform or environment.

Built-in conditions include `OnCI`, `OnLocal`, `OnLinux`, `OnWindows`, `OnMacOS`, `OnFreeBSD`, and `OnUnix`:

```
[RunIf<OnLinux>]

public class LinuxModule : Module
```

Parameterized built-ins cover environment variables. Combine platform conditions when a module can run on alternative operating systems:

```
[RunIfEnvironmentVariable("NUGET_API_KEY")]

[SkipIfEnvironmentVariable("CI", "true")]

[RunIfAny<OnLinux, OnMacOS>]

public class PublishModule : Module<None>
```

Use `RunIfEnvironmentVariableUnset` or `SkipIfEnvironmentVariableUnset` for the inverse environment-variable check. The `ModularPipelines.Git` package also provides `RunIfBranch`, `RunIfBranchStartsWith`, and `SkipIfBranch`; these stateful attributes use the same base classes.

One-off conditions can use `Configure(ModuleConfigurationBuilder).WithSkipWhen(...)`.
