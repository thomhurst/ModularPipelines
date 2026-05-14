namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System
open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Time.Testing
open ModularPipelines.Attributes
open ModularPipelines.Context
open ModularPipelines.Engine
open ModularPipelines.Extensions
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module TimedDependencyTestModules =
    let longModuleDelay = TimeSpan.FromMilliseconds(100:int64)
    let shortModuleDelay = TimeSpan.FromMilliseconds(20:int64)

    type FiveSecondModule() =
        inherit Module<bool>()

        override _.ExecuteAsync(_, cancellationToken) =
            task {
                do! Task.Delay(longModuleDelay, cancellationToken)
                return true
            }

    [<DependsOn(typeof<FiveSecondModule>)>]
    type OneSecondModuleDependentOnFiveSecondModule() =
        inherit Module<bool>()

        override _.ExecuteAsync(_, cancellationToken) =
            task {
                do! Task.Delay(shortModuleDelay, cancellationToken)
                return true
            }

type TimedDependencyTests() =
    [<Test>]
    member _.OneSecondModule_WillWaitForFiveSecondModule_ThenExecute() = async {
        let timeProvider = FakeTimeProvider()

        let! host =
            TestPipelineHostBuilder.Create(TestHostSettings(), timeProvider)
                .AddModule<TimedDependencyTestModules.FiveSecondModule>()
                .AddModule<TimedDependencyTestModules.OneSecondModuleDependentOnFiveSecondModule>()
                .BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let fiveSecondResult = resultRegistry.GetResult(typeof<TimedDependencyTestModules.FiveSecondModule>)
        let oneSecondResult = resultRegistry.GetResult(typeof<TimedDependencyTestModules.OneSecondModuleDependentOnFiveSecondModule>)

        do! check(Assert.That(oneSecondResult.ModuleDuration >= TimedDependencyTestModules.shortModuleDelay.Add(TimeSpan.FromMilliseconds(-10:int64))).IsTrue())
        do! check(Assert.That(oneSecondResult.ModuleEnd >= fiveSecondResult.ModuleStart + TimedDependencyTestModules.longModuleDelay + TimedDependencyTestModules.shortModuleDelay.Add(TimeSpan.FromMilliseconds(-20:int64))).IsTrue())
        do! check(Assert.That(oneSecondResult.ModuleStart >= fiveSecondResult.ModuleEnd).IsTrue())
    }
