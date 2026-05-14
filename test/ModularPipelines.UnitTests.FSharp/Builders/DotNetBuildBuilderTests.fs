namespace ModularPipelines.UnitTests.FSharp.Builders

open System
open System.Collections.Generic
open System.Linq
open System.Threading
open ModularPipelines.Context
open ModularPipelines.DotNet.Builders
open ModularPipelines.DotNet.Options
open ModularPipelines.Models
open ModularPipelines.Options
open ModularPipelines.TestHelpers
open Moq
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module private DotNetBuildBuilderTestHelpers =
    let createDotNetBuildResult workingDirectory =
        CommandResult(
            "dotnet build",
            workingDirectory,
            "",
            "",
            Dictionary<string, string>(),
            DateTimeOffset.Now,
            DateTimeOffset.Now,
            TimeSpan.Zero,
            0
        )

    let createDotNetBuildMockCommand() =
        let mockCommand = Mock<ICommand>()

        mockCommand
            .Setup(fun c ->
                c.ExecuteCommandLineTool(
                    It.IsAny<CommandLineToolOptions>(),
                    It.IsAny<CommandExecutionOptions>(),
                    It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync(createDotNetBuildResult "/working/dir")
        |> ignore

        mockCommand

open DotNetBuildBuilderTestHelpers

type DotNetBuildBuilderTests() =
    inherit TestBase()

    [<Test>]
    member _.ForProject_SetsProjectPath() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder.ForProject("MyProject.csproj") |> ignore

        let struct (toolOptions, _) = builder.ToOptions()
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.ProjectSolution), "MyProject.csproj"))
    }

    [<Test>]
    member _.WithConfiguration_SetsConfiguration() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder.WithConfiguration("Release") |> ignore

        let struct (toolOptions, _) = builder.ToOptions()
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Configuration), "Release"))
    }

    [<Test>]
    member _.WithFramework_SetsFramework() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder.WithFramework("net8.0") |> ignore

        let struct (toolOptions, _) = builder.ToOptions()
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Framework), "net8.0"))
    }

    [<Test>]
    member _.WithRuntime_SetsRuntime() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder.WithRuntime("win-x64") |> ignore

        let struct (toolOptions, _) = builder.ToOptions()
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Runtime), "win-x64"))
    }

    [<Test>]
    member _.WithOutput_SetsOutput() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder.WithOutput("/output/path") |> ignore

        let struct (toolOptions, _) = builder.ToOptions()
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Output), "/output/path"))
    }

    [<Test>]
    member _.WithNoRestore_EnablesNoRestore() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder.WithNoRestore() |> ignore

        let struct (toolOptions, _) = builder.ToOptions()
        do! check(Assert.That(toolOptions.NoRestore.HasValue && toolOptions.NoRestore.Value).IsTrue())
    }

    [<Test>]
    member _.WithNoIncremental_EnablesNoIncremental() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder.WithNoIncremental() |> ignore

        let struct (toolOptions, _) = builder.ToOptions()
        do! check(Assert.That(toolOptions.NoIncremental.HasValue && toolOptions.NoIncremental.Value).IsTrue())
    }

    [<Test>]
    member _.WithNoLogo_EnablesNoLogo() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder.WithNoLogo() |> ignore

        let struct (toolOptions, _) = builder.ToOptions()
        do! check(Assert.That(toolOptions.Nologo.HasValue && toolOptions.Nologo.Value).IsTrue())
    }

    [<Test>]
    member _.WithProperty_AddsProperty() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder.WithProperty("Version", "1.0.0") |> ignore

        let struct (toolOptions, _) = builder.ToOptions()
        match toolOptions.Properties with
        | null -> failwith "Expected properties"
        | properties ->
            do! check(Assert.That(properties.Count() = 1).IsTrue())
            do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(properties.First().Key), "Version"))
            do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(properties.First().Value), "1.0.0"))
    }

    [<Test>]
    member _.WithProperty_AddsMultipleProperties() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder
            .WithProperty("Version", "1.0.0")
            .WithProperty("AssemblyVersion", "1.0.0.0")
        |> ignore

        let struct (toolOptions, _) = builder.ToOptions()
        match toolOptions.Properties with
        | null -> failwith "Expected properties"
        | properties ->
            do! check(Assert.That(properties.Count() = 2).IsTrue())
    }

    [<Test>]
    member _.WithWorkingDirectory_SetsWorkingDirectory() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder.WithWorkingDirectory("/project/dir") |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(execOptions.WorkingDirectory), "/project/dir"))
    }

    [<Test>]
    member _.WithTimeout_SetsTimeout() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)
        let timeout = TimeSpan.FromMinutes(30.0)

        builder.WithTimeout(timeout) |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        do! check(Assert.That(execOptions.ExecutionTimeout = timeout).IsTrue())
    }

    [<Test>]
    member _.WithEnvironmentVariable_AddsVariable() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder.WithEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1") |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        match execOptions.EnvironmentVariables with
        | null -> failwith "Expected environment variables"
        | environmentVariables ->
            do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(environmentVariables["DOTNET_CLI_TELEMETRY_OPTOUT"]), "1"))
    }

    [<Test>]
    member _.WithThrowOnError_SetsThrowOnError() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder.WithThrowOnError(false) |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        do! check(Assert.That(execOptions.ThrowOnNonZeroExitCode).IsFalse())
    }

    [<Test>]
    member _.FluentChaining_SetsAllOptions() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder
            .ForProject("MyProject.sln")
            .WithConfiguration("Release")
            .WithFramework("net8.0")
            .WithNoRestore()
            .WithNoLogo()
            .WithProperty("Version", "2.0.0")
            .WithWorkingDirectory("/project")
            .WithTimeout(TimeSpan.FromMinutes(15.0))
        |> ignore

        let struct (toolOptions, execOptions) = builder.ToOptions()

        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.ProjectSolution), "MyProject.sln"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Configuration), "Release"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Framework), "net8.0"))
        do! check(Assert.That(toolOptions.NoRestore.HasValue && toolOptions.NoRestore.Value).IsTrue())
        do! check(Assert.That(toolOptions.Nologo.HasValue && toolOptions.Nologo.Value).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Properties.First().Key), "Version"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(execOptions.WorkingDirectory), "/project"))
        do! check(Assert.That(execOptions.ExecutionTimeout = TimeSpan.FromMinutes(15.0)).IsTrue())
    }

    [<Test>]
    member _.FluentChaining_ReturnsSameBuilderInstance() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)

        let result1 = builder.ForProject("test.csproj")
        let result2 = result1.WithConfiguration("Release")
        let result3 = result2.WithFramework("net8.0")

        do! check(Assert.That(obj.ReferenceEquals(builder, result1)).IsTrue())
        do! check(Assert.That(obj.ReferenceEquals(result1, result2)).IsTrue())
        do! check(Assert.That(obj.ReferenceEquals(result2, result3)).IsTrue())
    }

    [<Test>]
    member _.ExecuteAsync_CallsCommandExecuteWithOptions() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let builder = DotNetBuildBuilder(mockCommand.Object)
        let mutable capturedToolOptions = Unchecked.defaultof<DotNetBuildOptions>
        let mutable callCount = 0

        mockCommand
            .Setup(fun c ->
                c.ExecuteCommandLineTool(
                    It.IsAny<CommandLineToolOptions>(),
                    It.IsAny<CommandExecutionOptions>(),
                    It.IsAny<CancellationToken>()
                ))
            .Callback(Action<CommandLineToolOptions, CommandExecutionOptions, CancellationToken>(fun options _ _ ->
                callCount <- callCount + 1
                capturedToolOptions <- options :?> DotNetBuildOptions))
            .ReturnsAsync(createDotNetBuildResult "/working/dir")
        |> ignore

        builder
            .ForProject("MyProject.csproj")
            .WithConfiguration("Release")
        |> ignore

        let! result = builder.ExecuteAsync() |> Async.AwaitTask
        let hasExpectedCallCount = callCount = 1
        let capturedToolOptionsExists = obj.ReferenceEquals(capturedToolOptions, null) = false

        do! check(Assert.That(result).IsNotNull())
        do! check(Assert.That(hasExpectedCallCount).IsTrue())
        do! check(Assert.That(capturedToolOptionsExists).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(capturedToolOptions.ProjectSolution), "MyProject.csproj"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(capturedToolOptions.Configuration), "Release"))
    }

    [<Test>]
    member _.ExecuteAsync_PassesExecutionOptions() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let mutable capturedExecOptions = Unchecked.defaultof<CommandExecutionOptions>

        mockCommand
            .Setup(fun c ->
                c.ExecuteCommandLineTool(
                    It.IsAny<CommandLineToolOptions>(),
                    It.IsAny<CommandExecutionOptions>(),
                    It.IsAny<CancellationToken>()
                ))
            .Callback(Action<CommandLineToolOptions, CommandExecutionOptions, CancellationToken>(fun _ execOptions _ ->
                capturedExecOptions <- execOptions))
            .ReturnsAsync(createDotNetBuildResult "/test/dir")
        |> ignore

        let builder = DotNetBuildBuilder(mockCommand.Object)

        builder
            .WithWorkingDirectory("/test/dir")
            .WithTimeout(TimeSpan.FromMinutes(10.0))
        |> ignore

        do! builder.ExecuteAsync() |> Async.AwaitTask |> Async.Ignore
        let capturedExecOptionsExists = obj.ReferenceEquals(capturedExecOptions, null) = false

        do! check(Assert.That(capturedExecOptionsExists).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(capturedExecOptions.WorkingDirectory), "/test/dir"))
        do! check(Assert.That(capturedExecOptions.ExecutionTimeout = TimeSpan.FromMinutes(10.0)).IsTrue())
    }

    [<Test>]
    member _.InitialOptions_UsesProvidedOptions() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let initialOptions = DotNetBuildOptions(Configuration = "Debug", Framework = "net7.0")
        let builder = DotNetBuildBuilder(mockCommand.Object, initialOptions)

        let struct (toolOptions, _) = builder.ToOptions()

        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Configuration), "Debug"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Framework), "net7.0"))
    }

    [<Test>]
    member _.InitialOptions_CanBeOverridden() = async {
        let mockCommand = createDotNetBuildMockCommand()
        let initialOptions = DotNetBuildOptions(Configuration = "Debug")
        let builder = DotNetBuildBuilder(mockCommand.Object, initialOptions)

        builder.WithConfiguration("Release") |> ignore

        let struct (toolOptions, _) = builder.ToOptions()
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Configuration), "Release"))
    }
