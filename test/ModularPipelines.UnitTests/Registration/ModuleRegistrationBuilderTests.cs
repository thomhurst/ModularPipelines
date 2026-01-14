using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Requirements;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Registration;

/// <summary>
/// Tests for the IModuleRegistrationBuilder fluent API.
/// </summary>
public class ModuleRegistrationBuilderTests
{
    private class TestModuleA : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class TestModuleB : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [Test]
    public async Task AddModule_ReturnsBuilder_WithServicesAccess()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var builder = services.AddModule<TestModuleA>();

        // Assert
        await Assert.That(builder).IsNotNull();
        await Assert.That(builder.Services).IsEqualTo(services);
    }

    [Test]
    public async Task AddModule_ChainedCalls_RegistersAllModules()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddModule<TestModuleA>()
            .AddModule<TestModuleB>();

        // Assert
        var moduleDescriptors = services.Where(sd => sd.ServiceType == typeof(ModularPipelines.Modules.IModule)).ToList();
        await Assert.That(moduleDescriptors.Count).IsEqualTo(2);
    }

    [Test]
    public async Task WithTags_DoesNotThrow()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act - should not throw and register options properly
        services.AddModule<TestModuleA>()
            .WithTags("tag1", "tag2");

        // Assert - verify module and options are registered
        var moduleDescriptors = services.Where(sd => sd.ServiceType == typeof(ModularPipelines.Modules.IModule)).ToList();
        await Assert.That(moduleDescriptors.Count).IsEqualTo(1);

        // Build provider to verify options configuration doesn't throw
        var provider = services.BuildServiceProvider();
        var options = provider.GetService<IOptions<ModularPipelines.Options.ModuleRegistrationOptions>>();
        await Assert.That(options).IsNotNull();
    }

    [Test]
    public async Task WithCategory_DoesNotThrow()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act - should not throw and register options properly
        services.AddModule<TestModuleA>()
            .WithCategory("TestCategory");

        // Assert - verify module is registered
        var moduleDescriptors = services.Where(sd => sd.ServiceType == typeof(ModularPipelines.Modules.IModule)).ToList();
        await Assert.That(moduleDescriptors.Count).IsEqualTo(1);

        // Build provider to verify options configuration doesn't throw
        var provider = services.BuildServiceProvider();
        var options = provider.GetService<IOptions<ModularPipelines.Options.ModuleRegistrationOptions>>();
        await Assert.That(options).IsNotNull();
    }

    [Test]
    public async Task WithTags_MultipleCalls_DoesNotThrow()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddModule<TestModuleA>()
            .WithTags("tag1")
            .WithTags("tag2", "tag3");

        // Assert - verify module is registered
        var moduleDescriptors = services.Where(sd => sd.ServiceType == typeof(ModularPipelines.Modules.IModule)).ToList();
        await Assert.That(moduleDescriptors.Count).IsEqualTo(1);
    }

    [Test]
    public async Task WithCategory_CanChainWithAddModule()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddModule<TestModuleA>()
            .WithCategory("Category1")
            .AddModule<TestModuleB>()
            .WithCategory("Category2");

        // Assert - verify both modules are registered
        var moduleDescriptors = services.Where(sd => sd.ServiceType == typeof(ModularPipelines.Modules.IModule)).ToList();
        await Assert.That(moduleDescriptors.Count).IsEqualTo(2);
    }

    [Test]
    public async Task Builder_CanAddRequirement()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act - should compile and not throw
        services.AddModule<TestModuleA>()
            .AddRequirement<TestRequirement>();

        // Assert
        var requirementDescriptors = services.Where(sd => sd.ServiceType == typeof(IPipelineRequirement)).ToList();
        await Assert.That(requirementDescriptors.Count).IsEqualTo(1);
    }

    [Test]
    public async Task Builder_CanConfigure()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuredValue = false;

        // Act
        services.AddModule<TestModuleA>()
            .Configure<TestOptions>(opt => configuredValue = true);

        // Build provider and trigger configuration
        var provider = services.BuildServiceProvider();
        _ = provider.GetRequiredService<IOptions<TestOptions>>().Value;

        // Assert
        await Assert.That(configuredValue).IsTrue();
    }

    private class TestRequirement : IPipelineRequirement
    {
        public Task<RequirementDecision> MustAsync(IPipelineHookContext context)
            => Task.FromResult(RequirementDecision.Passed);
    }

    private class TestOptions
    {
        public string? Value { get; set; }
    }
}
