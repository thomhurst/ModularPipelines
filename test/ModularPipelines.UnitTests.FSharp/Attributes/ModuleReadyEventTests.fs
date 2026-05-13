namespace ModularPipelines.UnitTests.FSharp.Attributes

open System
open System.Threading
open System.Threading.Tasks
open ModularPipelines.Attributes
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

module ModuleReadyEventTests =
    let eventLog = ResizeArray<string>()

    type LogReadyAttribute() =
        inherit Attribute()
        interface IModuleReadyHandler with
            member _.ContinueOnError = false
            member _.OnModuleReadyAsync(context: IModuleHookContext) =
                eventLog.Add($"Ready:{context.ModuleName}")
                Task.CompletedTask

    type LogReadyWithTimingAttribute() =
        inherit Attribute()
        interface IModuleReadyHandler with
            member _.ContinueOnError = false
            member _.OnModuleReadyAsync(context: IModuleHookContext) =
                eventLog.Add($"Ready:{context.ModuleName}:ElapsedTime:{context.ElapsedTime.TotalMilliseconds >= 0.0}")
                Task.CompletedTask

    type LogReadyAndStartAttribute() =
        inherit Attribute()
        interface IModuleReadyHandler with
            member _.ContinueOnError = false
            member _.OnModuleReadyAsync(context: IModuleHookContext) =
                eventLog.Add($"Ready:{context.ModuleName}")
                Task.CompletedTask
        interface IModuleStartHandler with
            member _.ContinueOnError = false
            member _.OnModuleStartAsync(context: IModuleHookContext) =
                eventLog.Add($"Start:{context.ModuleName}")
                Task.CompletedTask

    type ThrowingReadyAttribute() =
        inherit Attribute()
        interface IModuleReadyHandler with
            member _.ContinueOnError = true
            member _.OnModuleReadyAsync(_: IModuleHookContext) =
                raise (InvalidOperationException("Ready event failed"))

    [<LogReady>]
    type SimpleModuleWithReadyEvent() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            Task.FromResult<string>("Done")

    [<LogReadyWithTiming>]
    type ModuleWithTimingCheck() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            Task.FromResult<string>("Done")

    [<LogReadyAndStart>]
    type ModuleWithReadyAndStart() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            Task.FromResult<string>("Done")

    type DependencyModule() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, ct: CancellationToken) =
            task {
                do! Task.Delay(10, ct)
                return "Dependency Done"
            }

    [<LogReady>]
    [<DependsOn(typeof<DependencyModule>)>]
    type DependentModuleWithReadyEvent() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            Task.FromResult<string>("Dependent Done")

    [<ThrowingReady>]
    type ModuleWithThrowingReadyEvent() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            Task.FromResult<string>("Done")

[<TUnit.Core.NotInParallel(nameof ModuleReadyEventTests)>]
type ModuleReadyEventTests() =
    inherit TestBase()

    [<Before(HookType.Test)>]
    member _.ClearEventLog() =
        ModuleReadyEventTests.eventLog.Clear()

    [<Test>]
    member _.ReadyEvent_IsFired_WhenModuleIsReady() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<ModuleReadyEventTests.SimpleModuleWithReadyEvent>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status = Status.Successful).IsTrue())
        do! check(Assert.That(ModuleReadyEventTests.eventLog |> Seq.contains "Ready:SimpleModuleWithReadyEvent").IsTrue())
    }

    [<Test>]
    member _.ReadyEvent_ProvidesValidContext() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<ModuleReadyEventTests.ModuleWithTimingCheck>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status = Status.Successful).IsTrue())
        do! check(Assert.That(ModuleReadyEventTests.eventLog |> Seq.exists (fun e -> e.Contains("Ready:ModuleWithTimingCheck:ElapsedTime:True"))).IsTrue())
    }

    [<Test>]
    member _.ReadyEvent_FiresBeforeStartEvent() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<ModuleReadyEventTests.ModuleWithReadyAndStart>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status = Status.Successful).IsTrue())

        let readyIndex = ModuleReadyEventTests.eventLog.IndexOf("Ready:ModuleWithReadyAndStart")
        let startIndex = ModuleReadyEventTests.eventLog.IndexOf("Start:ModuleWithReadyAndStart")

        do! check(Assert.That(readyIndex).IsGreaterThanOrEqualTo(0))
        do! check(Assert.That(startIndex).IsGreaterThanOrEqualTo(0))
        do! check(Assert.That(readyIndex).IsLessThan(startIndex))
    }

    [<Test>]
    member _.ReadyEvent_IsFired_WhenDependenciesComplete() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<ModuleReadyEventTests.DependencyModule>()
                .AddModule<ModuleReadyEventTests.DependentModuleWithReadyEvent>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status = Status.Successful).IsTrue())
        do! check(Assert.That(ModuleReadyEventTests.eventLog |> Seq.contains "Ready:DependentModuleWithReadyEvent").IsTrue())
    }

    [<Test>]
    member _.ReadyEvent_WithContinueOnError_DoesNotFailPipeline() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<ModuleReadyEventTests.ModuleWithThrowingReadyEvent>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))
    }
