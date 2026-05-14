namespace ModularPipelines.UnitTests.FSharp.Execution

open System.Collections.Concurrent
open System.Threading
open System.Threading.Tasks
open ModularPipelines.Attributes
open ModularPipelines.Context
open ModularPipelines.Enums
open ModularPipelines.Extensions
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open TUnit.Assertions.Extensions

module private SharedState =
    let cpuModulesExecuting = ConcurrentBag<string>()
    let cpuViolations = ConcurrentBag<string>()
    let mutable maxCpuConcurrency = 0

[<ExecutionHint(ExecutionType.CpuIntensive)>]
type private CpuIntensiveModule1() =
    inherit Module<string>()

    override this.ExecuteAsync(_: IModuleContext, cancellationToken: CancellationToken) =
        task {
            let moduleName = this.GetType().Name
            SharedState.cpuModulesExecuting.Add(moduleName.ToString())

            let currentCount = SharedState.cpuModulesExecuting.Count
            if currentCount > SharedState.maxCpuConcurrency then
                Interlocked.Exchange(&SharedState.maxCpuConcurrency, currentCount) |> ignore

            do! Task.Delay(50, cancellationToken)

            if SharedState.cpuModulesExecuting.Count > 2 then
                SharedState.cpuViolations.Add($"{moduleName}: {SharedState.cpuModulesExecuting.Count} concurrent CPU-intensive modules")

            let mutable ignored = Unchecked.defaultof<string>
            SharedState.cpuModulesExecuting.TryTake(&ignored) |> ignore
            return moduleName
        }

[<ExecutionHint(ExecutionType.CpuIntensive)>]
type private CpuIntensiveModule2() =
    inherit Module<string>()

    override this.ExecuteAsync(_: IModuleContext, cancellationToken: CancellationToken) =
        task {
            let moduleName = this.GetType().Name
            SharedState.cpuModulesExecuting.Add(moduleName)

            let currentCount = SharedState.cpuModulesExecuting.Count
            if currentCount > SharedState.maxCpuConcurrency then
                Interlocked.Exchange(&SharedState.maxCpuConcurrency, currentCount) |> ignore

            do! Task.Delay(50, cancellationToken)

            let mutable ignored = Unchecked.defaultof<string>
            SharedState.cpuModulesExecuting.TryTake(&ignored) |> ignore
            return moduleName
        }

[<ExecutionHint(ExecutionType.CpuIntensive)>]
type private CpuIntensiveModule3() =
    inherit Module<string>()

    override this.ExecuteAsync(_: IModuleContext, cancellationToken: CancellationToken) =
        task {
            let moduleName = this.GetType().Name
            SharedState.cpuModulesExecuting.Add(moduleName)

            let currentCount = SharedState.cpuModulesExecuting.Count
            if currentCount > SharedState.maxCpuConcurrency then
                Interlocked.Exchange(&SharedState.maxCpuConcurrency, currentCount) |> ignore

            do! Task.Delay(50, cancellationToken)

            let mutable ignored = Unchecked.defaultof<string>
            SharedState.cpuModulesExecuting.TryTake(&ignored) |> ignore
            return moduleName
        }

[<ExecutionHint(ExecutionType.IoIntensive)>]
type private IoIntensiveModule() =
    inherit Module<string>()
    override _.ExecuteAsync(_: IModuleContext, cancellationToken: CancellationToken) =
        task {
            do! Task.Delay(10, cancellationToken)
            return "IoIntensive"
        }

[<ExecutionHint(ExecutionType.Default)>]
type private DefaultExecutionTypeModule() =
    inherit Module<string>()
    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) = Task.FromResult("Default")

type private NoHintModule() =
    inherit Module<string>()
    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) = Task.FromResult("NoHint")

[<NotInParallel(nameof ExecutionHintTests)>]
type ExecutionHintTests() =
    inherit TestBase()

    [<Before(HookType.Test)>]
    member _.ClearState() =
        let mutable executingItem = Unchecked.defaultof<string>
        while SharedState.cpuModulesExecuting.TryTake(&executingItem) do ()

        let mutable violationItem = Unchecked.defaultof<string>
        while SharedState.cpuViolations.TryTake(&violationItem) do ()

        SharedState.maxCpuConcurrency <- 0

    [<Test>]
    member _.ExecutionHintAttribute_CanBeAppliedToModule() = async {
        let! result =
            TestPipelineHostBuilder.Create().AddModule<CpuIntensiveModule1>().ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.ModulesWithoutExecutionHint_UseDefaultType() = async {
        let! result =
            TestPipelineHostBuilder.Create().AddModule<NoHintModule>().ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.AllExecutionTypes_ExecuteSuccessfully() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<CpuIntensiveModule1>()
                .AddModule<IoIntensiveModule>()
                .AddModule<DefaultExecutionTypeModule>()
                .AddModule<NoHintModule>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.CpuIntensiveModules_AreThrottled() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<CpuIntensiveModule1>()
                .AddModule<CpuIntensiveModule2>()
                .AddModule<CpuIntensiveModule3>()
                .ConfigurePipelineOptions(fun _ options -> options.Concurrency.MaxCpuIntensiveModules <- 2)
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))
        do! check(Assert.That(SharedState.maxCpuConcurrency).IsLessThanOrEqualTo(2))
    }
