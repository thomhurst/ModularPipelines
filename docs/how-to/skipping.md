# Skipping Modules

## Using ModuleConfiguration[​](#using-moduleconfiguration "Direct link to Using ModuleConfiguration")

The recommended way to configure module skipping is through the `Configure()` method with the fluent builder API:

Attribute conditions (`[SkipIf<T>]`, `[RunIfAll<T>]`, and `[RunIfAny<T>]`) remain supported, but they run during module discovery, before dependency waiting. An ignored module receives a skipped result directly. Fluent `.WithSkipWhen(...)` conditions run in the execution pipeline, where skipped hooks and lifecycle notifications are invoked. Use the fluent API when consumers depend on those notifications.

### Simple Boolean Condition[​](#simple-boolean-condition "Direct link to Simple Boolean Condition")

```
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

### Using Pipeline Context[​](#using-pipeline-context "Direct link to Using Pipeline Context")

When you need access to the pipeline context for your skip condition:

```
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

### With Skip Reason[​](#with-skip-reason "Direct link to With Skip Reason")

For better reporting, you can return a `SkipDecision` with a reason:

```
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

### Async Skip Conditions[​](#async-skip-conditions "Direct link to Async Skip Conditions")

For conditions that require async operations:

```
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

## Combining with Other Behaviors[​](#combining-with-other-behaviors "Direct link to Combining with Other Behaviors")

You can combine skip conditions with other module behaviors:

```
public class CleanupModule : Module<CommandResult>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithSkipWhen(ctx => ctx.Git().Information.BranchName != "main")

        .WithAlwaysRun()  // Run even if dependencies fail (when not skipped)

        .WithTimeout(TimeSpan.FromMinutes(5))

        .Build();

}
```

## History[​](#history "Direct link to History")

If a module was skipped, you can attempt to find its history from a previous run. See [History](/ModularPipelines/docs/how-to/storing-and-retrieving-results.md)

## Run Conditions[​](#run-conditions "Direct link to Run Conditions")

See [Run Conditions](/ModularPipelines/docs/how-to/run-conditions.md)

## Categories[​](#categories "Direct link to Categories")

See [Categories](/ModularPipelines/docs/how-to/categories.md)
