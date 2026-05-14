namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System.Threading.Tasks
open ModularPipelines.Attributes
open ModularPipelines.Context
open ModularPipelines.Exceptions
open ModularPipelines.Extensions
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

[<DependsOn(typeof<DependencyConflictModule2>)>]
type DependencyConflictModule1() =
    inherit Module<bool>()

    override _.ExecuteAsync(context, _) =
        task {
            let module2 = context.GetModule<DependencyConflictModule2>()
            let! _ = module2.CompletionSource.Task
            do! Task.Yield()
            return true
        }

and [<DependsOn(typeof<DependencyConflictModule1>)>] DependencyConflictModule2() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)

type DirectCollisionTests() =
    [<Test>]
    member _.Modules_Dependent_On_Each_Other_Throws_Exception() = async {
        let mutable message = null

        try
            let! _ =
                TestPipelineHostBuilder.Create()
                    .AddModule<DependencyConflictModule1>()
                    .AddModule<DependencyConflictModule2>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
            ()
        with :? DependencyCollisionException as ex ->
            message <- ex.Message
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(message), "Dependency collision detected: **DependencyConflictModule1** -> DependencyConflictModule2 -> **DependencyConflictModule1**"))
    }
