namespace ModularPipelines.UnitTests.FSharp.Modules

open System.Collections.Generic
open System.Linq
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open ModularPipelines.Configuration
open ModularPipelines.Context
open ModularPipelines.Engine
open ModularPipelines.Extensions
open ModularPipelines.Models
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open ModularPipelines.Enums

type private SimpleSyncModule() =
    inherit SyncModule<string>()
    override _.Execute(_, _) = "Hello from sync module"

type private SyncModuleReturningNull() =
    inherit SyncModule<string>()
    override _.Execute(_, _) = null

type private SyncModuleWithComplexType() =
    inherit SyncModule<Dictionary<string, int>>()
    override _.Execute(_, _) =
        let d = Dictionary<string, int>()
        d["one"] <- 1
        d["two"] <- 2
        d["three"] <- 3
        d

type private ThrowingSyncModule() =
    inherit SyncModule<string>()
    override _.Execute(_, _) =
        raise (System.InvalidOperationException("Sync module exception"))

type private SyncModuleWithBeforeHook() =
    inherit SyncModule<string>()
    let mutable _beforeCalled = false
    member _.BeforeHookCalled = _beforeCalled
    override _.OnBeforeExecute(_, _) = _beforeCalled <- true
    override _.Execute(_, _) = "executed"

type private SyncModuleWithAfterHook() =
    inherit SyncModule<string>()
    let mutable _afterCalled = false
    let mutable _capturedResult: ModuleResult<string> = Unchecked.defaultof<_>
    member _.AfterHookCalled = _afterCalled
    member _.CapturedResult = _capturedResult
    override _.OnAfterExecute(_, result, _) =
        _afterCalled <- true
        _capturedResult <- result
        Unchecked.defaultof<ModuleResult<string>>
    override _.Execute(_, _) = "original"

type private SyncModuleWithFailedHook() =
    inherit SyncModule<string>()
    let mutable _failedCalled = false
    let mutable _capturedException: System.Exception = null
    member _.FailedHookCalled = _failedCalled
    member _.CapturedExceptionInHook = _capturedException
    override _.OnFailed(_, ex, _) =
        _failedCalled <- true
        _capturedException <- ex
    override _.Execute(_, _) =
        raise (System.InvalidOperationException("Test failure"))

type private SyncModuleWithSkipConfig() =
    inherit SyncModule<string>()
    let mutable _skippedCalled = false
    let mutable _capturedSkip: SkipDecision = Unchecked.defaultof<_>
    member _.SkippedHookCalled = _skippedCalled
    member _.CapturedSkipDecision = _capturedSkip
    override _.Configure() =
        ModuleConfiguration.Create().WithSkipWhen(fun () -> SkipDecision.Skip("Always skip")).Build()
    override _.OnSkipped(_, skipDecision, _) =
        _skippedCalled <- true
        _capturedSkip <- skipDecision
    override _.Execute(_, _) = "should not execute"

type private SyncDependencyModule() =
    inherit SyncModule<int>()
    override _.Execute(_, _) = 42

[<ModularPipelines.Attributes.DependsOn(typeof<SyncDependencyModule>)>]
type private SyncDependentModule() =
    inherit SyncModule<string>()
    override _.Execute(context, _) =
        let dep = context.GetModule<SyncDependencyModule>().GetAwaiter().GetResult()
        sprintf "Dependency value: %d" dep.ValueOrDefault

type private AsyncDependencyModule() =
    inherit Module<int>()
    override _.ExecuteAsync(_, _) =
        task {
            do! System.Threading.Tasks.Task.Yield()
            return 100
        }

[<ModularPipelines.Attributes.DependsOn(typeof<AsyncDependencyModule>)>]
type private SyncDependsOnAsync() =
    inherit SyncModule<string>()
    override _.Execute(context, _) =
        let dep = context.GetModule<AsyncDependencyModule>().GetAwaiter().GetResult()
        sprintf "Async dependency value: %d" dep.ValueOrDefault

type private SyncModuleForAsyncToDepend() =
    inherit SyncModule<int>()
    override _.Execute(_, _) = 200

[<ModularPipelines.Attributes.DependsOn(typeof<SyncModuleForAsyncToDepend>)>]
type private AsyncDependsOnSync() =
    inherit Module<string>()
    override _.ExecuteAsync(context, _) =
        task {
            do! System.Threading.Tasks.Task.Yield()
            let! dep = context.GetModule<SyncModuleForAsyncToDepend>()
            return sprintf "Sync dependency value: %d" dep.ValueOrDefault
        }

