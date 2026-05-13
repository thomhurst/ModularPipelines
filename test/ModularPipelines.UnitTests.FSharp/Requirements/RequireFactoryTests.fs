namespace ModularPipelines.UnitTests.FSharp.Requirements

open System
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

type private RequireFactoryDummyModule() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) =
        task {
            do! System.Threading.Tasks.Task.Yield()
            return true
        }

[<AutoOpen>]
module private RequirementFactoryTestHelpers =
    let unwrapMessage (ex: exn) =
        match ex with
        | :? AggregateException as aggregate -> aggregate.GetBaseException().Message
        | _ -> ex.Message

    let isRequirementFailure (ex: exn) =
        match ex with
        | :? FailedRequirementsException -> true
        | :? AggregateException as aggregate -> aggregate.GetBaseException() :? FailedRequirementsException
        | _ -> false

type RequireFactoryTests() =
    [<Test>]
    member _.Require_That_With_True_Condition_Passes() = async {
        let! host =
            TestPipelineHostBuilder
                .Create()
                .AddModule<RequireFactoryDummyModule>()
                .AddRequirement(Require.That((fun _ -> true), "Should not fail"))
                .BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let result = resultRegistry.GetResult(typeof<RequireFactoryDummyModule>)

        do! check(Assert.That(result.ModuleStatus = Status.Successful).IsTrue())
    }

    [<Test>]
    member _.Require_That_With_False_Condition_Fails() = async {
        let reason = "Custom failure reason"
        let mutable threw = false
        let mutable exMessage = ""

        try
            do!
                TestPipelineHostBuilder
                    .Create()
                    .AddModule<RequireFactoryDummyModule>()
                    .AddRequirement(Require.That((fun _ -> false), reason))
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
                |> Async.Ignore
        with ex when isRequirementFailure ex ->
            threw <- true
            exMessage <- unwrapMessage ex

        do! check(Assert.That(threw).IsTrue())
        do! check(Assert.That(exMessage.Contains(reason)).IsTrue())
    }

    [<Test>]
    member _.Require_ThatAsync_With_True_Condition_Passes() = async {
        let! host =
            TestPipelineHostBuilder
                .Create()
                .AddModule<RequireFactoryDummyModule>()
                .AddRequirement(
                    Require.ThatAsync(
                        (fun _ ->
                            task {
                                do! System.Threading.Tasks.Task.Yield()
                                return true
                            }),
                        "Should not fail"
                    )
                )
                .BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let result = resultRegistry.GetResult(typeof<RequireFactoryDummyModule>)

        do! check(Assert.That(result.ModuleStatus = Status.Successful).IsTrue())
    }

    [<Test>]
    member _.Require_ThatAsync_With_False_Condition_Fails() = async {
        let reason = "Async failure reason"
        let mutable threw = false
        let mutable exMessage = ""

        try
            do!
                TestPipelineHostBuilder
                    .Create()
                    .AddModule<RequireFactoryDummyModule>()
                    .AddRequirement(
                        Require.ThatAsync(
                            (fun _ ->
                                task {
                                    do! System.Threading.Tasks.Task.Yield()
                                    return false
                                }),
                            reason
                        )
                    )
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
                |> Async.Ignore
        with ex when isRequirementFailure ex ->
            threw <- true
            exMessage <- unwrapMessage ex

        do! check(Assert.That(threw).IsTrue())
        do! check(Assert.That(exMessage.Contains(reason)).IsTrue())
    }

    [<Test>]
    member _.Require_EnvironmentVariable_When_Set_Passes() = async {
        let varName = "TEST_REQUIREMENT_VAR_FS"
        Environment.SetEnvironmentVariable(varName, "some-value")

        try
            let! host =
                TestPipelineHostBuilder
                    .Create()
                    .AddModule<RequireFactoryDummyModule>()
                    .AddRequirement(Require.EnvironmentVariable(varName))
                    .BuildHostAsync()
                |> Async.AwaitTask

            do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

            let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
            let result = resultRegistry.GetResult(typeof<RequireFactoryDummyModule>)

            do! check(Assert.That(result.ModuleStatus = Status.Successful).IsTrue())
        finally
            Environment.SetEnvironmentVariable(varName, null)
    }

    [<Test>]
    member _.Require_EnvironmentVariable_When_Not_Set_Fails() = async {
        let varName = "UNLIKELY_TO_EXIST_VAR_FS_12345"
        Environment.SetEnvironmentVariable(varName, null)

        let mutable threw = false
        let mutable exMessage = ""

        try
            do!
                TestPipelineHostBuilder
                    .Create()
                    .AddModule<RequireFactoryDummyModule>()
                    .AddRequirement(Require.EnvironmentVariable(varName))
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
                |> Async.Ignore
        with ex when isRequirementFailure ex ->
            threw <- true
            exMessage <- unwrapMessage ex

        do! check(Assert.That(threw).IsTrue())
        do! check(Assert.That(exMessage.Contains(varName)).IsTrue())
    }

    [<Test>]
    member _.Require_EnvironmentVariable_With_Custom_Reason() = async {
        let varName = "UNLIKELY_TO_EXIST_VAR_FS_67890"
        let customReason = "My custom message about the var"
        Environment.SetEnvironmentVariable(varName, null)

        let mutable threw = false
        let mutable exMessage = ""

        try
            do!
                TestPipelineHostBuilder
                    .Create()
                    .AddModule<RequireFactoryDummyModule>()
                    .AddRequirement(Require.EnvironmentVariable(varName, customReason))
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
                |> Async.Ignore
        with ex when isRequirementFailure ex ->
            threw <- true
            exMessage <- unwrapMessage ex

        do! check(Assert.That(threw).IsTrue())
        do! check(Assert.That(exMessage.Contains(customReason)).IsTrue())
    }

    [<Test>]
    member _.DelegateRequirement_Respects_Order() = async {
        let requirement = Require.That((fun _ -> true), "test", order = 5)
        do! check(Assert.That(requirement.Order = 5).IsTrue())
    }
