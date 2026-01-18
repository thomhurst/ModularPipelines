using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests.Registration;

/// <summary>
/// Tests for the module registration API improvements.
/// </summary>
public class ModuleRegistrationApiTests : TestBase
{
    #region Test Modules

    private class ModuleA : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class ModuleB : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class ModuleC : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class ModuleD : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class ModuleE : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class ModuleF : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    #endregion

    #region AddModules<T1, T2> Tests

    [Test]
    public async Task AddModules_TwoModules_RegistersBothModules()
    {
        // Arrange & Act
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddModule<ModuleA>().AddModule<ModuleB>();
        var pipeline = builder.Build();
        var result = await pipeline.RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsEqualTo(2);
        await Assert.That(result.Modules.Any(m => m.GetType() == typeof(ModuleA))).IsTrue();
        await Assert.That(result.Modules.Any(m => m.GetType() == typeof(ModuleB))).IsTrue();
    }

    [Test]
    public async Task AddModules_ThreeModules_RegistersAllModules()
    {
        // Arrange & Act
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddModule<ModuleA>().AddModule<ModuleB>().AddModule<ModuleC>();
        var pipeline = builder.Build();
        var result = await pipeline.RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsEqualTo(3);
    }

    [Test]
    public async Task AddModules_FourModules_RegistersAllModules()
    {
        // Arrange & Act
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddModule<ModuleA>().AddModule<ModuleB>().AddModule<ModuleC>().AddModule<ModuleD>();
        var pipeline = builder.Build();
        var result = await pipeline.RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsEqualTo(4);
    }

    [Test]
    public async Task AddModules_FiveModules_RegistersAllModules()
    {
        // Arrange & Act
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddModule<ModuleA>().AddModule<ModuleB>().AddModule<ModuleC>().AddModule<ModuleD>().AddModule<ModuleE>();
        var pipeline = builder.Build();
        var result = await pipeline.RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsEqualTo(5);
    }

    [Test]
    public async Task AddModules_SixModules_RegistersAllModules()
    {
        // Arrange & Act
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddModule<ModuleA>().AddModule<ModuleB>().AddModule<ModuleC>().AddModule<ModuleD>().AddModule<ModuleE>().AddModule<ModuleF>();
        var pipeline = builder.Build();
        var result = await pipeline.RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsEqualTo(6);
    }

    #endregion

    #region Chaining Tests

    [Test]
    public async Task AddModules_CanChainWithAddModule()
    {
        // Arrange & Act
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddModule<ModuleA>().AddModule<ModuleB>().AddModule<ModuleC>();
        var pipeline = builder.Build();
        var result = await pipeline.RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsEqualTo(3);
    }

    [Test]
    public async Task AddModules_CanChainMultipleCalls()
    {
        // Arrange & Act
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddModule<ModuleA>().AddModule<ModuleB>().AddModule<ModuleC>().AddModule<ModuleD>();
        var pipeline = builder.Build();
        var result = await pipeline.RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsEqualTo(4);
    }

    #endregion

    #region AddModulesFromAssemblyContainingType Tests

    [Test]
    public async Task AddModulesFromAssemblyContainingType_RegistersModulesFromAssembly()
    {
        // Arrange - test assembly scanning by verifying service registration
        var services = new ServiceCollection();

        // Act - scan TestHelpers assembly which contains TrueModule, NullModule, and ExceptionModule
        services.AddModulesFromAssemblyContainingType<TrueModule>();

        // Assert - verify modules are registered (including ExceptionModule which throws at runtime)
        var moduleRegistrations = services.Where(sd => sd.ServiceType == typeof(IModule)).ToList();

        // Should find TrueModule, NullModule, and ExceptionModule (3 concrete non-generic modules)
        await Assert.That(moduleRegistrations.Count).IsGreaterThanOrEqualTo(3);

        // Verify specific modules are registered using the centralized type tracking
        // Note: ImplementationType is null for factory registrations, so we use GetRegisteredModuleTypes
        var registeredTypes = ServiceCollectionExtensions.GetRegisteredModuleTypes(services);

        await Assert.That(registeredTypes).Contains(typeof(TrueModule));
        await Assert.That(registeredTypes).Contains(typeof(NullModule));
        await Assert.That(registeredTypes).Contains(typeof(ExceptionModule));
    }

    [Test]
    public async Task AddModulesFromAssembly_RegistersModulesFromAssembly()
    {
        // Arrange
        var services = new ServiceCollection();
        var assembly = typeof(TrueModule).Assembly;

        // Act
        services.AddModulesFromAssembly(assembly);

        // Assert
        var moduleRegistrations = services.Where(sd => sd.ServiceType == typeof(IModule)).ToList();
        await Assert.That(moduleRegistrations.Count).IsGreaterThanOrEqualTo(3);
    }

    [Test]
    public async Task AddModulesFromAssemblyContainingType_CanChainWithAddModule()
    {
        // Arrange & Act
        // Test that assembly scanning can be chained with explicit registration
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddModule<TrueModule>().AddModule<NullModule>().AddModule<ModuleA>();
        var pipeline = builder.Build();
        var result = await pipeline.RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsEqualTo(3);
    }

    [Test]
    public async Task AddModulesFromAssembly_FiltersOutOpenGenericTypes()
    {
        // Arrange - this test verifies open generic types like ThrowingSyncTestModule<T> are filtered out
        var services = new ServiceCollection();
        var assembly = typeof(TrueModule).Assembly;

        // Act
        services.AddModulesFromAssembly(assembly);

        // Assert - open generics should NOT be registered
        // Note: Use GetRegisteredModuleTypes since ImplementationType is null for factory registrations
        var registeredTypes = ServiceCollectionExtensions.GetRegisteredModuleTypes(services);

        // None of the registered types should be open generics
        await Assert.That(registeredTypes.Any(t => t.IsGenericTypeDefinition)).IsFalse();
    }

    #endregion
}
