using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class DependencyChainProviderTests
{
    [Test]
    public async Task Initialize_IndexesDependencyModelsByType()
    {
        var dependency = new DependencyModule();
        var dependent = new DependentModule();
        var provider = new DependencyChainProvider(
            Mock.Of<IModuleMetadataRegistry>(),
            new ModuleDependencyRegistry());

        provider.Initialize([dependent, dependency]);

        var dependentModel = provider.ModuleDependencyModels.Single(model => model.Module == dependent);
        var dependencyModel = provider.ModuleDependencyModels.Single(model => model.Module == dependency);
        await Assert.That(dependentModel.IsDependentOn).IsEquivalentTo([dependencyModel]);
        await Assert.That(dependencyModel.IsDependencyFor).IsEquivalentTo([dependentModel]);
    }

    [Test]
    public async Task Initialize_IncludesRegistrationTimeDependencies()
    {
        var dependency = new DependencyModule();
        var dependent = new DynamicDependentModule();
        var dependencyRegistry = new ModuleDependencyRegistry();
        dependencyRegistry.AddDynamicDependency(typeof(DynamicDependentModule), typeof(DependencyModule));
        var provider = new DependencyChainProvider(
            Mock.Of<IModuleMetadataRegistry>(),
            dependencyRegistry);

        provider.Initialize([dependent, dependency]);

        var dependentModel = provider.ModuleDependencyModels.Single(model => model.Module == dependent);
        var dependencyModel = provider.ModuleDependencyModels.Single(model => model.Module == dependency);
        await Assert.That(dependentModel.IsDependentOn).IsEquivalentTo([dependencyModel]);
        await Assert.That(dependencyModel.IsDependencyFor).IsEquivalentTo([dependentModel]);
    }

    private class DependencyModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModularPipelines.DependsOn<DependencyModule>]
    private class DependentModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class DynamicDependentModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }
}
