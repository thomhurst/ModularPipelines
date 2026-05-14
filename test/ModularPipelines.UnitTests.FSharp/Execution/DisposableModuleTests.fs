namespace ModularPipelines.UnitTests.FSharp.Execution

open System
open System.Threading
open System.Threading.Tasks
open System.Linq
open ModularPipelines.Context
open ModularPipelines.Extensions
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open TUnit.Assertions.Extensions

type private DisposableModule() =
    inherit Module<bool>()

    member val IsDisposed = false with get, set

    override _.ExecuteAsync(_: IModuleContext, cancellationToken: CancellationToken) =
        task {
            do! Task.Delay(1, cancellationToken)
            return true
        }

    interface IDisposable with
        member this.Dispose() =
            this.IsDisposed <- true
            GC.SuppressFinalize(this)

type DisposableModuleTests() =
    [<Test>]
    member _.SuccessfullyDisposed() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create().AddModule<DisposableModule>().ExecutePipelineAsync()
            |> Async.AwaitTask

        let isDisposed = pipelineSummary.Modules.OfType<DisposableModule>().Single().IsDisposed
        do! check(Assert.That(isDisposed).IsTrue())
    }
