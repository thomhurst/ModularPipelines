using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Host;
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
        var result = await TestPipelineHostBuilder.Create()
            .AddModules<ModuleA, ModuleB>()
            .ExecutePipelineAsync();

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
        var result = await TestPipelineHostBuilder.Create()
            .AddModules<ModuleA, ModuleB, ModuleC>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsEqualTo(3);
    }

    [Test]
    public async Task AddModules_FourModules_RegistersAllModules()
    {
        // Arrange & Act
        var result = await TestPipelineHostBuilder.Create()
            .AddModules<ModuleA, ModuleB, ModuleC, ModuleD>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsEqualTo(4);
    }

    [Test]
    public async Task AddModules_FiveModules_RegistersAllModules()
    {
        // Arrange & Act
        var result = await TestPipelineHostBuilder.Create()
            .AddModules<ModuleA, ModuleB, ModuleC, ModuleD, ModuleE>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsEqualTo(5);
    }

    [Test]
    public async Task AddModules_SixModules_RegistersAllModules()
    {
        // Arrange & Act
        var result = await TestPipelineHostBuilder.Create()
            .AddModules<ModuleA, ModuleB, ModuleC, ModuleD, ModuleE, ModuleF>()
            .ExecutePipelineAsync();

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
        var result = await TestPipelineHostBuilder.Create()
            .AddModules<ModuleA, ModuleB>()
            .AddModule<ModuleC>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsEqualTo(3);
    }

    [Test]
    public async Task AddModules_CanChainMultipleCalls()
    {
        // Arrange & Act
        var result = await TestPipelineHostBuilder.Create()
            .AddModules<ModuleA, ModuleB>()
            .AddModules<ModuleC, ModuleD>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsEqualTo(4);
    }

    #endregion

    #region AddModulesFromAssemblyContainingType Tests

    [Test]
    public async Task AddModulesFromAssemblyContainingType_RegistersModulesFromAssembly()
    {
        // Arrange & Act
        // This should register at least TrueModule and NullModule from TestHelpers assembly
        var result = await TestPipelineHostBuilder.Create()
            .AddModulesFromAssemblyContainingType<TrueModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        // At least TrueModule and NullModule should be registered
        await Assert.That(result.Modules.Count).IsGreaterThanOrEqualTo(2);
    }

    [Test]
    public async Task AddModulesFromAssembly_RegistersModulesFromAssembly()
    {
        // Arrange
        var assembly = typeof(TrueModule).Assembly;

        // Act
        var result = await TestPipelineHostBuilder.Create()
            .AddModulesFromAssembly(assembly)
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(result.Modules.Count).IsGreaterThanOrEqualTo(2);
    }

    [Test]
    public async Task AddModulesFromAssemblyContainingType_CanChainWithAddModule()
    {
        // Arrange & Act
        var result = await TestPipelineHostBuilder.Create()
            .AddModulesFromAssemblyContainingType<TrueModule>()
            .AddModule<ModuleA>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        // At least TrueModule, NullModule, and ModuleA should be registered
        await Assert.That(result.Modules.Count).IsGreaterThanOrEqualTo(3);
    }

    #endregion
}
