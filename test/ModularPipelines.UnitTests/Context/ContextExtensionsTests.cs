using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Cmd;
using ModularPipelines.Cmd.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
using ModularPipelines.Context.Domains.Environment;
using ModularPipelines.Context.Domains.Implementations;
using ModularPipelines.Enums;
using ModularPipelines.Options;
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
        using var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var servicesContext = CreateServicesContext(serviceProvider);

        var mockContext = new Mock<IModuleContext>();
        mockContext.Setup(c => c.Services).Returns(servicesContext);

        // Act & Assert
        var exception = await Assert.That(() => mockContext.Object.GetService<TestService>())
            .ThrowsExactly<InvalidOperationException>();

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Message).Contains("Register it with the pipeline service collection");
            await Assert.That(exception.Message).DoesNotContain("ModularPipelinesIntegration");
            await Assert.That(exception.Message).DoesNotContain("Native AOT");
        }
    }

    [Test]
    public async Task GetTool_WhenIntegrationNotRegistered_ShouldSuggestExplicitRegistration()
    {
        using var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var toolsContext = new ToolsContext(CreateServicesContext(serviceProvider));

        var exception = await Assert.That(() => toolsContext.Get<ICmdContext>())
            .ThrowsExactly<InvalidOperationException>();

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Message).Contains("ModularPipelines.Cmd");
            await Assert.That(exception.Message).Contains("[ModularPipelinesIntegration]");
            await Assert.That(exception.Message).Contains("Native AOT");
            await Assert.That(exception.Message).DoesNotContain("Register*Context()");
            await Assert.That(exception.Message).DoesNotContain("No service for type");
        }
    }

    [Test]
    public async Task GenericTool_FromContractsAssembly_UsesOwningIntegrationAssembly()
    {
        const string integrationAssemblyName = "Test.Tool.Integration";
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName(integrationAssemblyName),
            AssemblyBuilderAccess.Run);
        var metadataConstructor = typeof(AssemblyMetadataAttribute).GetConstructor(
            [typeof(string), typeof(string)])!;
        var genericDefinition = typeof(IGenericTool<>);
        var stringType = typeof(string);
        var typeIdentity =
            $"{genericDefinition.Assembly.GetName().Name}:{genericDefinition.FullName}" +
            $"[framework:{stringType.FullName}]";
        assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(
            metadataConstructor,
            ["ModularPipelines.ToolTypeIdentity:GenericTool", typeIdentity]));

        var integrationAssembly = ToolRegistrationExceptionFactory.FindIntegrationAssemblyName(
            typeof(IGenericTool<string>));
        var exception = ToolRegistrationExceptionFactory.Create(
            typeof(IGenericTool<string>),
            integrationAssembly);

        using (Assert.Multiple())
        {
            await Assert.That(integrationAssembly).IsEqualTo(integrationAssemblyName);
            await Assert.That(exception.Message)
                .Contains($"integration assembly '{integrationAssemblyName}'");
            await Assert.That(exception.Message)
                .DoesNotContain(" package");
        }
    }

    [Test]
    public async Task TryGetService_WhenServiceNotRegistered_ShouldReturnNull()
    {
        // Arrange
        using var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var servicesContext = CreateServicesContext(serviceProvider);

        var mockContext = new Mock<IModuleContext>();
        mockContext.Setup(c => c.Services).Returns(servicesContext);

        // Act
        var result = mockContext.Object.TryGetService<TestService>();

        // Assert
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task TryGetService_WhenServiceExists_ShouldReturnService()
    {
        // Arrange
        var expectedService = new TestService();
        using var serviceProvider = new ServiceCollection()
            .AddSingleton(expectedService)
            .BuildServiceProvider();
        var servicesContext = CreateServicesContext(serviceProvider);

        var mockContext = new Mock<IModuleContext>();
        mockContext.Setup(c => c.Services).Returns(servicesContext);

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
        mockConfiguration.Setup(c => c["MissingKey"]).Returns((string?) null);

        var mockServices = new Mock<IServicesContext>();
        mockServices.Setup(s => s.Configuration).Returns(mockConfiguration.Object);

        var mockContext = new Mock<IModuleContext>();
        mockContext.Setup(c => c.Services).Returns(mockServices.Object);

        // Act & Assert
        await Assert.That(() => mockContext.Object.GetRequiredConfigValue("MissingKey"))
            .ThrowsExactly<InvalidOperationException>();
    }

    [Test]
    [Arguments(BuildSystem.AzurePipelines)]
    [Arguments(BuildSystem.TeamCity)]
    [Arguments(BuildSystem.GitHubActions)]
    [Arguments(BuildSystem.Jenkins)]
    [Arguments(BuildSystem.GitLab)]
    [Arguments(BuildSystem.Bitbucket)]
    [Arguments(BuildSystem.TravisCI)]
    [Arguments(BuildSystem.AppVeyor)]
    [Arguments(BuildSystem.Unknown)]
    public async Task IsRunningIn_Uses_Current_Build_System(BuildSystem buildSystem)
    {
        var buildSystemContext = new Mock<IBuildSystemContext>();
        buildSystemContext.SetupGet(context => context.Current).Returns(buildSystem);
        var environmentContext = new Mock<IEnvironmentContext>();
        environmentContext
            .SetupGet(context => context.BuildSystem)
            .Returns(buildSystemContext.Object);
        var context = new Mock<IPipelineContext>();
        context.SetupGet(pipelineContext => pipelineContext.Environment).Returns(environmentContext.Object);

        await Assert.That(context.Object.IsRunningIn(buildSystem)).IsTrue();
    }

    private static ServicesContext CreateServicesContext(IServiceProvider serviceProvider)
    {
        return new ServicesContext(
            serviceProvider,
            new ConfigurationBuilder().Build(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
    }

    private class TestService
    {
    }

    private interface IGenericTool<T>
    {
    }
}
