---
title: Migrating to V4
sidebar_label: Overview
sidebar_position: 0
---

# Migrating from V3 to V4

This guide takes a pipeline from **V3.2.8** (the last V3 release) to **V4.0.0**.

Most pipelines only need the [six changes](#the-six-changes-most-pipelines-need) on this page.
Every other page covers one area. Read only the ones for features you use.

## Before you start

- Use a **.NET 11** SDK and runtime for the pipeline project and the CI runner. The applications
  your pipeline builds keep their own target frameworks.
- Use **C# 14** or later. The `context.Tools.<Tool>` properties need it.
- Upgrade **every** ModularPipelines package together, including custom integration projects and
  test helpers.

Staying on 3.x for now? Read the
[v3.2.8 documentation source](https://github.com/thomhurst/ModularPipelines/tree/v3.2.8/docs/docs).
If you used a V4 pre-release build, also check the
[GitHub releases](https://github.com/thomhurst/ModularPipelines/releases).

## The six changes most pipelines need

### 1. Register and run on the builder

```csharp
// V3
builder.Services.AddModule<BuildModule>();
await builder.Build().RunAsync(); // or builder.ExecutePipelineAsync()

// V4
builder.AddModule<BuildModule>();
await builder.RunAsync();
```

Ordinary DI registrations stay on `builder.Services`.
More: [Entry point and options](./entry-point-and-options.md).

### 2. Options are immutable

```csharp
// V3
builder.Options.ExecutionMode = ExecutionMode.WaitForAllModules;
builder.Options.PrintLogo = false;

// V4
builder.ConfigureOptions(options => options with
{
    FailureMode = FailureMode.ContinueOnFailure,
    Console = options.Console with { PrintLogo = false },
});
```

More: [Pipeline options](./entry-point-and-options.md#pipeline-options).

### 3. `Configure` takes a builder and returns nothing

Dependencies, categories, tags, and retries all move into it.

```csharp
// V3
protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
    .WithRetryCount(3)
    .Build();

// V4
protected override void Configure(ModuleConfigurationBuilder module) => module
    .WithRetry(3);
```

More: [Modules](./modules.md).

### 4. One namespace for core types

Start module files with `using ModularPipelines;`. Remove `using ModularPipelines.Models;`,
`using ModularPipelines.Modules;`, and `using ModularPipelines.Extensions;`.

More: [Packages and namespaces](./setup.md).

### 5. Results are not implicitly nullable

`Module<T>` now returns `Task<T>`, not `Task<T?>`. Result checks use typed variants, and the
summary exposes results directly.

```csharp
// V3
protected override async Task<BuildOutput?> ExecuteAsync(...)
var results = await summary.GetModuleResultsAsync();

// V4
protected override async Task<BuildOutput> ExecuteAsync(...)
var results = summary.Results;
```

More: [Modules](./modules.md#execute-signatures) and [Results and summaries](./results.md).

### 6. Tools move to `context.Tools`, and commands end in `Async`

```csharp
// V3
await context.DotNet().Build(options, cancellationToken: cancellationToken);

// V4
await context.Tools.DotNet.BuildAsync(options, cancellationToken: cancellationToken);
```

Tool options were regenerated from current CLI help, so some option names and types differ.
More: [Tools and commands](./commands.md) and [CLI options](./cli-options.md).

## Complete example

A pipeline that restores and builds a project, before and after.

**V3**

```csharp
using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

var builder = Pipeline.CreateBuilder(args);
builder.Options.ExecutionMode = ExecutionMode.WaitForAllModules;
builder.Services.AddModule<RestoreModule>();
builder.Services.AddModule<BuildModule>();
await builder.Build().RunAsync();

public class RestoreModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
        => await context.DotNet().Restore(new DotNetRestoreOptions(), cancellationToken: cancellationToken);
}

[DependsOn<RestoreModule>]
public class BuildModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithRetryCount(2)
        .Build();

    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
        => await context.DotNet().Build(
            new DotNetBuildOptions { Configuration = "Release", NoRestore = true },
            cancellationToken: cancellationToken);
}
```

**V4**

```csharp
using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Options;

var builder = Pipeline.CreateBuilder(args);
builder.ConfigureOptions(options => options with
{
    FailureMode = FailureMode.ContinueOnFailure,
});
builder.AddModule<RestoreModule>().AddModule<BuildModule>();
await builder.RunAsync();

public class RestoreModule : Module<CommandResult>
{
    protected override Task<CommandResult> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
        => context.Tools.DotNet.RestoreAsync(
            new DotNetRestoreOptions(), cancellationToken: cancellationToken);
}

[DependsOn<RestoreModule>]
public class BuildModule : Module<CommandResult>
{
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithRetry(2);

    protected override Task<CommandResult> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
        => context.Tools.DotNet.BuildAsync(
            new DotNetBuildOptions { Configuration = "Release", NoRestore = true },
            cancellationToken: cancellationToken);
}
```

## Migration steps

Work in this order. Earlier steps clear the most compiler errors.

1. **Upgrade packages and the SDK.** See [Packages and namespaces](./setup.md).
2. **Fix namespaces.** See [Packages and namespaces](./setup.md#namespaces).
3. **Fix the entry point and options.** See [Entry point and options](./entry-point-and-options.md).
4. **Fix each module**: `Configure`, dependencies, retries, and return types. See [Modules](./modules.md).
5. **Fix result and summary access.** See [Results and summaries](./results.md).
6. **Fix tool and command calls.** See [Tools and commands](./commands.md), [CLI options](./cli-options.md),
   and [Integration packages](./integrations.md) (Git changed the most).
7. **Fix whatever else you use.** Each of these is its own page:
   [hooks and plugins](./hooks-and-plugins.md),
   [conditions and requirements](./conditions-and-requirements.md),
   [context services](./context.md),
   [logging and HTTP](./logging-and-http.md),
   [files and utilities](./files-and-utilities.md),
   [exceptions and extension points](./exceptions-and-extension-points.md).
8. **Once it compiles, read [Runtime behavior changes](./runtime-behavior.md).** These do not
   produce compiler errors. At minimum, ignore or disable the new
   [run history folder](./runtime-behavior.md#default-run-history).
9. **Build and test** the migrated pipeline, including command arguments and failure and skip
   paths, before you run deployment modules.

Stuck on a compiler error? Look it up in the [old-to-new API lookup](./api-lookup.md).
Using a coding agent? Point it at the [agent reference](./agents.md).

## What has not changed

These still work, although some types moved namespace:

- `Pipeline.CreateBuilder(args)`, direct configuration access, and constructor dependency injection.
- `Module<T>` and `SyncModule<T>`, `[DependsOn<T>]`, categories, and tags.
- `await context.GetModule<TModule>()` and `GetModuleIfRegistered<TModule>()`.
- The `context.Files`, `context.Data`, `context.Network`, `context.Environment`, and
  `context.Security` groups.
- Logging through `context.Logger.LogInformation(...)` and the other `ILogger` extensions.
- Module virtual lifecycle hooks, including replacing a result in `OnAfterExecuteAsync`.
- Separate tool options and `CommandExecutionOptions` parameters.
- `ValueOrDefault`, `ExceptionOrDefault`, `SkipDecisionOrDefault`, `Match`, and `Switch` on results.

Do not apply the V2-to-V3 changes again. `IModuleContext` and `context.GetModule<T>()` already
exist in V3.

## New in V4

None of these are needed to migrate.

- [Pipeline CLI](../how-to/command-line.md): selection, validation, and dry-run planning.
- [Fingerprint-based caching](../how-to/module-caching.md), with a pipeline-wide disable switch.
- [Run reports and local history](../how-to/run-reports.md), including correlation, duration
  comparisons, and bounded output excerpts.
- [OpenTelemetry](../how-to/opentelemetry.md), dependency graph export, and richer CI output.
- [Testing helpers](../how-to/testing.md) and [F# module support](../how-to/fsharp.md).
- [Trimming and Native AOT](../how-to/native-aot.md) for supported statically described C# pipelines.
- [Distributed execution](../distributed/architecture.md), artifact contracts, worker capabilities,
  and execution backends. These packages are not in V3.2.8. If you used them from an intermediate
  build, see the [distributed configuration guide](../distributed/configuration.md).
- More CLI integrations, and much wider command coverage in existing ones.
- Command interceptors. `ModularPipelines.ICommandInterceptor` wraps command execution as
  middleware through `InvokeAsync(invocation, next, cancellationToken)`. Register one with
  `builder.AddCommandInterceptor<T>()` or `AddCommandInterceptor(instance)`. See
  [Testing](../how-to/testing.md).
- Planning-safe conditions. Mark a condition or attribute with `IPlanningSafe` when dry-run
  planning may evaluate it. See [Run conditions](../how-to/run-conditions.md).
- The `OnCachedResultAsync` module hook, `ModuleResult<T>.TryGetValue`, fluent `WithExecutionHint`,
  `WithPriority`, and `WithNotInParallel`, and `context.Tools.Git.Changes.HasChangesAsync`.

## Getting help

1. Check the linked feature guides and the installed V4 package's signatures for the failing API.
2. Review the [current examples](https://github.com/thomhurst/ModularPipelines/tree/main/src/ModularPipelines.Examples).
3. Search or open a [GitHub issue](https://github.com/thomhurst/ModularPipelines/issues). Include the
   old and new package versions, a minimal module or command example, and the compiler or runtime error.
