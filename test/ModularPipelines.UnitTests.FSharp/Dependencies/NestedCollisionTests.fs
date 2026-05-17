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

module NestedCollisionTestModules =
    [<DependsOn(typeof<DependencyConflictModule2>)>]
    type DependencyConflictModule1() =
        inherit Module<bool>()
        override _.ExecuteAsync(_, _) = Task.FromResult(true)

    and [<DependsOn(typeof<DependencyConflictModule3>)>] DependencyConflictModule2() =
        inherit Module<bool>()
        override _.ExecuteAsync(_, _) = Task.FromResult(true)

    and [<DependsOn(typeof<DependencyConflictModule4>)>] DependencyConflictModule3() =
        inherit Module<bool>()
        override _.ExecuteAsync(_, _) = Task.FromResult(true)

    and [<DependsOn(typeof<DependencyConflictModule5>)>] DependencyConflictModule4() =
        inherit Module<bool>()
        override _.ExecuteAsync(_, _) = Task.FromResult(true)

    and [<DependsOn(typeof<DependencyConflictModule2>)>] DependencyConflictModule5() =
        inherit Module<bool>()
        override _.ExecuteAsync(_, _) = Task.FromResult(true)

type NestedCollisionTests() =
    [<Test>]
    member _.Modules_Dependent_On_Each_Other_Throws_Exception() = async {
        let mutable message = null

        try
            let! _ =
                TestPipelineHostBuilder.Create()
                    .AddModule<NestedCollisionTestModules.DependencyConflictModule1>()
                    .AddModule<NestedCollisionTestModules.DependencyConflictModule2>()
                    .AddModule<NestedCollisionTestModules.DependencyConflictModule3>()
                    .AddModule<NestedCollisionTestModules.DependencyConflictModule4>()
                    .AddModule<NestedCollisionTestModules.DependencyConflictModule5>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
            ()
        with :? DependencyCollisionException as ex ->
            message <- ex.Message
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(message), "Dependency collision detected: **DependencyConflictModule2** -> DependencyConflictModule3 -> DependencyConflictModule4 -> DependencyConflictModule5 -> **DependencyConflictModule2**"))
    }
