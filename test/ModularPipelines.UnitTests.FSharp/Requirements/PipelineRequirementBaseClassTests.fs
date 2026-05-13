namespace ModularPipelines.UnitTests.FSharp.Requirements

open System
open System.Threading
open Microsoft.Extensions.DependencyInjection
open ModularPipelines.Context
open ModularPipelines.Engine
open ModularPipelines.Exceptions
open ModularPipelines.Extensions
open ModularPipelines.Models
open ModularPipelines.Modules
open ModularPipelines.Requirements
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open ModularPipelines.Enums

type private RequirementDummyModule() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) =
        task {
            do! System.Threading.Tasks.Task.Yield()
            return true
        }

type private PassingSyncRequirement() =
    inherit PipelineRequirement()
    override _.Must(_) = RequirementDecision.Passed

type private FailingSyncRequirement() =
    inherit PipelineRequirement()
    override _.Must(_) = RequirementDecision.Failed("Sync requirement failed")

type private PassingAsyncRequirement() =
    inherit PipelineRequirement()
    override _.MustAsync(_) =
        task {
            do! System.Threading.Tasks.Task.Yield()
            return RequirementDecision.Passed
        }

type private FailingAsyncRequirement() =
    inherit PipelineRequirement()
    override _.MustAsync(_) =
        task {
            do! System.Threading.Tasks.Task.Yield()
            return RequirementDecision.Failed("Async requirement failed")
        }

type private WhenTrueRequirement() =
    inherit PipelineRequirement()
    override _.Must(_) = RequirementDecision.Of(true, "Should not see this")

type private WhenFalseRequirement() =
    inherit PipelineRequirement()
    override _.Must(_) = RequirementDecision.Of(false, "When condition failed")

type private CustomOrderRequirement() =
    inherit PipelineRequirement()
    override _.Order = 10

[<AutoOpen>]
module private RequirementTestHelpers =
    let unwrapMessage (ex: exn) =
        match ex with
        | :? AggregateException as aggregate -> aggregate.GetBaseException().Message
        | _ -> ex.Message

    let isRequirementFailure (ex: exn) =
        match ex with
        | :? FailedRequirementsException -> true
        | :? AggregateException as aggregate -> aggregate.GetBaseException() :? FailedRequirementsException
        | _ -> false

type PipelineRequirementBaseClassTests() =
    [<Test>]
    member _.Sync_Requirement_With_Pass_Succeeds() = async {
        let! host =
            TestPipelineHostBuilder
                .Create()
                .AddModule<RequirementDummyModule>()
                .AddRequirement<PassingSyncRequirement>()
                .BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let result = resultRegistry.GetResult(typeof<RequirementDummyModule>)

        do! check(Assert.That(result.ModuleStatus = Status.Successful).IsTrue())
    }

    [<Test>]
    member _.Sync_Requirement_With_Fail_Throws() = async {
        let mutable threw = false
        let mutable exMessage = ""

        try
            do!
                TestPipelineHostBuilder
                    .Create()
                    .AddModule<RequirementDummyModule>()
                    .AddRequirement<FailingSyncRequirement>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
                |> Async.Ignore
        with ex when isRequirementFailure ex ->
            threw <- true
            exMessage <- unwrapMessage ex

        do! check(Assert.That(threw).IsTrue())
        do! check(Assert.That(exMessage.Contains("Sync requirement failed")).IsTrue())
    }

    [<Test>]
    member _.Async_Requirement_With_Pass_Succeeds() = async {
        let! host =
            TestPipelineHostBuilder
                .Create()
                .AddModule<RequirementDummyModule>()
                .AddRequirement<PassingAsyncRequirement>()
                .BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let result = resultRegistry.GetResult(typeof<RequirementDummyModule>)

        do! check(Assert.That(result.ModuleStatus = Status.Successful).IsTrue())
    }

    [<Test>]
    member _.Async_Requirement_With_Fail_Throws() = async {
        let mutable threw = false
        let mutable exMessage = ""

        try
            do!
                TestPipelineHostBuilder
                    .Create()
                    .AddModule<RequirementDummyModule>()
                    .AddRequirement<FailingAsyncRequirement>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
                |> Async.Ignore
        with ex when isRequirementFailure ex ->
            threw <- true
            exMessage <- unwrapMessage ex

        do! check(Assert.That(threw).IsTrue())
        do! check(Assert.That(exMessage.Contains("Async requirement failed")).IsTrue())
    }

    [<Test>]
    member _.When_Helper_With_True_Passes() = async {
        let! host =
            TestPipelineHostBuilder
                .Create()
                .AddModule<RequirementDummyModule>()
                .AddRequirement<WhenTrueRequirement>()
                .BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let result = resultRegistry.GetResult(typeof<RequirementDummyModule>)

        do! check(Assert.That(result.ModuleStatus = Status.Successful).IsTrue())
    }

    [<Test>]
    member _.When_Helper_With_False_Fails() = async {
        let mutable threw = false
        let mutable exMessage = ""

        try
            do!
                TestPipelineHostBuilder
                    .Create()
                    .AddModule<RequirementDummyModule>()
                    .AddRequirement<WhenFalseRequirement>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
                |> Async.Ignore
        with ex when isRequirementFailure ex ->
            threw <- true
            exMessage <- unwrapMessage ex

        do! check(Assert.That(threw).IsTrue())
        do! check(Assert.That(exMessage.Contains("When condition failed")).IsTrue())
    }

    [<Test>]
    member _.Custom_Order_Is_Respected() = async {
        let requirement = CustomOrderRequirement()
        do! check(Assert.That(requirement.Order = 10).IsTrue())
    }
