---
title: Old-to-new API lookup
sidebar_position: 15
---

# Old-to-new API lookup

Use this page to look up a V3 API or a compiler error. Each row points at the most common
replacement. The linked pages have the details.

These are **symbol-scoped** replacements, not global find-and-replace rules. Check that a match
really is the ModularPipelines symbol before you change it.

## Common compiler errors

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

## Builder and options

Details: [Entry point and options](./entry-point-and-options.md).

| V3 API | V4 API | Check |
| --- | --- | --- |
| `PipelineBuilderOptions` | `PipelineBuilderSettings` | Immutable record; set properties in the initializer |
| `builder.Services.AddModule<T>()` | `builder.AddModule<T>()` | ModularPipelines registration extensions only. Preserve conditional registration and factories |
| Both `builder.ConfigureServices(...)` overloads | Register directly on `builder.Services` | Use the builder variable for registrations that need it |
| `builder.Build()` | `await builder.BuildAsync()` | Dispose the pipeline with `await using` |
| `builder.ExecutePipelineAsync()` | `builder.RunAsync()` | Await completion |
| `builder.Options.X = value` | `builder.ConfigureOptions(o => o with { X = value })` | Nested settings moved |
| `ExecutionMode.StopOnFirstException` | `FailureMode.FailFast` | |
| `ExecutionMode.WaitForAllModules` | `FailureMode.ContinueOnFailure` | |
| `PluginRegistry.Register(plugin)` | `builder.AddPlugin(plugin)` | Merge `ConfigureServices` and `ConfigurePipeline` into `Configure(PipelineBuilder)` |
| `IPipelineValidator.Validate` | `ValidateAsync(services, cancellationToken)` | Return `Task<ValidationResult>` |

## Modules

Details: [Modules](./modules.md).

| V3 API | V4 API | Check |
| --- | --- | --- |
| `Configure(): ModuleConfiguration` | `Configure(ModuleConfigurationBuilder): void` | Remove `Create` and `Build`; preserve inherited configuration |
| `DeclareDependencies(...)` | Dependencies in `Configure(module)` | Keep required/optional/conditional distinctions |
| `DependsOnLazy<T>()` | `DependsOnOptional<T>()` | Same scheduling behavior |
| `WithRetryCount` / `WithRetryPolicy` | `WithRetry` / `WithShield` | Preserve policy semantics; review delays and exception filters |
| `WithTimeout(TimeSpan.Zero)` | `WithTimeout(Timeout.InfiniteTimeSpan)` | Zero now throws |
| `Module<T>` returning `Task<T?>` | `Task<T>`, or explicitly `Module<T?>` | Decide whether null is valid |
| `ExecuteModuleAsync` / `ExecuteModule` | `ExecuteAsync` / `Execute` | Non-generic module overrides only |
| `context.SubModule(...)` | `context.RunSubModuleAsync(name, token => ..., cancellationToken)` | Forward the body token |

## Results and summaries

Details: [Results and summaries](./results.md).

