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

[<AutoOpen>]
module private DependsOnAllInheritingFromTestsHelpers =
    [<Literal>]
    let toleranceMilliseconds = -25.0

    let moduleDelay = TimeSpan.FromMilliseconds(50.0)


[<AbstractClass>]
type InheritingBaseModule() =
    inherit Module<bool>()

[<AbstractClass>]
type InheritingGenericBaseModule<'T>() =
    inherit Module<'T>()

type InheritingGenericModule1() =
    inherit InheritingGenericBaseModule<int>()

    override _.ExecuteAsync(_, cancellationToken) =
        task {
            do! Task.Delay(moduleDelay, cancellationToken)
            return 42
        }

type InheritingGenericModule2() =
    inherit InheritingGenericBaseModule<string>()

    override _.ExecuteAsync(_, cancellationToken) =
        task {
            do! Task.Delay(moduleDelay, cancellationToken)
            return "test"
        }

[<DependsOnAllModulesInheritingFrom(typedefof<InheritingGenericBaseModule<_>>)>]
type InheritingDependsOnOpenGenericModule() =
    inherit Module<bool>()

    override _.ExecuteAsync(_, _) =
        task {
            do! Task.Yield()
            return true
        }

type InheritingModule1() =
    inherit InheritingBaseModule()

    override _.ExecuteAsync(_, cancellationToken) =
        task {
            do! Task.Delay(moduleDelay, cancellationToken)
            return true
        }

[<DependsOn(typeof<InheritingModule1>)>]
type InheritingModule2() =
    inherit InheritingBaseModule()

    override _.ExecuteAsync(_, cancellationToken) =
        task {
            do! Task.Delay(moduleDelay, cancellationToken)
            return true
        }

[<ModularPipelines.Attributes.DependsOn(typeof<InheritingModule1>, Optional = true)>]
type InheritingModule3() =
    inherit InheritingBaseModule()

    override _.ExecuteAsync(_, cancellationToken) =
        task {
            do! Task.Delay(moduleDelay, cancellationToken)
            return true
        }

[<DependsOnAllModulesInheritingFrom(typeof<InheritingBaseModule>)>]
type InheritingModule4() =
    inherit Module<bool>()

    override _.ExecuteAsync(_, _) =
        task {
            do! Task.Yield()
            return true
        }

type DependsOnAllInheritingFromTests() =
    inherit TestBase()

    [<Test>]
    member _.No_Exception_Thrown_When_Dependent_Module_Present() = async {
        let timeProvider = FakeTimeProvider()

        let! host =
            TestPipelineHostBuilder.Create(TestHostSettings(), timeProvider)
                .AddModule<InheritingModule1>()
                .AddModule<InheritingModule2>()
                .AddModule<InheritingModule3>()
                .AddModule<InheritingModule4>()
                .BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let result1 = resultRegistry.GetResult(typeof<InheritingModule1>)
        let result2 = resultRegistry.GetResult(typeof<InheritingModule2>)
        let result3 = resultRegistry.GetResult(typeof<InheritingModule3>)
        let result4 = resultRegistry.GetResult(typeof<InheritingModule4>)

        do! check(Assert.That(result4.ModuleStart >= result1.ModuleStart.Add(moduleDelay.Add(TimeSpan.FromMilliseconds(toleranceMilliseconds)))).IsTrue())
        do! check(Assert.That(result4.ModuleStart >= result1.ModuleEnd).IsTrue())
        do! check(Assert.That(result4.ModuleStart >= result2.ModuleStart.Add(moduleDelay.Add(TimeSpan.FromMilliseconds(toleranceMilliseconds)))).IsTrue())
        do! check(Assert.That(result4.ModuleStart >= result2.ModuleEnd).IsTrue())
        do! check(Assert.That(result4.ModuleStart >= result3.ModuleStart.Add(moduleDelay.Add(TimeSpan.FromMilliseconds(toleranceMilliseconds)))).IsTrue())
        do! check(Assert.That(result4.ModuleStart >= result3.ModuleEnd).IsTrue())
    }

    [<Test>]
    member _.DependsOnAllModulesInheritingFrom_Works_With_Open_Generic_Types() = async {
        let timeProvider = FakeTimeProvider()

        let! host =
            TestPipelineHostBuilder.Create(TestHostSettings(), timeProvider)
                .AddModule<InheritingGenericModule1>()
                .AddModule<InheritingGenericModule2>()
                .AddModule<InheritingDependsOnOpenGenericModule>()
                .BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let result1 = resultRegistry.GetResult(typeof<InheritingGenericModule1>)
        let result2 = resultRegistry.GetResult(typeof<InheritingGenericModule2>)
        let dependentResult = resultRegistry.GetResult(typeof<InheritingDependsOnOpenGenericModule>)

        do! check(Assert.That(dependentResult.ModuleStart >= result1.ModuleEnd).IsTrue())
        do! check(Assert.That(dependentResult.ModuleStart >= result2.ModuleEnd).IsTrue())
    }
