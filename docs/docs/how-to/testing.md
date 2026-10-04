---
title: Testing
sidebar_position: 9
---

Install `ModularPipelines.Testing` to execute one module without starting the full
pipeline scheduler:

```bash
dotnet add package ModularPipelines.Testing
```

The test harness uses the normal module execution pipeline, so skip conditions,
timeouts, retries, and direct module hooks behave as they do in a pipeline. It
provides test-safe defaults:

- external commands are intercepted and return a successful result;
- file and directory operations use an isolated in-memory filesystem;
- progress, logos, dependency chains, and result printing are disabled;
- module failures are returned for assertions instead of escaping from the harness.

## Execute a module

Specify the module and result types for strongly typed value access:

```csharp
using ModularPipelines.Testing;

[Test]
public async Task Build_returns_the_artifact()
{
    var run = await ModuleTester.For<BuildModule, BuildArtifact>()
        .ExecuteAsync();

    await Assert.That(run.Value!.Name).IsEqualTo("application.zip");
    await Assert.That(run.Exception).IsNull();
}
```

If only the module type is convenient, use the type-erased overload. `Value` is
then `object?`, while `Result` still contains the full module metadata:

```csharp
var run = await ModuleTester.For<BuildModule>().ExecuteAsync();

var artifact = (BuildArtifact)run.Value!;
```

## Seed dependency results

Register a dependency result without executing that dependency:

```csharp
var restoredPackages = CommandResult.Ok("Restore succeeded.");

var run = await ModuleTester.For<BuildModule, BuildArtifact>()
    .WithDependencyResult<RestoreModule, CommandResult>(restoredPackages)
    .ExecuteAsync();
```

The dependency module is registered normally, then its successful result is
completed before the target module starts. Calls such as
`await context.GetModule<RestoreModule>()` therefore receive the seeded value.
If a required dependency has no seeded result, `ExecuteAsync` fails immediately
and names the missing dependency instead of waiting for the module timeout.

Use `WithDependencyFailure<RestoreModule, CommandResult>(exception)` or
`WithSkippedDependency<RestoreModule, CommandResult>("not needed")` to seed other
outcomes. Both register and complete the dependency without executing it. Normal
dependency failure and skip rules still apply. A failed registered dependency
produces `ModuleStatus.DependencyFailed` without executing the target, unless the
target uses `WithAlwaysRun`. A skipped required dependency skips the target,
including an always-run target; a skipped optional dependency does not. Optional
means that registration is optional, so a registered optional dependency's failure
still blocks a target without `WithAlwaysRun`.

## Seed files and configure the pipeline

```csharp
var run = await ModuleTester.For<BuildModule, BuildArtifact>()
    .WithFile("inputs/settings.json", "{\"environment\":\"test\"}")
    .WithFile("inputs/data.bin", new byte[] { 1, 2, 3 })
    .ConfigurePipeline(builder =>
    {
        builder.Configuration["Build:Configuration"] = "Release";
        builder.ConfigureOptions(options => options with
        {
            Concurrency = options.Concurrency with { MaxParallelism = 1 },
        });
    })
    .ExecuteAsync();
```

File paths resolve exactly as `context.Files.GetFile`: relative paths use the
pipeline working directory, and absolute paths retain their location. The harness
creates parent directories and writes seeds before pipeline service initialization,
so initializers registered through `ConfigurePipeline` can read them. Strings use
UTF-8; byte arrays are copied when registered. Repeating the same seed path replaces
its contents. Each execution gets a fresh in-memory provider by default.

To supply your own provider, call `WithService<IFileSystemProvider>(provider)`.
File seeds and module operations both use that provider, which is also returned as
`run.FileSystem`. A supplied provider is shared across runs and may perform real I/O;
use `InMemoryFileSystemProvider` to retain isolation.

`ConfigurePipeline` callbacks run after harness defaults, in registration order,
before `BuildAsync`. They can configure options, configuration, capabilities, and
services through the normal `PipelineBuilder` API. Overrides can replace the
harness's safe defaults, including command interception and console services.

## Seed consumed artifacts

Seed each artifact declared by the module before executing it:

```csharp
var run = await ModuleTester.For<DeployModule, string>()
    .WithDependencyResult<BuildModule, BuildArtifact>(buildArtifact)
    .WithArtifact<BuildModule>("application", "artifact contents")
    .ExecuteAsync();
```

The harness writes a seeded single-file artifact to the declaration's `RestorePath`, using the
artifact name as the file name. Binary contents can be passed as a `byte[]`. A consumed artifact
that was not seeded fails the run before the module body executes.

The isolated harness does not exercise artifact upload/download, archive extraction, or produced
artifact glob matching. Assert files produced through `context.Files` via `run.FileSystem`; use an
integration pipeline test when the artifact transport lifecycle itself is under test.

## Intercept and inspect commands

Commands never start real processes unless you explicitly replace the test
harness behavior. The default interceptor returns `CommandResult.Ok()`.

Provide a handler when a module needs command output:

```csharp
var run = await ModuleTester.For<BuildModule, BuildArtifact>()
    .InterceptCommands(invocation =>
    {
        if (invocation.CommandLine.Tool == "dotnet")
        {
            return CommandResult.Ok("Build succeeded.");
        }

        return CommandResult.Ok();
    })
    .ExecuteAsync();

await Assert.That(run.Commands).Count().IsEqualTo(1);
await Assert.That(run.Commands[0].CommandLine.Arguments)
    .IsEquivalentTo(["build", "--configuration", "Release"]);
```

Each `RecordedCommand` contains the parsed `CommandInvocation` and the simulated
`CommandResult`. This avoids assertions against a quoted display string.
Intercepted nonzero exit codes follow `CommandExecutionOptions` normally and
throw `CommandException` when `ThrowOnNonZeroExitCode` is enabled.

