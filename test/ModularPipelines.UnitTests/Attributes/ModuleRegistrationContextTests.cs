// test/ModularPipelines.UnitTests/Attributes/ModuleRegistrationContextTests.cs
using ModularPipelines.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleRegistrationContextTests
{
    private class ModuleA : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("A");
    }

    private class ModuleB : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("B");
    }

    private class ModuleC : ModuleA;

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
    public async Task RemoveDependency_Type_RemovesOnlyTheSelectedDependency()
    {
        var registry = new ModuleDependencyRegistry();
        IModuleRegistrationContext context = CreateContext(typeof(ModuleA), dependencyRegistry: registry);
        context.AddDependency<ModuleB>();
        context.AddDependency<ModuleC>();
        registry.AddDynamicDependency(typeof(ModuleB), typeof(ModuleA));

        context.RemoveDependency(typeof(ModuleB));

        await Assert.That(registry.GetDynamicDependencies(typeof(ModuleA))).IsEquivalentTo(new[] { typeof(ModuleC) });
        await Assert.That(registry.GetDynamicDependencies(typeof(ModuleB))).Contains(typeof(ModuleA));
    }

    [Test]
    public async Task RemoveDependency_Generic_RemovesRuntimeDependency()
    {
        var registry = new ModuleDependencyRegistry();
        IModuleRegistrationContext context = CreateContext(typeof(ModuleA), dependencyRegistry: registry);
        context.AddDependency(typeof(ModuleB));

        context.RemoveDependency<ModuleB>();

        await Assert.That(registry.GetDynamicDependencies(typeof(ModuleA))).IsEmpty();
    }

    [Test]
    public async Task RemoveDependency_AbsentOrAlreadyRemovedDependency_IsHarmless()
    {
        var registry = new ModuleDependencyRegistry();
        IModuleRegistrationContext context = CreateContext(typeof(ModuleA), dependencyRegistry: registry);

        context.RemoveDependency(typeof(ModuleB));
        context.AddDependency<ModuleB>();
        context.RemoveDependency(typeof(ModuleB));
        context.RemoveDependency(typeof(ModuleB));

        await Assert.That(registry.GetDynamicDependencies(typeof(ModuleA))).IsEmpty();
    }

    [Test]
    public async Task RemoveDependency_NullType_Throws()
    {
        IModuleRegistrationContext context = CreateContext(typeof(ModuleA));

        await Assert.That(() => context.RemoveDependency(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task RemoveDependency_NonModuleType_ThrowsWithoutRemovingDependencies()
    {
        var registry = new ModuleDependencyRegistry();
        IModuleRegistrationContext context = CreateContext(typeof(ModuleA), dependencyRegistry: registry);
        context.AddDependency<ModuleB>();

        await Assert.That(() => context.RemoveDependency(typeof(string))).Throws<InvalidModuleTypeException>();
        await Assert.That(registry.GetDynamicDependencies(typeof(ModuleA))).Contains(typeof(ModuleB));
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
        var metadataRegistry = new ModuleMetadataRegistry(new ModuleAttributeEventService());
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
        return new ModuleRegistrationContext(
            moduleType,
            moduleType.GetCustomAttributes(true).OfType<Attribute>().ToList(),
            configuration,
            environment,
            registeredModules ?? new List<Type> { moduleType },
            dependencyRegistry ?? new ModuleDependencyRegistry(),
            metadataRegistry ?? new ModuleMetadataRegistry(new ModuleAttributeEventService()));
    }
}
