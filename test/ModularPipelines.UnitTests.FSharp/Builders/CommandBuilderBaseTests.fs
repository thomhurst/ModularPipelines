namespace ModularPipelines.UnitTests.FSharp.Builders

open System
open System.Collections.Generic
open System.Threading
open ModularPipelines.Attributes
open ModularPipelines.Builders
open ModularPipelines.Context
open ModularPipelines.Models
open ModularPipelines.Options
open ModularPipelines.TestHelpers
open Moq
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

[<CliTool("testtool")>]
[<CliCommand("testtool", "command")>]
type private TestToolOptions() =
    inherit CommandLineToolOptions()

    [<CliOption("--config")>]
    member val Configuration: string = Unchecked.defaultof<_> with get, set

    [<CliOption("--framework")>]
    member val Framework: string = Unchecked.defaultof<_> with get, set

    [<CliFlag("--no-restore")>]
    member val NoRestore: Nullable<bool> = Nullable() with get, set

    override this.``<Clone>$``() =
        let clonedOptions = TestToolOptions()
        clonedOptions.Configuration <- this.Configuration
        clonedOptions.Framework <- this.Framework
        clonedOptions.NoRestore <- this.NoRestore
        clonedOptions :> CommandLineToolOptions

module private CommandBuilderBaseTestHelpers =
    let cloneTestToolOptions (options: TestToolOptions) =
        let clonedOptions = TestToolOptions()
        clonedOptions.Configuration <- options.Configuration
        clonedOptions.Framework <- options.Framework
        clonedOptions.NoRestore <- options.NoRestore
        clonedOptions

    let createCommandResult command workingDirectory =
        CommandResult(
            command,
            workingDirectory,
            "output",
            "",
            Dictionary<string, string>(),
            DateTimeOffset.Now,
            DateTimeOffset.Now,
            TimeSpan.Zero,
            0
        )

    let createMockCommand command workingDirectory =
        let mockCommand = Mock<ICommand>()

        mockCommand
            .Setup(fun c ->
                c.ExecuteCommandLineTool(
                    It.IsAny<CommandLineToolOptions>(),
                    It.IsAny<CommandExecutionOptions>(),
                    It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync(createCommandResult command workingDirectory)
        |> ignore

        mockCommand

open CommandBuilderBaseTestHelpers

type private TestToolBuilder(command: ICommand, ?initialOptions: TestToolOptions) =
    inherit CommandBuilderBase<TestToolBuilder, TestToolOptions>(command, defaultArg initialOptions (TestToolOptions()))

    member private this.UpdateToolOptions(update: TestToolOptions -> unit) =
        let options = cloneTestToolOptions this.ToolOptions
        update options
        this.ToolOptions <- options
        this

    member this.WithConfiguration(configuration: string) =
        this.UpdateToolOptions(fun options -> options.Configuration <- configuration)

    member this.WithFramework(framework: string) =
        this.UpdateToolOptions(fun options -> options.Framework <- framework)

    member this.WithNoRestore(?noRestore: bool) =
        this.UpdateToolOptions(fun options -> options.NoRestore <- Nullable(defaultArg noRestore true))

type CommandBuilderBaseTests() =
    inherit TestBase()

    [<Test>]
    member _.WithWorkingDirectory_SetsWorkingDirectory() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)

        builder.WithWorkingDirectory("/test/directory") |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(execOptions.WorkingDirectory), "/test/directory"))
    }

    [<Test>]
    member _.WithTimeout_SetsTimeout() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)
        let timeout = TimeSpan.FromMinutes(5.0)

        builder.WithTimeout(timeout) |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        do! check(Assert.That(execOptions.ExecutionTimeout).IsEqualTo(timeout))
    }

    [<Test>]
    member _.WithEnvironmentVariable_AddsVariable() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)

        builder.WithEnvironmentVariable("MY_VAR", "my_value") |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        match execOptions.EnvironmentVariables with
        | null -> failwith "Expected environment variables"
        | environmentVariables ->
            do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(environmentVariables["MY_VAR"]), "my_value"))
    }

    [<Test>]
    member _.WithEnvironmentVariable_AddsMultipleVariables() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)

        builder
            .WithEnvironmentVariable("VAR1", "value1")
            .WithEnvironmentVariable("VAR2", "value2")
        |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        match execOptions.EnvironmentVariables with
        | null -> failwith "Expected environment variables"
        | environmentVariables ->
            do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(environmentVariables["VAR1"]), "value1"))
            do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(environmentVariables["VAR2"]), "value2"))
    }

    [<Test>]
    member _.WithEnvironmentVariables_AddsDictionary() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)
        let variables = Dictionary<string, string>()
        variables["VAR1"] <- "value1"
        variables["VAR2"] <- "value2"

        builder.WithEnvironmentVariables(variables) |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        match execOptions.EnvironmentVariables with
        | null -> failwith "Expected environment variables"
        | environmentVariables ->
            do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(environmentVariables["VAR1"]), "value1"))
            do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(environmentVariables["VAR2"]), "value2"))
    }

    [<Test>]
    member _.WithSudo_EnablesSudo() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)

        builder.WithSudo() |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        do! check(Assert.That(execOptions.Sudo).IsTrue())
    }

    [<Test>]
    member _.WithSudo_DisablesSudo_WhenFalse() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)

        builder.WithSudo(false) |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        do! check(Assert.That(execOptions.Sudo).IsFalse())
    }

    [<Test>]
    member _.WithThrowOnError_EnablesThrowOnError() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)

        builder.WithThrowOnError() |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        do! check(Assert.That(execOptions.ThrowOnNonZeroExitCode).IsTrue())
    }

    [<Test>]
    member _.WithThrowOnError_DisablesThrowOnError_WhenFalse() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)

        builder.WithThrowOnError(false) |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        do! check(Assert.That(execOptions.ThrowOnNonZeroExitCode).IsFalse())
    }

    [<Test>]
    member _.WithGracefulShutdownTimeout_SetsTimeout() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)
        let timeout = TimeSpan.FromSeconds(60.0)

        builder.WithGracefulShutdownTimeout(timeout) |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        do! check(Assert.That(execOptions.GracefulShutdownTimeout = timeout).IsTrue())
    }

    [<Test>]
    member _.WithLogging_SetsLoggingOptions() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)
        let loggingOptions = CommandLoggingOptions(Verbosity = CommandLogVerbosity.Diagnostic, ShowExitCode = true)

        builder.WithLogging(loggingOptions) |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        match execOptions.LogSettings with
        | null -> failwith "Expected logging settings"
        | logSettings ->
            do! check(Assert.That(logSettings.Verbosity).IsEqualTo(CommandLogVerbosity.Diagnostic))
            do! check(Assert.That(logSettings.ShowExitCode).IsTrue())
    }

    [<Test>]
    member _.WithLogging_ConfiguresUsingAction() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)

        builder.WithLogging(Action<CommandLoggingOptions>(fun _ -> ())) |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        do! check(Assert.That(execOptions.LogSettings <> null).IsTrue())
    }

    [<Test>]
    member _.ToolSpecificOption_SetsToolOptions() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)

        builder.WithConfiguration("Release") |> ignore

        let struct (toolOptions, _) = builder.ToOptions()
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Configuration), "Release"))
    }

    [<Test>]
    member _.InitialOptions_UsesProvidedOptions() = async {
        let mockCommand = Mock<ICommand>()
        let initialOptions = TestToolOptions()
        initialOptions.Configuration <- "Debug"
        let builder = TestToolBuilder(mockCommand.Object, initialOptions)

        let struct (toolOptions, _) = builder.ToOptions()
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Configuration), "Debug"))
    }

    [<Test>]
    member _.InitialOptions_CanBeModified() = async {
        let mockCommand = Mock<ICommand>()
        let initialOptions = TestToolOptions()
        initialOptions.Configuration <- "Debug"
        let builder = TestToolBuilder(mockCommand.Object, initialOptions)

        builder.WithFramework("net8.0") |> ignore

        let struct (toolOptions, _) = builder.ToOptions()
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Configuration), "Debug"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Framework), "net8.0"))
    }

    [<Test>]
    member _.FluentChaining_SetsAllOptions() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)

        builder
            .WithConfiguration("Release")
            .WithFramework("net8.0")
            .WithNoRestore()
            .WithTimeout(TimeSpan.FromMinutes(10.0))
            .WithWorkingDirectory("/project")
            .WithEnvironmentVariable("CI", "true")
        |> ignore

        let struct (toolOptions, execOptions) = builder.ToOptions()

        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Configuration), "Release"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Framework), "net8.0"))
        do! check(Assert.That(toolOptions.NoRestore.HasValue && toolOptions.NoRestore.Value).IsTrue())
        do! check(Assert.That(execOptions.ExecutionTimeout).IsEqualTo(TimeSpan.FromMinutes(10.0)))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(execOptions.WorkingDirectory), "/project"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(execOptions.EnvironmentVariables["CI"]), "true"))
    }

    [<Test>]
    member _.FluentChaining_ReturnsSameBuilderInstance() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)

        let result1 = builder.WithConfiguration("Release")
        let result2 = result1.WithFramework("net8.0")
        let result3 = result2.WithTimeout(TimeSpan.FromMinutes(5.0))

        do! check(Assert.That(obj.ReferenceEquals(builder, result1)).IsTrue())
        do! check(Assert.That(obj.ReferenceEquals(result1, result2)).IsTrue())
        do! check(Assert.That(obj.ReferenceEquals(result2, result3)).IsTrue())
    }

    [<Test>]
    member _.ExecuteAsync_CallsCommandExecuteWithOptions() = async {
        let mockCommand = createMockCommand "testtool command" "/working/dir"
        let builder = TestToolBuilder(mockCommand.Object)
        let mutable capturedToolOptions = Unchecked.defaultof<TestToolOptions>
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
                capturedToolOptions <- options :?> TestToolOptions))
            .ReturnsAsync(createCommandResult "testtool command" "/working/dir")
        |> ignore

        builder.WithConfiguration("Release") |> ignore

        let! result = builder.ExecuteAsync() |> Async.AwaitTask
        let hasExpectedCallCount = callCount = 1
        let capturedToolOptionsExists = obj.ReferenceEquals(capturedToolOptions, null) = false
        let configurationWasCaptured = capturedToolOptions.Configuration = "Release"

        do! check(Assert.That(result).IsNotNull())
        do! check(Assert.That(hasExpectedCallCount).IsTrue())
        do! check(Assert.That(capturedToolOptionsExists).IsTrue())
        do! check(Assert.That(configurationWasCaptured).IsTrue())
    }

    [<Test>]
    member _.ExecuteAsync_PassesCancellationToken() = async {
        let mockCommand = createMockCommand "testtool command" "/working/dir"
        let builder = TestToolBuilder(mockCommand.Object)
        let mutable capturedToken = CancellationToken.None
        use cts = new CancellationTokenSource()

        mockCommand
            .Setup(fun c ->
                c.ExecuteCommandLineTool(
                    It.IsAny<CommandLineToolOptions>(),
                    It.IsAny<CommandExecutionOptions>(),
                    It.IsAny<CancellationToken>()
                ))
            .Callback(Action<CommandLineToolOptions, CommandExecutionOptions, CancellationToken>(fun _ _ cancellationToken ->
                capturedToken <- cancellationToken))
            .ReturnsAsync(createCommandResult "testtool command" "/working/dir")
        |> ignore

        do! builder.ExecuteAsync(cts.Token) |> Async.AwaitTask |> Async.Ignore
        let sameToken = capturedToken = cts.Token

        do! check(Assert.That(sameToken).IsTrue())
    }

    [<Test>]
    member _.ExecuteAsync_PassesExecutionOptions() = async {
        let mockCommand = createMockCommand "testtool command" "/test/dir"
        let builder = TestToolBuilder(mockCommand.Object)
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
            .ReturnsAsync(createCommandResult "testtool command" "/test/dir")
        |> ignore

        builder
            .WithWorkingDirectory("/test/dir")
            .WithTimeout(TimeSpan.FromMinutes(5.0))
        |> ignore

        do! builder.ExecuteAsync() |> Async.AwaitTask |> Async.Ignore
        let capturedExecOptionsExists = obj.ReferenceEquals(capturedExecOptions, null) = false

        do! check(Assert.That(capturedExecOptionsExists).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(capturedExecOptions.WorkingDirectory), "/test/dir"))
        do! check(Assert.That(capturedExecOptions.ExecutionTimeout).IsEqualTo(TimeSpan.FromMinutes(5.0)))
    }

    [<Test>]
    member _.ToOptions_ReturnsBothOptionsTuple() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)

        builder
            .WithConfiguration("Release")
            .WithWorkingDirectory("/project")
        |> ignore

        let struct (toolOptions, execOptions) = builder.ToOptions()

        do! check(Assert.That(toolOptions).IsNotNull())
        do! check(Assert.That(execOptions).IsNotNull())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(toolOptions.Configuration), "Release"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(execOptions.WorkingDirectory), "/project"))
    }

    [<Test>]
    member _.ToOptions_CanBeCalledMultipleTimes() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)

        builder.WithConfiguration("Release") |> ignore
        let struct (options1, _) = builder.ToOptions()

        builder.WithFramework("net8.0") |> ignore
        let struct (options2, _) = builder.ToOptions()

        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(options1.Configuration), "Release"))
        do! check(Assert.That(options1.Framework).IsNull())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(options2.Configuration), "Release"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(options2.Framework), "net8.0"))
    }

    [<Test>]
    member _.NonGenericInterface_CanBeUsedForChaining() = async {
        let mockCommand = Mock<ICommand>()
        let builder = TestToolBuilder(mockCommand.Object)
        let nonGenericBuilder = builder :> ICommandBuilder

        nonGenericBuilder
            .WithWorkingDirectory("/test")
            .WithTimeout(TimeSpan.FromMinutes(1.0))
        |> ignore

        let struct (_, execOptions) = builder.ToOptions()
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(execOptions.WorkingDirectory), "/test"))
        do! check(Assert.That(execOptions.ExecutionTimeout).IsEqualTo(TimeSpan.FromMinutes(1.0)))
    }
