using MEL.Spectre;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Plugins;

namespace ModularPipelines.UnitTests.Plugins;

public class PluginIntegrationTests
{
    [Test]
    public async Task AddPlugin_Configures_The_Builder_Immediately()
    {
        var plugin = new TrackingPlugin("Tracking");
        var builder = Pipeline.CreateBuilder();

        builder.AddPlugin(plugin);

        await Assert.That(plugin.ConfiguredBuilder).IsSameReferenceAs(builder);
    }

    [Test]
    public async Task AddPlugin_Generic_Creates_And_Applies_The_Plugin()
    {
        var builder = Pipeline.CreateBuilder()
            .AddPlugin<ServiceRegisteringPlugin>();

        await Assert.That(builder.Services.Any(descriptor => descriptor.ServiceType == typeof(ITestService)))
            .IsTrue();
    }

    [Test]
    public async Task Plugins_Apply_In_Registration_Order()
    {
        var callOrder = new List<string>();
        Pipeline.CreateBuilder()
            .AddPlugin(new OrderTrackingPlugin("Second", callOrder))
            .AddPlugin(new OrderTrackingPlugin("First", callOrder))
            .AddPlugin(new OrderTrackingPlugin("Third", callOrder));

        await Assert.That(callOrder).IsEquivalentTo(new[] { "Second", "First", "Third" });
    }

    [Test]
    public async Task Application_Configuration_After_A_Plugin_Wins()
    {
        var builder = Pipeline.CreateBuilder()
            .AddPlugin(new OptionsPlugin(FailureMode.ContinueOnFailure))
            .ConfigureOptions(options => options with { FailureMode = FailureMode.FailFast });

        await Assert.That(builder.Options.FailureMode).IsEqualTo(FailureMode.FailFast);
    }

    [Test]
    public async Task Plugin_Configuration_After_Application_Configuration_Wins()
    {
        var builder = Pipeline.CreateBuilder()
            .ConfigureOptions(options => options with { FailureMode = FailureMode.FailFast })
            .AddPlugin(new OptionsPlugin(FailureMode.ContinueOnFailure));

        await Assert.That(builder.Options.FailureMode).IsEqualTo(FailureMode.ContinueOnFailure);
    }

    [Test]
    public async Task AddPlugin_Throws_For_Duplicate_Name()
    {
        var builder = Pipeline.CreateBuilder()
            .AddPlugin(new TrackingPlugin("Duplicate"));

        await Assert.That(() => builder.AddPlugin(new TrackingPlugin("Duplicate")))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task AddPlugin_Wraps_Plugin_Failures()
    {
        var builder = Pipeline.CreateBuilder();

        var exception = await Assert.That(() => builder.AddPlugin(new FailingPlugin("FailingPlugin")))
            .Throws<PluginInitializationException>();

        using (Assert.Multiple())
        {
            await Assert.That(exception!.PluginName).IsEqualTo("FailingPlugin");
            await Assert.That(exception.InnerException).IsTypeOf<InvalidOperationException>();
        }
    }

    [Test]
    public async Task Plugins_CanRegisterServices()
    {
        var builder = Pipeline.CreateBuilder()
            .AddPlugin(new ServiceRegisteringPlugin())
            .AddModule<PluginLoggingModule>();

        await using var pipeline = await builder.BuildAsync();
        var testService = pipeline.Services.GetService<ITestService>();

        await Assert.That(testService).IsTypeOf<TestService>();
    }

    [Test]
    public async Task Plugins_Are_Not_Shared_Between_Builders()
    {
        var plugin = new ServiceRegisteringPlugin();
        Pipeline.CreateBuilder().AddPlugin(plugin);

        await using var pipeline = await Pipeline.CreateBuilder()
            .AddModule<PluginLoggingModule>()
            .BuildAsync();

        await Assert.That(pipeline.Services.GetService<ITestService>()).IsNull();
    }

    [Test]
    public async Task PluginSpectreLoggingRestoresLoggerControlAfterClearingDefaults()
    {
        var builder = Pipeline.CreateBuilder()
            .AddModule<PluginLoggingModule>();
        builder.Logging.ClearProviders();
        builder.AddPlugin(new SpectreLoggingPlugin());

        await using var pipeline = await builder.BuildAsync();
        var loggerControl = pipeline.Services
            .GetRequiredService<ISpectreConsoleLoggerControl>();

        await Assert.That(loggerControl is ModularPipelines.Console.NoopSpectreConsoleLoggerControl)
            .IsFalse();
    }

    [Test]
    public async Task PluginRemovingSpectreLoggingUsesNoopLoggerControl()
    {
        var builder = Pipeline.CreateBuilder()
            .AddPlugin(new RemoveLoggingProvidersPlugin())
            .AddModule<PluginLoggingModule>();

        await using var pipeline = await builder.BuildAsync();
        var loggerControl = pipeline.Services
            .GetRequiredService<ISpectreConsoleLoggerControl>();

        await Assert.That(loggerControl is ModularPipelines.Console.NoopSpectreConsoleLoggerControl)
            .IsTrue();
    }

    private sealed class TrackingPlugin(string name) : IModularPipelinesPlugin
    {
        public string Name { get; } = name;

        public PipelineBuilder? ConfiguredBuilder { get; private set; }

        public void Configure(PipelineBuilder builder)
        {
            ConfiguredBuilder = builder;
        }
    }

    private sealed class FailingPlugin(string name) : IModularPipelinesPlugin
    {
        public string Name { get; } = name;

        public void Configure(PipelineBuilder builder)
        {
            throw new InvalidOperationException("Simulated failure in Configure");
        }
    }

    private sealed class OrderTrackingPlugin(string name, List<string> callOrder) : IModularPipelinesPlugin
    {
        public string Name { get; } = name;

        public void Configure(PipelineBuilder builder)
        {
            callOrder.Add(Name);
        }
    }

    private sealed class OptionsPlugin(FailureMode failureMode) : IModularPipelinesPlugin
    {
        public string Name => nameof(OptionsPlugin);

        public void Configure(PipelineBuilder builder)
        {
            builder.ConfigureOptions(options => options with { FailureMode = failureMode });
        }
    }

    private interface ITestService
    {
    }

    private sealed class TestService : ITestService
    {
    }

    private sealed class ServiceRegisteringPlugin : IModularPipelinesPlugin
    {
        public string Name => "ServiceRegistering";

        public void Configure(PipelineBuilder builder)
        {
            builder.Services.AddSingleton<ITestService, TestService>();
        }
    }

    private sealed class SpectreLoggingPlugin : IModularPipelinesPlugin
    {
        public string Name => "SpectreLogging";

        public void Configure(PipelineBuilder builder)
        {
            builder.Logging.AddSpectreConsole();
        }
    }

    private sealed class RemoveLoggingProvidersPlugin : IModularPipelinesPlugin
    {
        public string Name => "RemoveLoggingProviders";

        public void Configure(PipelineBuilder builder)
        {
            builder.Logging.ClearProviders();
        }
    }

    private sealed class PluginLoggingModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }
}
