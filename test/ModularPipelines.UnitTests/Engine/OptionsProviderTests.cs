using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
