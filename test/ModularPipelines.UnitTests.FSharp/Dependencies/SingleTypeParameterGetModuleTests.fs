namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System.Threading.Tasks
open ModularPipelines.Attributes
open ModularPipelines.Context
open ModularPipelines.Enums
open ModularPipelines.Exceptions
open ModularPipelines.Extensions
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module SingleTypeParameterGetModuleTestModules =
    type ComplexResult = {
        Id: int
        Name: string
    }

    type StringModule() =
        inherit Module<string>()

        override _.ExecuteAsync(_, _) =
            task {
                do! Task.Yield()
                return "Hello from StringModule"
            }

    type ComplexResultModule() =
        inherit Module<ComplexResult>()

        override _.ExecuteAsync(_, _) =
            task {
                do! Task.Yield()
                return { Id = 42; Name = "Test" }
            }

    [<DependsOn(typeof<StringModule>)>]
    type ConsumerModule() =
        inherit Module<string>()

        override _.ExecuteAsync(context, _) =
            task {
                let! result = context.GetModule<StringModule>()

                if result.IsSuccess then
                    return result.ValueOrDefault
                else
                    return "failed"
            }

    [<DependsOn(typeof<ComplexResultModule>)>]
    type ComplexConsumerModule() =
        inherit Module<int>()

        override _.ExecuteAsync(context, _) =
            task {
                let! result = context.GetModule<ComplexResultModule>()

                if result.IsSuccess && not (isNull result.ValueOrDefault) then
                    return result.ValueOrDefault.Id
                else
                    return -1
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<StringModule>, Optional = true)>]
    type OptionalConsumerModule() =
        inherit Module<string>()

        override _.ExecuteAsync(context, _) =
            task {
                let moduleInstance = context.GetModuleIfRegistered<StringModule>()

                if isNull moduleInstance then
                    return "not registered"
                else
                    let! result = moduleInstance
                    return if isNull result.ValueOrDefault then "default" else result.ValueOrDefault
            }

    type SelfReferencingModule() =
        inherit Module<bool>()

        override _.ExecuteAsync(context, _) =
            task {
                let _ = context.GetModule<SelfReferencingModule>()
                do! Task.Yield()
                return true
            }

    type UnregisteredConsumerModule() =
        inherit Module<bool>()

        override _.ExecuteAsync(context, _) =
            task {
                let _ = context.GetModule<StringModule>()
                do! Task.Yield()
                return true
            }

type SingleTypeParameterGetModuleTests() =
    inherit TestBase()

    [<Test>]
    member _.GetModule_SingleTypeParameter_ReturnsCorrectlyTypedResult() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<SingleTypeParameterGetModuleTestModules.StringModule>()
                .AddModule<SingleTypeParameterGetModuleTestModules.ConsumerModule>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.GetModule_SingleTypeParameter_WithComplexType_InfersTypeCorrectly() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<SingleTypeParameterGetModuleTestModules.ComplexResultModule>()
                .AddModule<SingleTypeParameterGetModuleTestModules.ComplexConsumerModule>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.GetModuleIfRegistered_SingleTypeParameter_ReturnsModule_WhenRegistered() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<SingleTypeParameterGetModuleTestModules.StringModule>()
                .AddModule<SingleTypeParameterGetModuleTestModules.OptionalConsumerModule>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.GetModuleIfRegistered_SingleTypeParameter_ReturnsNull_WhenNotRegistered() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<SingleTypeParameterGetModuleTestModules.OptionalConsumerModule>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.GetModule_SingleTypeParameter_ThrowsModuleReferencingSelfException() = async {
        let mutable innerException = null

        try
            let! _ =
                TestPipelineHostBuilder.Create()
                    .AddModule<SingleTypeParameterGetModuleTestModules.SelfReferencingModule>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
            ()
        with :? ModuleFailedException as ex ->
            innerException <- ex.InnerException

        do! check(Assert.That(innerException).IsTypeOf<ModuleReferencingSelfException>())
    }

    [<Test>]
    member _.GetModule_SingleTypeParameter_ThrowsModuleNotRegisteredException() = async {
        let mutable innerException = null

        try
            let! _ =
                TestPipelineHostBuilder.Create()
                    .AddModule<SingleTypeParameterGetModuleTestModules.UnregisteredConsumerModule>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
            ()
        with :? ModuleFailedException as ex ->
            innerException <- ex.InnerException

        do! check(Assert.That(innerException).IsTypeOf<ModuleNotRegisteredException>())
    }
