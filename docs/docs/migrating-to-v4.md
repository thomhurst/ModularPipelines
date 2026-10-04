---
title: Migrating to V4
sidebar_position: 8
---

# Migrating from V3 to V4

ModularPipelines V4 consolidates module configuration, makes pipeline options immutable, and
standardizes the public API. This guide lists what changed from V3 and how to migrate, with
a reference for automated migrations at the end.

> **TL;DR - The 6 biggest changes:**
> 1. Register modules on `builder`, then use `await builder.RunAsync()` or `await builder.BuildAsync()`.
> 2. Replace mutable `builder.Options` assignments with `builder.ConfigureOptions(options => options with { ... })`.
> 3. Override `void Configure(ModuleConfigurationBuilder module)`; move dependencies and metadata into it.
> 4. Import `ModularPipelines` for core authoring types and use `context.Tools.<Tool>` for integrations.
> 5. Use `Task<T>` for `Module<T>`, typed result variants, and `summary.Results`.
> 6. Use `Async` command methods, `cancellationToken:`, and the regenerated CLI option contracts.

## Quick Migration Checklist

- [ ] Upgrade all used ModularPipelines packages together, including custom integration projects and test helpers.
- [ ] Use a .NET 10 SDK/runtime for the pipeline project and CI runner. Use C# 14 or later for generated `context.Tools.<Tool>` properties.
- [ ] Update namespaces for modules, contexts, results, dependencies, events, secrets, and reporting services.
- [ ] Move framework registrations from `builder.Services` to `builder`; keep ordinary DI registrations on `builder.Services`.
- [ ] Replace `Build()` / `ExecutePipelineAsync()` with `BuildAsync()` / `RunAsync()` and dispose explicitly built pipelines.
- [ ] Replace mutable pipeline options with `ConfigureOptions` and the grouped console, command, and HTTP settings.
- [ ] Rewrite `Configure()`, `DeclareDependencies()`, metadata overrides, retries, and fluent lifecycle hooks.
- [ ] Make module result nullability explicit; update result metadata, status checks, and summary consumers.
- [ ] Migrate hook interfaces, plugins, requirements, run conditions, build-system checks, and sub-modules.
- [ ] Update tool access, command method names, named cancellation parameters, logging options, and generated arguments.
- [ ] Rewrite Git information and command calls for the async, grouped Git API.
- [ ] Review timeouts, output capture limits, working directories, skip propagation, and failure handling.
- [ ] Rebuild custom integrations with the V4 source generator and update analyzer suppressions.
- [ ] Build and test the migrated pipeline, including command arguments and failure/skip paths, before running deployment modules.

## Table of Contents

**What Still Works:**

