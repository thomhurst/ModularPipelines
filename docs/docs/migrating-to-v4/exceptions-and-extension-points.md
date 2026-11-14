---
title: Exceptions and extension points
sidebar_position: 13
---

# Exceptions and extension points

## Exceptions

### Renamed and removed

| V3 type | V4 replacement |
| --- | --- |
| `PipelineCancelledException` | `PipelineCanceledException` |
| `ModuleReferencingSelfException` | `ModuleSelfDependencyException` |
| `FailedRequirementsException` | `RequirementNotMetException` |
| `HttpResponseException` | `PipelineHttpResponseException` |
| `SubModuleFailedException` | Catch the original sub-module exception |
| `AlwaysRunPostponedException` | Removed |

### Review your `catch` blocks

Several exceptions changed type or base class:

| Situation | V4 exception |
| --- | --- |
| Pipeline canceled | `PipelineCanceledException` now derives from `OperationCanceledException`. `catch (PipelineException)` no longer catches it. Catch `OperationCanceledException` for ordinary cancellation |
| Non-success HTTP response | `PipelineHttpResponseException` derives from `HttpRequestException`, so it is not a `PipelineException` either. Its inherited `StatusCode` is nullable |
| Missing executable | `ToolNotFoundException`, which is a `CommandException` |
| Validation errors | Their own types, such as `CommandOptionsValidationException` and `PipelineValidationException` |
| No modules registered | `PipelineException` |

### `CommandException`

`CommandException` now carries the failed command's `CommandResult` in its `Result` property. The
old members move there:

| V3 | V4 |
| --- | --- |
| `exception.ExitCode` | `exception.Result.ExitCode` |
| `exception.StandardOutput` / `StandardError` | `exception.Result.StandardOutput` / `Result.StandardError` |
| `exception.ExecutionTime` | `exception.Result.Duration` |

- The exception message no longer includes the captured output. Read it from `Result`.
- Construct custom command exceptions with a `CommandResult`. The individual output and exit-code
  constructor parameters are removed.

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

When a nonzero exit code is expected data, set
`CommandExecutionOptions.ThrowOnNonZeroExitCode = false` and inspect the returned `ExitCode`.

## Extension points

The rest of this page is for code that extends the framework: custom repositories, validators,
attributes, and anything that reached into engine types.

### Engine internals are closed

Engine execution state, schedulers, awaiter completion, and configuration internals are no longer
general-purpose public extension points. Depending on what you were doing, use event handlers,
reporting interfaces, `IExecutionBackend`, or the
[ModularPipelines.Testing harness](../how-to/testing.md). Do not recreate removed internal types as
compatibility shims.

| Removed or changed | Do this instead |
| --- | --- |
| `IModule.ModuleRunType` and the `ModuleRunType` enum | Configure `.WithAlwaysRun()` for cleanup; read `module.Configuration.AlwaysRun` to inspect the policy |
| Injecting dependencies dynamically through scheduler APIs | Declare a module's dependencies before execution |
| `ITaggedModule` implementations, registration-time metadata customization | Module configuration or the supported attributes |
| `ModuleRegistrationOptions`, `DeclaredDependency`, the `DependencyType` and `WaitResult` enums | Removed with the old registration and scheduling APIs |
| `ModuleActivityTracing` | Internal; use the `PipelineTelemetry` constants for activity source and tag names, which are unchanged |

`DependsOnAttribute<T>` no longer derives from `DependsOnAttribute`. Reflection over
`DependsOnAttribute` alone misses the generic declarations.

Telemetry status tag *values* did change. See
[Runtime behavior changes](./runtime-behavior.md#telemetry-and-secrets).

### Changed contracts

| V3 contract | V4 contract |
| --- | --- |
| `IModuleEstimatedTimeProvider` methods | Each method takes a `ModuleId` instead of a `Type`, plus a trailing `CancellationToken`. The sub-module methods have default implementations. Re-key stored estimates that used `Type.FullName`. Estimates are best-effort and never fail a module |
| `IModuleResultRepository.SaveResultAsync` / `GetResultAsync` | Take a `CancellationToken`; `IsEnabled` defaults to `true` |
| `IPipelineValidator.Validate(IServiceProvider)` | `ValidateAsync(IServiceProvider, CancellationToken)`; `Order` defaults to `0` |
| `ISecretObfuscator` | Internal; supply secrets through `ISecretRegistry` or `[SecretValue]`, and configure masking with `PipelineOptions.Secrets` |
| Custom `IFileSystemProvider` | Also implement `GetAttributes`/`SetAttributes` and the `Get`/`Set` creation, last-access, and last-write UTC time members; several other members now have default implementations |
| Custom `IModule` implementations | Not supported; derive from `Module<T>` or `SyncModule<T>` |
| Subclasses of `DependsOnAttribute`, `DependsOnAttribute<T>`, `DependsOnAllModulesInheritingFromAttribute`, or `SecretValueAttribute` | These attributes are sealed; declare dependencies in `Configure(module)` or use a registration handler |

### Result repositories and time estimators

- Implement the interfaces in `ModularPipelines.Reporting`.
- Register with `builder.AddResultsRepository<T>()` and
  `builder.AddModuleEstimatedTimeProvider<T>()`.
- Keep repository enablement (`IsEnabled`), generic result types, and the new result metadata
  consistent when you update stored data.

### Test helpers

Move legacy private test helpers to `ModularPipelines.Testing`. Use command interception to supply
`CommandResult.Ok(...)` without launching real tools.

## Analyzer IDs

Analyzer IDs are consolidated under `MP####`. Update `.editorconfig`, `NoWarn`, and `#pragma`
entries with the [analyzer ID migration table](../how-to/analyzers.md#id-migration).

Source-generator diagnostics use the separate `MPG####` family. Fix missing or incompatible
metadata rather than suppressing it.