Use `CommandResult.Fail(exitCode: 17, standardError: "build failed")` for failed
commands. It defaults to exit code 1 and rejects zero; standard output and standard
error are preserved for assertions.

`ICommandInterceptor` is also a public framework seam: middleware that wraps every
command after it is parsed and before the process starts. Register one with
`builder.AddCommandInterceptor<TInterceptor>()` (adding the same type twice has no
effect) or `builder.AddCommandInterceptor(instance)`. Interceptors run in
registration order, so the first registered one is outermost.

```csharp
public sealed class ForceVerbosityInterceptor : ICommandInterceptor
{
    public async ValueTask<CommandResult> InvokeAsync(
        CommandInvocation invocation,
        CommandDelegate next,
        CancellationToken cancellationToken)
    {
        var changed = invocation with
        {
            CommandLine = new CommandLine(
                invocation.CommandLine.Tool,
                [.. invocation.CommandLine.Arguments, "--verbosity", "minimal"]),
        };

        var result = await next(changed, cancellationToken);
        // Observe or replace the result here.
        return result;
    }
}
```

Call `next` to continue to the next interceptor or the process executor, optionally
with a modified `CommandLine`, `ExecutionOptions`, or `WorkingDirectory`.
`CommandInput` and `EnvironmentVariables` are secret-masked views that the framework
regenerates; change environment variables through `ExecutionOptions`. Return a
result without calling `next` to short-circuit: the framework applies command
metadata, logs it, and throws `CommandException` for a nonzero exit code when
`ThrowOnNonZeroExitCode` is enabled. The execution timeout starts before the
interceptors run and cannot be changed by them.

## Use the in-memory filesystem

Files obtained through `context.Files` automatically use the harness filesystem:

```csharp
var run = await ModuleTester.For<ManifestModule, string>()
    .ExecuteAsync();

var manifest = await run.FileSystem.ReadAllTextAsync("/output/manifest.json");
```

`InMemoryFileSystemProvider` implements `IFileSystemProvider`, including file and
directory creation, reads, writes, streams, copies, moves, deletion, enumeration,
path helpers, and metadata (attributes, UTC timestamps, and file length). You can also
construct and register it directly in other tests. `FilePath` and `FolderPath` read and
write metadata through the provider, so an in-memory-backed path never touches the real
filesystem, and `FolderPath.CopyTo(target, preserveTimestamps: true)` copies the in-memory
timestamps.

To write your own provider, implement the primitive members of `IFileSystemProvider`
(`Open`, file and directory management, existence checks, enumeration, and the metadata
getters and setters). The text, bytes, lines, append, `OpenRead`, `Create`, `CopyFile`,
`GetFileLength`, and path-helper members have default implementations built on those
primitives.

Code under test must obtain `FilePath` and `FolderPath` instances from `context.Files`.
Direct construction such as `new FilePath("path")` intentionally uses the physical
`SystemFileSystemProvider`.

## Register constructor services

Use `WithService` for module constructor dependencies:

```csharp
var settings = new BuildSettings { Configuration = "Release" };

var run = await ModuleTester.For<BuildModule, BuildArtifact>()
    .WithService(settings)
    .ExecuteAsync();
```

## Assert skipped and failed runs

The harness configures `ThrowOnPipelineFailure = false`, so failed modules are
returned for assertions. Successful runs expose `ModuleStatus.Succeeded`.

The run object exposes safe outcome properties:

```csharp
var skipped = await ModuleTester.For<OptionalModule, string>().ExecuteAsync();
await Assert.That(skipped.SkipDecision!.Reason).IsEqualTo("Feature disabled");

var failed = await ModuleTester.For<FailingModule, string>().ExecuteAsync();
await Assert.That(failed.Exception).IsTypeOf<InvalidOperationException>();
await Assert.That(failed.Result).IsTypeOf<ModuleResult<string>.Failure>();
```

Use `Result` when assertions need timing, status, or the discriminated result
variant. Use `Value`, `Exception`, and `SkipDecision` for concise safe access.

## Verify a custom distributed coordinator

The `ModularPipelines.Testing` package ships
`ModularPipelines.Testing.Distributed.DistributedCoordinatorContract`. It contains
the same 19 checks used for the in-memory, Redis, and SignalR coordinator backends.
Call these asynchronous methods from any test framework; violations throw
`InvalidOperationException`, and timed-out waits fail with `TimeoutException`.

```csharp
using ModularPipelines.Testing.Distributed;

// Your factory supplies a fresh backend with an isolated queue/key namespace.
await using var coordinator = await CreateIsolatedCoordinatorAsync(
    workerTimeout: DistributedCoordinatorContract.LeaseTimeout);
await DistributedCoordinatorContract.EnqueueAndDequeueRoundTripsAsync(coordinator);
```

Run each check with a fresh coordinator and dispose its resources afterward. Checks
cover assignment/result round trips, completion, cancellation, withdrawal, leases,
heartbeats, worker registration, capability matching, and queue ordering. Run all
public asynchronous checks for full coverage; passing one method covers only that
behavior. The factory helpers create sample assignments/results, not backend instances.

Configure lease checks with `LeaseTimeout` (500 ms). Registration, duplicate-worker,
and capability-scarcity checks should use a longer worker timeout, such as 30 seconds,
so workers stay live during the check. For `FinalMetricsKeepRegistrationAfterHeartbeatExpiresAsync`,
configure a short worker lifetime and pass a `heartbeatExpiration` delay longer than
that lifetime. Subscription-based backends can pass `waitUntilReady` to applicable
methods to signal that their pending subscription is installed before publishing.
