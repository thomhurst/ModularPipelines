using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleDependencyRegistryTests
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

    private class ModuleC : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("C");
    }

    [Test]
    public async Task AddDynamicDependency_AddsDependency()
    {
        var registry = new ModuleDependencyRegistry();

        registry.AddDynamicDependency(typeof(ModuleA), typeof(ModuleB));

        var dependencies = registry.GetDynamicDependencies(typeof(ModuleA));
        await Assert.That(dependencies).Contains(typeof(ModuleB));
    }

    [Test]
    public async Task AddDynamicDependency_MultipleDependencies_AddsAll()
    {
        var registry = new ModuleDependencyRegistry();

        registry.AddDynamicDependency(typeof(ModuleA), typeof(ModuleB));
        registry.AddDynamicDependency(typeof(ModuleA), typeof(ModuleC));

        var dependencies = registry.GetDynamicDependencies(typeof(ModuleA));
        await Assert.That(dependencies.Count()).IsEqualTo(2);
        await Assert.That(dependencies).Contains(typeof(ModuleB));
        await Assert.That(dependencies).Contains(typeof(ModuleC));
    }

    [Test]
    public async Task RemoveDependency_RemovesDependency()
    {
        var registry = new ModuleDependencyRegistry();

        registry.AddDynamicDependency(typeof(ModuleA), typeof(ModuleB));
        registry.RemoveDependency(typeof(ModuleA), typeof(ModuleB));

        var dependencies = registry.GetDynamicDependencies(typeof(ModuleA));
        await Assert.That(dependencies).IsEmpty();
    }

    [Test]
    public async Task GetDynamicDependencies_NoDependencies_ReturnsEmpty()
    {
        var registry = new ModuleDependencyRegistry();

        var dependencies = registry.GetDynamicDependencies(typeof(ModuleA));
        await Assert.That(dependencies).IsEmpty();
    }
}
