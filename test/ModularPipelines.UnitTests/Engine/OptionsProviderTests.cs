using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Engine;

namespace ModularPipelines.UnitTests.Engine;

public class OptionsProviderTests
{
    [Test]
    public async Task GetOptions_Reads_Registered_Names_Without_Creating_Unconfigured_Defaults()
    {
        var services = new ServiceCollection();
        services.AddOptions<TestOptions>("cache")
            .Configure(options => options.Value = "named")
            .Validate(options => options.Value is not null, "Value is required.");
        OptionsRegistration.Register<TestOptions>(services, "cache");
        using var serviceProvider = services.BuildServiceProvider();
        var provider = new OptionsProvider(new PipelineServiceContainerWrapper(services), serviceProvider);

        var options = provider.GetOptions().OfType<TestOptions>().Single();

        await Assert.That(options.Value).IsEqualTo("named");
    }

    [Test]
    public async Task GetOptions_Reads_Each_Registered_Name_Once_And_Preserves_Default_Contract()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IOptions<TestOptions>>(Microsoft.Extensions.Options.Options.Create(new TestOptions { Value = "custom-default" }));
        services.Configure<TestOptions>("cache", options => options.Value = "named");
        OptionsRegistration.Register<TestOptions>(services, "");
        OptionsRegistration.Register<TestOptions>(services, "cache");
        OptionsRegistration.Register<TestOptions>(services, "cache");
        using var serviceProvider = services.BuildServiceProvider();
        var provider = new OptionsProvider(new PipelineServiceContainerWrapper(services), serviceProvider);

        var values = provider.GetOptions().OfType<TestOptions>().Select(options => options.Value).ToArray();

        string?[] expected = ["custom-default", "named"];
        await Assert.That(values).IsEquivalentTo(expected);
    }

    [Test]
    public async Task GetOptions_Reads_Value_Through_IOptions_Contract()
    {
        var services = new ServiceCollection();
        services.Configure<TestOptions>(options => options.Value = "configured");
        using var serviceProvider = services.BuildServiceProvider();
        var provider = new OptionsProvider(
            new PipelineServiceContainerWrapper(services),
            serviceProvider);

        var options = provider.GetOptions().OfType<TestOptions>().Single();

        await Assert.That(options.Value).IsEqualTo("configured");
    }

    [Test]
    public async Task GetOptions_Preserves_Value_Getter_Exception()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IOptions<TestOptions>, ThrowingOptions>();
        using var serviceProvider = services.BuildServiceProvider();
        var provider = new OptionsProvider(
            new PipelineServiceContainerWrapper(services),
            serviceProvider);

        var exception = Assert.Throws<InvalidOperationException>(
            () => provider.GetOptions().ToList());

        await Assert.That(exception!.Message).IsEqualTo("Invalid options.");
        await Assert.That(exception.StackTrace).Contains(nameof(ThrowingOptions));
    }

    private sealed class TestOptions
    {
        public string? Value { get; set; }
    }

    private sealed class ThrowingOptions : IOptions<TestOptions>
    {
        public TestOptions Value => throw new InvalidOperationException("Invalid options.");
    }
}
