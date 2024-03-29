using Azure.Identity;
using Azure.ResourceManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;
using ModularPipelines.Host;
using ModularPipelines.Options;

namespace ModularPipelines.TestHelpers;

public static class TestPipelineHostBuilder
{
    public static PipelineHostBuilder Create()
    {
        return new PipelineHostBuilder()
            .SetLogLevel(LogLevel.Warning)
            .ConfigureServices((_, collection) =>
            {
                collection.AddSingleton(new ArmClient(new DefaultAzureCredential()));
                collection.Configure<PipelineOptions>(opt =>
                {
                    opt.DefaultCommandLogging = CommandLogging.Input | CommandLogging.Error;
                    opt.ShowProgressInConsole = false;
                    opt.PrintResults = false;
                });
                collection.AddLogging(builder => builder.ClearProviders());
            });
    }
}