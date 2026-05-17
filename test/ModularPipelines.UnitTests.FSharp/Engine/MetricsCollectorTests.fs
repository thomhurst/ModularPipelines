namespace ModularPipelines.UnitTests.FSharp.Engine

open System
open System.Threading
open System.Threading.Tasks
open ModularPipelines.Context
open ModularPipelines.Enums
open ModularPipelines.Extensions
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open TUnit.Assertions.Sources
open ModularPipelines.Models

type private QuickModule1() =
    inherit Module<string>()

    override _.ExecuteAsync(_: IModuleContext, cancellationToken: CancellationToken) =
        task {
            do! Task.Delay(10, cancellationToken)
            return "Done"
        }

type private QuickModule2() =
    inherit Module<string>()

    override _.ExecuteAsync(_: IModuleContext, cancellationToken: CancellationToken) =
        task {
            do! Task.Delay(10, cancellationToken)
            return "Done"
        }

type private QuickModule3() =
    inherit Module<string>()

    override _.ExecuteAsync(_: IModuleContext, cancellationToken: CancellationToken) =
        task {
            do! Task.Delay(10, cancellationToken)
            return "Done"
        }

[<NotInParallel(nameof MetricsCollectorTests)>]
type MetricsCollectorTests() =
    [<Test>]
    member _.PipelineSummary_ContainsMetrics() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<QuickModule1>()
                .AddModule<QuickModule2>()
                .AddModule<QuickModule3>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))
        do! check(Assert.That(result.Metrics).IsNotNull())
    }

    [<Test>]
    member _.PipelineMetrics_HasParallelismFactor() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<QuickModule1>()
                .AddModule<QuickModule2>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        let metrics = result.Metrics
        do! check(Assert.That(metrics).IsNotNull())
        do! check(Assert.That(metrics.ParallelismFactor >= 0.0).IsTrue())
    }

    [<Test>]
    member _.PipelineMetrics_HasPeakConcurrency() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<QuickModule1>()
                .AddModule<QuickModule2>()
                .AddModule<QuickModule3>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        let metrics = result.Metrics
        do! check(Assert.That(metrics).IsNotNull())
        do! check(Assert.That(metrics.PeakConcurrency >= 1).IsTrue())
    }

    [<Test>]
    member _.PipelineMetrics_HasAverageConcurrency() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<QuickModule1>()
                .AddModule<QuickModule2>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        let metrics = result.Metrics
        do! check(Assert.That(metrics).IsNotNull())
        do! check(Assert.That(metrics.AverageConcurrency >= 0.0).IsTrue())
    }

    [<Test>]
    member _.PipelineMetrics_HasEfficiency() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<QuickModule1>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        let metrics = result.Metrics
        do! check(Assert.That(metrics).IsNotNull())
        do! check(Assert.That(metrics.Efficiency >= 0.0).IsTrue())
        do! check(Assert.That(metrics.Efficiency <= 1.0).IsTrue())
    }

    [<Test>]
    member _.PipelineMetrics_HasModuleCounts() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<QuickModule1>()
                .AddModule<QuickModule2>()
                .AddModule<QuickModule3>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        let metrics = result.Metrics
        do! check(Assert.That(metrics).IsNotNull())
        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(metrics.TotalModules), 3))
        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(metrics.SuccessfulModules), 3))
        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(metrics.FailedModules), 0))
    }

    [<Test>]
    member _.PipelineMetrics_HasTimingData() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<QuickModule1>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        let metrics = result.Metrics
        do! check(Assert.That(metrics).IsNotNull())
        do! check(Assert.That(metrics.WallClockDuration > TimeSpan.Zero).IsTrue())
        do! check(Assert.That(metrics.TotalModuleExecutionTime >= TimeSpan.Zero).IsTrue())
    }

    [<Test>]
    member _.PipelineSummary_ContainsModuleTimelines() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<QuickModule1>()
                .AddModule<QuickModule2>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))
        do! check(Assert.That<ModuleTimeline>(result.ModuleTimelines).IsNotNull())
        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(result.ModuleTimelines.Count), 2))
    }

    [<Test>]
    member _.ModuleTimeline_ContainsModuleName() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<QuickModule1>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That<ModuleTimeline>(result.ModuleTimelines).IsNotNull())
        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(result.ModuleTimelines.Count), 1))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.ModuleTimelines[0].ModuleName), "QuickModule1"))
    }

    [<Test>]
    member _.ModuleTimeline_ContainsTimingData() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<QuickModule1>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That<ModuleTimeline>(result.ModuleTimelines).IsNotNull())

        let timeline = result.ModuleTimelines[0]
        do! check(Assert.That(timeline.StartTime).IsNotNull())
        do! check(Assert.That(timeline.EndTime).IsNotNull())
        do! check(Assert.That(timeline.ExecutionDuration).IsNotNull())
    }
