using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Configuration;

public class ModuleConfigureTests
{
    private class TestModule : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
            => Task.FromResult<string?>("test");
    }

    private class ConfiguredModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromSeconds(60))
            .WithAlwaysRun()
            .Build();

        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
            => Task.FromResult<string?>("test");
    }

    [Test]
    public async Task Module_DefaultConfiguration_ReturnsDefault()
    {
        var module = new TestModule();
        var config = ((IModule)module).Configuration;

        await Assert.That(config).IsSameReferenceAs(ModuleConfiguration.Default);
    }

    [Test]
    public async Task Module_OverriddenConfigure_ReturnsCustomConfig()
    {
        var module = new ConfiguredModule();
        var config = ((IModule)module).Configuration;

        await Assert.That(config.Timeout).IsEqualTo(TimeSpan.FromSeconds(60));
        await Assert.That(config.AlwaysRun).IsTrue();
    }

    [Test]
    public async Task Module_Configuration_IsCached()
    {
        var module = new ConfiguredModule();
        var config1 = ((IModule)module).Configuration;
        var config2 = ((IModule)module).Configuration;

        await Assert.That(config1).IsSameReferenceAs(config2);
    }
}
