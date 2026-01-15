using Azure.Identity;
using Azure.ResourceManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Options;

namespace ModularPipelines.TestHelpers;

public static class TestPipelineHostBuilder
{
    public static PipelineBuilder Create() => Create(new TestHostSettings());

    public static PipelineBuilder Create(TestHostSettings testHostSettings)
    {
        return Create(testHostSettings, null);
    }

    public static PipelineBuilder Create(TestHostSettings testHostSettings, TimeProvider? timeProvider)
    {
        var builder = Pipeline.CreateBuilder();

        builder.SetLogLevel(testHostSettings.LogLevel);

        builder.Services.AddSingleton(new ArmClient(new DefaultAzureCredential()));

        builder.Options.DefaultLoggingOptions = testHostSettings.CommandLogging;
        builder.Options.ShowProgressInConsole = testHostSettings.ShowProgressInConsole;
        builder.Options.PrintResults = false;
        builder.Options.PrintLogo = false;
        builder.Options.PrintDependencyChains = false;

        if (testHostSettings.ClearLogProviders)
        {
            builder.Services.AddLogging(b => b.ClearProviders());
        }

        // Register TimeProvider for tests
        if (timeProvider != null)
        {
            builder.Services.AddSingleton(timeProvider);
        }

        return builder;
    }

    /// <summary>
    /// Creates a FakeTimeProvider for tests that need instant time control
    /// </summary>
    public static FakeTimeProvider CreateFakeTimeProvider(DateTimeOffset? startTime = null)
    {
        return new FakeTimeProvider(startTime ?? DateTimeOffset.UtcNow);
    }
}
