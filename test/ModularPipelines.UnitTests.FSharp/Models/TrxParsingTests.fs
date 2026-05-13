namespace ModularPipelines.UnitTests.FSharp.Models

open System
open System.IO
open System.Linq
open Microsoft.Extensions.DependencyInjection
open ModularPipelines.Context
open ModularPipelines.DotNet
open ModularPipelines.DotNet.Enums
open ModularPipelines.DotNet.Extensions
open ModularPipelines.DotNet.Options
open ModularPipelines.DotNet.Parsers.Trx
open ModularPipelines.Engine
open ModularPipelines.Extensions
open ModularPipelines.Git.Extensions
open ModularPipelines.Models
open ModularPipelines.Modules
open ModularPipelines.Options
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type private NUnitModule() =
    inherit Module<DotNetTestResult>()

    let mutable _lastResult: DotNetTestResult = Unchecked.defaultof<_>

    member _.LastResult = _lastResult

    override _.ExecuteAsync(context, cancellationToken) =
        task {
            let testProject =
                context.Git().RootDirectory.FindFile(fun x -> x.Name = "ModularPipelines.TestsForTests.csproj")

            let resultsDirectory = Path.Combine(Path.GetTempPath(), System.Guid.NewGuid().ToString())
            Directory.CreateDirectory(resultsDirectory) |> ignore

            let trxFileName = "test-results.trx"

            let! _ =
                context
                    .DotNet()
                    .Test(
                        DotNetTestOptions(
                            Framework = "net10.0",
                            ResultsDirectory = resultsDirectory,
                            Arguments = [| "--report-trx"; "--report-trx-filename"; trxFileName |]
                        ),
                        CommandExecutionOptions(
                            WorkingDirectory = testProject.Folder.Path,
                            ThrowOnNonZeroExitCode = false,
                            LogSettings =
                                CommandLoggingOptions(
                                    Verbosity = CommandLogVerbosity.Minimal,
                                    ShowStandardOutput = false,
                                    ShowStandardError = true
                                )
                        ),
                        cancellationToken
                    )

            let trxFilePath = Path.Combine(resultsDirectory, trxFileName)
            let! trxContents = File.ReadAllTextAsync(trxFilePath, cancellationToken)
            _lastResult <- TrxParser().ParseTrxContents(trxContents)
            return _lastResult
        }

type TrxParsingTests() =
    inherit TestBase()

    [<Test>]
    member _.NUnit() = async {
        let! host =
            TestPipelineHostBuilder.Create()
                .AddModule<NUnitModule>()
                .BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let modules = host.RootServices.GetServices<IModule>()
        let m = modules.OfType<NUnitModule>().First()
        let testResult = m.LastResult
        let failedCount =
            testResult.UnitTestResults.Where(fun x -> x.Outcome = Nullable<TestOutcome>(TestOutcome.Failed))
            |> Seq.length

        let notExecutedCount =
            testResult.UnitTestResults.Where(fun x -> x.Outcome = Nullable<TestOutcome>(TestOutcome.NotExecuted))
            |> Seq.length

        let passedCount =
            testResult.UnitTestResults.Where(fun x -> x.Outcome = Nullable<TestOutcome>(TestOutcome.Passed))
            |> Seq.length

        do! check(Assert.That(testResult.Successful).IsFalse())
        do! check(Assert.That((failedCount = 1)).IsTrue())
        do! check(Assert.That((notExecutedCount = 1)).IsTrue())
        do! check(Assert.That((passedCount = 2)).IsTrue())
    }
