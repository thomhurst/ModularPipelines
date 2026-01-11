---
title: Skipping Modules
sidebar_position: 7
---

# Skipping Modules

## Using ModuleConfiguration

The recommended way to configure module skipping is through the `Configure()` method with the fluent builder API:

### Simple Boolean Condition

```csharp
public class MyModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(() => Environment.GetEnvironmentVariable("SKIP_MODULE") == "true")
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
        .WithSkipWhen(ctx => ctx.Git().Information.BranchName != "main")
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
        .WithSkipWhen(ctx =>
        {
            if (ctx.Git().Information.BranchName == "main")
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
        .WithSkipWhen(async () =>
        {
            var response = await HttpClient.GetAsync("https://api.example.com/should-run");
            return !response.IsSuccessStatusCode;
        })
        .Build();
}
```

## Combining with Other Behaviors

You can combine skip conditions with other module behaviors:

```csharp
public class CleanupModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(ctx => ctx.Git().Information.BranchName != "main")
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
