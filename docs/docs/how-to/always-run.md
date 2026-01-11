---
title: Always Run
---

# Always Run

## Module Run Types

By default, modules only run if their dependencies succeed. If a dependency fails, the module waiting will not start and will abort.

With `WithAlwaysRun()`, a module will run regardless of whether any dependencies failed. This is useful for cleanup modules that need to run regardless of whether the pipeline passed or failed.

## Using ModuleConfiguration

```csharp
public class CleanupModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithAlwaysRun()
        .Build();

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Clean up resources - runs even if other modules failed
    }
}
```

## Combining with Other Behaviors

Always run can be combined with other module behaviors:

```csharp
[DependsOn<BuildModule>]
[DependsOn<TestModule>]
public class CleanupModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithAlwaysRun()
        .WithIgnoreFailures()  // Don't fail the pipeline if cleanup fails
        .WithTimeout(TimeSpan.FromMinutes(5))
        .Build();

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Clean up resources - runs even if Build or Test failed
        // Won't fail the pipeline even if cleanup itself fails
    }
}
```

## Use Cases

Common scenarios for `WithAlwaysRun()`:
- **Resource cleanup**: Deleting temporary files, stopping services
- **Notifications**: Sending pipeline status notifications
- **Logging**: Final summary logging regardless of pipeline outcome
- **State reset**: Resetting environment state after tests
