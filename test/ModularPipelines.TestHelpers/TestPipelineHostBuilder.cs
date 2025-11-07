using Azure.Identity;
using Azure.ResourceManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Host;
using ModularPipelines.Options;

namespace ModularPipelines.TestHelpers;

public static class TestPipelineHostBuilder
{
    public static PipelineHostBuilder Create() => Create(new TestHostSettings());

    public static PipelineHostBuilder Create(TestHostSettings testHostSettings)
    {
        return Create(testHostSettings, null);
    }

    public static PipelineHostBuilder Create(TestHostSettings testHostSettings, TimeProvider? timeProvider)
    {
        return new PipelineHostBuilder()
            .SetLogLevel(testHostSettings.LogLevel)
            .ConfigureServices((_, collection) =>
            {
                collection.AddSingleton(new ArmClient(new DefaultAzureCredential()));
                collection.Configure<PipelineOptions>(opt =>
                {
                    opt.DefaultCommandLogging = testHostSettings.CommandLogging;
                    opt.ShowProgressInConsole = testHostSettings.ShowProgressInConsole;
                    opt.PrintResults = false;
                    opt.PrintLogo = false;
                    opt.PrintDependencyChains = false;
                });

                if (testHostSettings.ClearLogProviders)
                {
                    collection.AddLogging(builder => builder.ClearProviders());
                }

                // Register TimeProvider for tests
                if (timeProvider != null)
                {
                    collection.AddSingleton(timeProvider);
                }
            });
    }

    /// <summary>
    /// Creates a FakeTimeProvider for tests that need instant time control
    /// </summary>
    public static FakeTimeProvider CreateFakeTimeProvider(DateTimeOffset? startTime = null)
    {
        return new FakeTimeProvider(startTime ?? DateTimeOffset.UtcNow);
    }
}