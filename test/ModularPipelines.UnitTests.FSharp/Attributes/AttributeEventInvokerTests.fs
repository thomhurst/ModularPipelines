namespace ModularPipelines.UnitTests.Attributes

open Microsoft.Extensions.Logging
open ModularPipelines.Attributes.Events
open ModularPipelines.Context
open ModularPipelines.Engine.Attributes
open Moq
open System
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module AttributeEventInvokerTests =
    type private SuccessfulHandler() =
        member val WasCalled = false with get, private set
        interface IModuleStartHandler with
            member _.ContinueOnError = false
            member this.OnModuleStartAsync(_: ModularPipelines.Context.IModuleHookContext): System.Threading.Tasks.Task =
                task {
                    this.WasCalled <- true
                }

    type private FailingHandler() =
        interface IModuleStartHandler with
            member _.ContinueOnError = false
            member _.OnModuleStartAsync(_: ModularPipelines.Context.IModuleHookContext): System.Threading.Tasks.Task =
                raise (InvalidOperationException("Test exception"))

    type private FailingHandlerWithContinue() =
        interface IModuleStartHandler with
            member _.ContinueOnError = true
            member _.OnModuleStartAsync(_: ModularPipelines.Context.IModuleHookContext): System.Threading.Tasks.Task =
                raise (InvalidOperationException("Test exception"))

    type AttributeEventInvokerTests() =
        [<Test>]
        member _.InvokeAsync_CallsAllHandlers() = async {
            let handler1 = SuccessfulHandler()
            let handler2 = SuccessfulHandler()
            let handlers = [ handler1 :> IModuleStartHandler; handler2 :> IModuleStartHandler ]
            let invoker = AttributeEventInvoker(Mock.Of<ILogger<AttributeEventInvoker>>())
            let context = Mock.Of<IModuleHookContext>()

            do! invoker.InvokeStartHandlersAsync(handlers, context) |> Async.AwaitTask

            do! check(Assert.That(handler1.WasCalled).IsTrue())
            do! check(Assert.That(handler2.WasCalled).IsTrue())
        }

        [<Test>]
        member _.InvokeAsync_HandlerThrows_ContinueOnErrorFalse_Propagates() = async {
            let handler = FailingHandler()
            let handlers = [ handler :> IModuleStartHandler ]
            let invoker = AttributeEventInvoker(Mock.Of<ILogger<AttributeEventInvoker>>())
            let context = Mock.Of<IModuleHookContext>()

            let mutable thrownException = None

            try
                do! invoker.InvokeStartHandlersAsync(handlers, context) |> Async.AwaitTask
            with ex ->
                thrownException <- Some ex

            do! check(Assert.That(thrownException.IsSome).IsTrue())

            match thrownException with
            | Some ex ->
                do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(ex.GetBaseException().Message), "Test exception"))
            | None -> ()
        }

        [<Test>]
        member _.InvokeAsync_HandlerThrows_ContinueOnErrorTrue_Continues() = async {
            let failingHandler = FailingHandlerWithContinue()
            let successHandler = SuccessfulHandler()
            let handlers = [ failingHandler :> IModuleStartHandler; successHandler :> IModuleStartHandler ]
            let invoker = AttributeEventInvoker(Mock.Of<ILogger<AttributeEventInvoker>>())
            let context = Mock.Of<IModuleHookContext>()

            do! invoker.InvokeStartHandlersAsync(handlers, context) |> Async.AwaitTask

            do! check(Assert.That(successHandler.WasCalled).IsTrue())
        }