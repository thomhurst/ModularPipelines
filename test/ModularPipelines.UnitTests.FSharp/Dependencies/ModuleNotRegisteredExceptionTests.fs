namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System.Threading
open System.Threading.Tasks
open ModularPipelines.Attributes
open ModularPipelines.Context
open ModularPipelines.Enums
open ModularPipelines.Exceptions
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open ModularPipelines.Extensions

[<ModularPipelines.Attributes.DependsOn(typeof<Module1>, Optional = true)>]
type Module2WithOptionalDep() =
    inherit Module<bool>()

    override _.ExecuteAsync(context: IModuleContext, _: CancellationToken) =
        task {
            let! _ = context.GetModule<Module1>()
            do! Task.Yield()
            return true
        }

and Module1() =
    inherit Module<bool>()

    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
        task {
            do! Task.Yield()
            return true
        }

[<DependsOn(typeof<Module1>)>]
type Module2WithRequiredDep() =
    inherit Module<bool>()

    override _.ExecuteAsync(context: IModuleContext, _: CancellationToken) =
        task {
            let! _ = context.GetModule<Module1>()
            do! Task.Yield()
            return true
        }

type ModuleNotRegisteredExceptionTests() =
    inherit TestBase()

    [<Test>]
    member _.Module_Getting_Non_Registered_Module_With_Optional_Dep_Throws_ModuleFailedException() = async {
        let mutable threw = false

        try
            let! _ =
                TestPipelineHostBuilder.Create()
                    .AddModule<Module2WithOptionalDep>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
            ()
        with :? ModuleFailedException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Module_With_Required_Dependency_Auto_Registers_Missing_Module() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<Module2WithRequiredDep>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask
        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(pipelineSummary.Modules |> Seq.length), 2))
    }

    [<Test>]
    member _.Module_Getting_Registered_Module_Does_Not_Throw_Exception() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<Module1>()
                .AddModule<Module2WithOptionalDep>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }
