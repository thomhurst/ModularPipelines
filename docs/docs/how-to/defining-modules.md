---
title: Defining Modules
sidebar_position: 2
---

## Defining Modules

Modules are defined by creating a class that inherits from the `Module<T>` base class.

`T` is the type of object that your Module will return, and that object can be seen by other Modules (if they depend on it).

```csharp
public class FindAFileModule : Module<FileInfo>
{
    protected override async Task<FileInfo?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        return context.FileSystem
            .GetFiles("C:\\", SearchOption.AllDirectories, file => file.Name == "MyJsonFile.json")
            .Single();
    }
}
```

You can also return `null` if your module doesn't need to return data:

```csharp
public class CleanupModule : Module<bool>
{
    protected override async Task<bool?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        await context.FileSystem.DeleteDirectoryAsync("./temp");
        return true;
    }
}
```

## Configuring Module Behavior

Configure module behaviors such as timeouts, retry policies, skip conditions, and hooks by overriding the `Configure()` method:

```csharp
public class MyModule : Module<FileInfo>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithTimeout(TimeSpan.FromMinutes(5))
        .WithRetryCount(3)
        .WithSkipWhen(() => !File.Exists("important.json"))
        .WithIgnoreFailures()
        .WithAlwaysRun()
        .Build();

    protected override async Task<FileInfo?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Module logic here
    }
}
```

### Available Configuration Options

| Method | Description |
|--------|-------------|
| `.WithTimeout(TimeSpan)` | Maximum execution time before module is cancelled |
| `.WithRetryCount(int)` | Number of retry attempts on failure |
| `.WithRetryPolicy(IAsyncPolicy)` | Custom Polly retry policy |
| `.WithSkipWhen(...)` | Condition to skip the module |
| `.WithIgnoreFailures()` | Don't fail the pipeline if this module fails |
| `.WithIgnoreFailuresWhen(...)` | Conditionally ignore failures |
| `.WithAlwaysRun()` | Run even if the pipeline has failed |
| `.WithBeforeExecute(...)` | Hook to run before execution |
| `.WithAfterExecute(...)` | Hook to run after execution |

## Lifecycle Hooks

You can also override lifecycle methods directly on the module class:

```csharp
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

    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        return "result";
    }
}
```

## Tags and Categories

Organize your modules with tags and categories:

```csharp
[ModuleCategory("Build")]
[ModuleTag("critical")]
[ModuleTag("fast")]
public class BuildModule : Module<BuildOutput>
{
    protected override async Task<BuildOutput?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // ...
    }
}
```

Or define them programmatically:

```csharp
public class BuildModule : Module<BuildOutput>
{
    public override string? Category => "Build";
    public override IReadOnlySet<string> Tags => new HashSet<string> { "critical", "fast" };

    protected override async Task<BuildOutput?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // ...
    }
}
```

See the individual documentation pages for more details on each behavior:
- [Skipping Modules](skipping)
- [Retry Policies](retry-policy)
- [Timeouts](timeouts)
- [Ignoring Failures](ignoring-failures)
- [Always Run](always-run)
- [Hooks](hooks)
- [Categories](categories)
- [Run Conditions](run-conditions)
