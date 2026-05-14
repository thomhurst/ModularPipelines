namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System.Threading.Tasks
open ModularPipelines.Attributes
open ModularPipelines.Context
open ModularPipelines.Enums
open ModularPipelines.Extensions
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module OneWayDependenciesNonCollisionTestModules =
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

    and DependencyConflictModule5() =
        inherit Module<bool>()
        override _.ExecuteAsync(_, _) = Task.FromResult(true)

type OneWayDependenciesNonCollisionTests() =
    [<Test>]
    member _.Modules_Not_Dependent_On_Each_Other_Succeed() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<OneWayDependenciesNonCollisionTestModules.DependencyConflictModule1>()
                .AddModule<OneWayDependenciesNonCollisionTestModules.DependencyConflictModule2>()
                .AddModule<OneWayDependenciesNonCollisionTestModules.DependencyConflictModule3>()
                .AddModule<OneWayDependenciesNonCollisionTestModules.DependencyConflictModule4>()
                .AddModule<OneWayDependenciesNonCollisionTestModules.DependencyConflictModule5>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }
