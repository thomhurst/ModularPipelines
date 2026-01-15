using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Plugins;

namespace ModularPipelines.UnitTests.Plugins;

[TUnit.Core.NotInParallel(nameof(PluginRegistryTests))]
public class PluginRegistryTests
{
    [Test]
    public async Task Register_AddsPluginToRegistry()
    {
        using var _ = PluginTestHelper.IsolatedRegistry();
        var plugin = new TestPlugin("TestPlugin1");

        PluginRegistry.Register(plugin);

        await Assert.That(PluginRegistry.Plugins).Contains(plugin);
    }

    [Test]
    public async Task Register_DuplicateName_ThrowsInvalidOperationException()
    {
        using var _ = PluginTestHelper.IsolatedRegistry();
        var plugin1 = new TestPlugin("DuplicateName");
        var plugin2 = new TestPlugin("DuplicateName");

        PluginRegistry.Register(plugin1);

        var exception = await Assert.That(() => PluginRegistry.Register(plugin2))
            .Throws<InvalidOperationException>();

        await Assert.That(exception.Message).Contains("already registered");
    }

    [Test]
    public async Task Register_NullPlugin_ThrowsArgumentNullException()
    {
        using var _ = PluginTestHelper.IsolatedRegistry();

        await Assert.That(() => PluginRegistry.Register(null!))
            .Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Plugins_ReturnsOrderedByPriority()
    {
        using var _ = PluginTestHelper.IsolatedRegistry();
        var lowPriority = new TestPlugin("Low", priority: 100);
        var highPriority = new TestPlugin("High", priority: -10);
        var defaultPriority = new TestPlugin("Default", priority: 0);

        // Register in random order
        PluginRegistry.Register(lowPriority);
        PluginRegistry.Register(defaultPriority);
        PluginRegistry.Register(highPriority);

        var plugins = PluginRegistry.Plugins.ToList();

        await Assert.That(plugins[0].Name).IsEqualTo("High");
        await Assert.That(plugins[1].Name).IsEqualTo("Default");
        await Assert.That(plugins[2].Name).IsEqualTo("Low");
    }

    [Test]
    public async Task Clear_RemovesAllPlugins()
    {
        using var _ = PluginTestHelper.IsolatedRegistry();
        PluginRegistry.Register(new TestPlugin("Plugin1"));
        PluginRegistry.Register(new TestPlugin("Plugin2"));

        PluginRegistry.Clear();

        await Assert.That(PluginRegistry.Plugins).IsEmpty();
    }

    [Test]
    public async Task IsolatedRegistry_RestoresOriginalPlugins()
    {
        // Register a plugin outside the isolated scope
        var originalPlugin = new TestPlugin("Original");

        // First, ensure we're starting clean for this test
        using (PluginTestHelper.IsolatedRegistry())
        {
            PluginRegistry.Register(originalPlugin);

            // Inside isolated scope, add another
            using (PluginTestHelper.IsolatedRegistry())
            {
                PluginRegistry.Register(new TestPlugin("Temporary"));

                await Assert.That(PluginRegistry.Plugins.Count).IsEqualTo(1);
                await Assert.That(PluginRegistry.Plugins[0].Name).IsEqualTo("Temporary");
            }

            // After inner scope ends, original should be restored
            await Assert.That(PluginRegistry.Plugins.Count).IsEqualTo(1);
            await Assert.That(PluginRegistry.Plugins[0].Name).IsEqualTo("Original");
        }
    }

    private class TestPlugin : IModularPipelinesPlugin
    {
        private readonly int _priority;

        public TestPlugin(string name, int priority = 0)
        {
            Name = name;
            _priority = priority;
        }

        public string Name { get; }
        public int Priority => _priority;

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void ConfigurePipeline(PipelineBuilder pipelineBuilder)
        {
        }
    }
}
