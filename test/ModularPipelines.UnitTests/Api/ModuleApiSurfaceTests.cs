using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Api;

public class ModuleApiSurfaceTests
{
    [Test]
    public async Task IModuleOnlyExposesAuthoringMetadata()
    {
        var publicProperties = typeof(IModule)
            .GetProperties()
            .Select(property => property.Name);

        await Assert.That(publicProperties)
            .IsEquivalentTo([nameof(IModule.ResultType), nameof(IModule.Configuration)]);
        await Assert.That(typeof(IModule).GetMethods().Where(method => !method.IsSpecialName))
            .IsEmpty();
    }

    [Test]
    public async Task ModuleExecutionContractIsInternal()
    {
        await Assert.That(typeof(IInternalModule).IsNotPublic).IsTrue();
        await Assert.That(typeof(IInternalModule).GetProperty(nameof(IInternalModule.ResultTask)))
            .IsNotNull();
        await Assert.That(typeof(IInternalModule).GetMethod(nameof(IInternalModule.TrySetDistributedResult)))
            .IsNotNull();
        await Assert.That(typeof(IModule).Assembly.GetType("ModularPipelines.Models.ModuleRunType"))
            .IsNull();
    }
}
