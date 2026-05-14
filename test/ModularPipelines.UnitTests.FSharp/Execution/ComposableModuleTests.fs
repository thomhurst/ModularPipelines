namespace ModularPipelines.UnitTests.FSharp.Execution

open System
open System.Linq
open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open ModularPipelines.Configuration
open ModularPipelines.Context
open ModularPipelines.Engine
open ModularPipelines.Extensions
open ModularPipelines.Enums
open ModularPipelines.Modules
open ModularPipelines.Models
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open TUnit.Assertions.Extensions

type private AlwaysSkippedModule() =
    inherit Module<string>()

    override _.Configure() =
        ModuleConfiguration.Create()
            .WithSkipWhen(Func<SkipDecision>(fun () -> SkipDecision.Skip("Skipped via composition")))
            .Build()

    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) = Task.FromResult("Executed")

type private NeverSkippedModule() =
    inherit Module<string>()

    override _.Configure() =
        ModuleConfiguration.Create()
            .WithSkipWhen(Func<SkipDecision>(fun () -> SkipDecision.DoNotSkip))
            .Build()

    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) = Task.FromResult("Executed")

type private TimeoutableModule() =
    inherit Module<string>()

    override _.Configure() =
        ModuleConfiguration.Create().WithTimeout(TimeSpan.FromSeconds(5:int64)).Build()

    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) = Task.FromResult("Executed with timeout")

type private MultiBehaviorModule() =
    inherit Module<int>()

    static let mutable beforeHookCalled = false
    static let mutable afterHookCalled = false

    static member BeforeHookCalled = beforeHookCalled
    static member AfterHookCalled = afterHookCalled
    static member Reset() =
        beforeHookCalled <- false
        afterHookCalled <- false

    override _.Configure() =
        ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromMinutes(1:int64))
            .WithSkipWhen(Func<SkipDecision>(fun () -> SkipDecision.DoNotSkip))
            .WithBeforeExecute(Func<IModuleContext, Task>(fun _ -> task { beforeHookCalled <- true }))
            .WithAfterExecute(Func<IModuleContext, Task>(fun _ -> task { afterHookCalled <- true }))
            .Build()

    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) = Task.FromResult(42)

type private AlwaysRunModule() =
    inherit Module<string>()

    override _.Configure() =
        ModuleConfiguration.Create().WithAlwaysRun().Build()

    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) = Task.FromResult("Always ran")

type ComposableModuleTests() =
    [<Test>]
    member _.Skippable_Module_Is_Skipped_When_Condition_True() = async {
        let! host =
            TestPipelineHostBuilder.Create().AddModule<AlwaysSkippedModule>().BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let moduleResult = resultRegistry.GetResult(typeof<AlwaysSkippedModule>)

        do! check(Assert.That(moduleResult.SkipDecisionOrDefault.ShouldSkip).IsTrue())
        do! check(Assert.That<SkipDecision>(moduleResult.SkipDecisionOrDefault.Reason).IsEqualTo(SkipDecision.Skip("Skipped via composition")))
    }

    [<Test>]
    member _.Skippable_Module_Executes_When_Condition_False() = async {
        let! host =
            TestPipelineHostBuilder.Create().AddModule<NeverSkippedModule>().BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let moduleResult = resultRegistry.GetResult(typeof<NeverSkippedModule>)

        do! check(Assert.That(moduleResult.SkipDecisionOrDefault.ShouldSkip).IsFalse())
    }

    [<Test>]
    member _.Timeoutable_Module_Has_Custom_Timeout() = async {
        let! host =
            TestPipelineHostBuilder.Create().AddModule<TimeoutableModule>().BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let moduleResult = resultRegistry.GetResult(typeof<TimeoutableModule>)

        do! check(Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Multi_Behavior_Module_Calls_Hooks() = async {
        MultiBehaviorModule.Reset()

        do! TestPipelineHostBuilder.Create().AddModule<MultiBehaviorModule>().ExecutePipelineAsync()
            |> Async.AwaitTask
            |> Async.Ignore

        do! check(Assert.That(MultiBehaviorModule.BeforeHookCalled).IsTrue())
        do! check(Assert.That(MultiBehaviorModule.AfterHookCalled).IsTrue())
    }

    [<Test>]
    member _.AlwaysRun_Module_Has_Correct_Configuration() = async {
        let! host =
            TestPipelineHostBuilder.Create().AddModule<AlwaysRunModule>().BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let alwaysRunModule = host.RootServices.GetServices<IModule>().OfType<AlwaysRunModule>().Single()
        do! check(Assert.That(alwaysRunModule).IsNotNull())
    }
