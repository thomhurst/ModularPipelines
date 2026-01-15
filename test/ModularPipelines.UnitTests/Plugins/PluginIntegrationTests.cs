using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Exceptions;
using ModularPipelines.Plugins;

namespace ModularPipelines.UnitTests.Plugins;

[TUnit.Core.NotInParallel(nameof(PluginIntegrationTests))]
public class PluginIntegrationTests
{
    [Test]
    public async Task ApplyPluginServices_CallsConfigureServicesOnAllPlugins()
    {
        using var _ = PluginTestHelper.IsolatedRegistry();
        var plugin1 = new TrackingPlugin("Plugin1");
        var plugin2 = new TrackingPlugin("Plugin2");
        PluginRegistry.Register(plugin1);
        PluginRegistry.Register(plugin2);

        var services = new ServiceCollection();
        PluginIntegration.ApplyPluginServices(services);

        await Assert.That(plugin1.ConfigureServicesCalled).IsTrue();
        await Assert.That(plugin2.ConfigureServicesCalled).IsTrue();
    }

    [Test]
    public async Task ApplyPluginServices_ThrowsPluginInitializationException_WhenPluginFails()
    {
        using var _ = PluginTestHelper.IsolatedRegistry();
        var failingPlugin = new FailingPlugin("FailingPlugin", failOnServices: true);
        PluginRegistry.Register(failingPlugin);

        var services = new ServiceCollection();

        var exception = await Assert.That(() => PluginIntegration.ApplyPluginServices(services))
            .Throws<PluginInitializationException>();

        await Assert.That(exception.PluginName).IsEqualTo("FailingPlugin");
    }

    [Test]
    public async Task ApplyPluginConfiguration_CallsConfigurePipelineOnAllPlugins()
    {
        using var _ = PluginTestHelper.IsolatedRegistry();
        var plugin1 = new TrackingPlugin("Plugin1");
        var plugin2 = new TrackingPlugin("Plugin2");
        PluginRegistry.Register(plugin1);
        PluginRegistry.Register(plugin2);

        var builder = Pipeline.CreateBuilder();
        PluginIntegration.ApplyPluginConfiguration(builder);

        await Assert.That(plugin1.ConfigurePipelineCalled).IsTrue();
        await Assert.That(plugin2.ConfigurePipelineCalled).IsTrue();
    }

    [Test]
    public async Task ApplyPluginConfiguration_ThrowsPluginInitializationException_WhenPluginFails()
    {
        using var _ = PluginTestHelper.IsolatedRegistry();
        var failingPlugin = new FailingPlugin("FailingPlugin", failOnPipeline: true);
        PluginRegistry.Register(failingPlugin);

        var builder = Pipeline.CreateBuilder();

        var exception = await Assert.That(() => PluginIntegration.ApplyPluginConfiguration(builder))
            .Throws<PluginInitializationException>();

        await Assert.That(exception.PluginName).IsEqualTo("FailingPlugin");
    }

    [Test]
    public async Task ApplyPluginServices_AppliesInPriorityOrder()
    {
        using var _ = PluginTestHelper.IsolatedRegistry();
        var callOrder = new List<string>();

        var lowPriority = new OrderTrackingPlugin("Low", 100, callOrder);
        var highPriority = new OrderTrackingPlugin("High", -10, callOrder);
        var defaultPriority = new OrderTrackingPlugin("Default", 0, callOrder);

        // Register in random order
        PluginRegistry.Register(lowPriority);
        PluginRegistry.Register(defaultPriority);
        PluginRegistry.Register(highPriority);

        var services = new ServiceCollection();
        PluginIntegration.ApplyPluginServices(services);

        await Assert.That(callOrder).IsEquivalentTo(new[] { "High", "Default", "Low" });
    }

    [Test]
    public async Task Plugins_CanRegisterServices()
    {
        using var _ = PluginTestHelper.IsolatedRegistry();
        var plugin = new ServiceRegisteringPlugin();
        PluginRegistry.Register(plugin);

        var services = new ServiceCollection();
        PluginIntegration.ApplyPluginServices(services);

        var provider = services.BuildServiceProvider();
        var testService = provider.GetService<ITestService>();

        await Assert.That(testService).IsNotNull();
        await Assert.That(testService).IsTypeOf<TestService>();
    }

    private class TrackingPlugin : IModularPipelinesPlugin
    {
        public TrackingPlugin(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public bool ConfigureServicesCalled { get; private set; }
        public bool ConfigurePipelineCalled { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesCalled = true;
        }

        public void ConfigurePipeline(PipelineBuilder pipelineBuilder)
        {
            ConfigurePipelineCalled = true;
        }
    }

    private class FailingPlugin : IModularPipelinesPlugin
    {
        private readonly bool _failOnServices;
        private readonly bool _failOnPipeline;

        public FailingPlugin(string name, bool failOnServices = false, bool failOnPipeline = false)
        {
            Name = name;
            _failOnServices = failOnServices;
            _failOnPipeline = failOnPipeline;
        }

        public string Name { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_failOnServices)
            {
                throw new InvalidOperationException("Simulated failure in ConfigureServices");
            }
        }

        public void ConfigurePipeline(PipelineBuilder pipelineBuilder)
        {
            if (_failOnPipeline)
            {
                throw new InvalidOperationException("Simulated failure in ConfigurePipeline");
            }
        }
    }

    private class OrderTrackingPlugin : IModularPipelinesPlugin
    {
        private readonly int _priority;
        private readonly List<string> _callOrder;

        public OrderTrackingPlugin(string name, int priority, List<string> callOrder)
        {
            Name = name;
            _priority = priority;
            _callOrder = callOrder;
        }

        public string Name { get; }
        public int Priority => _priority;

        public void ConfigureServices(IServiceCollection services)
        {
            _callOrder.Add(Name);
        }

        public void ConfigurePipeline(PipelineBuilder pipelineBuilder)
        {
        }
    }

    private interface ITestService
    {
    }

    private class TestService : ITestService
    {
    }

    private class ServiceRegisteringPlugin : IModularPipelinesPlugin
    {
        public string Name => "ServiceRegistering";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITestService, TestService>();
        }

        public void ConfigurePipeline(PipelineBuilder pipelineBuilder)
        {
        }
    }
}
