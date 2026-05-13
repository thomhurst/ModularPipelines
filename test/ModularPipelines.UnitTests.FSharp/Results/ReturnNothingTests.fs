namespace ModularPipelines.UnitTests.FSharp.Results

open ModularPipelines.Models
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type private ReturnNothingModule1() =
    inherit SimpleTestModule<CommandResult>()
    override _.Result = Unchecked.defaultof<CommandResult>

type private ReturnNothingModule2() =
    inherit SimpleTestModule<CommandResult>()
    override _.Result = Unchecked.defaultof<CommandResult>

type private ReturnNothingModule3() =
    inherit SimpleTestModule<CommandResult>()
    override _.Result = Unchecked.defaultof<CommandResult>

type ReturnNothingTests() =
    inherit TestBase()

    member private _.AssertResult(result: ModuleResult<CommandResult>) = async {
        do! check(Assert.That(result.IsSuccess).IsTrue())
        do! check(Assert.That(result.ModuleResultType = ModuleResultType.Success).IsTrue())
        do! check(Assert.That(result.ValueOrDefault).IsNull())
        do! check(Assert.That(result.ExceptionOrDefault).IsNull())
    }

    [<Test>]
    member this.Module1_HasValue_False() = async {
        let! m = this.RunModule<ReturnNothingModule1>() |> Async.AwaitTask
        let! result = m.CompletionSource.Task |> Async.AwaitTask
        do! this.AssertResult(result)
    }

    [<Test>]
    member this.Module2_HasValue_False() = async {
        let! m = this.RunModule<ReturnNothingModule2>() |> Async.AwaitTask
        let! result = m.CompletionSource.Task |> Async.AwaitTask
        do! this.AssertResult(result)
    }

    [<Test>]
    member this.Module3_HasValue_False() = async {
        let! m = this.RunModule<ReturnNothingModule3>() |> Async.AwaitTask
        let! result = m.CompletionSource.Task |> Async.AwaitTask
        do! this.AssertResult(result)
    }
