# Defining Modules

## Defining Modules[​](#defining-modules "Direct link to Defining Modules")

Modules are defined by creating a class that inherits from the `Module<T>` base class.

`T` is the type of object that your Module will return, and that object can be seen by other Modules (if they depend on it).

```
public class FindAFileModule : Module<FileInfo>

{

    protected override async Task<FileInfo> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        return context.Files

            .Glob("C:\\**\\MyJsonFile.json")

            .Single();

    }

}
```

### Modules Without Return Values[​](#modules-without-return-values "Direct link to Modules Without Return Values")

For modules that perform actions without returning meaningful data, use the non-generic `Module`:

```
public class CleanupModule : Module

{

    protected override Task ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        var folder = context.Files.GetFolder("./temp");

        folder.Delete();

        return Task.CompletedTask;

    }

}
```

For synchronous operations, `SyncModule<None>` remains available:

```
public class LoggingModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context, CancellationToken cancellationToken)

    {

        context.Logger.LogInformation("Pipeline executed at {Time}", DateTime.UtcNow);

        return None.Value;

    }

}
```

The pipeline represents a non-generic module's successful result internally with `None.Value`. You only need to return that sentinel yourself when using `SyncModule<None>`.

## Configuring Module Behavior[​](#configuring-module-behavior "Direct link to Configuring Module Behavior")

Configure module behaviors such as timeouts, retry policies, skip conditions, and hooks by overriding the `Configure()` method:

```
public class MyModule : Module<FileInfo>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithTimeout(TimeSpan.FromMinutes(5))

        .WithRetry(3)

        .WithSkipWhen(_ => !File.Exists("important.json")

            ? SkipDecision.Skip("important.json does not exist")

            : SkipDecision.DoNotSkip)

        .WithPriority(ModulePriority.High)

        .WithExecutionHint(ExecutionType.IoIntensive)

        .WithTags("build", "critical")

        .WithCategory("build")

        .DependsOn<RestoreModule>()

        .WithIgnoreFailures()

        .WithAlwaysRun()

        .Build();



    protected override async Task<FileInfo> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        // Module logic here

    }

}
```

### Available Configuration Options[​](#available-configuration-options "Direct link to Available Configuration Options")

| Method                                               | Description                                                         |
| ---------------------------------------------------- | ------------------------------------------------------------------- |
| `.WithTimeout(TimeSpan)`                             | Maximum execution time before module is cancelled                   |
| `.WithRetry(int, TimeSpan?, Func<Exception, bool>?)` | Retry attempts, jittered base delay, and optional exception filter  |
| `.Advanced.WithRetryPolicy(IAsyncPolicy)`            | Custom Polly policy for advanced scenarios                          |
| `.WithSkipWhen(...)`                                 | Condition to skip the module                                        |
| `.WithIgnoreFailures()`                              | Don't fail the pipeline if this module fails                        |
| `.WithIgnoreFailuresWhen(...)`                       | Conditionally ignore failures                                       |
| `.WithAlwaysRun()`                                   | Run even if the pipeline has failed                                 |
| `.WithNotInParallel(...)`                            | Prevent parallel execution globally or for matching constraint keys |
| `.WithPriority(ModulePriority)`                      | Set scheduler priority                                              |
| `.WithExecutionHint(ExecutionType)`                  | Select CPU, I/O, or default concurrency limits                      |
| `.WithTags(...)`                                     | Add tags used by metadata-based dependencies                        |
| `.WithCategory(string)`                              | Set the module category                                             |
| `.DependsOn<TModule>()`                              | Add a required dependency                                           |
| `.DependsOnOptional<TModule>()`                      | Add an optional dependency                                          |

The fluent configuration is the canonical runtime model. Existing attributes such as `[Priority]`, `[ExecutionHint]`, `[NotInParallel]`, `[ModuleTag]`, `[ModuleCategory]`, and `[DependsOn<T>]` remain supported as declarative sugar and are merged into the same model.

## Lifecycle Hooks[​](#lifecycle-hooks "Direct link to Lifecycle Hooks")

You can also override lifecycle methods directly on the module class:

```
public class MyModule : Module<string>

{

    protected override Task OnBeforeExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        context.Logger.LogInformation("Starting module execution");

        return Task.CompletedTask;

    }



    protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(

        IModuleContext context,

        ModuleResult<string> result,

        CancellationToken cancellationToken)

    {

        context.Logger.LogInformation("Module completed with status: {Status}", result.ModuleStatus);

        return Task.FromResult<ModuleResult<string>?>(null);

    }



    protected override Task OnSkippedAsync(

        IModuleContext context,

        SkipDecision skipDecision,

        CancellationToken cancellationToken)

    {

        context.Logger.LogWarning("Module skipped: {Reason}", skipDecision.Reason);

        return Task.CompletedTask;

    }



    protected override Task OnFailedAsync(

        IModuleContext context,

        Exception exception,

        CancellationToken cancellationToken)

    {

        context.Logger.LogError(exception, "Module failed");

        return Task.CompletedTask;

    }



    protected override async Task<string> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        return "result";

    }

}
```

## Tags and Categories[​](#tags-and-categories "Direct link to Tags and Categories")

Organize your modules with tags and categories:

```
[ModuleCategory("Build")]

[ModuleTag("critical")]

[ModuleTag("fast")]

public class BuildModule : Module<BuildOutput>

{

    protected override async Task<BuildOutput> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        // ...

    }

}
```

Or define them programmatically:

```
public class BuildModule : Module<BuildOutput>

{

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()

        .WithCategory("Build")

        .WithTags("critical", "fast")

        .Build();



    protected override async Task<BuildOutput> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        // ...

    }

}
```

See the individual documentation pages for more details on each behavior:

* [Skipping Modules](/ModularPipelines/docs/next/how-to/skipping.md)
* [Retry Policies](/ModularPipelines/docs/next/how-to/retry-policy.md)
* [Timeouts](/ModularPipelines/docs/next/how-to/timeouts.md)
* [Ignoring Failures](/ModularPipelines/docs/next/how-to/ignoring-failures.md)
* [Always Run](/ModularPipelines/docs/next/how-to/always-run.md)
* [Hooks](/ModularPipelines/docs/next/how-to/hooks.md)
* [Categories](/ModularPipelines/docs/next/how-to/categories.md)
* [Run Conditions](/ModularPipelines/docs/next/how-to/run-conditions.md)
