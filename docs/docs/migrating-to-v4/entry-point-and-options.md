---
title: Entry point and options
sidebar_position: 2
---

# Entry point and options

## Building and running

```csharp
// V3
var builder = Pipeline.CreateBuilder(args);
builder.Services.AddModule<BuildModule>();
builder.Services.AddModule<TestModule>();
await builder.Build().RunAsync();
// Or: await builder.ExecutePipelineAsync();

// V4
var builder = Pipeline.CreateBuilder(args);
builder.AddModule<BuildModule>().AddModule<TestModule>();
await builder.RunAsync();
```

`builder.RunAsync()` builds, runs, and disposes the pipeline for you. If you build the pipeline
yourself, dispose it asynchronously:

```csharp
await using var pipeline = await builder.BuildAsync();
var summary = await pipeline.RunAsync();
```

| V3 | V4 |
| --- | --- |
| `builder.Build()` | `await builder.BuildAsync()` |
| `builder.ExecutePipelineAsync()`, `pipeline.ExecutePipelineAsync()` | `RunAsync()` |
| `builder.BuildHostAsync()` | `builder.BuildAsync()` |
| `pipeline.RootServices` | `pipeline.Services` |

:::warning `BuildAsync()` validates
V3's `Build()` did not validate. A pipeline that used to build can now throw
`PipelineValidationException` from `BuildAsync()`. To inspect errors without running modules, call
`await builder.ValidateAsync()`.
:::

## Registration

Framework registrations move from `builder.Services` to `builder`. Ordinary `AddSingleton`,
`AddScoped`, `Configure<TOptions>`, and other application DI calls stay on `builder.Services`.

| V3 registration | V4 registration |
| --- | --- |
| `builder.Services.AddModule<T>()` | `builder.AddModule<T>()` |
| Module instance/factory registration on services | `builder.AddModule(instance)` / `builder.AddModule<T>(factory)` |
| `builder.Services.AddModulesFromAssembly(...)` | `builder.AddModulesFromAssembly(...)` |
| `builder.Services.AddModulesFromAssemblyContainingType<T>()` | `builder.AddModulesFromAssemblyContainingType<T>()` |
| `builder.AddModules<T1, T2>()` (and higher arities) | Chain `.AddModule<T1>().AddModule<T2>()`, or use `.AddModules(typeof(T1), typeof(T2))` |
| `builder.Services.AddRequirement<T>()` | `builder.AddRequirement<T>()` |
| `AddPipelineGlobalHooks<T>()` on the builder or its services | `builder.AddPipelineEventHandler<T>()`, with the [new interface](./hooks-and-plugins.md) |
| `AddPipelineModuleHooks<T>()` on the builder or its services | `builder.AddModuleEventHandler<T>()`, with the [new interface](./hooks-and-plugins.md) |
| Registration handles / `IModuleRegistrationBuilder` | Registration returns `PipelineBuilder`; configure the module in its `Configure` override |

- V3 code that already called `builder.AddModule<T>()` or `builder.AddRequirement<T>()` keeps working.
- Your own helpers that extended `IServiceCollection` to add modules should now accept and return
  `PipelineBuilder`.

### `ConfigureServices` is removed

Both callback forms are gone. Register on `builder.Services` directly, and use the builder variable
where the old callback needed it.

```csharp
// V3: either form
builder.ConfigureServices(services => services.AddSingleton<MyService>());
builder.ConfigureServices((pipelineBuilder, services) => services.AddSingleton<MyService>());

// V4
builder.Services.AddSingleton<MyService>();
```

### `AddPipelineFileWriter<T>()` is removed

To generate a supported provider's minimal CI file, set `ModularPipelinesBuildSystem` in the
project. See [Generate build server files](../how-to/build-server-files.md). Invoke custom workflow
generation explicitly.

## Builder settings

`PipelineBuilderOptions` becomes `PipelineBuilderSettings`, an immutable record. Set its properties
in the initializer. `Args` accepts any `IReadOnlyList<string>`.

```csharp
var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
{
    Args = args,
    EnvironmentName = "Production",
    ContentRootPath = repositoryRoot,
    WorkingDirectory = repositoryRoot,
});
```

Three defaults changed:

