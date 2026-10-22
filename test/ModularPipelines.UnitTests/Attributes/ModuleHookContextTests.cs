using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Events;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleHookContextTests
{
    [Test]
    public async Task ModuleId_DefaultIdentity_MatchesModuleType()
    {
        var context = CreateContext(new TestModule());

        await Assert.That(context.ModuleId).IsEqualTo(ModuleId.FromType(context.Module.GetType()));
        await Assert.That(context.ModuleType).IsEqualTo(typeof(TestModule));
    }

    [Test]
    public async Task ModuleId_ExplicitIdentity_UsesConfiguredValue()
    {
        var context = CreateContext(new NamedModule());

        await Assert.That(context.ModuleId).IsEqualTo(new ModuleId("hook-module"));
        await Assert.That(context.ModuleId).IsEqualTo(ModuleId.FromType(context.ModuleType));
    }

    [Test]
    public async Task ModuleId_ClosedGenericModules_HaveDistinctIdentities()
    {
        var first = CreateContext(new GenericModule<string>());
        var second = CreateContext(new GenericModule<int>());

        await Assert.That(first.ModuleId).IsEqualTo(ModuleId.FromType(typeof(GenericModule<string>)));
        await Assert.That(second.ModuleId).IsEqualTo(ModuleId.FromType(typeof(GenericModule<int>)));
        await Assert.That(first.ModuleId).IsNotEqualTo(second.ModuleId);
    }

    private static IModuleHookContext CreateContext(IModule module) =>
        new ModuleHookContext(module, [], DateTimeOffset.UtcNow, null,
            Mock.Of<IPipelineContext>(), Mock.Of<IModuleMetadataRegistry>());

    private class TestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("test");
    }

    [ModuleId("hook-module")]
    private class NamedModule : TestModule;

    private class GenericModule<T> : TestModule;
}
