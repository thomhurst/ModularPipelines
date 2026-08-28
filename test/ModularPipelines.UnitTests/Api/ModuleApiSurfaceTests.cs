using ModularPipelines.Configuration;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Api;

public class ModuleApiSurfaceTests
{
    private sealed class DirectModule : IModule
    {
        public Type ResultType => typeof(string);

        public ModuleConfiguration Configuration => ModuleConfiguration.Default;
    }

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

    [Test]
    public async Task DirectIModuleImplementationsFailAtRegistrationWithGuidance()
    {
        var builder = Pipeline.CreateBuilder();

        var genericException = Assert.Throws<InvalidOperationException>(
            () => builder.AddModule<DirectModule>());
        var instanceException = Assert.Throws<InvalidOperationException>(
            () => builder.AddModule(new DirectModule()));
        var factoryException = Assert.Throws<InvalidOperationException>(
            () => builder.AddModule<DirectModule>(_ => new DirectModule()));
        var runtimeException = Assert.Throws<InvalidOperationException>(
            () => builder.AddModules(typeof(DirectModule)));
        var assemblyScanException = Assert.Throws<InvalidOperationException>(
            () => builder.AddModulesFromAssembly(typeof(DirectModule).Assembly));
        var executionException = Assert.Throws<InvalidOperationException>(
            () => new DirectModule().AsInternal());

        foreach (var exception in new[]
                 {
                     genericException,
                     instanceException,
                     factoryException,
                     runtimeException,
                     assemblyScanException,
                     executionException,
                 })
        {
            await Assert.That(exception.Message).Contains("must derive from Module<T> or SyncModule<T>");
        }
    }
}
