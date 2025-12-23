// test/ModularPipelines.UnitTests/Attributes/ModuleRegistrationContextTests.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleRegistrationContextTests
{
    private class ModuleA : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("A");
    }

    private class ModuleB : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("B");
    }

    [Test]
    public async Task ModuleType_ReturnsCorrectType()
    {
        var context = CreateContext(typeof(ModuleA));

        await Assert.That(context.ModuleType).IsEqualTo(typeof(ModuleA));
    }

    [Test]
    public async Task AddDependency_AddsToDependencyRegistry()
    {
        var dependencyRegistry = new ModuleDependencyRegistry();
        var context = CreateContext(typeof(ModuleA), dependencyRegistry: dependencyRegistry);

        context.AddDependency<ModuleB>();

        var dependencies = dependencyRegistry.GetDynamicDependencies(typeof(ModuleA));
        await Assert.That(dependencies).Contains(typeof(ModuleB));
    }

    [Test]
    public async Task IsModuleRegistered_RegisteredModule_ReturnsTrue()
    {
        var registeredModules = new List<Type> { typeof(ModuleA), typeof(ModuleB) };
        var context = CreateContext(typeof(ModuleA), registeredModules: registeredModules);

        await Assert.That(context.IsModuleRegistered<ModuleB>()).IsTrue();
    }

    [Test]
    public async Task IsModuleRegistered_UnregisteredModule_ReturnsFalse()
    {
        var registeredModules = new List<Type> { typeof(ModuleA) };
        var context = CreateContext(typeof(ModuleA), registeredModules: registeredModules);

        await Assert.That(context.IsModuleRegistered<ModuleB>()).IsFalse();
    }

    [Test]
    public async Task SetMetadata_GetMetadata_RoundTrips()
    {
        var metadataRegistry = new ModuleMetadataRegistry();
        var context = CreateContext(typeof(ModuleA), metadataRegistry: metadataRegistry);

        context.SetMetadata("key", "value");

        await Assert.That(context.GetMetadata<string>("key")).IsEqualTo("value");
    }

    private static ModuleRegistrationContext CreateContext(
        Type moduleType,
        IModuleDependencyRegistry? dependencyRegistry = null,
        IModuleMetadataRegistry? metadataRegistry = null,
        IReadOnlyList<Type>? registeredModules = null)
    {
        var configuration = Mock.Of<IConfiguration>();
        var environment = Mock.Of<IHostEnvironment>();
        var services = new ServiceCollection();

        return new ModuleRegistrationContext(
            moduleType,
            moduleType.GetCustomAttributes(true).OfType<Attribute>().ToList(),
            configuration,
            environment,
            registeredModules ?? new List<Type> { moduleType },
            services,
            dependencyRegistry ?? new ModuleDependencyRegistry(),
            metadataRegistry ?? new ModuleMetadataRegistry());
    }
}
