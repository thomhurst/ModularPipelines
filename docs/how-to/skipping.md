# Skipping Modules

## Using ModuleConfiguration[​](#using-moduleconfiguration "Direct link to Using ModuleConfiguration")

The recommended way to configure module skipping is through the `Configure(ModuleConfigurationBuilder)` method with the fluent builder API:

Attribute conditions (`[SkipIf<T1, ..., T4>]`, `[RunIf<T1, ..., T4>]`, and `[RunIfAny<T1, ..., T4>]`) and the equivalent `.WithRunIf<T>()` and `.WithSkipIf<T>()` builder methods remain supported; see [Run conditions](/ModularPipelines/docs/how-to/run-conditions.md). Attribute and fluent conditions run in the same execution pipeline after dependency waiting, so both invoke skipped hooks and lifecycle notifications.

`PlanAsync()` and `--dry-run` can invoke `WithSkipWhen` and `WithSkipWhenAll` delegates without an `IPlanningSafe` marker, including asynchronous delegates. Typed conditions registered with attributes, `WithRunIf`, or `WithSkipIf` require that marker to run during planning. Module selection, earlier skip decisions, and short-circuiting can prevent evaluation; not every delegate runs on every dry-run. Conditions must be side-effect-free and must not rely on being evaluated exactly once. If a fluent condition reads an awaited module result, that result does not exist during planning; the condition remains unresolved without executing modules. The plan reports an unknown skip decision unless another condition determines the outcome. Any delegate work before the result access can still run, including remote I/O. Use unmarked typed conditions for checks that must wait until execution.

### Simple Condition[​](#simple-condition "Direct link to Simple Condition")

```
public class MyModule : Module<CommandResult>

{

    protected override void Configure(ModuleConfigurationBuilder module) => module

        .WithSkipWhen(

            _ => Environment.GetEnvironmentVariable("SKIP_MODULE") == "true",

            "SKIP_MODULE is true");



    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)

    {

        // Module logic here

    }

}
```

### Using Pipeline Context[​](#using-pipeline-context "Direct link to Using Pipeline Context")

Delegates can read local values from the pipeline context:

```
public class MyModule : Module<CommandResult>

{

    protected override void Configure(ModuleConfigurationBuilder module) => module

        .WithSkipWhen(

            ctx => ctx.Environment.Variables.Get("DEPLOY_ENV") != "production",

            "This should only run in production");



    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)

    {

        // This only runs in production

    }

}
```

### With Skip Reason[​](#with-skip-reason "Direct link to With Skip Reason")

The reason is optional. When it is omitted, a skipped module reports `Skip condition was met`:

```
protected override void Configure(ModuleConfigurationBuilder module) => module

    .WithSkipWhen(_ => Environment.GetEnvironmentVariable("SKIP_DEPLOY") == "true");
```

For better reporting, pass the reason alongside the boolean condition:

```
public class MyModule : Module<CommandResult>

{

    protected override void Configure(ModuleConfigurationBuilder module) => module

        .WithSkipWhen(

            ctx => ctx.Environment.Variables.Get("DEPLOY_ENV") != "production",

            "This should only run in production");



    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)

    {

        // Module logic here

    }

}
```

### Async Skip Conditions[​](#async-skip-conditions "Direct link to Async Skip Conditions")

For an execution-only remote check, implement `IRunCondition` without `IPlanningSafe` and register it with `WithRunIf` (or `WithSkipIf` for the inverse predicate). Planning leaves this check unresolved and does not send the HTTP request. Reuse an `HttpClient` instance and dispose of each response:

```
using System.Net.Http;

using ModularPipelines;



public sealed class RemoteServiceIsAvailable : IRunCondition

{

    private static readonly HttpClient Client = new();



    public async Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)

    {

        using var response = await Client.GetAsync(

            "https://api.example.com/should-run",

            cancellationToken);

        return response.IsSuccessStatusCode;

    }

}



public class MyModule : Module

{

    protected override void Configure(ModuleConfigurationBuilder module) => module

        .WithRunIf<RemoteServiceIsAvailable>();



    protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)

    {

        // Perform the work that requires the remote service here.

        return Task.CompletedTask;

    }

}
```

## Combining with Other Behaviors[​](#combining-with-other-behaviors "Direct link to Combining with Other Behaviors")

Repeated `WithSkipWhen` conditions use OR-to-skip semantics, matching repeated `[SkipIf<T>]` attributes. They run in registration order, and evaluation stops when any condition returns `SkipDecision.Skip`.

For example, this module skips cleanup for either CI builds or non-main branches. The Git package's `RunIfBranch` attribute is not planning-safe, so its Git lookup waits until execution:

```
using ModularPipelines.Git.Attributes;



[RunIfBranch("main")]

public class CleanupModule : Module<CommandResult>

{

    protected override void Configure(ModuleConfigurationBuilder module) => module

        .WithSkipWhen(_ => Environment.GetEnvironmentVariable("CI") == "true", "Running in CI")

        .WithAlwaysRun()  // Run even if dependencies fail (when not skipped)

        .WithTimeout(TimeSpan.FromMinutes(5));

}
```

When every condition must match before the module is skipped, group them explicitly with `WithSkipWhenAll`:

```
protected override void Configure(ModuleConfigurationBuilder module) => module

    .WithSkipWhenAll(

        _ => Environment.GetEnvironmentVariable("CI") == "true"

            ? SkipDecision.Skip("Running in CI")

            : SkipDecision.DoNotSkip,

        _ => Environment.GetEnvironmentVariable("DEPLOY_ENV") != "production"

            ? SkipDecision.Skip("Not deploying to production")

            : SkipDecision.DoNotSkip);
```

Conditions inside a `WithSkipWhenAll` group use AND-to-skip semantics and combine their reasons. The group composes with other skip conditions using OR-to-skip semantics.

In V3, a later `WithSkipWhen` call replaced an earlier one. Analyzer [`MP0020`](/ModularPipelines/docs/analyzers/MP0020.md) reports repeated `WithSkipWhen` calls on the same builder as a suggestion; suppress it when OR-to-skip is intended, or switch to `WithSkipWhenAll` when every condition must match.

## History[​](#history "Direct link to History")

If a module was skipped, you can attempt to find its history from a previous run. See [History](/ModularPipelines/docs/how-to/storing-and-retrieving-results.md)

## Run Conditions[​](#run-conditions "Direct link to Run Conditions")

See [Run Conditions](/ModularPipelines/docs/how-to/run-conditions.md)

## Categories[​](#categories "Direct link to Categories")

See [Categories](/ModularPipelines/docs/how-to/categories.md)