| V3 API | V4 API | Check |
| --- | --- | --- |
| Result `IsSuccess` / `IsFailure` / `IsSkipped` | Typed variants or safe accessors | Match the result's actual generic type |
| `Status.Successful`, etc. (`ModularPipelines.Enums.Status`) | `ModuleStatus.Succeeded`, etc. | See the [status table](./results.md#statuses); `Retried` is removed |
| `summary.GetModuleResultsAsync()` | `summary.Results` | Remove the `await` for this access only. No live module handles |
| `summary.GetModule<T>()` | `summary.GetResult<TModule, TResult>()` / `TryGetResult<TModule, TResult>(out var result)` | Returns a completed result; inspect its outcome before reading `.Value` |
| `summary.GetFailedModuleResults()` | `summary.Failures` and `summary.IgnoredFailures` | V3 returned both kinds together |
| `summary.Status` | `summary.Succeeded` | Boolean overall outcome; inspect `Failures` for details |
| `summary.Start` / `End` / `TotalDuration` | `StartTime` / `EndTime` / `Duration` | `PipelineSummary` is now sealed |

## Hooks, conditions, and requirements

Details: [Hooks and plugins](./hooks-and-plugins.md) and
[Conditions and requirements](./conditions-and-requirements.md).

| V3 API | V4 API | Check |
| --- | --- | --- |
| `IPipelineGlobalHooks` / `IPipelineModuleHooks` | `IPipelineEventHandler` / `IModuleEventHandler` | Update callback signatures |
| `IPipelineHookContext` | `IPipelineContext` | Requirements, run conditions, and pipeline event handlers |
| `IEventHandlerPriority.Priority` | `IEventHandler.Order` | Ascending order; add the trailing `CancellationToken` to handler methods |
| `[RunIfAll<...>]` | `[RunIf<...>]` | Same AND semantics |
| `MandatoryRunConditionAttribute` / `RunConditionAttribute.Condition` | `RunConditionAttribute(ConditionIntent.Run)` / `EvaluateAsync` | Use `GroupKey` for OR-ed alternatives |
| `SkipDecision.Of(condition, reason)` | `SkipDecision.When(condition, reason)` | |
| `MustAsync` / `Must` | `EvaluateAsync(context, cancellationToken)` | Return explicit requirement decisions |

## Context

Details: [Context services](./context.md).

| V3 API | V4 API | Check |
| --- | --- | --- |
| `context.Services.Get<T>()` | `context.Services.GetRequiredService<T>()` | |
| `context.TryGetService<T>()` | `context.Services.GetRequiredService<T>()` | Use `GetService<T>()` with null handling only where absence is allowed |
| `BuildSystem.IsGitHubActions`, etc. | `BuildSystem.Is(BuildSystem.GitHubActions)`, etc. | Repeat for each per-system flag. The enum is in `ModularPipelines` |
| `context.Environment.BuildSystem.IsBuildServer` | `context.Environment.BuildSystem.IsCI` | Also honors a truthy `CI` variable; `IsLocal` is the inverse |
| `context.IsRunningInCI()` | `context.Environment.BuildSystem.IsCI` | Also honors a truthy `CI` variable |
| `context.IsRunningLocally()` | `context.Environment.BuildSystem.IsLocal` | Inverse of `IsCI` |
| `context.IsRunningIn(buildSystem)` | `context.Environment.BuildSystem.Is(buildSystem)` | V4 compares all enum values, including `Unknown`; V3 returned `false` outside its four supported systems |

## Tools and commands

Details: [Tools and commands](./commands.md), [CLI options](./cli-options.md), and
[Logging and HTTP](./logging-and-http.md).

| V3 API | V4 API | Check |
| --- | --- | --- |
| `context.DotNet()` and other tool accessors | `context.Tools.DotNet`, etc. | Upgrade integration and generator together |
| Tool command `Build(...)`, etc. | `BuildAsync(...)`, etc. | Inspect regenerated signatures |
| `context.Shell.Command.ExecuteCommandLineTool(...)` | `context.Shell.RunAsync(...)` | Pass individual argument tokens |
| `token:` | `cancellationToken:` | Only changed framework parameters |
| `LogSettings` | `Logging` | `CommandExecutionOptions` and `HttpOptions` |
| `CommandLogVerbosity.Minimal` | `CommandLogVerbosity.InputOnly` | V3 documented `Minimal` as errors and warnings; `InputOnly` logs command input only. Pick the level you need |
| `HttpLoggingOptions.LogRequest`, etc.; `None` / `Full` | `ShowRequest`, etc.; `Silent` / `Diagnostic` | Bodies are no longer logged by default |
| `[CliCommand("tool", ...)]` | `[CliTool("tool")]` + `[CliSubCommand(...)]` | Handwritten option types only |
| `[CliArgument(Placement = ...)]` | `[CliArgument(Phase = ...)]` | Handwritten option types only. `BeforeOptions` and `ImmediatelyAfterCommand` both become `EarlyOperand`; the default is `Passthrough` |

## Files and utilities

Details: [Files and utilities](./files-and-utilities.md).

| V3 API | V4 API | Check |
| --- | --- | --- |
| Framework `File` / `Folder` | `FilePath` / `FolderPath` | Keep `System.IO` symbols intact |
| `await context.Files.ExistsAsync(path, cancellationToken)` | `context.Files.Exists(path)` | Also covers calls without an explicit token. Remove this call's `await` and token; relative paths use the pipeline working directory |
| `context.Data.Hex.FromHex(value)` | `context.Data.Hex.FromHex(value, Encoding.UTF8)` for text | One-argument return type changes from `string` to `byte[]`; review `var` declarations and legacy input validation |
| `context.Data.Hex.ToHex(bytes)` with `IEnumerable<byte>` | `context.Data.Hex.ToHex(bytes.ToArray())` | Keep existing `byte[]` and string arguments unchanged; `ToArray` needs `System.Linq` |

## Exceptions

Details: [Exceptions and extension points](./exceptions-and-extension-points.md).

| V3 API | V4 API | Check |
| --- | --- | --- |
| `PipelineCancelledException` | `PipelineCanceledException` | Derives from `OperationCanceledException`, not `PipelineException`; V4 spells "canceled" with one L throughout |
| `ModuleReferencingSelfException` | `ModuleSelfDependencyException` | |
| `FailedRequirementsException` | `RequirementNotMetException` | |
| `HttpResponseException` | `PipelineHttpResponseException` | Derives from `HttpRequestException` |
| `CommandException.ExitCode`, etc. | `CommandException.Result.ExitCode`, etc. | `ExecutionTime` becomes `Result.Duration` |

## Integration packages

Details: [Integration packages](./integrations.md).

| V3 API | V4 API | Check |
| --- | --- | --- |
| `Git().Information.BranchName`, etc. | `(await Tools.Git.Information.GetRequiredInfoAsync(ct)).BranchName` | Use `GetInfoAsync` if Git may be unavailable; properties can be null |
| `Git().Commands.Push(...)`, etc. | `Tools.Git.Commands.Remotes.PushAsync(...)`, etc. | Commands are grouped: `Repository`, `WorkingTree`, `Branches`, `Remotes`, `History`, `Maintenance` |
| `new AzureResourceIdentifier(subscription, group, name)` | `new AzureResourceIdentifier(subscription, group, resourceType, name)` | Same for the `GetResourceIdentifier` factories |
| Azure provisioner methods such as `AuthorizationRoleDefinition` | Same names with an `Async` suffix | Handwritten `ModularPipelines.Azure` provisioners |
| `ModularPipelines.AmazonWebServices.Lambda` | `AmazonLambdaClient` `*Async` methods | Add an explicit `AWSSDK.Lambda` reference |
