namespace ModularPipelines.UnitTests.FSharp.Configuration

open System
open System.Threading.Tasks
open ModularPipelines.Configuration
open ModularPipelines.Context
open ModularPipelines.Models
open Moq
open Polly
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type ModuleConfigurationTests() =
    [<Test>]
    member _.Default_SkipCondition_IsNull() = async {
        let config = ModuleConfiguration.Default
        do! check(Assert.That(config.SkipCondition).IsNull())
    }

    [<Test>]
    member _.Default_Timeout_IsNull() = async {
        let config = ModuleConfiguration.Default
        do! check(Assert.That(config.Timeout.HasValue).IsFalse())
    }

    [<Test>]
    member _.Default_RetryPolicyFactory_IsNull() = async {
        let config = ModuleConfiguration.Default
        do! check(Assert.That(config.RetryPolicyFactory).IsNull())
    }

    [<Test>]
    member _.Default_IgnoreFailuresCondition_IsNull() = async {
        let config = ModuleConfiguration.Default
        do! check(Assert.That(config.IgnoreFailuresCondition).IsNull())
    }

    [<Test>]
    member _.Default_AlwaysRun_IsFalse() = async {
        let config = ModuleConfiguration.Default
        do! check(Assert.That(config.AlwaysRun).IsFalse())
    }

    [<Test>]
    member _.Default_OnBeforeExecute_IsNull() = async {
        let config = ModuleConfiguration.Default
        do! check(Assert.That(config.OnBeforeExecute).IsNull())
    }

    [<Test>]
    member _.Default_OnAfterExecute_IsNull() = async {
        let config = ModuleConfiguration.Default
        do! check(Assert.That(config.OnAfterExecute).IsNull())
    }

    [<Test>]
    member _.Create_ReturnsBuilder() = async {
        let builder = ModuleConfiguration.Create()
        do! check(Assert.That(builder).IsNotNull())
        do! check(Assert.That(builder).IsTypeOf<ModuleConfigurationBuilder>())
    }

    [<Test>]
    member _.WithSkipWhen_SyncBool_SetsSkipCondition() = async {
        let config =
            ModuleConfiguration.Create()
                .WithSkipWhen(Func<bool>(fun () -> true))
                .Build()

        do! check(Assert.That(config.SkipCondition).IsNotNull())

        let context = Mock.Of<IModuleContext>()
        let! decision = config.SkipCondition.Invoke(context) |> Async.AwaitTask

        do! check(Assert.That(decision.ShouldSkip).IsTrue())
    }

    [<Test>]
    member _.WithSkipWhen_SyncBoolFalse_ReturnsDoNotSkip() = async {
        let config =
            ModuleConfiguration.Create()
                .WithSkipWhen(Func<bool>(fun () -> false))
                .Build()

        let context = Mock.Of<IModuleContext>()
        let! decision = config.SkipCondition.Invoke(context) |> Async.AwaitTask

        do! check(Assert.That(decision.ShouldSkip).IsFalse())
    }

    [<Test>]
    member _.WithSkipWhen_AsyncBool_SetsSkipCondition() = async {
        let config =
            ModuleConfiguration.Create()
                .WithSkipWhen(
                    Func<Task<bool>>(fun () ->
                        task {
                            do! Task.Delay(1)
                            return true
                        })
                )
                .Build()

        do! check(Assert.That(config.SkipCondition).IsNotNull())

        let context = Mock.Of<IModuleContext>()
        let! decision = config.SkipCondition.Invoke(context) |> Async.AwaitTask

        do! check(Assert.That(decision.ShouldSkip).IsTrue())
    }

    [<Test>]
    member _.WithSkipWhen_SyncSkipDecision_SetsSkipCondition() = async {
        let expectedDecision = SkipDecision.Skip("Test reason")

        let config =
            ModuleConfiguration.Create()
                .WithSkipWhen(Func<SkipDecision>(fun () -> expectedDecision))
                .Build()

        let context = Mock.Of<IModuleContext>()
        let! decision = config.SkipCondition.Invoke(context) |> Async.AwaitTask

        do! check(Assert.That(decision.ShouldSkip).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(decision.Reason), "Test reason"))
    }

    [<Test>]
    member _.WithSkipWhen_AsyncSkipDecision_SetsSkipCondition() = async {
        let expectedDecision = SkipDecision.Skip("Async reason")

        let config =
            ModuleConfiguration.Create()
                .WithSkipWhen(
                    Func<Task<SkipDecision>>(fun () ->
                        task {
                            do! Task.Delay(1)
                            return expectedDecision
                        })
                )
                .Build()

        let context = Mock.Of<IModuleContext>()
        let! decision = config.SkipCondition.Invoke(context) |> Async.AwaitTask

        do! check(Assert.That(decision.ShouldSkip).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(decision.Reason), "Async reason"))
    }

    [<Test>]
    member _.WithSkipWhen_WithContext_SyncBool_SetsSkipCondition() = async {
        let config =
            ModuleConfiguration.Create()
                .WithSkipWhen(Func<IModuleContext, bool>(fun ctx -> not (obj.ReferenceEquals(ctx, null))))
                .Build()

        let context = Mock.Of<IModuleContext>()
        let! decision = config.SkipCondition.Invoke(context) |> Async.AwaitTask

        do! check(Assert.That(decision.ShouldSkip).IsTrue())
    }

    [<Test>]
    member _.WithSkipWhen_WithContext_AsyncBool_SetsSkipCondition() = async {
        let config =
            ModuleConfiguration.Create()
                .WithSkipWhen(
                    Func<IModuleContext, Task<bool>>(fun ctx ->
                        task {
                            do! Task.Delay(1)
                            return not (obj.ReferenceEquals(ctx, null))
                        })
                )
                .Build()

        let context = Mock.Of<IModuleContext>()
        let! decision = config.SkipCondition.Invoke(context) |> Async.AwaitTask

        do! check(Assert.That(decision.ShouldSkip).IsTrue())
    }

    [<Test>]
    member _.WithSkipWhen_WithContext_SyncSkipDecision_SetsSkipCondition() = async {
        let config =
            ModuleConfiguration.Create()
                .WithSkipWhen(
                    Func<IModuleContext, SkipDecision>(fun ctx ->
                        if not (obj.ReferenceEquals(ctx, null)) then
                            SkipDecision.Skip("Has context")
                        else
                            SkipDecision.DoNotSkip)
                )
                .Build()

        let context = Mock.Of<IModuleContext>()
        let! decision = config.SkipCondition.Invoke(context) |> Async.AwaitTask

        do! check(Assert.That(decision.ShouldSkip).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(decision.Reason), "Has context"))
    }

    [<Test>]
    member _.WithSkipWhen_WithContext_AsyncSkipDecision_SetsSkipCondition() = async {
        let config =
            ModuleConfiguration.Create()
                .WithSkipWhen(
                    Func<IModuleContext, Task<SkipDecision>>(fun ctx ->
                        task {
                            do! Task.Delay(1)

                            return
                                if not (obj.ReferenceEquals(ctx, null)) then
                                    SkipDecision.Skip("Async context")
                                else
                                    SkipDecision.DoNotSkip
                        })
                )
                .Build()

        let context = Mock.Of<IModuleContext>()
        let! decision = config.SkipCondition.Invoke(context) |> Async.AwaitTask

        do! check(Assert.That(decision.ShouldSkip).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(decision.Reason), "Async context"))
    }

    [<Test>]
    member _.WithTimeout_SetsTimeout() = async {
        let timeout = TimeSpan.FromMinutes(5.0)

        let config =
            ModuleConfiguration.Create()
                .WithTimeout(timeout)
                .Build()

        do! check(Assert.That(config.Timeout.HasValue).IsTrue())
        do! check(Assert.That(config.Timeout.Value = timeout).IsTrue())
    }

    [<Test>]
    member _.WithRetryPolicy_Direct_SetsRetryPolicyFactory() = async {
        let policy = Policy.NoOpAsync()

        let config =
            ModuleConfiguration.Create()
                .WithRetryPolicy(policy)
                .Build()

        do! check(Assert.That(config.RetryPolicyFactory).IsNotNull())

        let context = Mock.Of<IModuleContext>()
        let result = config.RetryPolicyFactory.Invoke(context)

        do! check(Assert.That(obj.ReferenceEquals(result, policy)).IsTrue())
    }

    [<Test>]
    member _.WithRetryPolicy_Factory_SetsRetryPolicyFactory() = async {
        let policy = Policy.NoOpAsync()

        let config =
            ModuleConfiguration.Create()
                .WithRetryPolicy(Func<IModuleContext, IAsyncPolicy>(fun _ -> policy))
                .Build()

        do! check(Assert.That(config.RetryPolicyFactory).IsNotNull())

        let context = Mock.Of<IModuleContext>()
        let result = config.RetryPolicyFactory.Invoke(context)

        do! check(Assert.That(obj.ReferenceEquals(result, policy)).IsTrue())
    }

    [<Test>]
    member _.WithRetryCount_SetsRetryPolicyFactory() = async {
        let config =
            ModuleConfiguration.Create()
                .WithRetryCount(3)
                .Build()

        do! check(Assert.That(config.RetryPolicyFactory).IsNotNull())
    }

    [<Test>]
    member _.WithIgnoreFailures_Always_SetsIgnoreFailuresCondition() = async {
        let config =
            ModuleConfiguration.Create()
                .WithIgnoreFailures()
                .Build()

        do! check(Assert.That(config.IgnoreFailuresCondition).IsNotNull())

        let context = Mock.Of<IModuleContext>()
        let! result = config.IgnoreFailuresCondition.Invoke(context, Exception("test")) |> Async.AwaitTask

        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.WithIgnoreFailuresWhen_SyncCondition_SetsIgnoreFailuresCondition() = async {
        let config =
            ModuleConfiguration.Create()
                .WithIgnoreFailuresWhen(Func<IModuleContext, Exception, bool>(fun _ ex -> ex.Message = "ignore"))
                .Build()

        do! check(Assert.That(config.IgnoreFailuresCondition).IsNotNull())

        let context = Mock.Of<IModuleContext>()

        let! shouldIgnore = config.IgnoreFailuresCondition.Invoke(context, Exception("ignore")) |> Async.AwaitTask
        do! check(Assert.That(shouldIgnore).IsTrue())

        let! shouldNotIgnore = config.IgnoreFailuresCondition.Invoke(context, Exception("fail")) |> Async.AwaitTask
        do! check(Assert.That(shouldNotIgnore).IsFalse())
    }

    [<Test>]
    member _.WithIgnoreFailuresWhen_AsyncCondition_SetsIgnoreFailuresCondition() = async {
        let config =
            ModuleConfiguration.Create()
                .WithIgnoreFailuresWhen(
                    Func<IModuleContext, Exception, Task<bool>>(fun _ ex ->
                        task {
                            do! Task.Delay(1)
                            return ex.Message = "ignore"
                        })
                )
                .Build()

        let context = Mock.Of<IModuleContext>()

        let! shouldIgnore = config.IgnoreFailuresCondition.Invoke(context, Exception("ignore")) |> Async.AwaitTask
        do! check(Assert.That(shouldIgnore).IsTrue())

        let! shouldNotIgnore = config.IgnoreFailuresCondition.Invoke(context, Exception("fail")) |> Async.AwaitTask
        do! check(Assert.That(shouldNotIgnore).IsFalse())
    }

    [<Test>]
    member _.WithAlwaysRun_SetsAlwaysRun() = async {
        let config =
            ModuleConfiguration.Create()
                .WithAlwaysRun()
                .Build()

        do! check(Assert.That(config.AlwaysRun).IsTrue())
    }

    [<Test>]
    member _.WithBeforeExecute_SetsOnBeforeExecute() = async {
        let mutable executed = false

        let config =
            ModuleConfiguration.Create()
                .WithBeforeExecute(
                    Func<IModuleContext, Task>(fun _ ->
                        task {
                            do! Task.Delay(1)
                            executed <- true
                        })
                )
                .Build()

        do! check(Assert.That(config.OnBeforeExecute).IsNotNull())

        let context = Mock.Of<IModuleContext>()
        do! config.OnBeforeExecute.Invoke(context) |> Async.AwaitTask

        do! check(Assert.That(executed).IsTrue())
    }

    [<Test>]
    member _.WithAfterExecute_SetsOnAfterExecute() = async {
        let mutable executed = false

        let config =
            ModuleConfiguration.Create()
                .WithAfterExecute(
                    Func<IModuleContext, Task>(fun _ ->
                        task {
                            do! Task.Delay(1)
                            executed <- true
                        })
                )
                .Build()

        do! check(Assert.That(config.OnAfterExecute).IsNotNull())

        let context = Mock.Of<IModuleContext>()
        do! config.OnAfterExecute.Invoke(context) |> Async.AwaitTask

        do! check(Assert.That(executed).IsTrue())
    }

    [<Test>]
    member _.Builder_FluentChaining_AllMethodsChain() = async {
        let policy = Policy.NoOpAsync()
        let timeout = TimeSpan.FromMinutes(1.0)

        let config =
            ModuleConfiguration.Create()
                .WithSkipWhen(Func<bool>(fun () -> false))
                .WithTimeout(timeout)
                .WithRetryPolicy(policy)
                .WithIgnoreFailures()
                .WithAlwaysRun()
                .WithBeforeExecute(Func<IModuleContext, Task>(fun _ -> Task.CompletedTask))
                .WithAfterExecute(Func<IModuleContext, Task>(fun _ -> Task.CompletedTask))
                .Build()

        do! check(Assert.That(config.SkipCondition).IsNotNull())
        do! check(Assert.That(config.Timeout.HasValue).IsTrue())
        do! check(Assert.That(config.Timeout.Value = timeout).IsTrue())
        do! check(Assert.That(config.RetryPolicyFactory).IsNotNull())
        do! check(Assert.That(config.IgnoreFailuresCondition).IsNotNull())
        do! check(Assert.That(config.AlwaysRun).IsTrue())
        do! check(Assert.That(config.OnBeforeExecute).IsNotNull())
        do! check(Assert.That(config.OnAfterExecute).IsNotNull())
    }
