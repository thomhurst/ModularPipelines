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

type DependsOnTestModule1() =
    inherit SimpleTestModule<bool>()
    override _.Result = true

[<DependsOn(typeof<DependsOnTestModule1>)>]
type DependsOnTestModule2() =
    inherit SimpleTestModule<bool>()
    override _.Result = true

[<ModularPipelines.Attributes.DependsOn(typeof<DependsOnTestModule1>, Optional = true)>]
type DependsOnTestModule3() =
    inherit SimpleTestModule<bool>()
    override _.Result = true

[<ModularPipelines.Attributes.DependsOn(typeof<DependsOnTestModule1>, Optional = true)>]
type DependsOnTestModule3WithGetIfRegistered() =
    inherit Module<bool>()

    override _.ExecuteAsync(context, _) =
        task {
            let _ = context.GetModuleIfRegistered<DependsOnTestModule1>()
            do! Task.Yield()
            return true
        }

[<ModularPipelines.Attributes.DependsOn(typeof<DependsOnTestModule1>, Optional = true)>]
type DependsOnTestModule3WithGet() =
    inherit Module<bool>()

    override _.ExecuteAsync(context, _) =
        task {
            let module1 = context.GetModule<DependsOnTestModule1>()
            let! _ = module1.CompletionSource.Task
            do! Task.Yield()
            return true
        }

[<DependsOn(typeof<DependsOnSelfModule>)>]
type DependsOnSelfModule() =
    inherit Module<bool>()

    override _.ExecuteAsync(context, _) =
        task {
            let module1 = context.GetModule<DependsOnTestModule1>()
            let! _ = module1.CompletionSource.Task
            do! Task.Yield()
            return true
        }

[<DependsOn(typeof<ModuleFailedException>)>]
type DependsOnNonModule() =
    inherit Module<bool>()

    override _.ExecuteAsync(context, _) =
        task {
            let module1 = context.GetModule<DependsOnTestModule1>()
            let! _ = module1.CompletionSource.Task
            do! Task.Yield()
            return true
        }

[<ModularPipelines.Attributes.DependsOn(typeof<DependsOnTestModule1>, Optional = true)>]
type DependsOnTestModuleWithOptionalDep() =
    inherit SimpleTestModule<bool>()
    override _.Result = true

[<DependsOn(typeof<DependsOnTestModule1>)>]
type DependsOnTestModuleWithRequiredDep() =
    inherit SimpleTestModule<bool>()
    override _.Result = true

[<ModularPipelines.Attributes.DependsOn(typeof<DependsOnTestModule1>, Optional = true)>]
type DependsOnTestModuleCheckingUnregisteredDep() =
    inherit Module<bool>()

    override _.ExecuteAsync(context, _) =
        task {
            let dep = context.GetModuleIfRegistered<DependsOnTestModule1>()
            do! Task.Yield()
            return isNull dep
        }

type DependsOnTests() =
    inherit TestBase()

    [<Test>]
    member _.No_Exception_Thrown_When_Dependent_Module_Present() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DependsOnTestModule1>()
                .AddModule<DependsOnTestModule2>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.No_Exception_Thrown_When_Dependent_Module_Present_With_Optional() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DependsOnTestModule1>()
                .AddModule<DependsOnTestModule3>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Required_Dependency_Is_Auto_Registered_When_Missing() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DependsOnTestModule2>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(Seq.length pipelineSummary.Modules), 2))
    }

    [<Test>]
    member _.Optional_Dependency_Not_Auto_Registered_When_Missing() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DependsOnTestModule3>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(Seq.length pipelineSummary.Modules), 1))
    }

    [<Test>]
    member _.No_Exception_Thrown_When_Optional_Dependency_Missing_And_Get_If_Registered_Called() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DependsOnTestModule3WithGetIfRegistered>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Exception_Thrown_When_Optional_Dependency_Missing_And_Get_Module_Called() = async {
        let mutable threw = false

        try
            let! _ =
                TestPipelineHostBuilder.Create()
                    .AddModule<DependsOnTestModule3WithGet>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
            ()
        with _ ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Depends_On_Self_Module_Throws_Exception() = async {
        let mutable threw = false

        try
            let! _ =
                TestPipelineHostBuilder.Create()
                    .AddModule<DependsOnSelfModule>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
            ()
        with :? ModuleReferencingSelfException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Depends_On_Non_Module_Throws_Exception() = async {
        let mutable message = null

        try
            let! _ =
                TestPipelineHostBuilder.Create()
                    .AddModule<DependsOnNonModule>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
            ()
        with :? InvalidModuleTypeException as ex ->
            message <- ex.Message
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(message), "ModularPipelines.Exceptions.ModuleFailedException is not a Module (does not implement IModule)"))
    }

    [<Test>]
    member _.Optional_Dependency_Works_When_Missing() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DependsOnTestModuleWithOptionalDep>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Required_Dependency_Auto_Registers_Missing_Module() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DependsOnTestModuleWithRequiredDep>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
        do! check(Assert.That(pipelineSummary.Modules |> Seq.exists (fun moduleInstance -> moduleInstance :? DependsOnTestModule1)).IsTrue())
    }

    [<Test>]
    member _.Optional_Dependency_Returns_Null_When_GetModuleIfRegistered_Called_On_Unregistered() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DependsOnTestModuleCheckingUnregisteredDep>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }
