namespace ModularPipelines.UnitTests.FSharp.Attributes

open System
open System.Threading
open System.Threading.Tasks
open ModularPipelines.Attributes.Events
open ModularPipelines.Context
open ModularPipelines.Enums
open ModularPipelines.Extensions
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module DynamicDependencyIntegrationTests =
    let executionOrder = ResizeArray<string>()

    type AddDependencyAttribute(dependencyType: Type) =
        inherit Attribute()
        interface IModuleRegistrationEventReceiver with
            member _.OnRegistrationAsync(context: IModuleRegistrationContext) =
                context.AddDependency(dependencyType)
                Task.CompletedTask

    type ModuleA() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                executionOrder.Add("A")
                do! Task.Yield()
                return "A"
            }

    [<AddDependency(typeof<ModuleA>)>]
    type ModuleB() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                executionOrder.Add("B")
                do! Task.Yield()
                return "B"
            }

[<NotInParallel(nameof DynamicDependencyIntegrationTests)>]
type DynamicDependencyIntegrationTests() =
    inherit TestBase()

    [<Before(HookType.Test)>]
    member _.ClearExecutionOrder() =
        DynamicDependencyIntegrationTests.executionOrder.Clear()

    [<Test>]
    member _.DynamicDependency_ModuleBWaitsForModuleA() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyIntegrationTests.ModuleA>()
                .AddModule<DynamicDependencyIntegrationTests.ModuleB>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status = Status.Successful).IsTrue())
        do! check(Assert.That((DynamicDependencyIntegrationTests.executionOrder |> Seq.toArray) = [| "A"; "B" |]).IsTrue())
    }
