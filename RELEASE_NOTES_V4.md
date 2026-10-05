# ModularPipelines V4 Release Notes

## Grype persistent settings

`GrypeOptions` now declares `Config`, `Profile`, `Quiet`, and `Verbose` for every
command. Existing initializers retain their names and types; reflection must
include inherited properties. Repeated configuration files and profiles render
before the command path. Scan-specific flags remain local. See the
[Grype migration guidance](docs/docs/mp-packages/grype.md#migration).

## Docker client global options

Generated `DockerOptions` now exposes the Docker client's global configuration,
context, host, logging, and TLS settings. These are inherited by command options
and emitted before the subcommand. Command-local options and positional operands
with the same C# name remain independent under scoped names, such as
`DockerBuildxCreateOptions.BuildxContext` instead of its former `Context` operand.
Use the global `Context` only to select the Docker client context. See the
[Docker migration guidance](docs/docs/mp-packages/docker.md#global-and-command-specific-names).

## Homebrew shared options

`BrewOptions` now declares `Debug`, `Quiet`, and `Verbose` for generated Homebrew
commands. These flags follow the command path (`brew list --verbose`). Existing
property initializers retain their names; reflection over command records must
include inherited properties. Cask-only options remain command-specific.

## Slack and Microsoft Teams webhooks

Slack posting is now `PostMessageAsync`; Teams posting is now `PostCardAsync`.
Both accept an optional `CancellationToken` and forward it to the HTTP request.
Each package exposes its options, models, and registration extensions in its root
namespace. Remove the old `.Options`, `.Models`, and `.Extensions` imports.
Registration methods remain public but are hidden with `EditorBrowsable(Never)`.
The old method names and namespaces have no compatibility shims.

HTTP behavior is unchanged: unsuccessful responses throw by default. Teams callers
can set `ThrowOnNonSuccessStatusCode = false` and must dispose returned responses.
Slack disposes responses internally. See the [Slack](docs/docs/mp-packages/slack.md)
and [Teams](docs/docs/mp-packages/microsoft-teams.md) examples.

## Trivy inherited settings

Trivy command records now inherit eight persistent settings from `TrivyOptions`:
`Cacert`, `CacheDir`, `Config`, `Debug`, `GenerateDefaultConfig`, `Insecure`, `Quiet`,
and `Timeout`. Existing initializer property names remain available; these values
now render before the command path. Root help/version controls and root version
format are excluded, while scan formats and registry credentials remain local.
See the [Trivy package guide](docs/docs/mp-packages/trivy.md) for migration details.

## Redis endpoint validation

Redis coordinator, artifact-store, and module-cache registrations now validate the
endpoints left by `ConfigureConnection` when the pipeline is built. Empty callbacks,
TLS-only callbacks without endpoints, and callbacks that remove every endpoint fail
with `OptionsValidationException`. The callback runs once per materialized options
instance; its validated snapshot is reused for lazy connections and retries.

## Asynchronous file operations

`IHashContext` now provides cancellable async file hashing for MD5, SHA-1,
SHA-256, SHA-384, and SHA-512. `IZipContext` adds `CreateFromDirectoryAsync` and
`ExtractToDirectoryAsync`, preserving compression, overwrite, working-directory,
and file-system provider behavior. Existing synchronous APIs remain available.
See [asynchronous file operations](docs/docs/how-to/async-file-operations.md)
for examples, cancellation behavior, and runtime I/O limitations.

This extends the required interface contract in V4. Custom `IHashContext`
implementations must implement all five async file-hash methods; custom
`IZipContext` implementations must implement both async ZIP methods. Rebuild these
implementations against V4 and use cancellable asynchronous stream I/O where
supported. Existing callers of synchronous methods can keep using them, but
implementations of the older interfaces are not source or binary compatible.

## Testing inputs and coordinator contracts

`ModuleTester` now supports `WithFile`, `WithDependencyFailure`,
`WithSkippedDependency`, and `ConfigurePipeline`. Seeded files use the effective
file-system provider and resolve paths through the pipeline files context.
`CommandResult.Fail` creates nonzero command results for interceptors.
`ModularPipelines.Testing.Distributed.DistributedCoordinatorContract` ships the
shared coordinator checks without a dependency on a test framework.

## CI detection

Use `context.Environment.BuildSystem.IsCI` (renamed from `IsBuildServer`) and
`context.Environment.BuildSystem.IsLocal` for the inverse. The `IsRunningInCI()`,
`IsRunningLocally()`, and `IsRunningIn(buildSystem)` context extensions are removed;
use the two properties or `context.Environment.BuildSystem.Is(buildSystem)`.
`OnCI`, `OnLocal`, `Require.Ci()`, and `Require.LocalEnvironment()` remain available
for run conditions and requirements. Detection rules are unchanged: a known build
system or a truthy generic `CI` variable identifies CI.

## Context nullability and hex encoding

- `DownloadStringAsync` returns `Task<string>`; HTTP failures throw instead of returning null.
- `FromYaml<T>` returns `T?`, reflecting empty or null YAML documents.
- `FromHex(string)` returns `byte[]`. To decode text, pass an explicit encoding, for example `FromHex(value, Encoding.UTF8)`.
- `ToHex` accepts `byte[]` instead of `IEnumerable<byte>`; materialize other sequences with `ToArray()`. Hex output remains lowercase.
- Hex decoding follows `Convert.FromHexString`: odd-length input and separators are rejected with `FormatException` instead of truncating or removing characters.

## Relative paths

`PipelineBuilderSettings.WorkingDirectory` controls context-based file operations
and command defaults. It does not change the process current directory.
`FilePath`/`FolderPath` constructors, implicit string conversions, and relative
copy/move destinations still use the process directory. For pipeline-relative
paths, use `context.Files.GetFile` or `GetFolder` and pass their absolute `Path`
to destination parameters. See the [relative-path migration guidance](docs/docs/how-to/relative-paths.md).

## Pipeline summary contract

`PipelineSummary` is now sealed. Its timing properties match the other result types:
`Start` becomes `StartTime`, `End` becomes `EndTime`, and `TotalDuration` becomes `Duration`.
The JSON property names change with them.

Replace comparisons against `PipelineSummary.Status` with `summary.Succeeded` (or
`!summary.Succeeded`). Success means the pipeline completed without unignored failures;
skipped, cached, restored, and ignored-failure results permit success. Incomplete, failed,
and canceled runs are not successful. `Failures` and `IgnoredFailures` provide module-level
details. The success value survives JSON serialization even though module results are omitted.

`ThrowOnPipelineFailure` also applies when a module result reports `Failed`, `TimedOut`,
`DependencyFailed`, or `Canceled` without a captured exception. When enabled, the completed
run throws `PipelineFailedException`; set it to `false` to inspect the unsuccessful summary.
The exception's `FailedModules` identifies these results and excludes ignored failures.

## Git option cleanup

Removed 26 unused generated Git option types absent from the validated command
coverage list, including documentation pages such as `GitAttributesOptions`,
`GitProtocolV2Options`, `GitRevisionsOptions`, and `GitFormatPackOptions`.
The handwritten grouped Git API and handwritten options remain available. Git
refreshes now prune obsolete generated option records after command coverage
validation, preventing documentation-page types from lingering in the package.

## Telemetry conventions

- Exceptions are now `exception` span events with masked messages, rather than `exception.type` and `exception.message` span attributes. Failed exception spans also expose `error.type`.
- The failed-module counter is now `modular_pipelines.module.failed` (formerly `modular_pipelines.modules.failed`).
- Pipeline and module status attributes use lowercase snake case, for example `succeeded`, `timed_out`, and `restored_from_cache`.
- The redundant `modular_pipelines.command.duration_ms` attribute and `PipelineTelemetry.CommandDurationTag` constant are removed. Use span duration instead.

Update dashboards and telemetry queries for these v4 changes.

## Command output capture

V3 retained complete command output. V4 defaults to 1,048,576 captured characters
per stream, configured by `CommandExecutionOptions.MaxCapturedOutputLength`.
Output above the limit retains the beginning and end with
`... [truncated N characters] ...` between them. Do not parse this shortened output
as complete JSON or other structured data.

`CommandResult.StandardOutputTruncatedCharacters` and
`CommandResult.StandardErrorTruncatedCharacters` report omitted characters for each
stream, including on `CommandException.Result`; zero means none were omitted.
Truncation also emits a warning naming the capture limit at effective command
verbosity `Normal` or higher. `Silent` and `InputOnly` suppress the warning without
changing the result counts. Raise the limit for large
results or set it to `0` for unlimited capture, accounting for the memory required.
Streamed command logging is independent of this capture limit.

## Generated runtime metadata

Generated runtime metadata now requires the v4 contracts: secret metadata schema 2
and command metadata schema 4. Legacy registration overloads, optional-value markers,
and reflection-based operand-count fallback have been removed. Rebuild plugins and
referenced generated-options assemblies against ModularPipelines v4; incompatible
metadata now fails with an actionable runtime error or the MPG0018 build diagnostic.
Assemblies with neither generated metadata nor `ModularPipelinesPluginAttribute` remain
outside plugin validation because referenced non-plugin assemblies use the same loading
path; plugin authors should declare the attribute explicitly.

## PowerShell casing

PowerShell types now use the product's canonical casing:

- `PowershellOptions` is now `PowerShellOptions`.
- `PowershellScriptOptions` is now `PowerShellScriptOptions`.
- `PowershellFileOptions` is now `PowerShellFileOptions`.

The internal implementation class also follows the `PowerShell` casing. The former
`Powershell7Async` predefined installer was removed with the rest of the predefined
installer surface; use the dedicated installation integrations instead.

## Installers

The core installer surface now contains only generic local and web installation:

```csharp
await context.Installers.InstallAsync(new InstallerOptions("./setup.sh"));
await context.Installers.InstallFromWebAsync(new WebInstallerOptions(downloadUri));
```

`context.Installers.File`, the Windows, Linux, macOS, and predefined installer
contexts, and their platform-specific option types have been removed. Use the
dedicated Brew, Chocolatey, Winget, Node, or other tool integration instead of the
removed core package-manager wrappers.
## Logging surface

- `context.Logger` now exposes the standard Microsoft.Extensions.Logging `ILogger`
  contract. The framework-owned logger lifecycle is no longer publicly disposable.
- Replace injected `IModuleLoggerProvider` with `IModuleLoggerAccessor` and read its
  `Logger` property.
- `ISummaryLogger.Info(...)` is now `Information(...)`.
- Summary read methods moved from `ISummaryLogger` to `ISummaryLogReader`.

## Logging options

Per-call command and HTTP options now use `Logging`, matching the global
`Commands.Logging` and `Http.Logging` properties. `IncludeTimestamps` is now
`ShowTimestamps`.

`HttpLoggingType` and `HttpOptions.LoggingType` were removed. Configure request,
response, status-code, duration, header, and body logging through
`HttpLoggingOptions`. `CommandLogVerbosity.Minimal` is renamed to
`CommandLogVerbosity.InputOnly` (command input only), so code that relied on the V3
meaning fails to compile instead of silently logging differently.

`CommandExecutionOptions.ExecutionTimeout` defaults to 30 minutes. A command that
exceeds it throws a `TimeoutException` whose message names the timeout and the option;
raise `ExecutionTimeout` for long-running commands or set it to `null` to disable it.

The unused `PipelineCommandOptions.Execution` property was removed. Continue to pass
execution behavior through `CommandExecutionOptions` on each command call.

## HTTP failure behavior

`HttpOptions.ThrowOnNonSuccessStatusCode` now defaults to `true`, matching the
failure defaults for commands and downloads. `context.Network.Http.SendAsync`
throws `PipelineHttpResponseException` for a non-success response, including calls
that implicitly convert a string, `Uri`, or `HttpRequestMessage` to `HttpOptions`.
The exception includes the status code and a preview of up to 2,000 response body bytes
(plus a truncation marker). The failure path reads at most one additional byte to detect
truncation; it does not drain an oversized or indefinitely streaming response. Use the
explicit opt-out below when you need to read the complete failure body.

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

Microsoft Teams webhook calls follow the same default. Set
`MicrosoftTeamsWebHookCardOptions.ThrowOnNonSuccessStatusCode = false` when the caller
needs to inspect an unsuccessful `HttpResponseMessage` and its body. The caller remains
responsible for disposing any returned response.

Slack webhook calls now throw on unsuccessful responses as well. The Slack wrapper
does not return a response; use the HTTP context directly with the explicit opt-out
when you need to handle an unsuccessful webhook response yourself.

## Hashing, ZIP, and Base64 APIs

Text and file hashing now share `context.Security.Hash` (`IHashContext`). Use
`Md5File`, `Sha256File`, and the other `*File` methods for files. `HashType` is now
`HashEncoding`; `context.Files.Checksum` and `IChecksumContext` have been removed.

ZIP methods now follow `System.IO.Compression.ZipFile` naming:
`CreateFromDirectory` and `ExtractToDirectory`. `IBase64Context.FromBase64String`
now returns `byte[]`, matching `Convert.FromBase64String`.

## Command Prompt integration

The documented `ModularPipelines.Cmd` entry point is now `context.Tools.Cmd`.
`ICmd` is now `ICmdContext`. `CmdScriptOptions` moved from
`ModularPipelines.Cmd.Models` to `ModularPipelines.Options`; `CmdFileOptions`
provides the typed `RunFileAsync` overload.

## Pipeline requirements

Requirement evaluation now uses `EvaluateAsync(IPipelineContext, CancellationToken)`
instead of `MustAsync(IPipelineContext)` or the synchronous `PipelineRequirement.Must`.
Synchronous implementations can return `Task.FromResult(...)` from `EvaluateAsync`.

`RequirementDecision.Success` is now `IsSatisfied`. The implicit conversions from a
failure-reason string and to `Task<RequirementDecision>`, along with the `Of` factory,
have been removed. Use `Passed`, `Failed(reason)`, or the remaining `bool` conversion.

Replace the removed `WindowsRequirement`, `LinuxRequirement`, `MacOSRequirement`, and
`WindowsAdminRequirement` classes with `Require.Windows()`, `Require.Linux()`,
`Require.MacOS()`, and `Require.WindowsAdmin()`. `Require.CIEnvironment()` is now
`Require.Ci()`. `FailedRequirementsException` is now
`RequirementNotMetException`.
## Service resolution

Service lookup on `context.Services` now follows the standard .NET naming and
nullability convention:

- `Get<T>()` is now `GetRequiredService<T>()` and throws when the service is missing.
- `TryGet<T>()` is now `GetService<T>()` and returns `null` when the service is missing.
- The duplicate `context.GetService<T>()` and `context.TryGetService<T>()` extension
  methods have been removed. Resolve services through `context.Services` instead.

```csharp
var required = context.Services.GetRequiredService<IMyService>();
var optional = context.Services.GetService<IOptionalService>();
```

## Distributed artifacts and options

Artifact operations are now available from `context.Artifacts`; the
`context.Artifacts()` extension has been removed. Artifact methods have optional
cancellation tokens, and `DownloadAsync<TProducerModule>(...)` avoids string-based
producer module names.

Distributed duration options now use `TimeSpan`, for example
`DistributedOptions.WorkerRegistrationTimeout` and `DistributedOptions.ModuleResultTimeout`.
`DistributedOptions.RunIdentifier` and `WorkerRegistration.RunIdentifier`
are now `RunId`. `ModuleAssignmentConfig` is now
`ModuleAssignmentConfiguration`. Custom stores can be
registered with `AddDistributedArtifactStore<TStore>()` or
`AddDistributedArtifactStoreFactory<TFactory>()`.

## Module console output

`IConsoleWriter` now lives in `ModularPipelines.Logging` and is available from
`context.Console`. Use `WriteLine` for plain text, `WriteMarkupLine` for
Spectre.Console markup, and `Write` for renderables. `LogToConsole` was removed.

## Failure modes and execution hints

Pipeline failure behavior now uses `FailureMode` instead of `ExecutionMode`:

- `ExecutionMode.StopOnFirstException` is now `FailureMode.FailFast`.
- `ExecutionMode.WaitForAllModules` is now `FailureMode.ContinueOnFailure`.
- `PipelineOptions.ExecutionMode` is now `PipelineOptions.FailureMode`.

Module resource classification now uses `ExecutionHint` instead of `ExecutionType`:

- `ExecutionType.CpuIntensive` is now `ExecutionHint.CpuBound`.
- `ExecutionType.IoIntensive` is now `ExecutionHint.IoBound`.
- `ModuleConfiguration.ExecutionType` is now `ModuleConfiguration.ExecutionHint`.
- `WithExecutionHint(ExecutionType)` now accepts `ExecutionHint`.

The `[ExecutionHint(...)]` attribute syntax is unchanged.

## Module result metadata

`IModuleResult` and `ModuleResult` now use concise metadata names:

- `ModuleName` is now `Name`.
- `ModuleTypeName` is now `TypeName`.
- `ModuleDuration` is now `Duration`.
- `ModuleStart` is now `StartTime`.
- `ModuleEnd` is now `EndTime`.

The custom JSON converters use the same new property names. Consumers of persisted or
distributed `ModuleResult` JSON must migrate those five field names together with the
.NET API.

`PipelineSummary.Failures` returns results with a `Failed`, `TimedOut`, `DependencyFailed`,
or `Canceled` status, even when no exception was captured. It also includes other results
with an exception unless their status is `FailureIgnored`. `PipelineSummary.IgnoredFailures`
returns the results whose failures were ignored. Use them instead of filtering `Results`
by hand. GitHub Mermaid summaries use the same failure criteria for critical styling.

## Git repository information

`context.Tools.Git.Information.GetInfoAsync(cancellationToken)` returns `null` when Git
information is unavailable. Pipelines that always run inside a repository can call
`GetRequiredInfoAsync(cancellationToken)` instead; it returns a non-null
`GitRepositoryInfo` or throws an `InvalidOperationException` that explains Git information
is unavailable. Individual `GitRepositoryInfo` properties such as `BranchName` remain
nullable, for example on a detached HEAD.

Git commands stay grouped by area under `context.Tools.Git.Commands`, for example
`Branches.CommitAsync`, `WorkingTree.StatusAsync`, and `Remotes.PushAsync`.

## Naming consistency

- ModuleStatus.Cancelled is now ModuleStatus.Canceled, matching OperationCanceledException and
  PipelineCanceledException. The numeric value is unchanged. Persisted run history and module results that
  contain "Cancelled" or the V3 "PipelineTerminated" still deserialize as Canceled; writers and the
  OpenTelemetry status tag emit Canceled.
- IGitInformation.Commits(...) is now IGitInformation.CommitsAsync(...). Public methods that return
  IAsyncEnumerable<T> use the Async suffix. Update existing callers to the new name.
- Documentation and log messages call nested work started with RunSubModuleAsync "sub-modules" instead of
  "sub-operations".

## File-system path types

`ModularPipelines.FileSystem.File` and `Folder` have been renamed to `FilePath`
and `FolderPath`. This avoids collisions with `System.IO.File` in projects that
use implicit global usings. Method names such as `IFilesContext.GetFile` and
`GetFolder` are unchanged; only their path types have changed.

Both path types expose `CreationTime` and `LastWriteTime` as UTC-normalized
`DateTimeOffset` values. Replace `LastWriteTimeUtc` with `LastWriteTime`; call
`ToLocalTime()` explicitly when local display is needed. `FolderPath.Extension`
has been removed; `FilePath.Extension` remains available.

Replace `await context.Files.ExistsAsync(path, cancellationToken)` with
`context.Files.Exists(path)`. The synchronous check returns true for either a
file or a directory and resolves relative paths against the pipeline working
directory. Use `GetFile(path).Exists` or `GetFolder(path).Exists` for a specific kind.
`FilePath.CreateAsync` now accepts an optional cancellation token and checks it
before creating or truncating the file.

Prefer `using` or `await using` with `TempFile` and `TempFolder` for automatic
cleanup. `TempFile` starts with an uncreated path; create or write its `File` when
needed. `TempFolder` creates its folder immediately. The lower-level
`GetNewTemporaryFilePath` and `CreateTemporaryFolder` helpers do not arrange cleanup.

## Namespace organization

The module authoring surface now lives in the root `ModularPipelines` namespace. A
single `using ModularPipelines;` covers `Module<T>`, `IModuleContext`, module results,
dependency and condition attributes, conditions, status and priority values, and
`ModuleConfigurationBuilder`.

`Capability`, `CapabilityRequirement`, `ICapabilityProvider`, and
`CapabilityPipelineBuilderExtensions` have moved from `ModularPipelines.Distributed`
to the root `ModularPipelines` namespace because capabilities also apply to local
pipelines. Update fully qualified references or aliases and rebuild consumers.

Other public contracts now use feature namespaces:

- `ModularPipelines.Context` contains all domain context interfaces.
- `ModularPipelines.Events` contains module and pipeline event contracts.
- `ModularPipelines.Secrets` contains masking options, attributes, and registries.
- `ModularPipelines.Reporting` contains run reports, history, metrics, and repository
  extension points.
- `ModularPipelines.Logging.IConsoleWriter` replaces the root type.

The package also ships a `buildTransitive` global using for `ModularPipelines`, so
C# consumers do not need to declare the root using explicitly. Projects with a
colliding root type can opt out with `<Using Remove="ModularPipelines" />` and add
explicit or aliased usings instead.

## Pipeline builder configuration

Pipeline settings now use one configuration path:
`builder.ConfigureOptions(options => options with { ... })`.
`ConfigureConsole`, `ConfigureConcurrency`, `ConfigureCommands`, `ConfigureHttp`, and
`ConfigureSecrets` are shortcuts over `ConfigureOptions` that replace one nested option
group, for example `builder.ConfigureConsole(console => console with { PrintLogo = false })`.
`ConfigurePipelineOptions`, `RunOnlyCategories`, `IgnoreCategories`, and `SetLogLevel`
have been removed. Configure logging through `builder.Logging`; category filters remain
available on `PipelineOptions`.

Scheduler settings now live under `PipelineOptions.Concurrency`, and secret masking
settings live under `PipelineOptions.Secrets`. `SchedulerOptions` and the documented
`builder.Services.Configure<SecretMaskingOptions>` path have been removed. The no-op
`Configure<ConcurrencyOptions>` and `Configure<HttpResilienceOptions>` registrations
have also been deleted.

## Module condition predicates

`WithSkipWhen` now has boolean predicate overloads that accept an optional skip reason.
When the reason is omitted, a skipped module reports `Skip condition was met`; an
explicitly passed reason must not be blank. Use
`SkipDecision.When(bool, string?)` when constructing a decision directly.
`SkipDecision.Of(bool, string?)` has been removed; use `When` or a `WithSkipWhen`
predicate overload.

Repeated `WithSkipWhen` calls are OR-ed: the module is skipped when any condition
returns skip. In V3 a later call replaced an earlier one. Analyzer `MP0020` now warns about
repeated calls on the same builder during normal builds. Projects that treat warnings as errors
must address or suppress this diagnostic. Use `WithSkipWhenAll` to skip only when every condition
applies, or suppress `MP0020` around calls that intentionally use OR semantics.

Because a reason-less `WithSkipWhen` call now matches both the boolean and the
`SkipDecision` overloads, a lambda whose body only throws (for example
`_ => throw new InvalidOperationException()`) no longer compiles (CS0121). Give the
lambda an explicit return type, such as `SkipDecision (_) => throw ...`, to select an
overload.

Asynchronous module predicates now consistently use `ValueTask`. The
`ModuleConfiguration.IgnoreFailuresCondition` property and the asynchronous
`ModuleConfigurationBuilder.WithIgnoreFailuresWhen` overload therefore accept
`Func<IModuleContext, Exception, CancellationToken, ValueTask<bool>>` instead of the previous
`Task<bool>` delegate. Explicitly typed callers must migrate inside their module's
configuration hook for v4:

```csharp
protected override void Configure(ModuleConfigurationBuilder module)
{
    Func<IModuleContext, Exception, CancellationToken, ValueTask<bool>> ignoreFailure =
        (context, exception, cancellationToken) => ValueTask.FromResult(exception is ApiValidationException);

    module.WithIgnoreFailuresWhen(ignoreFailure);
}
```

## Pipeline builder creation

`Pipeline.CreateBuilder(args)` now infers the pipeline project directory from the
calling source file. `Pipeline.CreateBuilderFromSource` has been removed; use
`CreateBuilder` for both inferred and explicitly configured builders.

`PipelineBuilderOptions` is now `PipelineBuilderSettings`. The build-time assembly
discovery flag moved from `PipelineOptions.LoadModularPipelineAssemblies` to
`PipelineBuilderSettings.LoadModularPipelinesAssemblies`:

```csharp
var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
{
    Args = args,
    LoadModularPipelinesAssemblies = true,
});
```

`PipelineBuilder` no longer implements `IDisposable`; its previous `Dispose` method
performed no cleanup. Remove `using` declarations around builders. Resources created
by a successful build remain owned by the resulting pipeline.

## CLI argument ordering

`CommandLinePhase` is now the only ordering model for flags, options, and arguments.
`ArgumentPlacement` has been removed. Migrate custom positional arguments as follows:

`CommandLinePhase.EndOfOptions` has been removed. Use
`CliArgumentAttribute.PrependOptionTerminator` instead.

```csharp
// V3
[CliArgument(0, Placement = ArgumentPlacement.BeforeOptions)]
public string? Path { get; init; }

// V4
[CliArgument(0, Phase = CommandLinePhase.EarlyOperand)]
public string? Path { get; init; }
```

Generated option types now emit an explicit phase for every positional argument.
Operands documented before an `[OPTIONS]` or `[flags]` token use `EarlyOperand`; operands
after that token use `Passthrough`. Generated command-line sequences otherwise remain
unchanged.

The complete subcommand chain is atomic. `EarlyOperand` values render after the final
subcommand, while properties inherited from a `[CliGlobalOptions]` type remain the only
values that render before the first subcommand.

Intentional behavior changes:

- A hand-written `[CliArgument]` defaults to `Passthrough`. The generator model's
  `CliPositionalArgument.Phase` defaults to `EarlyOperand`, so generator extensions
  that omit the phase place operands before normal options.
- A contradictory placement/phase combination can no longer silently ignore its phase,
  because `ArgumentPlacement` no longer exists.

## Canonical key-value CLI options

Generated key-value option properties now consistently use `IReadOnlyList<KeyValue>?`.
Properties previously emitted as `KeyValue[]?` or `IEnumerable<KeyValue>?` are
source-breaking only for callers that depend on those exact declared types. Collection
expressions and `KeyValue` tuple conversions continue to work:

```csharp
var options = new DockerRunOptions
{
    Annotation = [("owner", "platform"), ("environment", "ci")],
};
```

Two-operand options now use `CliValuePair` instead of `CliOptionValuePair`:

```csharp
var options = new JqExecuteOptions
{
    Arg = [new CliValuePair("name", "Ada")],
};
```

`CliValuePair` options must use a space separator because each occurrence renders as
`--option first second`. Other `OptionFormat` values now throw during command construction
instead of being silently ignored.

## CLI argument placeholders removed

`CliArgumentAttribute.Name` and `<PLACEHOLDER>` substitution in command parts have
been removed. Every `[CliArgument]` value is now rendered as a positional argument;
it can no longer disappear because a matching placeholder was absent.

For a command chain that depends on constructor input, compute `CommandParts`
explicitly:

```csharp
// Before: Action disappeared when <ACTION> was missing or did not match.
[CliCommand("tool", "resource", "<ACTION>")]
public record ResourceOptions(
    [property: CliArgument(0, Name = "<ACTION>")] string Action)
    : CommandLineToolOptions;

// V4: the constructor owns the dynamic command chain.
[CliTool("tool")]
public record ResourceOptions : CommandLineToolOptions
{
    public ResourceOptions(string action)
    {
        CommandParts = ["resource", action];
    }
}
```

Use `[CliArgument]` only for positional values that follow the command chain.

## Distributed coordination contract

- Workers claim work as a `ModuleLease` under a `WorkerId`: `DequeueModuleAsync(WorkerId, capabilities, ct)`
  returns `ModuleLease?`, and `PublishResultAsync(result, lease, ct)` publishes against it. Heartbeats renew
  leases, and the master requeues work whose lease expired. The first published result is final.
  A module whose lease expires can run more than once (at-least-once execution), so make modules with
  external side effects idempotent.
- `WorkerId` replaces the `int WorkerIndex` on registrations, statuses, results, `RemoteModuleException`,
  `ModuleResult` and run reports. `WorkerStatus.IsFinal` marks final metrics; `WorkerStatus.IsLive` is internal.
- `BroadcastCancellationAsync(reason, ct)` takes a `PipelineFailed` or `Stopped` reason, and
  `WaitForCancellationAsync` returns it. Master coordinators add `WithdrawAssignmentAsync`,
  `GetActiveLeasesAsync` and `RequeueExpiredLeasesAsync`. After a failure, only AlwaysRun work is handed out.
- Wire types (`ModuleAssignment`, `SerializedModuleResult`, `WorkerRegistration`, `WorkerStatus`, artifact
  references) are `required`/`init` records. `ModuleAssignmentOptions` is removed; `AlwaysRun` is on the
  assignment.
- `IExecutionBackend.ExecuteAsync` takes an `ExecutionBackendRequest`.
- `DistributedOptions.CapabilityTimeout` is now `WorkerRegistrationTimeout`. `ModuleResultTimeout` counts from
  the claim, and the per-attempt module timeout is enforced on the worker.
- Converting a `string` to `Capability` is explicit. `IMasterDiscovery` exchanges a `MasterEndpoint`.
- Run ids are restricted to `[A-Za-z0-9._-]`. A multi-instance run without a shared coordinator fails at
  startup, and registering a second coordinator or artifact store backend throws.
- A failed artifact upload fails the module, and consumers download the artifacts referenced by the accepted
  result.

## Distributed packages

- SignalR: `MasterUrl` is split into `ListenUrl` and `AdvertisedUrl`; `MaxReceiveMessageSize` is
  `MaxMessageSizeBytes`; tunnel settings move to `Tunnel`; `EnableAutoReconnect` and `ReconnectGrace` are
  removed. The hub requires an `AccessToken` whenever it is reachable beyond the machine; one is generated and
  shared through discovery when unset. Workers pull leases instead of receiving pushed work.
- Redis: `RedisDistributedOptions` is `RedisOptions`, `KeyExpiration` is `TimeToLive`, and
  `ConfigureConnection` adjusts the parsed connection. The package neither registers nor uses an application
  `IConnectionMultiplexer`. Module cache keys move to `v2` with a cluster hash tag.
- S3: `S3ArtifactOptions` is `S3StorageOptions`, `KeyPrefix` defaults to `modpipe`, `SetLifecycleRule`
  defaults to `false` and merges with existing rules, and large objects use multipart uploads.
- Discovery: `Ttl` is `TimeToLive`, `KeyPrefix` defaults to `modpipe`, and the package owns its connection.
- `RedisModuleCache` and `S3ModuleCache` are internal. Each backend has one `Action<TOptions>` and one
  `IConfigurationSection` registration overload; options are validated at startup.
- `ArtifactOptions` is removed. Configure compression through `DistributedOptions.ArtifactCompressionLevel`;
  `AutoCleanup`, `ChunkSizeBytes`, `MaxSingleUploadBytes` and `TimeToLive` move to backend
  options or are removed.

## Hooks, plugins and requirements

- `IModuleHookContext.RequestRetry`, `SkipDependentModules` and `FailPipeline` are removed; they had no effect.
- Module, pipeline and registration handler methods take a trailing `CancellationToken`.
  `IEventHandler.Priority` is `Order` (ascending, default 0), matching requirements and validators.
- A failing global Ready or Start handler fails the module. End, Failure and Skipped handler failures no longer
  change the module's outcome and are reported as additional pipeline errors. A failing pipeline-end handler
  no longer hides an execution failure.
- `IModuleRegistrationContext.Services` is removed.
- `PluginRegistry` and `PluginTestHelper` are removed. `IModularPipelinesPlugin` has `Name` and
  `Configure(PipelineBuilder)`; register plugins with `builder.AddPlugin<T>()` or `AddPlugin(instance)`.
- `IBuildSystemContext` exposes `Current`, `Is(BuildSystem)`, `IsCI` and `IsLocal` instead of one flag per CI
  system. `OnCI`, `OnLocal` and `Require.Ci()` share one CI definition. A truthy `CI` variable marks the run as
  CI even when no known build agent is detected; the pipeline logs this once at startup.
- `ISecretObfuscator` is internal; provide secrets through `ISecretRegistry`, `[SecretValue]` or
  `SecretMaskingOptions`.
- `IModuleEstimatedTimeProvider`, `IModuleResultRepository` and `IPipelineValidator` take cancellation tokens.
  Time estimates are best-effort and never fail a module.
- `PipelineRequirement.EvaluateAsync` is abstract, `DelegateRequirement` is internal, and all requirement
  failures are reported together in one `RequirementNotMetException`.

## Conditions and extension seams

- `IRunCondition` has one member, `EvaluateAsync(IPipelineContext, CancellationToken)`. Custom condition
  attributes derive from `RunConditionAttribute(ConditionIntent)` and may override `GroupKey`.
  `IConditionAttribute`, `IGroupedConditionAttribute`, the abstract `RunIfAttribute`, `RunIfAllAttribute`,
  `RunIfAnyAttribute` and `SkipIfAttribute` bases, `RunIfAll<...>` and `ConditionLogic.Skip` are removed.
- The builder adds `WithRunIf<T>()`, `WithRunIf(IRunCondition)`, `WithSkipIf<T>()` and `WithSkipIf(IRunCondition)`.
- `IPlanningSafe` replaces `IPlanningRunCondition`, `PlanningSafeDependsOnBaseAttribute`,
  `IPlanningSafeDependencySelector` and `IPlanningSafeModuleRegistrationHandler`.
- `ICommandInterceptor` is middleware: `InvokeAsync(invocation, next, ct)`. Register interceptors with
  `AddCommandInterceptor<T>()` or `AddCommandInterceptor(instance)`. Interceptors run in registration order,
  outermost first. Call `next` to continue to the next interceptor and then the process. Return without calling
  `next` to short-circuit; the framework still applies command metadata, logging and the nonzero-exit-code check
  to that result. An interceptor must return a non-null `CommandResult`; a null result throws
  `InvalidOperationException`.
  `ICommandInterceptor`, `CommandDelegate` and `CommandInvocation` moved from `ModularPipelines.Context.Domains.Shell`
  to the root `ModularPipelines` namespace, next to the other extension seams. Replace
  `using ModularPipelines.Context.Domains.Shell;` with `using ModularPipelines;`.
- `IModule` cannot be implemented outside ModularPipelines. `DependsOnAttribute`, `DependsOnAttribute<T>`,
  `DependsOnAllModulesInheritingFromAttribute` and `SecretValueAttribute` are sealed.
- Custom `IFileSystemProvider` implementations add attribute, timestamp and length members; custom
  `IModuleCacheStore` implementations add `DeleteAsync`. `IModuleCacheStore.ExistsAsync` is new too, with a
  default implementation that stores can override.
- `ModuleCacheOptions` is an init-only record configured with `Func<ModuleCacheOptions, ModuleCacheOptions>`.
- `WithTimeout` rejects zero and negative values; use `Timeout.InfiniteTimeSpan` to disable the timeout.
- Registration helpers for single-instance services replace earlier registrations; multi-instance helpers
  add each type once.

## Capability condition evaluation

`ICapabilityCondition` now supplies a default `IRunCondition.EvaluateAsync` implementation.
Custom conditions only need a `Capability` property; evaluation checks the executing
process's declared capabilities, including registered `ICapabilityProvider` results.
Move hardware detection into a capability provider so routing and execution agree.
Existing explicit implementations remain supported during local and worker execution.
The distributed master uses only the capability declaration when constructing routes;
it does not execute a worker's predicate on the master.

## Builder and configuration contracts

Register dependencies through `builder.Services` instead of the removed
`builder.ConfigureServices(...)` extension. For example, replace
`builder.ConfigureServices(services => services.AddSingleton<MyService>())` with
`builder.Services.AddSingleton<MyService>()`.

`ModuleConfiguration` exposes read-only values and metadata. Its setters and
execution delegates are internal; configure module behavior through
`ModuleConfigurationBuilder` in your module's `Configure` override.
`PipelineOptions` and `ConcurrencyOptions` are sealed records; continue using
object initializers and `with` expressions instead of deriving from them.

The `IEnumerable<IModule>.GetModule<T>()` extension is now internal. Use LINQ
`OfType<T>().Single()` when selecting a module from a collection.

## Runtime secret registration

Modules can register dynamically discovered secrets with
`context.Security.Secrets.AddSecret(value)` or `AddSecrets(values)`. This exposes
the same per-pipeline `ISecretRegistry` available through dependency injection.
Register values before logging them so subsequent output uses the configured
masking behavior.

## Artifact store registration conflicts

`AddDistributedArtifactStore<TStore>()` and `AddDistributedArtifactStoreFactory<TFactory>()`
now reject conflicting backends in either registration order. Direct registration no longer
silently removes an earlier factory, and two different direct stores cannot be selected together.
Choose one registration at the call site. Repeating the same typed registration is a no-op;
keyed services and the default filesystem fallback are unchanged.

### kind inherited logging options

`Quiet` and `Verbosity` now live on `KindOptions` and render before the subcommand. Existing initializers remain valid. Reflection code using `DeclaredOnly` should include inherited properties. Cluster-specific options remain local, and root help/version actions are not inherited settings. See the [kind package guide](docs/docs/mp-packages/kind.md).

## Azure CLI parser contracts

Azure generated properties now follow the installed argparse action. Values that
look numeric can remain strings (for example Spark job `Executors = "1"`), while
optional-value options use `CliOptionValue` for distinct omitted, bare, and explicit
value forms. Replace boolean assignments to optional `ForceString` with `"true"`
or `"false"`; use `CliOptionValue.Bare` for its bare-switch form.

Repeated groups use `IEnumerable<CliValueGroup>` instead of a flat string collection.
For example, Cassandra `ClusterArguments = [new CliValueGroup(["first=value",
"second=value"])]` emits one `--arguments` occurrence with both operands. Add another
group to repeat the option. ARO `AssignPlatformWi` similarly preserves each operator
and identity pair. See the Azure package guide for the DevOps extension prerequisite
and noninteractive PAT authentication.

## Module cache option names

The `ModuleCacheOptions` limits use `Max` instead of `Maximum`: `MaxInputFiles`,
`MaxArtifactEntries`, `MaxArtifactBytes`, `MaxCacheEntryBytes`, `MaxResultBytes`, and
`MaxHashConcurrency`. Update object initializers, `with` expressions, and any serialized
configuration keys. Defaults and enforcement are unchanged for local, Redis, and S3 caches.
See the [cache option migration table](docs/docs/how-to/module-caching.md#v4-option-names).

## Cargo common options

Cargo command records inherit stable root settings from `CargoOptions`, rendered
before the command name. `Verbose` remains a nullable count; `Config` remains a
collection with one `--config` per entry and is now masked as secret-bearing input.
Replace per-command color enum names (for example `CargoBuildColor`) with `CargoColor`.
Command-specific manifest/build settings and nightly `Z` options stay on their
applicable records. No unconditional `-C` or rustup `+toolchain` property is added.

## Core namespaces and module attributes

One `using ModularPipelines;` now also covers `NotInParallelAttribute`,
`PriorityAttribute`, `ExecutionHintAttribute`, `ExecutionHint`, `BuildSystem`,
`PipelineSummary`, `IModuleResult`, `IParallelLimit`, `CommandExtensions`, and
`EnumerableExtensions`. Replace fully qualified references to their former
`.Attributes`, `.Enums`, `.Models`, `.Interfaces`, or `.Extensions` namespaces.
The three attributes are sealed; use their constructor arguments or fluent module
configuration instead of deriving from them.

File and folder extension methods now live beside their path types in
`ModularPipelines.FileSystem`. Dependency graph export uses
`ModularPipelines.Reporting.DependencyGraphFormat`, alongside
`IDependencyGraphExporter`.

Tests that also use TUnit's `NotInParallel` attribute should qualify it as
`TUnit.Core.NotInParallel` to distinguish test scheduling from module scheduling.

## .NET SDK and nbgv option scope

Generated .NET SDK command records now inherit `Diagnostics` from `DotNetOptions`.
`Diagnostics = true` renders `--diagnostics` before the command path; false and null
omit it. Command verbosity, runtime-host settings, and root information actions are
not universal SDK execution properties.

The .NET `nbgv` 3.10.94 audit requires no global API change: its root has only
help/version actions. Continue setting `Project` and other values on command
records. In particular, `NbgvCloudOptions.Version` remains the cloud build-number
value and is not the root version-information action.
