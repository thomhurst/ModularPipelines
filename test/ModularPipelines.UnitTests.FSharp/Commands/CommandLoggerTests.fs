namespace ModularPipelines.UnitTests.FSharp.Commands

open System
open System.IO
open System.Text.RegularExpressions
open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open ModularPipelines.Context
open ModularPipelines.Options
open ModularPipelines.TestHelpers
open NReco.Logging.File
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open Vertical.SpectreLogger.Options

type CommandLoggerTests() =
    inherit TestBase()

    member private this.GetCommandWithFileLogger(file: string) : Task<struct (ICommand * ModularPipelines.IPipeline)> =
        this.GetService<ICommand>(
            Action<IServiceCollection>(fun services ->
                services.AddLogging(fun builder ->
                    services.Configure<SpectreLoggerOptions>(fun (options: SpectreLoggerOptions) ->
                        options.MinimumLogLevel <- LogLevel.Information)
                    |> ignore

                    services.Configure<LoggerFilterOptions>(fun (options: LoggerFilterOptions) ->
                        options.MinLevel <- LogLevel.Information)
                    |> ignore

                    builder.AddFile(file) |> ignore)
                |> ignore)
        )

    member private this.RunPowershellCommand(command: string, logInput: bool, logOutput: bool, logError: bool, logExitCode: bool, logDuration: bool) = task {
        let file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt")

        let! struct (commandService, host) = this.GetCommandWithFileLogger(file)

        let verbosity =
            if not logInput && not logOutput && not logError && not logExitCode && not logDuration then
                CommandLogVerbosity.Silent
            else
                CommandLogVerbosity.Normal

        let loggingOptions =
            CommandLoggingOptions(
                Verbosity = verbosity,
                ShowCommandArguments = logInput,
                ShowStandardOutput = logOutput,
                ShowStandardError = logError,
                ShowExitCode = logExitCode,
                ShowExecutionTime = logDuration
            )

        let! _ =
            commandService.ExecuteCommandLineTool(
                PowershellScriptOptions(command),
                CommandExecutionOptions(LogSettings = loggingOptions, ThrowOnNonZeroExitCode = false)
            )

        do! host.DisposeAsync().AsTask()

        return file
    }

    member private this.RunPowershellCommandWithLoggingOptions(command: string, loggingOptions: CommandLoggingOptions) = task {
        let file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt")

        let! struct (commandService, host) = this.GetCommandWithFileLogger(file)

        let! _ =
            commandService.ExecuteCommandLineTool(
                PowershellScriptOptions(command),
                CommandExecutionOptions(LogSettings = loggingOptions, ThrowOnNonZeroExitCode = false)
            )

        do! host.DisposeAsync().AsTask()

        return file
    }

    [<Test>]
    [<MatrixDataSource>]
    member this.Logs_As_Expected_With_Options(
        [<Matrix(true, false)>] logInput: bool,
        [<Matrix(true, false)>] logOutput: bool,
        [<Matrix(true, false)>] logError: bool,
        [<Matrix(true, false)>] logExitCode: bool,
        [<Matrix(true, false)>] logDuration: bool
    ) = async {
        let! file =
            this.RunPowershellCommand(
                "echo Hello world!\nthrow \"Error!\"",
                logInput,
                logOutput,
                logError,
                logExitCode,
                logDuration
            )
            |> Async.AwaitTask

        let! logFile = File.ReadAllTextAsync(file) |> Async.AwaitTask

        if not logInput && not logOutput && not logError && not logDuration && not logExitCode then
            do! check(Assert.That(logFile.Contains("INFO\t[ModularPipelines.Pipeline]")).IsFalse())
        else
            do! check(Assert.That(logFile.Contains("INFO\t[ModularPipelines.Pipeline]")).IsTrue())

            if logInput then
                do! check(Assert.That(logFile.Contains($"{Environment.CurrentDirectory}> pwsh -Command \"echo Hello world!")).IsTrue())
            else
                do! check(Assert.That(logFile.Contains($"{Environment.CurrentDirectory}> ********")).IsTrue())

            if logOutput then
                do! check(Assert.That(logFile.Contains("→") || logFile.Contains("↳")).IsTrue())

            if logError then
                do! check(Assert.That(logFile.Contains("✗")).IsTrue())

            if logDuration then
                do! check(Assert.That(Regex.IsMatch(logFile, @"\[\d+m?s")).IsTrue())

            if logExitCode then
                do! check(Assert.That(logFile.Contains("exit ")).IsTrue())
    }

    [<Test>]
    member this.Silent_Verbosity_Logs_Nothing() = async {
        let! file =
            this.RunPowershellCommandWithLoggingOptions(
                "echo Hello",
                CommandLoggingOptions(Verbosity = CommandLogVerbosity.Silent)
            )
            |> Async.AwaitTask

        let! logFile = File.ReadAllTextAsync(file) |> Async.AwaitTask
        do! check(Assert.That(logFile.Contains($"{Environment.CurrentDirectory}>")).IsFalse())
        do! check(Assert.That(logFile.Contains("→")).IsFalse())
        do! check(Assert.That(logFile.Contains("↳")).IsFalse())
        do! check(Assert.That(logFile.Contains("exit ")).IsFalse())
        do! check(Assert.That(logFile.Contains("Working Directory:")).IsFalse())
    }

    [<Test>]
    member this.Minimal_Verbosity_Logs_Only_Input() = async {
        let! file =
            this.RunPowershellCommandWithLoggingOptions(
                "echo Hello",
                CommandLoggingOptions(Verbosity = CommandLogVerbosity.Minimal)
            )
            |> Async.AwaitTask

        let! logFile = File.ReadAllTextAsync(file) |> Async.AwaitTask
        do! check(Assert.That(logFile.Contains($"{Environment.CurrentDirectory}>")).IsTrue())
        do! check(Assert.That(logFile.Contains("→")).IsFalse())
        do! check(Assert.That(logFile.Contains("↳")).IsFalse())
        do! check(Assert.That(logFile.Contains("exit ")).IsFalse())
        do! check(Assert.That(Regex.IsMatch(logFile, @"\[\d+m?s")).IsFalse())
    }

    [<Test>]
    member this.Normal_Verbosity_Logs_Input_And_Output() = async {
        let! file =
            this.RunPowershellCommandWithLoggingOptions(
                "echo Hello",
                CommandLoggingOptions(Verbosity = CommandLogVerbosity.Normal)
            )
            |> Async.AwaitTask

        let! logFile = File.ReadAllTextAsync(file) |> Async.AwaitTask
        do! check(Assert.That(logFile.Contains($"{Environment.CurrentDirectory}>")).IsTrue())
        do! check(Assert.That(logFile.Contains("→")).IsTrue())
        do! check(Assert.That(logFile.Contains("exit ")).IsFalse())
        do! check(Assert.That(Regex.IsMatch(logFile, @"\[\d+m?s")).IsFalse())
    }

    [<Test>]
    member this.Detailed_Verbosity_Logs_Input_Output_ExitCode_Duration() = async {
        let! file =
            this.RunPowershellCommandWithLoggingOptions(
                "echo Hello",
                CommandLoggingOptions(Verbosity = CommandLogVerbosity.Detailed)
            )
            |> Async.AwaitTask

        let! logFile = File.ReadAllTextAsync(file) |> Async.AwaitTask
        do! check(Assert.That(logFile.Contains($"{Environment.CurrentDirectory}>")).IsTrue())
        do! check(Assert.That(logFile.Contains("→")).IsTrue())
        do! check(Assert.That(logFile.Contains("exit ")).IsTrue())
        do! check(Assert.That(Regex.IsMatch(logFile, @"\[\d+m?s")).IsTrue())
    }

    [<Test>]
    member this.Diagnostic_Verbosity_Logs_Everything_Including_WorkingDirectory() = async {
        let! file =
            this.RunPowershellCommandWithLoggingOptions(
                "echo Hello",
                CommandLoggingOptions(Verbosity = CommandLogVerbosity.Diagnostic)
            )
            |> Async.AwaitTask

        let! logFile = File.ReadAllTextAsync(file) |> Async.AwaitTask
        do! check(Assert.That(logFile.Contains($"{Environment.CurrentDirectory}>")).IsTrue())
        do! check(Assert.That(logFile.Contains("→")).IsTrue())
        do! check(Assert.That(logFile.Contains("exit ")).IsTrue())
        do! check(Assert.That(Regex.IsMatch(logFile, @"\[\d+m?s")).IsTrue())
        do! check(Assert.That(logFile.Contains("Working Directory:")).IsTrue())
    }