- [Unchanged Features](#unchanged-features)

**Breaking Changes (in reading order):**

1. [Namespaces and Package Setup](#namespaces-and-package-setup)
2. [Entry Point and Registration Changes](#entry-point-and-registration-changes)
3. [Pipeline Options and Failure Modes](#pipeline-options-and-failure-modes)
4. [Module Configuration and Dependencies](#module-configuration-and-dependencies)
5. [Module Results and Pipeline Summaries](#module-results-and-pipeline-summaries)
6. [Lifecycle Hooks and Event Handlers](#lifecycle-hooks-and-event-handlers)
7. [Run Conditions and Requirements](#run-conditions-and-requirements)
8. [Context Services and Sub-modules](#context-services-and-sub-modules)
9. [Tool Access and Command Execution](#tool-access-and-command-execution)
10. [Generated CLI Options and Custom Commands](#generated-cli-options-and-custom-commands)
11. [Logging and HTTP](#logging-and-http)
12. [Files, Working Directories, and Utility APIs](#files-working-directories-and-utility-apis)
13. [Exceptions and Extension Points](#exceptions-and-extension-points)
14. [Runtime Behavior Changes](#runtime-behavior-changes)

**Reference:**

- [New Features in V4](#new-features-in-v4)
- [Complete Migration Example](#complete-migration-example)
- [Breaking API Reference](#breaking-api-reference)
- [Agents / LLM Migration Reference](#agents--llm-migration-reference)
- [Getting Help](#getting-help)

## Unchanged Features

These concepts remain supported, although some types have moved namespaces:

- `Pipeline.CreateBuilder(args)`, direct configuration access, and constructor dependency injection.
- Generic `Module<T>` and `SyncModule<T>`, explicit `[DependsOn<T>]` dependencies, categories, and tags.
- `await context.GetModule<TModule>()` and optional lookup through `GetModuleIfRegistered<TModule>()`.
- The `context.Files`, `context.Data`, `context.Network`, `context.Environment`, and `context.Security` capability groups.
- Structured logging through `context.Logger.LogInformation(...)` and other `ILogger` extensions.
- Module virtual lifecycle hooks, including the ability to replace a result in `OnAfterExecuteAsync`.
- Separate tool options and `CommandExecutionOptions` parameters.
- `ValueOrDefault`, `ExceptionOrDefault`, `SkipDecisionOrDefault`, `Match`, and `Switch` on module results.

Do not apply V2-to-V3 transformations again: `IModuleContext` and `context.GetModule<T>()` already
exist in released V3. The next sections describe the changes from that release.

## Namespaces and Package Setup

The pipeline targets .NET 10, as V3 did; upgrading
does not mean that the applications your pipeline builds must target .NET 10.
Keep the core framework, integrations, and projects containing shared module or command types
on compatible V4 versions. Recompile those projects to refresh source-generated metadata.

Most module-authoring types now live directly in `ModularPipelines`:

| V3 location or type | V4 location or type |
| --- | --- |
| `ModularPipelines.Modules.Module<T>`, `Module`, `SyncModule<T>`, `SyncModule` | `ModularPipelines` |
| `ModularPipelines.Context.IModuleContext`, `IPipelineContext` | `ModularPipelines` |
| `ModularPipelines.Models.ModuleResult<T>`, `ModuleResult`, `CommandResult`, `None`, `SkipDecision` | `ModularPipelines` |
| `ModularPipelines.Configuration.ModuleConfigurationBuilder` | `ModularPipelines.ModuleConfigurationBuilder` |
| `ModularPipelines.Attributes.DependsOn*` | `ModularPipelines` |
| Run-condition types in `Attributes` / `Conditions` | `ModularPipelines` |
| `ModularPipelines.Enums.ModulePriority` | `ModularPipelines.ModulePriority` |
| `ModularPipelines.Attributes.ModuleCategoryAttribute`, `ModuleTagAttribute` | `ModularPipelines` |
| Capability interfaces under `Context.Domains` and its subnamespaces | `ModularPipelines.Context` (some renamed; see below) |
| `Context.Domains.IEnvironmentDomainContext` (`context.Environment`) | `ModularPipelines.Context.IEnvironmentContext` |
| `Context.Domains.Security.IHasherContext` (`context.Security.Hasher`) | `ModularPipelines.Context.IHashContext` (`context.Security.Hash`) |
| `Enums.ExecutionType` | `ModularPipelines.Enums.ExecutionHint` |
| `Attributes.Events` handler interfaces and `Context.IModuleHookContext` | `ModularPipelines.Events` |
| `Attributes.SecretValueAttribute`, `Options.SecretMaskingOptions`, secret services in `Engine` | `ModularPipelines.Secrets` |
| `Engine.IModuleResultRepository`, `Engine.IModuleEstimatedTimeProvider` | `ModularPipelines.Reporting` |
| `Models.PipelineMetrics`, `Models.ModuleTimeline`, `Models.SubModuleEstimation` | `ModularPipelines.Reporting` |

Start ordinary modules with `using ModularPipelines;`. Keep other imports where you need them,
such as `ModularPipelines.Options`, `ModularPipelines.Extensions`, `ModularPipelines.FileSystem`,
and tool option namespaces.

Do not replace whole namespaces blindly. Some types did not move:

- CLI attributes such as `CliOptionAttribute` stay in `ModularPipelines.Attributes`.
- `PipelineSummary`, `IModuleResult`, and `RequirementDecision` stay in `ModularPipelines.Models`.
- Generated `context.Tools.<Tool>` properties need `using ModularPipelines.Context;`.

The V4 package adds a `buildTransitive` global `using ModularPipelines;` to C# consumers. If your
own types collide with root types such as `Module` or `SkipDecision`, opt out with
`<Using Remove="ModularPipelines" />` in the project file and add explicit or aliased usings.

Watch for one name that V4 reuses with a different meaning. V4's `IEnvironmentContext` is the
renamed V3 `IEnvironmentDomainContext` (`context.Environment`). It is not V3's separate standalone
`IEnvironmentContext`. Update mocks and signatures against the V4 members.

`IFilesContext.Checksum` and `IChecksumContext` are removed; see
[Utility replacements](#utility-replacements). The injectable `IFileSystemContext` is also removed;
use `FilePath` / `FolderPath` members or `context.Files`.

The older duplicate service interfaces such as `ICommand`, `IBash`, `IPowershell`, `IHttp`,
`IJson`, and `IHasher` have been consolidated into the `*Context` interfaces. Update custom
implementations and mocks to the actual V4 contract, for example `ICommandContext`,
`IPowerShellContext`, `IHttpContext`, and `IHashContext`.

The remaining utility interface mappings are explicit below. V4 interfaces in this table live in
`ModularPipelines.Context`; update injected types, implementations, and mocks as well as imports.
The V3 `Context.Domains.*` interfaces with the same `*Context` names also move to that namespace.

| V3 interface | V4 interface | Context access |
| --- | --- | --- |
| `IZip` | `IZipContext` | `context.Files.Zip` |
| `IXml` | `IXmlContext` | `context.Data.Xml` |
| `IYaml` | `IYamlContext` | `context.Data.Yaml` |
| `IBase64` | `IBase64Context` | `context.Data.Base64` |
| `IHex` | `IHexContext` | `context.Data.Hex` |
| `ICertificates` | `ICertificatesContext` | `context.Security.Certificates` |
| `IDownloader` | `IDownloaderContext` | `context.Network.Downloader` |
| `IEnvironmentVariables` | `IEnvironmentVariablesContext` | `context.Environment.Variables` |

The core `Context.Linux.AptGet` / `IAptGet` wrappers and the
`ModularPipelines.Options.Linux.AptGet` family (`AptGetOptions`, `AptGetInstallOptions`, etc.) are
removed, with no typed core replacement. To keep invoking `apt-get`, use the command context
with explicit argument tokens, or define your own typed integration using the
[custom integration guide](./how-to/generate-private-cli-integration.md):

```csharp
await context.Shell.RunAsync(
    "apt-get",
    ["install", "--yes", "curl"],
    cancellationToken: cancellationToken);
```

Run package-manager commands with the permissions your environment requires. Translate each old
`AptGet*Options` value to its CLI argument; there is no `context.Tools.AptGet` property in core.

## Entry Point and Registration Changes

### Before (V3)

```csharp
var builder = Pipeline.CreateBuilder(args);
builder.Services.AddModule<BuildModule>();
builder.Services.AddModule<TestModule>();
await builder.Build().RunAsync();
// Or: await builder.ExecutePipelineAsync();
```

### After (V4)

```csharp
var builder = Pipeline.CreateBuilder(args);
builder.AddModule<BuildModule>().AddModule<TestModule>();
await builder.RunAsync();
```

For a separately built pipeline, use asynchronous disposal:

```csharp
await using var pipeline = await builder.BuildAsync();
var summary = await pipeline.RunAsync();
```

`BuildAsync()` validates before returning; the synchronous `Build()` is gone. V3's `Build()` did not
validate, so a pipeline that previously built can now throw `PipelineValidationException` here.
The `ExecutePipelineAsync()` extensions on the builder and on `IPipeline` are also removed; call `RunAsync()`.
Use `await builder.ValidateAsync()` to inspect errors without executing modules.
`RunAsync()` on the builder builds, runs, and disposes the pipeline for you.

| V3 registration | V4 registration |
| --- | --- |
| `builder.Services.AddModule<T>()` | `builder.AddModule<T>()` |
| Module instance/factory registration on services | `builder.AddModule(instance)` / `builder.AddModule<T>(factory)` |
| `builder.Services.AddModulesFromAssembly(...)` | `builder.AddModulesFromAssembly(...)` |
| `builder.AddModules<T1, T2>()` (and higher arities) | Chain `.AddModule<T1>().AddModule<T2>()`, or use `.AddModules(typeof(T1), typeof(T2))` |
| `builder.Services.AddRequirement<T>()` | `builder.AddRequirement<T>()` |
| `AddPipelineGlobalHooks<T>()` on the builder or its services | `builder.AddPipelineEventHandler<T>()`, with the new interface |
| `AddPipelineModuleHooks<T>()` on the builder or its services | `builder.AddModuleEventHandler<T>()`, with the new interface |
| Registration handles / `IModuleRegistrationBuilder` | Registration returns `PipelineBuilder`; configure the module in its override |

V3 code that already called `builder.AddModule<T>()` or `builder.AddRequirement<T>()` keeps working.

Ordinary `AddSingleton`, `AddScoped`, `Configure<TOptions>`, and other application DI calls stay
on `builder.Services`. Registration helpers that previously extended `IServiceCollection` to
add pipeline modules should now accept and return `PipelineBuilder`.
For the removed two-parameter `ConfigureServices((builder, services) => ...)` overload, configure
`builder.Services` directly or capture the builder in the remaining one-parameter callback.
`BuildHostAsync()` becomes `BuildAsync()`, and `pipeline.RootServices` becomes `pipeline.Services`.

The `AddPipelineFileWriter<T>()` shortcut is removed. To generate a supported provider's minimal
CI file, set `ModularPipelinesBuildSystem` in the project as described in
[Generate build server files](./how-to/build-server-files.md). Custom workflow generation should
be invoked explicitly rather than relying on the removed registration shortcut.

Replace `PipelineBuilderOptions` with `PipelineBuilderSettings`. The new type is an immutable record:
set its properties in the initializer, and pass `Args` as any `IReadOnlyList<string>`.

```csharp
var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
{
    Args = args,
    EnvironmentName = "Production",
    ContentRootPath = repositoryRoot,
    WorkingDirectory = repositoryRoot,
});
```

V4 consumes its built-in pipeline CLI switches from `args`. If an application already owns those
arguments, set `EnableCommandLineOptions = false` in the settings. Referenced ModularPipelines
assemblies still load, but eager loading of unreferenced `*ModularPipeline*.dll` files from the
application directory is disabled by default; copied plugin assemblies that rely on it need
`LoadModularPipelinesAssemblies = true` or explicit registration.

## Pipeline Options and Failure Modes

Pipeline options and nested option records are immutable. `builder.Options` is a read-only
view; use `ConfigureOptions` and `with` expressions, preserving existing settings.

### Before (V3)

```csharp
builder.Options.ExecutionMode = ExecutionMode.WaitForAllModules;
builder.Options.ThrowOnPipelineFailure = false;
builder.Options.PrintLogo = false;
builder.Options.Concurrency.MaxParallelism = 4;
```

### After (V4)

```csharp
builder.ConfigureOptions(options => options with
{
    FailureMode = FailureMode.ContinueOnFailure,
    ThrowOnPipelineFailure = false,
    Console = options.Console with { PrintLogo = false },
    Concurrency = options.Concurrency with { MaxParallelism = 4 },
});
```

To change one group of settings, use its shortcut. Each shortcut calls `ConfigureOptions` for you
and returns the builder, so you can chain them:

```csharp
builder
    .ConfigureConsole(console => console with { PrintLogo = false })
    .ConfigureConcurrency(concurrency => concurrency with { MaxParallelism = 4 });
```

The shortcuts are `ConfigureConsole`, `ConfigureConcurrency`, `ConfigureCommands`, `ConfigureHttp`,
and `ConfigureSecrets`.

| V3 option | V4 option |
| --- | --- |
| `ExecutionMode.StopOnFirstException` | `FailureMode.FailFast` |
| `ExecutionMode.WaitForAllModules` | `FailureMode.ContinueOnFailure` |
| `ShowProgressInConsole` | `Console.ShowProgress` |
| `PrintResults`, `PrintLogo`, `PrintDependencyChains` | Same names under `Console` |
| `ConsoleWidth` | `Console.Width` |
| `DefaultLoggingOptions` | `Commands.Logging` |
| `DefaultHttpLoggingOptions` | `Http.Logging` |
| `DefaultHttpTimeout` | `Http.Timeout` |
| `DefaultHttpResilienceOptions` | `Http.Resilience` |
| `DefaultExecutionOptions` | Pass `CommandExecutionOptions` to each command; use builder settings for the pipeline working directory |
| `builder.RunCategories(...)` / `IgnoreCategories(...)` | Set `RunOnlyCategories` / `IgnoreCategories` in `ConfigureOptions` |
| `builder.ConfigurePipelineOptions(...)`, including the `(builder, options)` overload | `builder.ConfigureOptions(options => options with { ... })`; capture the builder in the lambda if needed |
| `builder.Services.Configure<SchedulerOptions>(...)` | `builder.ConfigureConcurrency(c => c with { NotificationTimeout = ... })`; `SchedulerOptions` is removed |
| `builder.Services.Configure<SecretMaskingOptions>(...)` | `builder.ConfigureSecrets(s => s with { ... })`; the DI `Configure` call no longer applies |
| `builder.SetLogLevel(level)` | `builder.Logging.SetMinimumLevel(level)` (`Microsoft.Extensions.Logging`) |

Collections such as category filters are now read-only contracts. Assign replacement collections
in `ConfigureOptions`, rather than calling `.Add()` on the existing options.

To inspect a failed pipeline's returned summary, use **both** `ContinueOnFailure` and
`ThrowOnPipelineFailure = false`. `FailFast` rethrows the module failure.

## Module Configuration and Dependencies

`Configure()` no longer returns a `ModuleConfiguration`. V4 supplies the builder to a `void`
override. There is no `ModuleConfiguration.Create()`, `.Default`, or final `.Build()` to call.

### Before (V3)

```csharp
protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
    .WithTimeout(TimeSpan.FromMinutes(10))
    .WithRetryCount(3)
    .Build();

protected override void DeclareDependencies(IDependencyDeclaration dependencies)
{
    dependencies.DependsOn<RestoreModule>();
}

public override string? Category => "Build";
```

### After (V4)

```csharp
protected override void Configure(ModuleConfigurationBuilder module)
{
    base.Configure(module);
    module
        .DependsOn<RestoreModule>()
        .WithCategory("Build")
        .WithTimeout(TimeSpan.FromMinutes(10))
        .WithRetry(3);
}
```

Call `base.Configure(module)` when inheriting configuration from another module class.
Keep configuration deterministic: it is initialized once for execution and also participates in
planning. Use context-aware callbacks for decisions that require runtime services.

| V3 pattern | V4 replacement |
| --- | --- |
| `DeclareDependencies(IDependencyDeclaration)` | `DependsOn`, `DependsOnOptional`, `DependsOnIf` on the supplied module builder |
| `DependsOnLazy<T>()` / `DependsOnLazy(Type)` | `DependsOnOptional<T>()`; V3 lazy dependencies were already optional and did not defer execution |
| `DependsOnIf<T>(Func<bool>)` | `DependsOnIf<T>(bool)`; evaluate the predicate while configuring |
| `Tags` / `Category` overrides | `WithTags(...)` / `WithCategory(...)`, or existing attributes |
| `WithRetryCount(count)` | `WithRetry(count, baseDelay: ..., shouldRetry: ...)` |
| `WithRetryPolicy(Polly.IAsyncPolicy)` | `WithRetry(...)` or `WithShield(Kevlar.Shield)` |
| `WithRetryPolicy(Func<IModuleContext, IAsyncPolicy>)` | `WithShield(Func<IModuleContext, Shield>)` |
| `WithBeforeExecute(...)` / `WithAfterExecute(...)` | Module virtual lifecycle hooks |
| `ExecutionType.CpuIntensive` / `IoIntensive` | `ExecutionHint.CpuBound` / `IoBound`, including `[ExecutionHint(...)]` arguments |

Code that reads `module.Configuration` should note that `RetryPolicyFactory`, `OnBeforeExecute`, and
`OnAfterExecute` are removed, and `ModuleConfiguration` can no longer be constructed directly.
`SkipCondition` and `IgnoreFailuresCondition` now take a `CancellationToken` and return `ValueTask<...>`.

### Retries

Polly is no longer the module resilience API. Simple retries need no policy object:

```csharp
protected override void Configure(ModuleConfigurationBuilder module) => module
    .WithRetry(
        count: 3,
        baseDelay: TimeSpan.FromSeconds(1),
        shouldRetry: exception => exception is HttpRequestException);
```

For a custom policy, reconstruct its intent with a Kevlar shield, for example:

```csharp
// using Kevlar;
protected override void Configure(ModuleConfigurationBuilder module) => module
    .WithShield(Shield.When<HttpRequestException>()
        .Retry(3, Backoff.Custom(attempt => TimeSpan.FromSeconds(attempt * attempt))));
```

Review filters, delays, and wrapped policies instead of replacing the Polly type name.
Standard `WithRetry` uses exponential backoff with jitter; its default base delay is 100 ms.
V3's `WithRetryCount` waited `attempt² × 100 ms` without jitter, so `WithRetry(n)` does not
reproduce the old delays exactly; pass `baseDelay` or a shield when timing matters.
V3 applied the module timeout to all attempts combined. V4 applies it **per attempt**, with
backoff outside that timeout.
See [Retries and Resilience Shields](./how-to/retry-policy.md).

## Module Results and Pipeline Summaries

### Explicit nullability

V3 implicitly made a generic module's result nullable. V4 uses the declared `T`:

```csharp
// V3: Module<BuildOutput>, returning Task<BuildOutput?>
// V4: result is required
public class BuildModule : Module<BuildOutput>
{
    protected override Task<BuildOutput> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult(new BuildOutput("artifacts"));
}
```

If `null` is meaningful, use `Module<BuildOutput?>` and `Task<BuildOutput?>` explicitly. Otherwise,
return a real result, or use `Module` / `Module<None>` for work without a value. This also applies
to `SyncModule<T>.Execute`, which now returns `T` rather than implicit `T?`.

For non-generic modules, rename `ExecuteModuleAsync` to `ExecuteAsync`; for non-generic
`SyncModule`, rename `ExecuteModule` to `Execute`.

### Result access

`ModuleResult<T>` now has public typed `Failure` and `Skipped` variants alongside `Success`. Replace
`IsSuccess`, `IsFailure`, `IsSkipped`, and `ModuleResultType` checks with typed patterns or safe
accessors:

```csharp
var result = await context.GetModule<BuildModule>();

if (result is ModuleResult<BuildOutput>.Success { Value: var output })
{
    // Use output.
}

// A successful result: throws for failure or skip, but can return null.
var successfulOutput = result.Value;

// Require a non-null value at runtime explicitly.
var requiredOutput = result.Value
    ?? throw new InvalidOperationException("BuildModule returned no output.");

// Optional data: preserves the absence of a value.
var optionalOutput = result.ValueOrDefault;
```

Do not turn every `.ValueOrDefault` into `.Value`: the latter deliberately changes failure and
skip handling. `Value` returns a successful result's value unchanged, including null. Non-nullable
annotations do not enforce a runtime check: nullable-oblivious code, F#, or `null!` can still supply
null even for `Module<BuildOutput>`. Use an explicit check when null must be rejected.
When matching a **typed** result, use `ModuleResult<T>.Failure` and
`ModuleResult<T>.Skipped`, rather than the non-generic variants.
`ModuleResult<T>.Success` is no longer a positional record. Positional patterns still work, but a
named constructor argument is now `value:` rather than `Value:`.

### Metadata and statuses

| V3 result member | V4 member |
| --- | --- |
| `ModuleName` | `Name` |
| `ModuleDuration` | `Duration` |
| `ModuleStart` / `ModuleEnd` | `StartTime` / `EndTime` |
| `ModuleStatus` | `Status` |

These renames apply to `ModuleResult` and `IModuleResult`, not every metadata object in the library.
For example, hook contexts and `ModuleTimeline` still expose `ModuleName`. `ModuleTimeline.ExecutionType`
is renamed `ExecutionHint`.

The status enum moved from `ModularPipelines.Enums.Status` to `ModularPipelines.ModuleStatus`:

| V3 enum value | V4 enum value |
| --- | --- |
| `NotYetStarted` | `NotStarted` |
| `Processing` | `Running` |
| `Successful` | `Succeeded` |
| `IgnoredFailure` | `FailureIgnored` |
| `UsedHistory` | `RestoredFromHistory` |
| `PipelineTerminated` | `Canceled` or `DependencyFailed`, depending on the cause |
| `Retried` | Removed; retries are attempts, not a terminal status |
| `Failed`, `Skipped`, `TimedOut`, `Unknown` | Same names on `ModuleStatus` |

`Canceled`, `DependencyFailed`, and `RestoredFromCache` (fingerprint-cache hits) are new statuses.
V4 spells "canceled" with one L everywhere, as .NET's `OperationCanceledException` does.

Status names also affect custom JSON consumers, stored results, dashboards, and exhaustive switches.
Do not rely on old numeric enum values. Built-in run history and stored results still read the old
names: `PipelineTerminated` and `Cancelled` load as `Canceled`.

### Pipeline summaries

The summary exposes results, rather than live modules:

```csharp
// V3
var results = await summary.GetModuleResultsAsync();
var failures = summary.GetFailedModuleResults();

// V4
var results = summary.Results;
var failures = summary.Failures;
var ignoredFailures = summary.IgnoredFailures;
```

V3's `GetFailedModuleResults()` mixed two kinds of result. V4 splits them:

- `summary.Failures` holds the failures that fail the pipeline.
- `summary.IgnoredFailures` holds results with status `FailureIgnored`. They still have an
  exception, but they do not fail the pipeline.

To reproduce the V3 list, combine both. For the overall outcome, read `summary.Succeeded`.

`PipelineSummary` is sealed in V4. Its timing properties are renamed from `Start`, `End`, and
`TotalDuration` to `StartTime`, `EndTime`, and `Duration`; update custom JSON consumers too.
`Succeeded` replaces the module-level `Status` property. It is true only when the pipeline
completes without unignored failures. Skipped, cached, restored, and ignored-failure results
permit success; incomplete, failed, and canceled runs do not. The value survives JSON
round-trips even though module results are not serialized.
`summary.Modules` and `summary.GetModule<T>()` are removed. Retrieve dependencies inside a
module, or inspect `summary.Results` after execution. If the old code needs a particular output,
select the matching typed result. When several modules return the same type, tell them apart by
`Name`. `IModuleResult.TypeName` is also available, but it can be null. Update custom serialization
and result repositories for the new metadata.

## Lifecycle Hooks and Event Handlers

Virtual hooks remain the extension point for behavior owned by one module. Move
`WithBeforeExecute` and `WithAfterExecute` callbacks into those overrides:

```csharp
protected override Task OnBeforeExecuteAsync(
    IModuleContext context, CancellationToken cancellationToken)
{
    context.Logger.LogInformation("Starting build");
    return Task.CompletedTask;
}

protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
    IModuleContext context, ModuleResult<string> result, CancellationToken cancellationToken)
{
    context.Logger.LogInformation("Build finished: {Status}", result.Status);
    return Task.FromResult<ModuleResult<string>?>(null); // Keep the original result.
}
```

Do not change `OnAfterExecuteAsync` to plain `Task`: its nullable result means “optionally replace
the outcome.” Before/after hooks run once around the full retry sequence, not for each attempt.

`SyncModule<T>` no longer has separate synchronous lifecycle overrides. Migrate `OnBeforeExecute`,
`OnAfterExecute`, `OnSkipped`, and `OnFailed` to the inherited `*Async` hooks. Return
`Task.CompletedTask` for synchronous work, or `Task.FromResult<ModuleResult<T>?>(replacement)`
from the after hook. Its main `Execute` method remains synchronous.

| V3 contract | V4 contract |
| --- | --- |
| `IPipelineGlobalHooks` | `IPipelineEventHandler` |
| `IPipelineModuleHooks` | `IModuleEventHandler` |
| `IPipelineHookContext` | `IPipelineContext` |
| `IEventHandlerPriority.Priority` | `IEventHandler.Order` (ascending, default `0`); global and registration handlers also gain `ContinueOnError` |
| `IModuleRegistrationEventReceiver` | `IModuleRegistrationHandler` |
| `IPipelineModuleHooks.OnModuleEndAsync(context)` | `OnModuleEndAsync(context, IModuleResult result, cancellationToken)` |
| `IPipelineModuleHooks.OnModuleFailureAsync(context)` | `OnModuleFailureAsync(context, Exception exception, cancellationToken)` |
| `IPipelineModuleHooks.OnModuleSkippedAsync(context)` | `OnModuleSkippedAsync(context, SkipDecision reason, cancellationToken)` |
| `IModuleHookContext.RequestRetry`, `SkipDependentModules`, `FailPipeline` | Removed; they had no effect. Use `WithRetry`, dependency declarations, or throw from the handler |
| `IModuleRegistrationContext.Services` | Removed; register services on `builder.Services` |

Every module, pipeline, and registration handler method now takes a trailing `CancellationToken`,
for example `OnPipelineStartAsync(IPipelineContext context, CancellationToken cancellationToken)` and
`OnRegistrationAsync(IModuleRegistrationContext context, CancellationToken cancellationToken)`.

Register the new contracts with `builder.AddPipelineEventHandler<T>()` and
`builder.AddModuleEventHandler<T>()`. Attribute handlers and global handlers share the
interfaces in `ModularPipelines.Events`. See [Hooks](./how-to/hooks.md) for signatures and ordering.

Handler failures have defined outcomes. Unless `ContinueOnError` is `true`, a failing Ready, Start,
registration, or pipeline handler fails the module or pipeline. A failing End, Failure, or Skipped
handler no longer changes the module outcome it observed; it is reported as an additional pipeline
error. A failing pipeline-end handler never hides an earlier execution failure.

### Plugins

`PluginRegistry` and `PluginTestHelper` are removed. `IModularPipelinesPlugin` keeps `Name` and
replaces `Priority`, `ConfigureServices`, and `ConfigurePipeline` with one
`Configure(PipelineBuilder builder)` method. Register plugins on the builder instead of in a static
registry:

```csharp
// V3
PluginRegistry.Register(new MyPlugin());

// V4
builder.AddPlugin<MyPlugin>(); // or builder.AddPlugin(new MyPlugin())

public sealed class MyPlugin : IModularPipelinesPlugin
{
    public string Name => "My plugin";

    public void Configure(PipelineBuilder builder)
    {
        builder.Services.AddSingleton<IMyService, MyService>();
        builder.AddModule<MyModule>();
    }
}
```

Plugins apply immediately, in the order they are added, so order `AddPlugin` calls where the V3
`Priority` mattered. Builder configuration after the call overrides the plugin's configuration, and
adding two plugins with the same `Name` (or a blank `Name`) throws. A plugin that throws from
`Configure` fails at `AddPlugin` with `PluginInitializationException`. Plugins that registered
themselves from a module initializer must now be added explicitly by the consumer. Remove
`PluginTestHelper.IsolatedRegistry()` from tests; each builder now owns its plugins.

Plugin assemblies that declare `[assembly: ModularPipelinesPlugin(3)]` must change it to `4`;
a mismatched major version throws `PluginVersionMismatchException`.

## Run Conditions and Requirements

### Run conditions and skip decisions

Use the condition attribute that expresses the intended logic:

| V3 pattern | V4 replacement |
| --- | --- |
| `[RunIfAll<T1, ..., T4>]` | `[RunIf<T1, ..., T4>]` (all conditions must be true) |
| `[RunIfAny<T>]` with one condition | `[RunIf<T>]`; `[RunIfAny<T1, T2, ...>]` still takes two to four conditions |
| `[SkipIf<T>]` / `[SkipIf<T1, T2>]` | Unchanged: skips when any condition is true; V4 also accepts three or four conditions |
| `IsCI` / `IsLocal` condition classes | `OnCI` / `OnLocal` |
| `[RunOnLinuxOnly]` and similar single-platform conditions | `[RunIf<OnLinux>]`, `[RunIf<OnWindows>]`, or `[RunIf<OnMacOS>]` |
| Multiple alternative `RunOn*` attributes | One `[RunIfAny<OnLinux, OnMacOS>]`, preserving the original OR intent |
| `MandatoryRunConditionAttribute` subclass | `RunConditionAttribute` subclass calling `base(ConditionIntent.Run)` |
| Non-mandatory `RunConditionAttribute` subclass (OR-ed alternatives) | `RunConditionAttribute` subclass with `ConditionIntent.Run` that overrides `GroupKey` with a shared key |
| `RunConditionAttribute.Condition(IPipelineHookContext)` | `EvaluateAsync(IPipelineContext, CancellationToken)` |
| Custom `IConditionAttribute` implementations, `ConditionLogic.Skip` | `RunConditionAttribute` subclass; use `ConditionIntent.Skip` for skip semantics |
| `IRunCondition.EvaluateAsync(IPipelineHookContext)` | `EvaluateAsync(IPipelineContext, CancellationToken)` |
| `SkipDecision.Of(condition, reason)` | `SkipDecision.When(condition, reason)` |
| Implicit bool/string/task skip decisions | Explicit `SkipDecision.Skip(reason)`, `.DoNotSkip`, or `.When(...)` |
| `ConditionGroup.Conditions` (`IRunCondition[]`), `ConditionGroup.EvaluateAsync(context)` | `IReadOnlyList<IRunCondition>`; `EvaluateAsync(context, cancellationToken)` |
| Subclasses of Git/GitHub condition attributes (`RunOnlyOnBranch`, `SkipIfBranch`, `SkipIfDependabot`, ...) | These now derive from `RunConditionAttribute`; override `EvaluateAsync` instead of `Condition` |

The `WithSkipWhen` overloads without a context parameter are removed, so every callback takes the
context. Asynchronous callbacks also take a cancellation token
and return `ValueTask`. A callback that returns `bool` can still omit the reason; the module then
reports "Skip condition was met". Add a reason when you want the summary to explain the skip:

```csharp
// V3
.WithSkipWhen(() => !publishEnabled)

// V4: true still means skip.
.WithSkipWhen(_ => !publishEnabled)
.WithSkipWhen(_ => !publishEnabled, "Publishing is disabled") // With a reason.
```

An `async` lambda can target the new overload directly:

```csharp
.WithSkipWhen(async (context, cancellationToken) =>
    await CheckAsync(context, cancellationToken)) // CheckAsync returns Task<SkipDecision>.
```

For an existing check without a cancellation parameter, adapt it with
`async (context, _) => await CheckAsync(context)`; add cancellation support when practical.
Asynchronous `WithIgnoreFailuresWhen` callbacks now take `(context, exception, cancellationToken)`
and return `ValueTask<bool>`; the synchronous `(context, exception)` overload is unchanged.

Fluent registration also accepts condition types and instances directly: `.WithRunIf<T>()`,
`.WithRunIf(IRunCondition)`, `.WithSkipIf<T>()`, and `.WithSkipIf(IRunCondition)`.

Repeated `WithSkipWhen` calls now combine with OR: the module skips when any condition is true.
In V3, the last call replaced the earlier ones. Remove earlier calls that V3 silently ignored, and
use `WithSkipWhenAll` when every condition must be true. Analyzer [`MP0020`](./analyzers/MP0020.md) (Info) flags the second
and later `WithSkipWhen` calls on the same builder so you can review them.
Keep conditions free of external side effects because planning can evaluate them.
See [Run conditions](./how-to/run-conditions.md) and [Skipping](./how-to/skipping.md).

### Requirements

Replace `MustAsync` / synchronous `Must` overrides with the cancellation-aware `EvaluateAsync`:

```csharp
public sealed class TokenRequirement : PipelineRequirement
{
    public override Task<RequirementDecision> EvaluateAsync(
        IPipelineContext context, CancellationToken cancellationToken)
        => Task.FromResult(
            string.IsNullOrEmpty(context.Environment.Variables.Get("NUGET_API_KEY"))
                ? RequirementDecision.Failed("NUGET_API_KEY is required")
                : RequirementDecision.Passed);
}
```

The protected `Pass()`, `Fail(reason)`, and `When(condition, failureReason)` helpers remain available
in `PipelineRequirement` subclasses; keep existing calls when migrating the override.
`RequirementDecision.Passed` / `.Failed(reason)` are alternatives, including for classes that
implement `IPipelineRequirement` directly. `RequirementDecision.Of` and implicit string/task conversions are removed;
the bool conversion remains. Replace the `Success` property with `IsSatisfied` for the outcome.

Rename `Require.CIEnvironment(...)` to `Require.Ci(...)`. Asynchronous factory conditions now
receive a cancellation token: change `Require.ThatAsync(async context => ...)` to
`Require.ThatAsync(async (context, cancellationToken) => ...)` and forward that token into the check.

Built-in requirement classes are replaced by factories: for example,
`builder.AddRequirement(Require.Windows())` replaces registration of `WindowsRequirement`.
The other shortcuts are `Require.Linux()`, `Require.MacOS()`, and `Require.WindowsAdmin()`.
`DelegateRequirement` is now internal; keep creating delegate requirements with `Require.That(...)`
or `Require.ThatAsync(...)` and type them as `IPipelineRequirement`. `PipelineRequirement.EvaluateAsync`
is abstract, so every subclass must override it. Unmet requirements are reported together in one
`RequirementNotMetException`.
See [Requirements](./how-to/requirements.md).

## Context Services and Sub-modules

### Service resolution

| V3 access | V4 access |
| --- | --- |
| `context.Services.Get<T>()` | `context.Services.GetRequiredService<T>()` |
| `context.GetService<T>()` (required lookup extension) | `context.Services.GetRequiredService<T>()` |
| `context.TryGetService<T>()` | `context.Services.GetRequiredService<T>()` to keep V3 behavior, or `context.Services.GetService<T>()` if absence is allowed |

Despite its name, V3's `TryGetService` threw when the service was missing, because it called
`GetRequiredService`. `GetRequiredService` preserves that behavior. V4's `GetService` really returns
`null`; use it only where absence is allowed, and add null handling. Required and optional lookups deliberately have
different names; a blanket `Get` replacement can change runtime behavior.

### Build system detection

`IBuildSystemContext` (`context.Environment.BuildSystem`) replaces one flag per CI system with
`Current` and `Is(BuildSystem)`:

```csharp
// V3
if (context.Environment.BuildSystem.IsGitHubActions) { ... }

// V4
if (context.Environment.BuildSystem.Is(BuildSystem.GitHubActions)) { ... }
```

The same pattern applies to `IsAzurePipelines`, `IsTeamCity`, `IsJenkins`, `IsGitLab`,
`IsBitbucket`, `IsTravisCI`, and `IsAppVeyor`. `BuildSystem` is in `ModularPipelines.Enums`.
The injectable `IBuildSystemDetector` is now internal; use `IBuildSystemContext` instead.

`IsBuildServer` remains, but it is now also `true` when the `CI` environment variable is set to a
truthy value, not only on a recognized build agent. `OnCI`, `OnLocal`, and `Require.Ci()` now share
this one definition, so they always agree. As a result, `CI=0` counts as local, and `CI=false`
no longer satisfies `Require.Ci()`. When the `CI` variable is the only reason a run counts as CI,
V4 logs this once at startup: `Treating this run as CI because CI=<value> and no known build agent
was detected.`

### Sub-modules

Sub-modules are still supported. You can still name and run them from runtime logic, for example
in a loop over discovered projects. Only the method name and the body signature changed:

```csharp
// V3
await context.SubModule("Upload", () => UploadAsync(cancellationToken));

// V4
await context.RunSubModuleAsync("Upload", token => UploadAsync(token), cancellationToken);
```

Both value-returning and non-value-returning overloads use token-aware bodies. Forward the token
supplied to the body into the operation. Each sub-module still appears by name in progress output.
Failures propagate their original exception; `SubModuleFailedException` has been removed.
`SubModuleBase`, the internal progress-tracking type, is no longer public; this does not affect
calls to `RunSubModuleAsync`. See [Sub-modules](./how-to/sub-modules.md).

## Tool Access and Command Execution

### Before (V3)

```csharp
await context.DotNet().Build(
    new DotNetBuildOptions { Configuration = "Release" },
    new CommandExecutionOptions { LogSettings = CommandLoggingOptions.Silent },
    cancellationToken: cancellationToken);
```

### After (V4)

```csharp
await context.Tools.DotNet.BuildAsync(
    new DotNetBuildOptions { Configuration = "Release" },
    new CommandExecutionOptions { Logging = CommandLoggingOptions.Silent },
    cancellationToken: cancellationToken);
```

Apply the tool-access change to Git, Docker, cloud CLIs, and other integrations. For example,
`context.Git().Information` becomes `context.Tools.Git.Information`. The generated `Tools`
properties use C# 14 extension members in `ModularPipelines.Context`; import that namespace.
The old per-integration extension imports are unnecessary. On C# 13 or another .NET language, resolve the service
with `context.Tools.Get<ModularPipelines.DotNet.Services.IDotNet>()`.

Command methods that return tasks now use `Async` suffixes. Rename `token:` to
`cancellationToken:` only when it names a changed ModularPipelines parameter. Some overloads
allow a token immediately after the tool options; a named argument makes the intended overload clear.

### Git

The Git integration changed beyond the accessor:

| V3 API | V4 API |
| --- | --- |
| `Git().Information.BranchName`, `LastCommitSha`, `Tag`, `DefaultBranchName`, `CommitsOnBranch`, `PreviousCommit`, `Root` (synchronous properties) | `(await Tools.Git.Information.GetRequiredInfoAsync(cancellationToken)).BranchName`, etc. |
| `Git().RootDirectory` | `(await Tools.Git.Information.GetRequiredInfoAsync(cancellationToken)).Root` |
| `Git().Commands.Commit(...)`, `Tag(...)`, `Checkout(...)` | `Tools.Git.Commands.Branches.CommitAsync(...)`, `TagAsync(...)`, `CheckoutAsync(...)` |
| `Git().Commands.Push(...)`, `Fetch(...)` | `Tools.Git.Commands.Remotes.PushAsync(...)`, `FetchAsync(...)` |
| `Git().Commands.Add(...)`, `Status(...)` | `Tools.Git.Commands.WorkingTree.AddAsync(...)`, `StatusAsync(...)` |
| `Git().Commands.Commits(...)` | `Tools.Git.Commands.History.CommitsAsync(...)` |
| `Git().Versioning.GetGitVersioningInformation()` | `Tools.Git.Versioning.GetVersioningInformationAsync(cancellationToken)` |

Commands are grouped into `Repository`, `WorkingTree`, `Branches`, `Remotes`, `History`, and
`Maintenance`; find any command not listed above in those groups.
`Tools.Git.Information.CommitsAsync(...)` returns the same commits as `Commands.History.CommitsAsync(...)`.
If upgrading an intermediate V4 build that exposed `IGitInformation.Commits(...)`, rename it to
`IGitInformation.CommitsAsync(...)`. In released V3.2.8, `Commits` existed only on the internal `GitInformation` implementation, not its public interface.

Repository information is now loaded once, asynchronously, and cached. Choose the method by
whether your pipeline can run outside a Git repository:

- `GetRequiredInfoAsync` throws `InvalidOperationException` when Git information is unavailable,
  for example outside a repository or without `git` installed. Use it when the pipeline always
  runs in a repository.
- `GetInfoAsync` returns `null` in that case. Use it when the pipeline must also work without Git.

Every `GitRepositoryInfo` property except `Root` is nullable. For example, `Tag` is `null` when no
tag exists, so handle missing values explicitly:

```csharp
var git = await context.Tools.Git.Information.GetRequiredInfoAsync(cancellationToken);
var branch = git.BranchName ?? "detached";
```

### Command Prompt

`context.Cmd().Script(options)` becomes `context.Tools.Cmd.RunAsync(options)`, with `RunFileAsync`
for script files. `ICmd` is replaced by `ICmdContext`, and `CmdScriptOptions` moved from
`ModularPipelines.Cmd.Models` to `ModularPipelines.Options`.

### Raw commands and scripts

| V3 API | V4 API |
| --- | --- |
| `context.Shell.Command.ExecuteCommandLineTool(options, executionOptions, token)` | `context.Shell.RunAsync(options, executionOptions, token)` |
| `GenericCommandLineToolOptions` / command-builder wrappers | `CommandLineToolOptions("tool")` or the raw `RunAsync` overload |
| `context.Shell.Bash.Command(options, token)` | `context.Shell.Bash.RunAsync(options, executionOptions, token)` |
| `context.Shell.Bash.FromFile(options, token)` | `context.Shell.Bash.RunFileAsync(options, executionOptions, token)` |
| `context.Shell.PowerShell.Script(options, token)` | `context.Shell.PowerShell.RunAsync(options, executionOptions, token)` |
| `context.Shell.PowerShell.FromFile(options, token)` | `context.Shell.PowerShell.RunFileAsync(options, executionOptions, token)` |
| `PowershellOptions`, `PowershellScriptOptions`, `PowershellFileOptions` | `PowerShellOptions`, `PowerShellScriptOptions`, `PowerShellFileOptions` |

Pass raw arguments as separate tokens:

```csharp
await context.Shell.RunAsync(
    "dotnet", ["build", "My App.csproj", "--configuration", "Release"],
    cancellationToken: cancellationToken);
```

For scripts, execution settings belong in the separate `CommandExecutionOptions` parameter,
as with tool integrations. Custom integration implementations can still use
`ICommandContext.ExecuteCommandLineToolAsync`; the module-facing convenience API is `Shell.RunAsync`.

CliWrap types are no longer part of the consumer contract. Use
`ModularPipelines.Options.CommandLineCredentials` for command credentials. For synthetic results
in tests or interceptors, use the new `CommandResult.Ok("output")` or the explicit constructor.

`CommandExecutionOptions.EnvironmentVariables` is now `IReadOnlyDictionary<string, string?>`; build
the dictionary before assigning it rather than mutating it afterwards.
`CommandExecutionOptions.ToCommandLineToolOptions(...)` is removed.

## Generated CLI Options and Custom Commands

V4 regenerates integrations from current CLI help. Command names, option
property names and types, enum values, required inputs, and subcommand grouping can all differ.
Migrate against the **installed V4 integration's** options and service signatures, not a universal
text replacement. Use the [CLI command catalogs](./mp-packages/cli/dotnet.md) and the corresponding
catalog for each tool to locate its current options type.

Update consumer calls using the regenerated contract and validate the resulting command line.
Required arguments and option combinations are
now validated before process execution; invalid options raise `CommandOptionsValidationException`.
Do not hand-edit generated models or add compatibility shims to preserve old generated APIs.

For handwritten command options, see [Custom Commands](./how-to/custom-commands.md) for the
`CliTool` / `CliSubCommand` identity attributes and argument-ordering rules. For missing generated
commands or incorrect models, fix or report the scraper/generator rather than working around it
in the generated source.

### .NET test runner options

`DotNetTestOptions` now follows the selected SDK's default VSTest help, independent
of this repository's test-runner configuration. Use its `Filter`, `Logger`, `Collect`,
`Settings`, and `Blame` properties for VSTest. The generated MTP-only `Project`,
`Solution`, `TestModules`, `PlatformOptions`, and `ExtensionOptions` members are removed.
For Microsoft.Testing.Platform, keep its `global.json` runner selection and pass
runner-specific switches through the existing `Arguments` property:

```csharp
new DotNetTestOptions
{
    Arguments = ["--project", "Tests.csproj", "--", "--filter", "Category=Unit", "--", "--report-trx"],
    ArgumentsContainOptionTerminator = true,
};
```

Generation does not change the runner selected when your command executes.
`DotNetPackOptions.Version` accepts a package-version string; `Verbosity`,
`SelfContained`, and `UseCurrentRuntime` are available where the CLI supports them.

### Handwritten command attributes

| V3 | V4 |
| --- | --- |
| `[CliCommand("tool", "sub", ...)]` | `[CliTool("tool")]` plus `[CliSubCommand("sub", ...)]` |
| `CliOptionAttribute.AllowMultiple` | Removed; collections repeat the option for each value, as before. Set `GroupValues = true` for one occurrence, or `CollectionSeparator` to join values |
| `CliOptionAttribute.CustomSeparator` | `Format` (`SpaceSeparated`, `EqualsSeparated`, `ColonSeparated`, `NoSeparator`); arbitrary separators are no longer supported |
| `CliOptionAttribute.GetSeparator()`, `GetEffectiveName()` | Removed |
| `CommandLineToolOptions.CommandParts` as `string[]?` | `IReadOnlyList<string>?` |
| `CliSubCommandAttribute.SubCommands`, `CliCommandAliasAttribute.CommandParts` as `string[]` | `IReadOnlyList<string>` |

`CommandLineToolOptions` is no longer abstract; `new CommandLineToolOptions("tool")` describes an
ad-hoc command.

`CliArgumentAttribute.Placement` and the `ArgumentPlacement` enum are replaced by
`CliArgumentAttribute.Phase` (`CommandLinePhase`):

| V3 | V4 |
| --- | --- |
| `Placement = ArgumentPlacement.AfterOptions` (the V3 default) | `Phase = CommandLinePhase.Passthrough` (the V4 default) |
| `Placement = ArgumentPlacement.BeforeOptions` | `Phase = CommandLinePhase.EarlyOperand` |
| `Placement = ArgumentPlacement.ImmediatelyAfterCommand` | `Phase = CommandLinePhase.EarlyOperand`; it renders after the final subcommand |

V3 rendered `ImmediatelyAfterCommand` arguments before `BeforeOptions` arguments. Both now map to
`EarlyOperand`, which orders by position only, so check argument positions when a type used both.

`CliArgumentAttribute.Name` and `<PLACEHOLDER>` substitution in command parts are removed. Every
`[CliArgument]` value renders as a positional argument. When a command chain depends on constructor
input, set `CommandParts` in the constructor instead:

```csharp
// V3: the argument replaced <ACTION>, or disappeared if it did not match.
[CliCommand("tool", "resource", "<ACTION>")]
public record ResourceOptions(
    [property: CliArgument(0, Name = "<ACTION>")] string Action)
    : CommandLineToolOptions;

// V4
[CliTool("tool")]
public record ResourceOptions : CommandLineToolOptions
{
    public ResourceOptions(string action)
    {
        CommandParts = ["resource", action];
    }
}
```

Generated key-value options are now typed `IReadOnlyList<KeyValue>?`; collection expressions and
tuple conversions still compile, but code that depends on `KeyValue[]` or `IEnumerable<KeyValue>`
must change. Two-value options use `CliValuePair`, which only supports the space separator.

### Source generation and private integrations

Custom command-option types require generated runtime metadata. The core NuGet package includes
the generator; ensure it runs in projects declaring your options and do not exclude its analyzer
assets. Rebuild shared libraries that declare command options against V4 so they carry V4
metadata.

The process-wide `ModularPipelinesContextRegistry` and parameterless registration module initializers
are removed. Mark an accessible static service-registration method with
`[ModularPipelinesIntegration]`, accepting `IServiceCollection`, or regenerate the integration with
the current generator. Follow the [private integration migration](./how-to/generate-private-cli-integration.md#migrate-custom-integration-registration-for-v4)
for the complete registration and `Tools` metadata pattern.

## Logging and HTTP

### Logging options

| V3 | V4 |
| --- | --- |
| `CommandExecutionOptions.LogSettings` | `CommandExecutionOptions.Logging` |
| `CommandLoggingOptions.IncludeTimestamps` | `CommandLoggingOptions.ShowTimestamps` |
| `CommandLogVerbosity.Minimal` (documented as "errors and warnings") | `CommandLogVerbosity.InputOnly`, which logs command input only. Choose the level by the output you need |
| `HttpOptions.LogSettings` | `HttpOptions.Logging` |
| `HttpOptions.LoggingType` / `HttpLoggingType` flags | `HttpLoggingOptions` booleans or presets |
| `DownloadOptions.LoggingType` | `DownloadOptions.Logging` with `HttpLoggingOptions` |
| `context.Logger` typed as `IModuleLogger` | Standard `ILogger`; rich console output goes through `context.Console` |
| `IModuleLoggerProvider` | `IModuleLoggerAccessor` for advanced logger access |
| `IConsoleWriter.LogToConsole(markup)` | `context.Console.WriteMarkupLine(markup)`; use `WriteLine` for literal text |

Use `context.Console.WriteLine`, `WriteMarkupLine`, or `Write(IRenderable)` for module-aware
console output. Normal `ILogger` calls stay as they are. Console output and structured logging
participate in module grouping and secret masking.
`IConsoleWriter` now lives in `ModularPipelines.Logging`. Update injected writer implementations
to provide `WriteLine`, `WriteMarkupLine`, and `Write`; do not dispose `context.Logger` yourself.
Code using `IModuleLoggerProvider.GetLogger()` should resolve `IModuleLoggerAccessor` and read
its `Logger` property instead. `IHttpLogger`, `ICommandLogger`, and `IExceptionOutputFormatter`
are no longer public; configure output through the logging options above.
`context.Summary.Info(...)` becomes `context.Summary.Information(...)`. Reading buffered summary
entries/output is separated into `ISummaryLogReader`; resolve that service for `GetEntries(...)`
or `GetOutput()` instead of calling them on `ISummaryLogger`.

### HTTP execution

`context.Network.Http.HttpClient` and `GetLoggingHttpClient(...)` are removed. Send requests through
`context.Network.Http.SendAsync` so pipeline logging, timeout, and resilience settings apply:

```csharp
using var response = await context.Network.Http.SendAsync(
    new HttpOptions(new HttpRequestMessage(HttpMethod.Get, endpoint))
    {
        Logging = HttpLoggingOptions.Minimal,
        Timeout = TimeSpan.FromSeconds(30),
    },
    cancellationToken);
```

`HttpOptions.ThrowOnNonSuccessStatusCode` now defaults to `true`, matching commands
and downloads. Non-success responses throw `PipelineHttpResponseException`, which
includes the status code and response content when available. This also applies to
the implicit string, `Uri`, and `HttpRequestMessage` conversions to `HttpOptions`.
If your pipeline intentionally handles failure responses, opt out explicitly:

```csharp
using var response = await context.Network.Http.SendAsync(
    new HttpOptions(new HttpRequestMessage(HttpMethod.Get, endpoint))
    {
        ThrowOnNonSuccessStatusCode = false,
    },
    cancellationToken);
```

Download helpers continue to require a successful response. Use `SendAsync` with
the explicit opt-out when you need to inspect a failure response body.

If a custom client is needed, inject one through your application DI and pass it in
`HttpOptions.HttpClient`. The pipeline's resilience (retry) handler applies only to the default
client, so configure retries on a custom client yourself. Replace old logging flags with `LogRequest`, `LogResponse`,
`LogStatusCode`, `LogDuration`, and the header/body switches. Use `HttpLoggingOptions.None` to
disable HTTP logging for a request. HTTP responses stream their bodies; dispose responses and
pass cancellation tokens to body-reading operations too.

## Files, Working Directories, and Utility APIs

### File paths and working directories

`ModularPipelines.FileSystem.File` and `Folder` are renamed to `FilePath` and `FolderPath`.
Their usual read/write/copy operations remain available. Update aliases and signatures, including
custom result DTOs. Do not rename `System.IO.File` or `System.IO.Directory` calls.

`context.Environment.WorkingDirectory` is now a read-only `FolderPath`. Set the pipeline default
with `PipelineBuilderSettings.WorkingDirectory`, or set `CommandExecutionOptions.WorkingDirectory`
for one invocation. Commands and context-relative file operations use the pipeline's directory
without changing the process-wide current directory. Plain `new FilePath(relativePath)` and
`new FolderPath(relativePath)`, implicit string conversions, and `System.IO` operations
do not acquire that context automatically: relative paths use the process current directory.

Use `context.Files.GetFile` and `context.Files.GetFolder` for pipeline-relative paths.
Resolve copy/move destinations too: even a path object obtained from the files context
does not make a later relative string destination pipeline-relative.

```csharp
var source = context.Files.GetFile("input.txt");
var output = context.Files.GetFolder("artifacts");
output.Create();
var destination = context.Files.GetFile("artifacts/copy.txt");
await source.CopyToAsync(destination.Path, cancellationToken);
```

The same rule applies to synchronous methods and folder copy/move operations. Absolute
destinations preserve their location. Standalone path types keep their existing resolution
behavior; do not change the shared process directory to make concurrent pipelines agree.

V4's default directory can derive from the content root or calling source project's directory;
make it explicit if V3 depended on the directory from which the process was launched.

### Utility replacements

| V3 API | V4 API |
| --- | --- |
| `Environment.Variables.GetEnvironmentVariable(...)` | `Environment.Variables.Get(...)` |
| `Environment.Variables.GetEnvironmentVariables(...)` (`IDictionary<string, string>`) | `Environment.Variables.GetAll(...)` (`IReadOnlyDictionary<string, string?>`; copy it before modifying) |
| `Environment.Variables.SetEnvironmentVariable(...)` | `Environment.Variables.Set(...)` |
| `context.Files.Checksum.Md5(path)` | `context.Security.Hash.Md5File(path)` |
| `context.Security.Hasher.Sha256(text)` | `context.Security.Hash.Sha256(text)` |
| `HashType` | `HashEncoding` |
| `context.Data.Base64.FromBase64String(value, encoding)` | `encoding.GetString(context.Data.Base64.FromBase64String(value))` |
| `context.Files.Zip.ZipFolder(...)` | `context.Files.Zip.CreateFromDirectory(...)` |
| `context.Files.Zip.UnZipToFolder(...)` | `context.Files.Zip.ExtractToDirectory(...)` |

Choose `*File` hashing methods for file contents; a text hash of the path is a different result.
V3's `Checksum.Md5` returned uppercase hex; `Md5File` returns lowercase hex, so compare hashes
case-insensitively. Mocks of `IHasherContext` / `IChecksumContext` move to `IHashContext`.
Base64 decoding now returns bytes, so select the text encoding explicitly when text is needed.

The general-purpose `AsTask()` and `AsSkipDecisionIfTrue(reason)` extensions are removed. Use
`Task.FromResult(value)` and `SkipDecision.When(condition, reason)`. Use the .NET async-LINQ
`FirstOrDefaultAsync` instead of the removed framework convenience extension.
`OperatingSystemHelper` / `OperatingSystemIdentifier` are removed; use
`context.Environment.OperatingSystem` (`OSPlatform`), .NET's `OperatingSystem.IsWindows()` and
equivalents, or the built-in module run conditions.
Implementations of the old standalone `IEnvironmentContext` should also replace
`OperatingSystemVersion` / `Is64BitOperatingSystem` with the corresponding .NET
`Environment.OSVersion.Version` / `Environment.Is64BitOperatingSystem` APIs, and use
`context.Files.GetFolder(specialFolder)` instead of its removed `GetFolder` method.

`FilePath.GetStream` no longer uses `FileMode.OpenOrCreate` for every access mode:

| Access | V3 | V4 |
| --- | --- | --- |
| `FileAccess.Read` | Created a missing empty file | `FileMode.Open`; throws `FileNotFoundException` for a missing file |
| `FileAccess.Write` | Opened without truncating | `FileMode.Create`; truncates an existing file |
| `FileAccess.ReadWrite` | `OpenOrCreate` | `OpenOrCreate` (unchanged) |

Callers that need a particular creation or truncation mode should open a stream with an explicit
`FileMode`.

### Installers

The nested `Installers.File`, `.Windows`, `.Linux`, `.Mac`, and `.Predefined` surfaces are removed.
`Installers.File.InstallFromFileAsync(...)` becomes
`context.Installers.InstallAsync(new InstallerOptions(path) { Arguments = [...] }, token)`, and
`Installers.File.InstallFromWebAsync(...)` becomes `context.Installers.InstallFromWebAsync(WebInstallerOptions, token)`.
Move package-manager-specific work to the corresponding integration or an explicit shell command.
There is no universal replacement for every old predefined installer: preserve its download,
platform, and unattended-install behavior deliberately.

## Exceptions and Extension Points

| V3 type | V4 replacement |
| --- | --- |
| `PipelineCancelledException` | `PipelineCanceledException` |
| `ModuleReferencingSelfException` | `ModuleSelfDependencyException` |
| `FailedRequirementsException` | `RequirementNotMetException` |
| `HttpResponseException` | `PipelineHttpResponseException` |
| `SubModuleFailedException` | Catch the original sub-module exception |
| `AlwaysRunPostponedException` | Removed |

Existing `CommandException` members move to that result: `ExitCode` becomes `Result.ExitCode`,
`StandardOutput` / `StandardError` become `Result.StandardOutput` / `Result.StandardError`, and
`ExecutionTime` becomes `Result.Duration`. Construct custom command exceptions with a
`CommandResult`, rather than the removed individual output/exit-code constructor parameters.

Review your `catch` blocks. Several exceptions changed type or base class:

- A missing executable throws `ToolNotFoundException`, which is a `CommandException`.
- Validation errors have their own types, such as `CommandOptionsValidationException` and
  `PipelineValidationException`.
- A pipeline with no registered modules fails with a `PipelineException`.
- `PipelineCanceledException` now derives from `OperationCanceledException`, so
  `catch (PipelineException)` no longer catches it. Catch `OperationCanceledException` for ordinary
  cancellation.
- `PipelineHttpResponseException` derives from `HttpRequestException`, so it is not a
  `PipelineException` either. Its inherited `StatusCode` is nullable.

```csharp
try
{
    await context.Shell.RunAsync("tool", ["argument"], cancellationToken: cancellationToken);
}
catch (CommandException exception)
{
    context.Logger.LogError("Exit code: {ExitCode}; stderr: {Error}",
        exception.Result.ExitCode, exception.Result.StandardError);
    throw;
}
```

For commands whose nonzero exit codes are expected data, set
`CommandExecutionOptions.ThrowOnNonZeroExitCode = false` and inspect the returned `ExitCode`.

Engine execution state, schedulers, awaiter completion, and configuration internals are no longer
general-purpose public extension points. Use event handlers, reporting interfaces,
`IExecutionBackend`, or the [ModularPipelines.Testing harness](./how-to/testing.md), depending on
the intended extension. Do not recreate removed internal types as compatibility shims.

`IModule.ModuleRunType` and the `ModuleRunType` enum are removed. Configure `.WithAlwaysRun()`
for cleanup and inspect `module.Configuration.AlwaysRun` when reading that policy. A module's
dependencies must be declared before execution, not injected dynamically through scheduler APIs.
Move `ITaggedModule` implementations and registration-time metadata customization to module
configuration or the supported attributes. `ModuleRegistrationOptions`, `DeclaredDependency`, and the
`DependencyType` and `WaitResult` enums are removed with the old registration and scheduling APIs.
`DependsOnAttribute<T>` no longer derives from `DependsOnAttribute`, so reflection over
`DependsOnAttribute` alone misses generic declarations.

`ModuleActivityTracing` is internal; use the `PipelineTelemetry` constants for activity source and tag
names, which are unchanged. `GitHubPipelineFileWriter` is internal, and
`GitHubPipelineFileWriterOptions.RunnerOperatingSystem` is removed (use `Runner`).

Custom result repositories and time estimators implement the interfaces in
`ModularPipelines.Reporting` and register with `builder.AddResultsRepository<T>()` and
`builder.AddModuleEstimatedTimeProvider<T>()`. Keep repository enablement (`IsEnabled`), generic
result types, and the new result metadata consistent when updating stored data. Legacy private
test helpers should move to `ModularPipelines.Testing`; use command interception to supply
`CommandResult.Ok(...)` without launching real tools.

Other custom extension points changed shape:

| V3 contract | V4 contract |
| --- | --- |
| `IModuleEstimatedTimeProvider` methods | Same methods with a trailing `CancellationToken`; estimates are best-effort and never fail a module |
| `IModuleResultRepository.SaveResultAsync` / `GetResultAsync` | Take a `CancellationToken`; `IsEnabled` defaults to `true` |
| `IPipelineValidator.Validate(IServiceProvider)` | `ValidateAsync(IServiceProvider, CancellationToken)`; `Order` defaults to `0` |
| `ISecretObfuscator` | Internal; supply secrets through `ISecretRegistry` or `[SecretValue]`, and configure masking with `PipelineOptions.Secrets` |
| Custom `IFileSystemProvider` | Also implement `GetAttributes`/`SetAttributes` and the `Get`/`Set` creation, last-access, and last-write UTC time members; several other members now have default implementations |
| Custom `IModule` implementations | Not supported; derive from `Module<T>` or `SyncModule<T>` |
| Subclasses of `DependsOnAttribute`, `DependsOnAttribute<T>`, `DependsOnAllModulesInheritingFromAttribute`, or `SecretValueAttribute` | These attributes are sealed; declare dependencies in `Configure(module)` or use a registration handler |

Analyzer IDs are consolidated under `MP####`. Update `.editorconfig`, `NoWarn`, and `#pragma`
entries using the [analyzer ID migration table](./how-to/analyzers.md#id-migration). Source-generator
diagnostics use the separate `MPG####` family; fix missing/incompatible metadata rather than suppressing it.

## Runtime Behavior Changes

These changes need behavioral checks even after the code compiles:

| Change | Migration action |
| --- | --- |
| Required dependencies that are skipped now cascade-skip their consumers | Mark a dependency optional only if the consumer can actually proceed without its result; handle missing/skipped results explicitly |
| Fluent and attribute conditions share the execution pipeline after dependency waiting | Verify condition ordering, skip hooks, and inherited condition composition |
| Dependencies are validated even when a condition would skip a module | Register required dependencies or use the intended conditional/optional declaration |
| Commands keep a 30-minute default `ExecutionTimeout`, but `ExecutionTimeout = null` now removes the limit (V3 treated `null` as 30 minutes) | Set a longer per-command `ExecutionTimeout`, or `null` for no command limit. The `TimeoutException` names the limit and this option. The module default is still 30 minutes, now configurable with `DefaultModuleTimeout` |
| Module timeouts apply to each retry attempt, not the whole retry sequence | Include attempts and backoff in the total runtime budget; forward cancellation into work |
| `WithTimeout(TimeSpan.Zero)` meant "no timeout" in V3; V4 rejects zero and negative values | Use `WithTimeout(Timeout.InfiniteTimeSpan)` to disable a module timeout |
| Registration helpers for single-instance services replace earlier registrations; multi-instance helpers add each type once | Check that repeated `Add*` calls, such as a results repository or time estimator, register the implementation you expect |
| Event handler failures follow fixed rules (see [Lifecycle Hooks](#lifecycle-hooks-and-event-handlers)) | Set `ContinueOnError` on handlers that must not fail the module or pipeline |
| Command results capture at most 1,048,576 characters per output stream by default | If parsing complete large output, set `MaxCapturedOutputLength` appropriately; `0` or a negative value requests unlimited capture |
| Canceled commands terminate descendant processes after graceful cancellation | Remove assumptions that child processes survive a canceled pipeline |
| Always-run cleanup is scheduled after failures/cancellation, with bounded progress waits | Test teardown under failure and cancellation; keep cleanup bounded and idempotent |
| Category matching and selection are validated more strictly | Check configured category/module selectors instead of silently accepting typos |
| Secrets are masked in more output surfaces and transformed forms | Test redaction; do not parse masked diagnostic command input/environment as raw execution data |
| `Md5File` returns lowercase hex | Compare hashes case-insensitively |
| `IsBuildServer`, `OnCI`, and `Require.Ci()` also honor a truthy `CI` variable | Check local runs that set `CI`; the startup log says when `CI` alone decided |

The output capture limit counts characters, not bytes. When a stream exceeds
`MaxCapturedOutputLength`, its captured string preserves the beginning and end, inserting
`... [truncated N characters] ...` between them. The marker adds characters beyond the configured
capture limit. `CommandResult.StandardOutputTruncatedCharacters` and
`CommandResult.StandardErrorTruncatedCharacters` report the omitted character count for each
stream; zero means no characters were omitted. These counts are also available on
`CommandException.Result`. A warning names `MaxCapturedOutputLength` and reports both counts
when output is truncated. Do not parse capped output as complete JSON, XML, or another
machine-readable document: raise the limit or set `MaxCapturedOutputLength = 0` when complete
capture is required and its memory cost is acceptable.

Required result access, read-only options, generated command validation, and pipeline-scoped working
directories described above also change behavior. Include them in migration acceptance tests.

## New Features in V4

These are additions in V4; adopting them is not required to migrate:

- [Pipeline CLI](./how-to/command-line.md): selection, validation, and dry-run planning.
- [Fingerprint-based caching](./how-to/module-caching.md), with a pipeline-wide cache disable switch.
- [Run reports and local history](./how-to/run-reports.md), including correlation, duration comparisons, and bounded output excerpts.
- [OpenTelemetry](./how-to/opentelemetry.md), dependency graph export, and richer CI output.
- [Testing helpers](./how-to/testing.md) and [F# module support](./how-to/fsharp.md).
- [Trimming and Native AOT support](./how-to/native-aot.md) for supported statically described C# pipelines.
- [Distributed execution](./distributed/architecture.md), artifact contracts, worker capabilities, and execution backends.
  Distributed packages are not in V3.2.8; if you used them from an intermediate development build, see the
  distributed sections of the [V4 release notes](https://github.com/thomhurst/ModularPipelines/blob/main/RELEASE_NOTES_V4.md).
- Additional CLI integrations and substantially expanded command coverage in existing integrations.
- Command interceptors: `ModularPipelines.ICommandInterceptor` wraps command execution as middleware through
  `InvokeAsync(invocation, next, cancellationToken)`. Register one with
  `builder.AddCommandInterceptor<T>()` or `AddCommandInterceptor(instance)`; see [Testing](./how-to/testing.md).
- Planning-safe conditions: mark a condition or attribute with the `IPlanningSafe` marker interface
  when dry-run planning may evaluate it; see [Run conditions](./how-to/run-conditions.md).
- `OnCachedResultAsync` module hook, `ModuleResult<T>.TryGetValue`, fluent `WithExecutionHint`,
  `WithPriority`, and `WithNotInParallel`, and `git.Changes.HasChangesAsync`.


## Complete Migration Example

The following pipeline restores and builds a project. It demonstrates entry-point configuration,
module dependencies, retries, and the tool API together.

### Before (V3)

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

### After (V4)

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

## Breaking API Reference

| V3 API | V4 API | Review needed |
| --- | --- | --- |
| `builder.Services.AddModule<T>()` | `builder.AddModule<T>()` | Preserve conditional registration and factories |
| `builder.Build()` | `await builder.BuildAsync()` | Dispose the pipeline with `await using` |
| `builder.ExecutePipelineAsync()` | `builder.RunAsync()` | Await completion |
| `builder.Options.X = value` | `builder.ConfigureOptions(o => o with { X = value })` | Nested settings moved |
| `Configure(): ModuleConfiguration` | `Configure(ModuleConfigurationBuilder): void` | Preserve inherited configuration |
| `DeclareDependencies(...)` | Dependencies in `Configure(module)` | Keep required/optional/conditional distinctions |
| `WithRetryCount` / `WithRetryPolicy` | `WithRetry` / `WithShield` | Preserve policy semantics |
| `Module<T>` returning `Task<T?>` | `Task<T>`, or explicitly `Module<T?>` | Decide whether null is valid |
| `ExecuteModuleAsync` / `ExecuteModule` | `ExecuteAsync` / `Execute` | Non-generic module overrides only |
| Result `IsSuccess` / `IsFailure` / `IsSkipped` | Typed variants or safe accessors | Match the result's actual generic type |
| `summary.GetModuleResultsAsync()` | `summary.Results` | No live module handles |
| `IPipelineGlobalHooks` / `IPipelineModuleHooks` | `IPipelineEventHandler` / `IModuleEventHandler` | Update callback signatures |
| `MustAsync` / `Must` | `EvaluateAsync(context, cancellationToken)` | Return explicit requirement decisions |
| `context.SubModule(...)` | `context.RunSubModuleAsync(...)` | Forward the body token |
| `context.DotNet()` and other tool accessors | `context.Tools.DotNet`, etc. | Upgrade integration and generator together |
| Tool command `Build(...)`, etc. | `BuildAsync(...)`, etc. | Inspect regenerated signatures |
| `context.Shell.Command.ExecuteCommandLineTool(...)` | `context.Shell.RunAsync(...)` | Pass individual argument tokens |
| `token:` | `cancellationToken:` | Only changed framework parameters |
| `LogSettings` | `Logging` | Command and HTTP execution options |
| Framework `File` / `Folder` | `FilePath` / `FolderPath` | Keep `System.IO` symbols intact |
| `PipelineCancelledException` | `PipelineCanceledException` | Derives from `OperationCanceledException`; V4 spells "canceled" with one L throughout |
| `IEventHandlerPriority.Priority` | `IEventHandler.Order` | Ascending order; add the trailing `CancellationToken` to handler methods |
| `PluginRegistry.Register(plugin)` | `builder.AddPlugin(plugin)` | Merge `ConfigureServices` and `ConfigurePipeline` into `Configure(PipelineBuilder)` |
| `[RunIfAll<...>]` | `[RunIf<...>]` | Same AND semantics |
| `MandatoryRunConditionAttribute` / `RunConditionAttribute.Condition` | `RunConditionAttribute(ConditionIntent.Run)` / `EvaluateAsync` | Use `GroupKey` for OR-ed alternatives |
| `BuildSystem.IsGitHubActions`, etc. | `BuildSystem.Is(BuildSystem.GitHubActions)`, etc. | `IsBuildServer` remains but also honors a truthy `CI` variable |
| `[CliArgument(Placement = ...)]` | `[CliArgument(Phase = ...)]` | `BeforeOptions` becomes `EarlyOperand`; the default is `Passthrough` |
| `IPipelineValidator.Validate` | `ValidateAsync(services, cancellationToken)` | Return `Task<ValidationResult>` |
| `[CliCommand("tool", ...)]` | `[CliTool("tool")]` + `[CliSubCommand(...)]` | Handwritten option types only |
| `Git().Information.BranchName`, etc. | `(await Tools.Git.Information.GetRequiredInfoAsync(ct)).BranchName` | Use `GetInfoAsync` if Git may be unavailable; properties can be null |
| `CommandLogVerbosity.Minimal` | `CommandLogVerbosity.InputOnly` | Logs command input only; pick the level you need |
| `summary.Start` / `End` / `TotalDuration` | `StartTime` / `EndTime` / `Duration` | `PipelineSummary` is now sealed |
| `summary.Status` | `summary.Succeeded` | Boolean overall outcome; inspect `Failures` for details |
| `summary.GetFailedModuleResults()` | `summary.Failures` and `summary.IgnoredFailures` | V3 returned both kinds together |
| `Git().Commands.Push(...)`, etc. | `Tools.Git.Commands.Remotes.PushAsync(...)`, etc. | Commands are grouped by area |
| `DependsOnLazy<T>()` | `DependsOnOptional<T>()` | Same scheduling behavior |
| `WithTimeout(TimeSpan.Zero)` | `WithTimeout(Timeout.InfiniteTimeSpan)` | Zero now throws |
| `CommandException.ExitCode`, etc. | `CommandException.Result.ExitCode`, etc. | `ExecutionTime` becomes `Result.Duration` |
| `Status.Successful`, etc. | `ModuleStatus.Succeeded`, etc. | See the status table |

## Agents / LLM Migration Reference

This section is intended for coding agents automating a consumer's migration. Use the consumer's
current V3 code and the installed V4 package APIs as evidence. Do not infer the V3 API from current
documentation examples.

### Migration workflow

1. Inventory all `ModularPipelines*` package references, shared module libraries, custom integrations,
   source-generator settings, global usings, aliases, analyzer suppressions, and target frameworks.
2. Select one compatible V4 package set. Upgrade and rebuild shared projects so their generated
   metadata matches the host. Preserve the consuming repository's SDK and validation instructions.
3. Update resolved symbols and entry points first. Keep application DI on `builder.Services` and
   move only framework registration calls to the pipeline builder.
4. Rewrite configuration overrides and immutable options. Preserve dependencies, base configuration,
   conditions, timeouts, retry intent, lifecycle ordering, and failure policy.
5. Update result contracts, integration calls, and generated option properties. Make a list of
   choices requiring intent: nullable outputs, optional dependencies, skip composition, custom Polly
   policies, CLI option types, installer behavior, and exception handling.
6. Build the affected pipeline and shared libraries. Resolve compiler and generator errors against
   the installed APIs. Use the compiler to resolve overloads rather than suppressing diagnostics.
7. Inspect a plan/validation run where appropriate. Planning can still construct services and
   evaluate planning-safe conditions; it is not a substitute for command and behavior tests.
8. Test representative commands without external side effects, including exact argument tokens,
   skip/failure/cancellation behavior, cleanup, working directories, output parsing, and redaction.
   Report remaining manual decisions and validation limits before running publishing/deployment work.

### API transformation map

These are **symbol-scoped** transformations, not global string substitutions:

```yaml
transformations:
  - old: PipelineBuilderOptions
    new: PipelineBuilderSettings
  - old: builder.Services.AddModule<T>()
    new: builder.AddModule<T>()
    scope: ModularPipelines registration extensions only
  - old: builder.Build()
    new: await builder.BuildAsync()
    follow_up: Own the returned pipeline with await using
  - old: builder.ExecutePipelineAsync()
    new: builder.RunAsync()
  - old: ExecutionMode.StopOnFirstException
    new: FailureMode.FailFast
  - old: ExecutionMode.WaitForAllModules
    new: FailureMode.ContinueOnFailure
  - old: ModuleConfiguration Configure()
    new: void Configure(ModuleConfigurationBuilder module)
    follow_up: Configure the supplied builder; remove Create and Build; preserve base configuration
  - old: DeclareDependencies(IDependencyDeclaration dependencies)
    new: Move declarations into Configure(module)
    follow_up: Preserve DependsOnOptional and DependsOnIf semantics
  - old: WithRetryCount(count)
    new: WithRetry(count)
    review: Delays and exception filters
  - old: SkipDecision.Of(condition, reason)
    new: SkipDecision.When(condition, reason)
  - old: IPipelineHookContext
    new: IPipelineContext
    scope: Requirements, run conditions, and pipeline event handlers
  - old: context.Services.Get<T>()
    new: context.Services.GetRequiredService<T>()
  - old: context.TryGetService<T>()
    new: context.Services.GetRequiredService<T>()
    review: Use context.Services.GetService<T>() with null handling only where absence is allowed
  - old: context.SubModule(name, body)
    new: context.RunSubModuleAsync(name, token => bodyUsing(token), cancellationToken)
  - old: context.DotNet().Build(options, executionOptions, token)
    new: context.Tools.DotNet.BuildAsync(options, executionOptions, token)
    scope: Repeat using the actual accessor and service method for each integration
  - old: context.Shell.Command.ExecuteCommandLineTool(options, executionOptions, token)
    new: context.Shell.RunAsync(options, executionOptions, token)
  - old: LogSettings
    new: Logging
    scope: CommandExecutionOptions and HttpOptions
  - old: ModularPipelines.FileSystem.File
    new: ModularPipelines.FileSystem.FilePath
  - old: ModularPipelines.FileSystem.Folder
    new: ModularPipelines.FileSystem.FolderPath
  - old: summary.GetModuleResultsAsync()
    new: summary.Results
    follow_up: Remove the await for this access only
  - old: IEventHandlerPriority.Priority
    new: IEventHandler.Order
    follow_up: Add a trailing CancellationToken to every handler method
  - old: PluginRegistry.Register(plugin)
    new: builder.AddPlugin(plugin)
    follow_up: Merge ConfigureServices and ConfigurePipeline into Configure(PipelineBuilder builder)
  - old: RunIfAllAttribute<T1, ..., T4>
    new: RunIfAttribute<T1, ..., T4>
  - old: MandatoryRunConditionAttribute
    new: RunConditionAttribute with base(ConditionIntent.Run)
    follow_up: Rename Condition(IPipelineHookContext) to EvaluateAsync(IPipelineContext, CancellationToken)
  - old: context.Environment.BuildSystem.IsGitHubActions
    new: context.Environment.BuildSystem.Is(BuildSystem.GitHubActions)
    scope: Repeat for each per-system flag; IsBuildServer remains but also honors a truthy CI variable
  - old: CliArgument Placement = ArgumentPlacement.BeforeOptions
    new: CliArgument Phase = CommandLinePhase.EarlyOperand
    scope: Handwritten option types only; AfterOptions becomes the default Passthrough
  - old: CliCommand(tool, subcommands...)
    new: CliTool(tool) + CliSubCommand(subcommands...)
    scope: Handwritten option types only
  - old: PipelineCancelledException
    new: PipelineCanceledException
    review: It now derives from OperationCanceledException, not PipelineException
  - old: ModuleReferencingSelfException
    new: ModuleSelfDependencyException
  - old: FailedRequirementsException
    new: RequirementNotMetException
  - old: HttpResponseException
    new: PipelineHttpResponseException
  - old: CommandException.ExitCode / StandardOutput / StandardError / ExecutionTime
    new: CommandException.Result.ExitCode / StandardOutput / StandardError / Duration
  - old: IPipelineValidator.Validate(services)
    new: IPipelineValidator.ValidateAsync(services, cancellationToken)
  - old: ModularPipelines.Enums.Status
    new: ModularPipelines.ModuleStatus
    follow_up: Map NotYetStarted, Processing, Successful, IgnoredFailure, UsedHistory per the status table
  - old: WithTimeout(TimeSpan.Zero)
    new: WithTimeout(Timeout.InfiniteTimeSpan)
  - old: DependsOnLazy<T>()
    new: DependsOnOptional<T>()
  - old: context.Git().Information.<Property>
    new: (await context.Tools.Git.Information.GetRequiredInfoAsync(cancellationToken)).<Property>
    review: Use GetInfoAsync with null handling if the pipeline can run without Git; properties are nullable
  - old: CommandLogVerbosity.Minimal
    new: CommandLogVerbosity.InputOnly
    review: V3 documented Minimal as errors and warnings; V4 InputOnly logs command input only
  - old: summary.GetFailedModuleResults()
    new: summary.Failures
    review: Add summary.IgnoredFailures where the V3 code also needed ignored failures
  - old: context.Git().Commands.<Command>(...)
    new: context.Tools.Git.Commands.<Group>.<Command>Async(...)
    scope: Groups are Repository, WorkingTree, Branches, Remotes, History, Maintenance
```

### Common compiler errors and fixes

| Symptom | Likely fix |
| --- | --- |
| `Configure`: no suitable method to override | Use `protected override void Configure(ModuleConfigurationBuilder module)` |
| `ModuleConfiguration.Create` / `.Build` unavailable | Use the supplied configuration builder |
| Init-only option assignment fails | Use `ConfigureOptions` and nested `with` expressions |
| `IServiceCollection` has no `AddModule` | Register on the `PipelineBuilder` |
| `PipelineBuilder` has no `Build` | Await `BuildAsync`, or use `RunAsync` |
| Module override/result has a nullability mismatch | Choose `Module<T>` + `Task<T>` or explicit `Module<T?>` + `Task<T?>` |
| `IModuleContext` / `Module<T>` / `CommandResult` not found in old namespace | Import `ModularPipelines` |
| `DotNet` tool property unavailable | Import `ModularPipelines.Context`; check integration reference, C# 14 support and generator execution, or use `Tools.Get<IDotNet>()` |
| A command rejects named argument `token` | Use `cancellationToken` on that framework call |
| `CommandLogVerbosity` has no `Minimal` | Use `InputOnly` for command input only, or another level for the output you need |
| `MPG0018` or missing command metadata at runtime | Rebuild the declaring assembly with the V4 generator; retain analyzer assets |
| Missing generated option or incompatible property type | Inspect the current options type and tool grammar; update the call site |
| Handler method does not implement the interface member | Add the trailing `CancellationToken` parameter; rename `Priority` to `Order` |
| `IModularPipelinesPlugin` members not implemented | Replace `ConfigureServices`/`ConfigurePipeline` with `Configure(PipelineBuilder builder)` |
| Cannot derive from a sealed `DependsOnAttribute` or implement `IModule` | Declare dependencies in `Configure(module)`; derive modules from `Module<T>` |

### Searches for migration candidates

Run these searches in the consumer repository. Treat matches as review candidates, including
fully qualified names and aliases; the patterns do not prove a symbol belongs to ModularPipelines.

```powershell
rg -n 'PipelineBuilderOptions|ExecutePipelineAsync|\.Build\(\)|Services\.AddModule' --glob '*.cs'
rg -n 'ModuleConfiguration|DeclareDependencies|WithRetryCount|WithRetryPolicy|WithBeforeExecute|WithAfterExecute' --glob '*.cs'
rg -n 'IPipelineGlobalHooks|IPipelineModuleHooks|IPipelineHookContext|MustAsync|ExecuteModuleAsync|ExecuteModule' --glob '*.cs'
rg -n '\.SubModule\(|\.DotNet\(\)|\.Git\(\)|ExecuteCommandLineTool|LogSettings|token:' --glob '*.cs'
rg -n 'ModuleResultType|IsSuccess|IsFailure|IsSkipped|GetModuleResultsAsync|GetFailedModuleResults|PipelineTerminated' --glob '*.cs'
rg -n 'ModularPipelines\.FileSystem\.(File|Folder)|WorkingDirectory\s*=|SkipDecision\.Of' --glob '*.cs'
rg -n 'IEventHandlerPriority|PluginRegistry|PluginTestHelper|RequestRetry|SkipDependentModules|FailPipeline|ISecretObfuscator' --glob '*.cs'
rg -n 'RunIfAll|MandatoryRunConditionAttribute|IConditionAttribute|BuildSystem\.Is[A-Z]|ArgumentPlacement|CliArgument\([^)]*Name' --glob '*.cs'
rg -n '\b(IZip|IXml|IYaml|IBase64|IHex|ICertificates|IDownloader|IEnvironmentVariables|IAptGet|AptGet\w*)\b' --glob '*.cs'
rg -n 'ExecutionMode|PipelineCancelledException|HttpResponseException|ModuleReferencingSelfException|FailedRequirementsException|SubModuleFailedException' --glob '*.cs'
rg -n 'ModuleRunType|ITaggedModule|IPipelineValidator|IModuleResultRepository|IModuleEstimatedTimeProvider|IFileSystemProvider|WithTimeout\(TimeSpan\.Zero' --glob '*.cs'
rg -n 'CliCommand\(|AllowMultiple\s*=|CustomSeparator|DependsOnLazy|SchedulerOptions|SecretMaskingOptions|CommandLogVerbosity\.Minimal' --glob '*.cs'
rg -n 'Information\.(BranchName|LastCommitSha|Tag|DefaultBranchName|CommitsOnBranch|PreviousCommit|Root)|RootDirectory|GetGitVersioningInformation|\.Commits\(|\.Cmd\(\)' --glob '*.cs'
rg -n 'ModularPipelinesPlugin\(|IBuildSystemDetector|IFileSystemContext|IEnvironmentDomainContext|TryGetService|WithSkipWhen' --glob '*.cs'
```

### Automation guardrails

- Do not globally replace `.Value`, `.Status`, `.Get`, `.Build`, `File`, `Folder`, or `token:`.
  Resolve the receiver/type first. Keep successful-result `Value` and `ValueOrDefault` semantics distinct.
  `Value` does not reject a successful null; retain or add explicit null checks where required.
- Preserve `PipelineRequirement.Pass`, `Fail`, and `When` helper calls in derived requirements;
  only their evaluation override needs migration to `EvaluateAsync`.
- Do not infer pipeline failure from the presence of any result exception. Ignored failures retain
  exceptions; use `summary.Succeeded` for the pipeline outcome and `summary.Failures` for the failures
  that caused it.
- Review every module with more than one `WithSkipWhen` call. V3 kept only the last call; V4 ORs
  them all. Remove calls that V3 ignored, or use `WithSkipWhenAll` for AND.
- Do not replace every `Task` with `ValueTask`. Only the changed configuration callback contracts need
  that adaptation; module execution and lifecycle hooks still use `Task`.
- Do not turn a required dependency into optional merely to make validation pass.
- Do not delete lifecycle hooks or custom policies because a fluent method disappeared.
- Do not blindly replace multiple OS attributes with multiple `[RunIf]` attributes; preserve OR/AND intent.
- Do not use regex to infer generic result types or strip nullable annotations. Inspect the module's
  declared result and its consumers.
- Do not hand-edit generated options, add obsolete aliases/shims, or invent missing properties. Migrate
  consumer calls to the actual generated contract; generator defects belong in the generator.
- Do not suppress metadata diagnostics to force an old integration assembly to load.
- Keep a final report of modified APIs, behavioral decisions, tests run, and unverified external commands.

### V4 module templates

```csharp
using ModularPipelines;

public sealed class VersionModule : Module<string>
{
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithTimeout(TimeSpan.FromMinutes(1));

    protected override Task<string> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult("1.0.0");
}

[DependsOn<VersionModule>]
public sealed class PrintVersionModule : Module
{
    protected override async Task ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        var version = (await context.GetModule<VersionModule>()).Value;
        context.Console.WriteLine(version);
    }
}
```

## Getting Help

1. Check the linked feature guides and the installed V4 package's signatures for the failing API.
2. Review the [current examples](https://github.com/thomhurst/ModularPipelines/tree/main/src/ModularPipelines.Examples).
3. Search or open a [GitHub issue](https://github.com/thomhurst/ModularPipelines/issues), including old/new
   package versions, a minimal module or command example, and the compiler/runtime error.
