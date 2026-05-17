namespace ModularPipelines.UnitTests.FSharp.Execution

open System
open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open ModularPipelines.Attributes
open ModularPipelines.Configuration
open ModularPipelines.Context
open ModularPipelines.Engine
open ModularPipelines.Enums
open ModularPipelines.Extensions
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open TUnit.Assertions.Extensions
[<AutoOpen>]
module private Internal =
    let waitForCancellationDelay = TimeSpan.FromMilliseconds(100:int64)

type private BadModule() =
    inherit ThrowingTestModule<bool>()

[<DependsOn(typeof<BadModule>)>]
type private Module1CancellationTest() =
    inherit SimpleTestModule<bool>()
    override _.Result = true

type private LongRunningModule() =
    inherit Module<bool>()

    let taskCompletionSource = TaskCompletionSource<bool>()

    override _.ExecuteAsync(_: IModuleContext, cancellationToken: CancellationToken) =
        task {
            let! _ = taskCompletionSource.Task.WaitAsync(cancellationToken)
            return true
        }

type private LongRunningModuleWithoutCancellation() =
    inherit Module<bool>()

    let taskCompletionSource = TaskCompletionSource<bool>()

    override _.Configure() =
        ModuleConfiguration.Create().WithTimeout(TimeSpan.FromSeconds(10:int64)).Build()

    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
        task {
            let! _ = taskCompletionSource.Task
            return true
        }

[<NotInParallel(nameof EngineCancellationTokenTests)>]
type EngineCancellationTokenTests() =
    inherit TestBase()

    [<Test>]
    member _.When_Cancel_Engine_Token_With_DependsOn_Then_Modules_Cancel() = async {
        let builder = TestPipelineHostBuilder.Create().AddModule<BadModule>().AddModule<Module1CancellationTest>()
        builder.Options.ThrowOnPipelineFailure <- true

        let! host = builder.BuildHostAsync() |> Async.AwaitTask
        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()

        let mutable threw = false

        try
            do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore
        with _ ->
            threw <- true

        let module1Result = resultRegistry.GetResult(typeof<Module1CancellationTest>)

        do! check(Assert.That(threw).IsTrue())
        do! check(Assert.That(module1Result).IsNotNull())
        do! check(Assert.That(module1Result.ModuleStatus).IsEqualTo(Status.PipelineTerminated))
    }

    [<Test>]
    member _.When_Cancel_Engine_Token_Without_DependsOn_Then_Modules_Cancel() = async {
        let builder = TestPipelineHostBuilder.Create().AddModule<BadModule>().AddModule<LongRunningModule>()
        builder.Options.ThrowOnPipelineFailure <- true

        let! host = builder.BuildHostAsync() |> Async.AwaitTask
        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let pipelineTask = host.ExecutePipelineAsync()

        do! Task.Delay(waitForCancellationDelay) |> Async.AwaitTask

        let mutable threw = false

        try
            do! pipelineTask |> Async.AwaitTask |> Async.Ignore
        with _ ->
            threw <- true

        let longRunningModuleResult = resultRegistry.GetResult(typeof<LongRunningModule>)

        do! check(Assert.That(threw).IsTrue())
        do! check(Assert.That(longRunningModuleResult).IsNotNull())
        do! check(Assert.That(longRunningModuleResult.ModuleStatus).IsEqualTo(Status.PipelineTerminated))
        do! check(Assert.That(longRunningModuleResult.ModuleDuration).IsLessThan(TimeSpan.FromSeconds(5:int64)))
    }

    [<Test>]
    member _.When_Cancel_Engine_Token_Without_DependsOn_Then_Modules_Cancel_Without_Cancellation() = async {
        let builder =
            TestPipelineHostBuilder.Create()
                .AddModule<BadModule>()
                .AddModule<LongRunningModuleWithoutCancellation>()

        builder.Options.ThrowOnPipelineFailure <- true

        let! host = builder.BuildHostAsync() |> Async.AwaitTask
        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let pipelineTask = host.ExecutePipelineAsync()

        do! Task.Delay(waitForCancellationDelay) |> Async.AwaitTask

        let mutable threw = false

        try
            do! pipelineTask |> Async.AwaitTask |> Async.Ignore
        with _ ->
            threw <- true

        let longRunningModuleResult = resultRegistry.GetResult(typeof<LongRunningModuleWithoutCancellation>)

        do! check(Assert.That(threw).IsTrue())
        do! check(Assert.That(longRunningModuleResult).IsNotNull())
        do! check(Assert.That(longRunningModuleResult.ModuleStatus).IsEqualTo(Status.PipelineTerminated))
    }
