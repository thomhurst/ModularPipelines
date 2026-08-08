using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Extensions;
using ModularPipelines.Options;

namespace ModularPipelines.TestHelpers;

public static class TestPipelineBuilder
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

        builder.ConfigurePipelineOptions(options => options with
        {
            Commands = options.Commands with
            {
                Logging = testHostSettings.CommandLogging,
            },
            Console = options.Console with
            {
                ShowProgress = testHostSettings.ShowProgressInConsole,
                PrintResults = false,
                PrintLogo = false,
                PrintDependencyChains = false,
            },
            RunReport = options.RunReport with
            {
                AutoWriteInCi = false,
                HistoryRetention = 0,
            },
            ThrowOnPipelineFailure = false, // Tests handle failures explicitly
        });

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
