using Microsoft.Extensions.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
using Moq;

namespace ModularPipelines.UnitTests.Context;

/// <summary>
/// Tests for context extension methods that provide simplified access to common operations.
/// </summary>
public class ContextExtensionsTests
{
    [Test]
    public async Task GetService_ShouldResolveFromDI()
    {
        // Arrange
        var mockServices = new Mock<IServicesContext>();
        var expectedService = new TestService();
        mockServices.Setup(s => s.Get<TestService>()).Returns(expectedService);

        var mockContext = new Mock<IModuleContext>();
        mockContext.Setup(c => c.Services).Returns(mockServices.Object);

        // Act
        var result = mockContext.Object.GetService<TestService>();

        // Assert
        await Assert.That(result).IsSameReferenceAs(expectedService);
    }

    [Test]
    public async Task GetService_WhenServiceNotRegistered_ShouldThrow()
    {
        // Arrange
        var mockServices = new Mock<IServicesContext>();
        mockServices.Setup(s => s.Get<TestService>()).Returns((TestService?)null!);

        var mockContext = new Mock<IModuleContext>();
        mockContext.Setup(c => c.Services).Returns(mockServices.Object);

        // Act & Assert
        await Assert.That(() => mockContext.Object.GetService<TestService>())
            .ThrowsExactly<InvalidOperationException>();
    }

    [Test]
    public async Task TryGetService_ShouldReturnServiceOrNull()
    {
        // Arrange
        var mockServices = new Mock<IServicesContext>();
        mockServices.Setup(s => s.Get<TestService>()).Returns((TestService?)null!);

        var mockContext = new Mock<IModuleContext>();
        mockContext.Setup(c => c.Services).Returns(mockServices.Object);

        // Act
        var result = mockContext.Object.TryGetService<TestService>();

        // Assert
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task TryGetService_WhenServiceExists_ShouldReturnService()
    {
        // Arrange
        var mockServices = new Mock<IServicesContext>();
        var expectedService = new TestService();
        mockServices.Setup(s => s.Get<TestService>()).Returns(expectedService);

        var mockContext = new Mock<IModuleContext>();
        mockContext.Setup(c => c.Services).Returns(mockServices.Object);

        // Act
        var result = mockContext.Object.TryGetService<TestService>();

        // Assert
        await Assert.That(result).IsSameReferenceAs(expectedService);
    }

    [Test]
    public async Task GetConfigValue_ShouldReturnConfigurationValue()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c["TestKey"]).Returns("TestValue");

        var mockServices = new Mock<IServicesContext>();
        mockServices.Setup(s => s.Configuration).Returns(mockConfiguration.Object);

        var mockContext = new Mock<IModuleContext>();
        mockContext.Setup(c => c.Services).Returns(mockServices.Object);

        // Act
        var result = mockContext.Object.GetConfigValue("TestKey");

        // Assert
        await Assert.That(result).IsEqualTo("TestValue");
    }

    [Test]
    public async Task GetRequiredConfigValue_WhenValueExists_ShouldReturnValue()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c["TestKey"]).Returns("TestValue");

        var mockServices = new Mock<IServicesContext>();
        mockServices.Setup(s => s.Configuration).Returns(mockConfiguration.Object);

        var mockContext = new Mock<IModuleContext>();
        mockContext.Setup(c => c.Services).Returns(mockServices.Object);

        // Act
        var result = mockContext.Object.GetRequiredConfigValue("TestKey");

        // Assert
        await Assert.That(result).IsEqualTo("TestValue");
    }

    [Test]
    public async Task GetRequiredConfigValue_WhenValueMissing_ShouldThrow()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c["MissingKey"]).Returns((string?)null);

        var mockServices = new Mock<IServicesContext>();
        mockServices.Setup(s => s.Configuration).Returns(mockConfiguration.Object);

        var mockContext = new Mock<IModuleContext>();
        mockContext.Setup(c => c.Services).Returns(mockServices.Object);

        // Act & Assert
        await Assert.That(() => mockContext.Object.GetRequiredConfigValue("MissingKey"))
            .ThrowsExactly<InvalidOperationException>();
    }

    private class TestService
    {
    }
}