type private SyncModuleWithTimeout() =
    inherit SyncModule<string>()
    override _.Configure() =
        ModuleConfiguration.Create().WithTimeout(System.TimeSpan.FromMinutes(5.0)).Build()
    override _.Execute(_, _) = "configured"

type private SyncModuleWithRetry() =
    inherit SyncModule<string>()
    let mutable _count = 0
    member _.ExecutionCount = _count
    override _.Configure() =
        ModuleConfiguration.Create().WithRetryCount(3).Build()
    override _.Execute(_, _) =
        _count <- _count + 1
        if _count < 3 then
            raise (System.InvalidOperationException(sprintf "Attempt %d failed" _count))
        "success on third try"

type private SyncModuleCheckingCancellation() =
    inherit SyncModule<string>()
    override _.Execute(_, cancellationToken) =
        cancellationToken.ThrowIfCancellationRequested()
        "not cancelled"

type private SyncModuleAccessingContext() =
    inherit SyncModule<string>()
    override _.Execute(context, _) =
        context.Logger.LogInformation("Logging from sync module")
        "context accessed"

type SyncModuleTests() =
    inherit TestBase()

    [<Test>]
    member this.SyncModule_Executes_And_Returns_Value() = async {
        let! m = this.RunModule<SimpleSyncModule>() |> Async.AwaitTask
        let! result = m.CompletionSource.Task |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.ValueOrDefault), "Hello from sync module"))
        do! check(Assert.That(result.ModuleResultType = ModuleResultType.Success).IsTrue())
    }

    [<Test>]
    member this.SyncModule_Can_Return_Null() = async {
        let! m = this.RunModule<SyncModuleReturningNull>() |> Async.AwaitTask
        let! result = m.CompletionSource.Task |> Async.AwaitTask
        do! check(Assert.That(result.ValueOrDefault).IsNull())
        do! check(Assert.That(result.ModuleResultType = ModuleResultType.Success).IsTrue())
    }

    [<Test>]
    member this.SyncModule_Can_Return_Complex_Types() = async {
        let! m = this.RunModule<SyncModuleWithComplexType>() |> Async.AwaitTask
        let! result = m.CompletionSource.Task |> Async.AwaitTask
        do! check(Assert.That(result.ValueOrDefault <> null).IsTrue())
        do! check(Assert.That(result.ValueOrDefault.Count = 3).IsTrue())
        do! check(Assert.That(result.ValueOrDefault["two"] = 2).IsTrue())
    }

    [<Test>]
    member _.SyncModule_Exception_Is_Captured() = async {
        let! host =
            TestPipelineHostBuilder.Create().AddModule<ThrowingSyncModule>().BuildHostAsync()
            |> Async.AwaitTask

        try
            do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore
        with _ -> ()

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let result = resultRegistry.GetResult(typeof<ThrowingSyncModule>)

        do! check(Assert.That(result.ModuleStatus = Status.Failed).IsTrue())
        do! check(Assert.That(result.ExceptionOrDefault).IsNotNull())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.ExceptionOrDefault.Message), "Sync module exception"))
    }

    [<Test>]
    member this.SyncModule_OnBeforeExecute_Is_Called() = async {
        let! m = this.RunModule<SyncModuleWithBeforeHook>() |> Async.AwaitTask
        do! check(Assert.That(m.BeforeHookCalled).IsTrue())
    }

    [<Test>]
    member this.SyncModule_OnAfterExecute_Is_Called() = async {
        let! m = this.RunModule<SyncModuleWithAfterHook>() |> Async.AwaitTask
        do! check(Assert.That(m.AfterHookCalled).IsTrue())
        do! check(Assert.That(m.CapturedResult).IsNotNull())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(m.CapturedResult.ValueOrDefault), "original"))
    }

    [<Test>]
    member _.SyncModule_OnFailed_Is_Called_On_Exception() = async {
        let! host =
            TestPipelineHostBuilder.Create().AddModule<SyncModuleWithFailedHook>().BuildHostAsync()
            |> Async.AwaitTask

        try
            do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore
        with _ -> ()

        let modules = host.RootServices.GetServices<IModule>()
        let m = modules.OfType<SyncModuleWithFailedHook>().Single()

        do! check(Assert.That(m.FailedHookCalled).IsTrue())
        do! check(Assert.That(m.CapturedExceptionInHook).IsNotNull())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(m.CapturedExceptionInHook.Message), "Test failure"))
    }

    [<Test>]
    member _.SyncModule_OnSkipped_Is_Called_When_Skipped() = async {
        let! host =
            TestPipelineHostBuilder.Create().AddModule<SyncModuleWithSkipConfig>().BuildHostAsync()
            |> Async.AwaitTask

        do! host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore

        let modules = host.RootServices.GetServices<IModule>()
        let m = modules.OfType<SyncModuleWithSkipConfig>().Single()

        do! check(Assert.That(m.SkippedHookCalled).IsTrue())
        do! check(Assert.That(m.CapturedSkipDecision).IsNotNull())
        do! check(Assert.That(m.CapturedSkipDecision.ShouldSkip).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(m.CapturedSkipDecision.Reason), "Always skip"))
    }

    [<Test>]
    member this.SyncModule_Can_Depend_On_Another_SyncModule() = async {
        let! struct (dep, dependent) = this.RunModules<SyncDependencyModule, SyncDependentModule>() |> Async.AwaitTask
        let! depResult = dep.CompletionSource.Task |> Async.AwaitTask
        let! dependentResult = dependent.CompletionSource.Task |> Async.AwaitTask
        do! check(Assert.That(depResult.ValueOrDefault = 42).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(dependentResult.ValueOrDefault), "Dependency value: 42"))
    }

    [<Test>]
    member this.SyncModule_Can_Depend_On_AsyncModule() = async {
        let! struct (asyncModule, syncModule) = this.RunModules<AsyncDependencyModule, SyncDependsOnAsync>() |> Async.AwaitTask
        let! asyncResult = asyncModule.CompletionSource.Task |> Async.AwaitTask
        let! syncResult = syncModule.CompletionSource.Task |> Async.AwaitTask
        do! check(Assert.That(asyncResult.ValueOrDefault = 100).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(syncResult.ValueOrDefault), "Async dependency value: 100"))
    }

    [<Test>]
    member this.AsyncModule_Can_Depend_On_SyncModule() = async {
        let! struct (syncModule, asyncModule) = this.RunModules<SyncModuleForAsyncToDepend, AsyncDependsOnSync>() |> Async.AwaitTask
        let! syncResult = syncModule.CompletionSource.Task |> Async.AwaitTask
        let! asyncResult = asyncModule.CompletionSource.Task |> Async.AwaitTask
        do! check(Assert.That(syncResult.ValueOrDefault = 200).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(asyncResult.ValueOrDefault), "Sync dependency value: 200"))
    }

    [<Test>]
    member this.SyncModule_Respects_Configuration() = async {
        let! m = this.RunModule<SyncModuleWithTimeout>() |> Async.AwaitTask
        let imodule = m :> IModule
        do! check(Assert.That(imodule.Configuration.Timeout = System.TimeSpan.FromMinutes(5.0)).IsTrue())
    }

    [<Test>]
    member this.SyncModule_Respects_Retry_Configuration() = async {
        let! m = this.RunModule<SyncModuleWithRetry>() |> Async.AwaitTask
        let! result = m.CompletionSource.Task |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.ValueOrDefault), "success on third try"))
        do! check(Assert.That(m.ExecutionCount = 3).IsTrue())
    }

    [<Test>]
    member this.SyncModule_Receives_CancellationToken() = async {
        let! m = this.RunModule<SyncModuleCheckingCancellation>() |> Async.AwaitTask
        let! result = m.CompletionSource.Task |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.ValueOrDefault), "not cancelled"))
    }

    [<Test>]
    member this.SyncModule_Can_Access_Context() = async {
        let! m = this.RunModule<SyncModuleAccessingContext>() |> Async.AwaitTask
        let! result = m.CompletionSource.Task |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.ValueOrDefault), "context accessed"))
    }

    [<Test>]
    member this.SimpleSyncTestModule_Helper_Works() = async {
        let! m = this.RunModule<SyncTrueModule>() |> Async.AwaitTask
        let! result = m.CompletionSource.Task |> Async.AwaitTask
        do! check(Assert.That(result.ValueOrDefault).IsTrue())
    }

    [<Test>]
    member this.SyncNullModule_Helper_Works() = async {
        let! m = this.RunModule<SyncNullModule>() |> Async.AwaitTask
        let! result = m.CompletionSource.Task |> Async.AwaitTask
        do! check(Assert.That(result.ValueOrDefault).IsNull())
    }
