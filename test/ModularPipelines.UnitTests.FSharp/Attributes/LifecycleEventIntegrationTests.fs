namespace ModularPipelines.UnitTests.FSharp.Attributes

open System
open System.Threading
open System.Threading.Tasks
open ModularPipelines.Attributes.Events
open ModularPipelines.Configuration
open ModularPipelines.Context
open ModularPipelines.Enums
open ModularPipelines.Extensions
open ModularPipelines.Models
open ModularPipelines.Modules
open ModularPipelines.Options
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module LifecycleEventIntegrationTests =
    let eventLog = ResizeArray<string>()

    type LogStartAttribute() =
        inherit Attribute()
        interface IModuleStartHandler with
            member _.ContinueOnError = false
            member _.OnModuleStartAsync(context: IModuleHookContext) =
                eventLog.Add($"Start:{context.ModuleName}")
                Task.CompletedTask

    type LogEndAttribute() =
        inherit Attribute()
        interface IModuleEndHandler with
            member _.ContinueOnError = false
            member _.OnModuleEndAsync(context: IModuleHookContext, _: IModuleResult) =
                eventLog.Add($"End:{context.ModuleName}")
                Task.CompletedTask

    type LogFailedAttribute() =
        inherit Attribute()
        interface IModuleFailureHandler with
            member _.ContinueOnError = true
            member _.OnModuleFailureAsync(context: IModuleHookContext, ex: Exception) =
                eventLog.Add($"Failed:{context.ModuleName}:{ex.Message}")
                Task.CompletedTask

    type LogSkippedAttribute() =
        inherit Attribute()
        interface IModuleSkippedHandler with
            member _.ContinueOnError = false
            member _.OnModuleSkippedAsync(context: IModuleHookContext, reason: SkipDecision) =
                eventLog.Add($"Skipped:{context.ModuleName}:{reason.Reason}")
                Task.CompletedTask

    [<LogStart>]
    [<LogEnd>]
    type SuccessfulModule() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                do! Task.Yield()
                return "Success"
            }

    [<LogStart>]
    [<LogFailed>]
    type FailingModule() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) : Task<string> =
            raise (InvalidOperationException("Intentional failure"))

    [<LogStart>]
    [<LogSkipped>]
    type SkippingModule() =
        inherit Module<string>()
        override _.Configure() =
            ModuleConfiguration.Create()
                .WithSkipWhen(fun () -> SkipDecision.Skip("Test skip reason"))
                .Build()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            Task.FromResult<string>("Should not execute")

[<NotInParallel(nameof LifecycleEventIntegrationTests)>]
type LifecycleEventIntegrationTests() =
    inherit TestBase()

    [<Before(HookType.Test)>]
    member _.ClearEventLog() =
        LifecycleEventIntegrationTests.eventLog.Clear()

    [<Test>]
    member _.SuccessfulModule_InvokesStartAndEndEvents() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<LifecycleEventIntegrationTests.SuccessfulModule>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status = Status.Successful).IsTrue())
        do! check(Assert.That(LifecycleEventIntegrationTests.eventLog |> Seq.contains "Start:SuccessfulModule").IsTrue())
        do! check(Assert.That(LifecycleEventIntegrationTests.eventLog |> Seq.contains "End:SuccessfulModule").IsTrue())
    }

    [<Test>]
    member _.FailingModule_InvokesStartAndFailedEvents() = async {
        try
            do!
                TestPipelineHostBuilder.Create()
                    .AddModule<LifecycleEventIntegrationTests.FailingModule>()
                    .ConfigurePipelineOptions(fun _ options ->
                        options.ExecutionMode <- ExecutionMode.WaitForAllModules)
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
                |> Async.Ignore
        with _ -> ()

        do! check(Assert.That(LifecycleEventIntegrationTests.eventLog |> Seq.contains "Start:FailingModule").IsTrue())
        do! check(Assert.That(LifecycleEventIntegrationTests.eventLog |> Seq.exists (fun e -> e.StartsWith("Failed:FailingModule:"))).IsTrue())
    }

    [<Test>]
    member _.SkippingModule_InvokesStartAndSkippedEvents() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<LifecycleEventIntegrationTests.SkippingModule>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status = Status.Successful).IsTrue())
        do! check(Assert.That(LifecycleEventIntegrationTests.eventLog |> Seq.contains "Start:SkippingModule").IsTrue())
        do! check(Assert.That(LifecycleEventIntegrationTests.eventLog |> Seq.exists (fun e -> e.Contains("Skipped:SkippingModule:Test skip reason"))).IsTrue())
    }
