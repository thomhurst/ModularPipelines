namespace ModularPipelines.UnitTests.FSharp.Attributes

open System
open System.Threading
open System.Threading.Tasks
open ModularPipelines.Attributes.Events
open ModularPipelines.Context
open ModularPipelines.Engine.Attributes
open ModularPipelines.Modules
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module ModuleAttributeEventServiceTests =
    type TestStartAttribute() =
        inherit Attribute()
        interface IModuleStartHandler with
            member _.ContinueOnError = false
            member _.OnModuleStartAsync(_: IModuleHookContext) = Task.CompletedTask

    type TestFailureAttribute() =
        inherit Attribute()
        interface IModuleFailureHandler with
            member _.ContinueOnError = false
            member _.OnModuleFailureAsync(_: IModuleHookContext, _: Exception) = Task.CompletedTask

    [<AttributeUsage(AttributeTargets.Class, AllowMultiple = true)>]
    type LowPriorityStartAttribute() =
        inherit Attribute()
        interface IModuleStartHandler with
            member _.ContinueOnError = false
            member _.OnModuleStartAsync(_: IModuleHookContext) = Task.CompletedTask
        interface IEventHandlerPriority with
            member _.Priority = 100

    [<AttributeUsage(AttributeTargets.Class, AllowMultiple = true)>]
    type MediumPriorityStartAttribute() =
        inherit Attribute()
        interface IModuleStartHandler with
            member _.ContinueOnError = false
            member _.OnModuleStartAsync(_: IModuleHookContext) = Task.CompletedTask
        interface IEventHandlerPriority with
            member _.Priority = 10

    [<AttributeUsage(AttributeTargets.Class, AllowMultiple = true)>]
    type HighPriorityStartAttribute() =
        inherit Attribute()
        interface IModuleStartHandler with
            member _.ContinueOnError = false
            member _.OnModuleStartAsync(_: IModuleHookContext) = Task.CompletedTask
        interface IEventHandlerPriority with
            member _.Priority = 1

    [<TestStart>]
    [<TestFailure>]
    type ModuleWithAttributes() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            Task.FromResult<string>("test")

    type ModuleWithoutAttributes() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            Task.FromResult<string>("test")

    [<LowPriorityStart>]
    [<MediumPriorityStart>]
    [<HighPriorityStart>]
    type ModuleWithPrioritizedHandlers() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            Task.FromResult<string>("test")

    [<LowPriorityStart>]
    [<TestStart>]
    [<HighPriorityStart>]
    type ModuleWithMixedPriorityHandlers() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            Task.FromResult<string>("test")

type ModuleAttributeEventServiceTests() =
    [<Test>]
    member _.GetStartHandlers_ModuleWithAttribute_ReturnsHandler() = async {
        let service = ModuleAttributeEventService()
        let handlers = service.GetStartHandlers(typeof<ModuleAttributeEventServiceTests.ModuleWithAttributes>)
        do! check(Assert.That(handlers.Count = 1).IsTrue())
        do! check(Assert.That(handlers.[0]).IsTypeOf<ModuleAttributeEventServiceTests.TestStartAttribute>())
    }

    [<Test>]
    member _.GetFailureHandlers_ModuleWithAttribute_ReturnsHandler() = async {
        let service = ModuleAttributeEventService()
        let handlers = service.GetFailureHandlers(typeof<ModuleAttributeEventServiceTests.ModuleWithAttributes>)
        do! check(Assert.That(handlers.Count = 1).IsTrue())
        do! check(Assert.That(handlers.[0]).IsTypeOf<ModuleAttributeEventServiceTests.TestFailureAttribute>())
    }

    [<Test>]
    member _.GetStartHandlers_ModuleWithoutAttributes_ReturnsEmpty() = async {
        let service = ModuleAttributeEventService()
        let handlers = service.GetStartHandlers(typeof<ModuleAttributeEventServiceTests.ModuleWithoutAttributes>)
        do! check(Assert.That(handlers.Count = 0).IsTrue())
    }

    [<Test>]
    member _.GetHandlers_CachesResults() = async {
        let service = ModuleAttributeEventService()
        let handlers1 = service.GetStartHandlers(typeof<ModuleAttributeEventServiceTests.ModuleWithAttributes>)
        let handlers2 = service.GetStartHandlers(typeof<ModuleAttributeEventServiceTests.ModuleWithAttributes>)
        do! check(Assert.That(System.Object.ReferenceEquals(handlers1, handlers2)).IsTrue())
    }

    [<Test>]
    member _.GetStartHandlers_WithPriority_ReturnsSortedByPriority() = async {
        let service = ModuleAttributeEventService()
        let handlers = service.GetStartHandlers(typeof<ModuleAttributeEventServiceTests.ModuleWithPrioritizedHandlers>)
        do! check(Assert.That(handlers.Count = 3).IsTrue())
        do! check(Assert.That(handlers.[0]).IsTypeOf<ModuleAttributeEventServiceTests.HighPriorityStartAttribute>())
        do! check(Assert.That(handlers.[1]).IsTypeOf<ModuleAttributeEventServiceTests.MediumPriorityStartAttribute>())
        do! check(Assert.That(handlers.[2]).IsTypeOf<ModuleAttributeEventServiceTests.LowPriorityStartAttribute>())
    }

    [<Test>]
    member _.GetStartHandlers_WithMixedPriority_DefaultsToZero() = async {
        let service = ModuleAttributeEventService()
        let handlers = service.GetStartHandlers(typeof<ModuleAttributeEventServiceTests.ModuleWithMixedPriorityHandlers>)
        do! check(Assert.That(handlers.Count = 3).IsTrue())
        do! check(Assert.That(handlers.[0]).IsTypeOf<ModuleAttributeEventServiceTests.TestStartAttribute>())
        do! check(Assert.That(handlers.[1]).IsTypeOf<ModuleAttributeEventServiceTests.HighPriorityStartAttribute>())
        do! check(Assert.That(handlers.[2]).IsTypeOf<ModuleAttributeEventServiceTests.LowPriorityStartAttribute>())
    }

    [<Test>]
    member _.GetStartHandlers_SingleHandler_ReturnsWithoutSorting() = async {
        let service = ModuleAttributeEventService()
        let handlers = service.GetStartHandlers(typeof<ModuleAttributeEventServiceTests.ModuleWithAttributes>)
        do! check(Assert.That(handlers.Count = 1).IsTrue())
        do! check(Assert.That(handlers.[0]).IsTypeOf<ModuleAttributeEventServiceTests.TestStartAttribute>())
    }
