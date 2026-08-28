# Always Run

## Module Run Types[​](#module-run-types "Direct link to Module Run Types")

By default, modules only run if their dependencies succeed. If a dependency fails, the module waiting will not start and will abort.

With `WithAlwaysRun()`, a module will run regardless of whether any dependencies failed. This is useful for cleanup modules that need to run regardless of whether the pipeline passed or failed.

## Using ModuleConfiguration[​](#using-moduleconfiguration "Direct link to Using ModuleConfiguration")

```
public class CleanupModule : Module<CommandResult>

{

    protected override void Configure(ModuleConfigurationBuilder module) => module

        .WithAlwaysRun();



    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)

    {

        // Clean up resources - runs even if other modules failed

    }

}
```

## Combining with Other Behaviors[​](#combining-with-other-behaviors "Direct link to Combining with Other Behaviors")

Always run can be combined with other module behaviors:

```
[DependsOn<BuildModule>]

[DependsOn<TestModule>]

public class CleanupModule : Module<CommandResult>

{

    protected override void Configure(ModuleConfigurationBuilder module) => module

        .WithAlwaysRun()

        .WithIgnoreFailures()  // Don't fail the pipeline if cleanup fails

        .WithTimeout(TimeSpan.FromMinutes(5));



    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)

    {

        // Clean up resources - runs even if Build or Test failed

        // Won't fail the pipeline even if cleanup itself fails

    }

}
```

## Use Cases[​](#use-cases "Direct link to Use Cases")

Common scenarios for `WithAlwaysRun()`:

* **Resource cleanup**: Deleting temporary files, stopping services
* **Notifications**: Sending pipeline status notifications
* **Logging**: Final summary logging regardless of pipeline outcome
* **State reset**: Resetting environment state after tests
