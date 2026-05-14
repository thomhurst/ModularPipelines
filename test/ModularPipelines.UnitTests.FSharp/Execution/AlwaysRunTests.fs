namespace ModularPipelines.UnitTests.FSharp.Execution

open System
open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open ModularPipelines.Attributes
open ModularPipelines.Configuration
open ModularPipelines.Context
open ModularPipelines.Engine
open ModularPipelines.Extensions
open ModularPipelines.Enums
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open TUnit.Assertions.Extensions

type private MyModule1() =
    inherit ThrowingTestModule<bool>()

[<DependsOn(typeof<MyModule1>)>]
type private MyModule2() =
    inherit Module<bool>()

    override _.Configure() =
        ModuleConfiguration.Create().WithAlwaysRun().Build()

    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
        task {
            do! Task.Yield()
            return raise (Exception())
        }

[<DependsOn(typeof<MyModule2>)>]
type private MyModule3() =
    inherit Module<bool>()

    override _.Configure() =
        ModuleConfiguration.Create().WithAlwaysRun().Build()

    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
        task {
            do! Task.Yield()
            return raise (Exception())
        }

[<DependsOn(typeof<MyModule3>)>]
type private MyModule4() =
    inherit Module<bool>()

    override _.Configure() =
        ModuleConfiguration.Create().WithAlwaysRun().Build()

    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
        task {
            do! Task.Yield()
            return raise (Exception())
        }

type AlwaysRunTests() =
    inherit TestBase()

    [<Test>]
    member _.AlwaysRunModules_Will_Run_Even_With_Exception() = async {
        let! host =
            TestPipelineHostBuilder.Create()
                .AddModule<MyModule1>()
                .AddModule<MyModule2>()
                .AddModule<MyModule3>()
                .AddModule<MyModule4>()
                .BuildHostAsync()
            |> Async.AwaitTask

        try
            do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore
        with _ ->
            ()

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let result1 = resultRegistry.GetResult(typeof<MyModule1>)
        let result2 = resultRegistry.GetResult(typeof<MyModule2>)
        let result3 = resultRegistry.GetResult(typeof<MyModule3>)
        let result4 = resultRegistry.GetResult(typeof<MyModule4>)

        do! check(Assert.That(result1.ModuleStatus).IsEqualTo(Status.Failed))
        do! check(Assert.That(result2.ModuleStatus).IsEqualTo(Status.Failed))
        do! check(Assert.That(result3.ModuleStatus).IsEqualTo(Status.Failed))
        do! check(Assert.That(result4.ModuleStatus).IsNotEqualTo(Status.NotYetStarted))
    }
