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

`ICommandInterceptor` is also a public framework seam. Register an implementation
in a normal pipeline when command interception is needed outside
`ModularPipelines.Testing`. Return `null` to let the next interceptor or the real
process executor handle the command.

## Use the in-memory filesystem

Files obtained through `context.Files` automatically use the harness filesystem:

```csharp
var run = await ModuleTester.For<ManifestModule, string>()
    .ExecuteAsync();

var manifest = await run.FileSystem.ReadAllTextAsync("/output/manifest.json");
```

`InMemoryFileSystemProvider` implements `IFileSystemProvider`, including file and
directory creation, reads, writes, streams, copies, moves, deletion, enumeration,
and path helpers. You can also construct and register it directly in other tests.
Physical metadata such as attributes, timestamps, and file length is not part of
`IFileSystemProvider`; accessing it through an in-memory-backed `File` or `Folder`
throws `NotSupportedException` rather than reading the real filesystem.

Code under test must obtain `File` and `Folder` instances from `context.Files`.
Direct construction such as `new File("path")` intentionally uses the physical
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
returned for assertions. Successful runs expose `Status.Successful`.

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
