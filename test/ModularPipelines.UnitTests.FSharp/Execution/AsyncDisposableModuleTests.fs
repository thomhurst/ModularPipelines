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

type private AsyncDisposableModule() =
    inherit Module<bool>()

    member val IsDisposed = false with get, set

    override _.ExecuteAsync(_: IModuleContext, cancellationToken: CancellationToken) =
        task {
            do! Task.Delay(1, cancellationToken)
            return true
        }

    interface IAsyncDisposable with
        member this.DisposeAsync() =
            ValueTask(
                task {
                    do! Task.Delay(1)
                    this.IsDisposed <- true
                    GC.SuppressFinalize(this)
                }
            )

type AsyncDisposableModuleTests() =
    [<Test>]
    member _.SuccessfullyDisposed() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create().AddModule<AsyncDisposableModule>().ExecutePipelineAsync()
            |> Async.AwaitTask

        let isDisposed = pipelineSummary.Modules.OfType<AsyncDisposableModule>().Single().IsDisposed
        do! check(Assert.That(isDisposed).IsTrue())
    }
