---
title: Skipping Modules
sidebar_position: 7
---

# Skipping Modules

## Using ModuleConfiguration

The recommended way to configure module skipping is through the `Configure()` method with the fluent builder API:

Attribute conditions (`[SkipIf<T>]`, `[RunIfAll<T>]`, and `[RunIfAny<T>]`) remain supported.
Attribute and fluent conditions run in the same execution pipeline after dependency waiting, so
both invoke skipped hooks and lifecycle notifications.

`PlanAsync()` and `--dry-run` also evaluate these conditions before execution. Conditions must be
side-effect-free and must not rely on being evaluated exactly once.
If a fluent condition reads an awaited module result, that result does not exist during planning;
the plan reports the module's skip decision as unknown and continues without executing modules.

### Simple Condition

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(_ => Environment.GetEnvironmentVariable("SKIP_MODULE") == "true"
            ? SkipDecision.Skip("SKIP_MODULE is true")
            : SkipDecision.DoNotSkip)
        .Build();

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Module logic here
    }
}
```

### Using Pipeline Context

When you need access to the pipeline context for your skip condition:

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(async (ctx, _) =>
            (await ctx.Git().Information.GetInfoAsync())?.BranchName != "main"
                ? SkipDecision.Skip("This should only run on the main branch")
                : SkipDecision.DoNotSkip)
        .Build();

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // This only runs on the main branch
    }
}
```

### With Skip Reason

For better reporting, you can return a `SkipDecision` with a reason:

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(async (ctx, _) =>
        {
            var repositoryInfo = await ctx.Git().Information.GetInfoAsync();
            if (repositoryInfo?.BranchName == "main")
            {
                return SkipDecision.DoNotSkip;
            }
            return SkipDecision.Skip("This should only run on the main branch");
        })
        .Build();

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Module logic here
    }
}
```

### Async Skip Conditions

For conditions that require async operations:

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(async (_, cancellationToken) =>
        {
            var response = await HttpClient.GetAsync(
                "https://api.example.com/should-run",
                cancellationToken);
            return response.IsSuccessStatusCode
                ? SkipDecision.DoNotSkip
                : SkipDecision.Skip("The remote service is unavailable");
        })
        .Build();
}
```

## Combining with Other Behaviors

Repeated `WithSkipWhen` conditions use OR-to-skip semantics, matching repeated `[SkipIf<T>]`
attributes. They run in registration order, and evaluation stops when any condition returns
`SkipDecision.Skip`.

For example, this module skips cleanup for either CI builds or non-main branches:

```csharp
public class CleanupModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(_ => Environment.GetEnvironmentVariable("CI") == "true"
            ? SkipDecision.Skip("Running in CI")
            : SkipDecision.DoNotSkip)
        .WithSkipWhen(async (ctx, _) =>
            (await ctx.Git().Information.GetInfoAsync())?.BranchName != "main"
                ? SkipDecision.Skip("Not on the main branch")
                : SkipDecision.DoNotSkip)
        .WithAlwaysRun()  // Run even if dependencies fail (when not skipped)
        .WithTimeout(TimeSpan.FromMinutes(5))
        .Build();
}
```

When every condition must match before the module is skipped, group them explicitly with
`WithSkipWhenAll`:

```csharp
protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
    .WithSkipWhenAll(
        _ => Environment.GetEnvironmentVariable("CI") == "true"
            ? SkipDecision.Skip("Running in CI")
            : SkipDecision.DoNotSkip,
        _ => Environment.GetEnvironmentVariable("DEPLOY_ENV") != "production"
            ? SkipDecision.Skip("Not deploying to production")
            : SkipDecision.DoNotSkip)
    .Build();
```

Conditions inside a `WithSkipWhenAll` group use AND-to-skip semantics and combine their reasons.
The group composes with other skip conditions using OR-to-skip semantics.

## History
If a module was skipped, you can attempt to find its history from a previous run. See [History](storing-and-retrieving-results)

## Run Conditions

See [Run Conditions](run-conditions)

## Categories

See [Categories](categories)