- **`AddJsonFile` paths.** Relative paths passed to `builder.Configuration.AddJsonFile(...)` now
  resolve against the pipeline working directory, not the application output directory. On a local
  run that is usually the pipeline project's source folder. If you run a published pipeline from
  another directory, set `WorkingDirectory` in the settings, set the `MODULAR_PIPELINES_DIRECTORY`
  environment variable, or pass an absolute path. See [Relative paths](../how-to/relative-paths.md).
- **Command-line switches.** V4 consumes its built-in pipeline CLI switches from `args`. If your
  application already owns those arguments, set `EnableCommandLineOptions = false`.
- **Assembly loading.** Referenced ModularPipelines assemblies still load. Unreferenced
  `*ModularPipeline*.dll` files in the application directory are no longer loaded eagerly. Copied
  plugin assemblies that relied on this need `LoadModularPipelinesAssemblies = true` or explicit
  registration.

## Pipeline options

Pipeline options and their nested records are immutable. `builder.Options` is a read-only view.
Use `ConfigureOptions` with `with` expressions, which keeps your other settings.

```csharp
// V3
builder.Options.ExecutionMode = ExecutionMode.WaitForAllModules;
builder.Options.ThrowOnPipelineFailure = false;
builder.Options.PrintLogo = false;
builder.Options.Concurrency.MaxParallelism = 4;

// V4
builder.ConfigureOptions(options => options with
{
    FailureMode = FailureMode.ContinueOnFailure,
    ThrowOnPipelineFailure = false,
    Console = options.Console with { PrintLogo = false },
    Concurrency = options.Concurrency with { MaxParallelism = 4 },
});
```

To change one group, use its shortcut. Each shortcut returns the builder, so they chain:

```csharp
builder
    .ConfigureConsole(console => console with { PrintLogo = false })
    .ConfigureConcurrency(concurrency => concurrency with { MaxParallelism = 4 });
```

The shortcuts are `ConfigureConsole`, `ConfigureConcurrency`, `ConfigureCommands`, `ConfigureHttp`,
`ConfigureSecrets`, and `ConfigureResourceMonitoring`.

### Renamed options

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
| `builder.Services.Configure<PipelineOptions>(o => o.X = ...)` | `builder.ConfigureOptions(o => o with { X = ... })` |
| `builder.ConfigurePipelineOptions(...)`, including the `(builder, options)` overload | `builder.ConfigureOptions(options => options with { ... })`; capture the builder in the lambda if needed |
| `builder.Services.Configure<SchedulerOptions>(...)` | `builder.ConfigureConcurrency(c => c with { NotificationTimeout = ... })`; `SchedulerOptions` is removed |
| `builder.Services.Configure<SecretMaskingOptions>(...)` | `builder.ConfigureSecrets(s => s with { ... })`; the DI `Configure` call no longer applies |
| `builder.SetLogLevel(level)` | `builder.Logging.SetMinimumLevel(level)` (`Microsoft.Extensions.Logging`) |

### Things to know

- **Collections are read-only.** For options such as category filters, assign a replacement
  collection in `ConfigureOptions`. You cannot call `.Add()` on the existing one.
- **Inspecting a failed run.** To get a summary back from a failed pipeline, set **both**
  `FailureMode.ContinueOnFailure` and `ThrowOnPipelineFailure = false`. `FailFast` rethrows the
  module failure.
- **New:** `RunOnlyModules` and `IgnoreModules` select modules by name or `[ModuleId]`. In V4
  pre-release builds these were `TargetModules` and `SkippedModules`.

## Canceling startup

`BuildAsync` and `ValidateAsync` now take an optional `CancellationToken`. The builder's
`RunAsync(cancellationToken)` passes its token through startup, validation, and execution.

```csharp
await using var pipeline = await builder.BuildAsync(cancellationToken);
var summary = await pipeline.RunAsync(cancellationToken);
```

Parameterless calls still compile, but the signature change is binary-breaking. Recompile callers
when you upgrade.

### Cancellable service initializers

To let a service's startup initialization be canceled, implement
`ModularPipelines.ICancellableInitializer.InitializeAsync(CancellationToken)` and register the
service as a singleton.

- It extends `Initialization.Microsoft.Extensions.DependencyInjection.IInitializer`, and keeps
  initializer ordering and parallel batches.
- Its inherited parameterless method forwards with the default token.
- Legacy `IInitializer.InitializeAsync()` implementations receive no token and cannot be interrupted.
- Startup awaits all started initialization work before it reports cancellation and disposes
  resources. Cancellation can therefore wait for a legacy initializer to finish.
- A token that is already canceled stops initialization and validation from starting.
