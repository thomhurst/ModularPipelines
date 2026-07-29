---
title: Skipping Modules
sidebar_position: 7
---

# Skipping Modules

## Using ModuleConfiguration

The recommended way to configure module skipping is through the `Configure()` method with the fluent builder API:

Attribute conditions (`[SkipIf<T>]`, `[RunIfAll<T>]`, and `[RunIfAny<T>]`) remain supported,
but they run during module discovery, before dependency waiting. An ignored module receives a
skipped result directly. Fluent `.WithSkipWhen(...)` conditions run in the execution pipeline,
where skipped hooks and lifecycle notifications are invoked. Use the fluent API when consumers
depend on those notifications.

### Simple Condition

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(_ => Environment.GetEnvironmentVariable("SKIP_MODULE") == "true"
            ? SkipDecision.Skip("SKIP_MODULE is true")
            : SkipDecision.DoNotSkip)
        .Build();

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

Repeated skip conditions use AND semantics. They run in registration order and stop as soon as
one returns `SkipDecision.DoNotSkip`. When every condition skips, their reasons are combined:

```csharp
public class CleanupModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(_ => Environment.GetEnvironmentVariable("SKIP_CLEANUP") == "true"
            ? SkipDecision.Skip("SKIP_CLEANUP is true")
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

## History
If a module was skipped, you can attempt to find its history from a previous run. See [History](storing-and-retrieving-results)

## Run Conditions

See [Run Conditions](run-conditions)

## Categories

See [Categories](categories)
