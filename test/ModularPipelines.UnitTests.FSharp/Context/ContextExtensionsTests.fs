namespace ModularPipelines.UnitTests.FSharp.Context

open System
open Microsoft.Extensions.Configuration
open ModularPipelines.Context
open ModularPipelines.Context.Domains
open ModularPipelines.Extensions
open Moq
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type private TestService() = class end

type ContextExtensionsTests() =
    [<Test>]
    member _.GetService_ShouldResolveFromDI() = async {
        let mockServices = Mock<IServicesContext>()
        let expectedService = TestService()
        mockServices.Setup(fun services -> services.Get<TestService>()).Returns(expectedService) |> ignore

        let mockContext = Mock<IModuleContext>()
        mockContext.Setup(fun context -> context.Services).Returns(mockServices.Object) |> ignore

        let result = mockContext.Object.GetService<TestService>()
        do! check(Assert.That(Object.ReferenceEquals(result, expectedService)).IsTrue())
    }

    [<Test>]
    member _.GetService_WhenServiceNotRegistered_ShouldThrow() = async {
        let mockServices = Mock<IServicesContext>()
        mockServices.Setup(fun services -> services.Get<TestService>()).Returns(Unchecked.defaultof<TestService>) |> ignore

        let mockContext = Mock<IModuleContext>()
        mockContext.Setup(fun context -> context.Services).Returns(mockServices.Object) |> ignore

        let mutable threw = false

        try
            mockContext.Object.GetService<TestService>() |> ignore
        with :? InvalidOperationException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.TryGetService_ShouldReturnServiceOrNull() = async {
        let mockServices = Mock<IServicesContext>()
        mockServices.Setup(fun services -> services.Get<TestService>()).Returns(Unchecked.defaultof<TestService>) |> ignore

        let mockContext = Mock<IModuleContext>()
        mockContext.Setup(fun context -> context.Services).Returns(mockServices.Object) |> ignore

        let result = mockContext.Object.TryGetService<TestService>()
        do! check(Assert.That(result).IsNull())
    }

    [<Test>]
    member _.TryGetService_WhenServiceExists_ShouldReturnService() = async {
        let mockServices = Mock<IServicesContext>()
        let expectedService = TestService()
        mockServices.Setup(fun services -> services.Get<TestService>()).Returns(expectedService) |> ignore

        let mockContext = Mock<IModuleContext>()
        mockContext.Setup(fun context -> context.Services).Returns(mockServices.Object) |> ignore

        let result = mockContext.Object.TryGetService<TestService>()
        do! check(Assert.That(Object.ReferenceEquals(result, expectedService)).IsTrue())
    }

    [<Test>]
    member _.GetConfigValue_ShouldReturnConfigurationValue() = async {
        let mockConfiguration = Mock<IConfiguration>()
        mockConfiguration.Setup(fun configuration -> configuration.["TestKey"]).Returns("TestValue") |> ignore

        let mockServices = Mock<IServicesContext>()
        mockServices.Setup(fun services -> services.Configuration).Returns(mockConfiguration.Object) |> ignore

        let mockContext = Mock<IModuleContext>()
        mockContext.Setup(fun context -> context.Services).Returns(mockServices.Object) |> ignore

        let result = mockContext.Object.GetConfigValue("TestKey")
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result), "TestValue"))
    }

    [<Test>]
    member _.GetRequiredConfigValue_WhenValueExists_ShouldReturnValue() = async {
        let mockConfiguration = Mock<IConfiguration>()
        mockConfiguration.Setup(fun configuration -> configuration.["TestKey"]).Returns("TestValue") |> ignore

        let mockServices = Mock<IServicesContext>()
        mockServices.Setup(fun services -> services.Configuration).Returns(mockConfiguration.Object) |> ignore

        let mockContext = Mock<IModuleContext>()
        mockContext.Setup(fun context -> context.Services).Returns(mockServices.Object) |> ignore

        let result = mockContext.Object.GetRequiredConfigValue("TestKey")
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result), "TestValue"))
    }

    [<Test>]
    member _.GetRequiredConfigValue_WhenValueMissing_ShouldThrow() = async {
        let mockConfiguration = Mock<IConfiguration>()
        mockConfiguration.Setup(fun configuration -> configuration.["MissingKey"]).Returns(Unchecked.defaultof<string>) |> ignore

        let mockServices = Mock<IServicesContext>()
        mockServices.Setup(fun services -> services.Configuration).Returns(mockConfiguration.Object) |> ignore

        let mockContext = Mock<IModuleContext>()
        mockContext.Setup(fun context -> context.Services).Returns(mockServices.Object) |> ignore

        let mutable threw = false

        try
            mockContext.Object.GetRequiredConfigValue("MissingKey") |> ignore
        with :? InvalidOperationException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }
