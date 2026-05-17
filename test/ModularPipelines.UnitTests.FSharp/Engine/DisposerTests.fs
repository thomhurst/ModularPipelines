namespace ModularPipelines.UnitTests.FSharp.Engine

open System
open System.Threading.Tasks
open TUnit.Assertions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open TUnit.Assertions.Extensions
open ModularPipelines.Helpers

type private AsyncDisposableClass() =
    member val DisposedAsync = false with get, set

    interface IAsyncDisposable with
        member this.DisposeAsync() =
            this.DisposedAsync <- true
            ValueTask()

type private DisposableClass() =
    member val Disposed = false with get, set

    interface IDisposable with
        member this.Dispose() =
            this.Disposed <- true

type DisposerTests() =
    [<Test>]
    member _.Disposer_Calls_Async() = async {
        let myClass = AsyncDisposableClass()
        do! check(Assert.That(myClass.DisposedAsync).IsFalse())

        do! Disposer.DisposeObjectAsync(myClass) |> Async.AwaitTask

        do! check(Assert.That(myClass.DisposedAsync).IsTrue())
    }

    [<Test>]
    member _.Disposer_Calls_Sync() = async {
        let myClass = new DisposableClass()
        do! check(Assert.That(myClass.Disposed).IsFalse())

        do! Disposer.DisposeObjectAsync(myClass) |> Async.AwaitTask

        do! check(Assert.That(myClass.Disposed).IsTrue())
    }
