namespace ModularPipelines.UnitTests.FSharp.Execution

open System
open System.Threading
open System.Threading.Tasks
open ModularPipelines.Context
open ModularPipelines.Enums
open ModularPipelines.Extensions
open ModularPipelines.Modules
open ModularPipelines.Options
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open TUnit.Assertions.Extensions

type SimpleModule() =
    inherit Module<string>()
    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
        Task.FromResult("Done")

type SimpleModule2() =
    inherit Module<string>()
    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
        Task.FromResult("Done")

[<NotInParallel("ConcurrencyOptionsTests")>]
type ConcurrencyOptionsTests() =
    inherit TestBase()

    [<Test>]
    member _.ConcurrencyOptions_HasCorrectDefaultValues() = async {
        let options = ConcurrencyOptions()
        let expectedMaxParallelism = Environment.ProcessorCount * 4

        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(options.MaxParallelism), expectedMaxParallelism))
        do! check(Assert.That(options.MaxCpuIntensiveModules).IsEqualTo(Environment.ProcessorCount))
        do! check(Assert.That(options.MaxIoIntensiveModules).IsNull())
    }

    [<Test>]
    member _.Pipeline_RespectsMaxParallelismSetting() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<SimpleModule>()
                .AddModule<SimpleModule2>()
                .ConfigurePipelineOptions(fun _ options ->
                    options.Concurrency.MaxParallelism <- 2
                )
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Pipeline_RespectsMaxCpuIntensiveModulesSetting() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<SimpleModule>()
                .ConfigurePipelineOptions(fun _ options ->
                    options.Concurrency.MaxCpuIntensiveModules <- 1
                )
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Pipeline_RespectsMaxIoIntensiveModulesSetting() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<SimpleModule>()
                .ConfigurePipelineOptions(fun _ options ->
                    options.Concurrency.MaxIoIntensiveModules <- Nullable 10
                )
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.PipelineOptions_HasConcurrencyProperty() = async {
        let options = PipelineOptions()

        do! check(Assert.That(options.Concurrency).IsNotNull())
        do! check(Assert.That(options.Concurrency).IsTypeOf<ConcurrencyOptions>())
    }
