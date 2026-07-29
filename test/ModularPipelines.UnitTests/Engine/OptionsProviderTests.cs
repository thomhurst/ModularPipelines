using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Engine;

namespace ModularPipelines.UnitTests.Engine;

public class OptionsProviderTests
{
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

    private sealed class TestOptions
    {
        public string? Value { get; set; }
    }
}
