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
    public static PipelineHostBuilder Create() => Create(new TestHostSettings());
    
    public static PipelineHostBuilder Create(TestHostSettings testHostSettings)
    {
        return new PipelineHostBuilder()
            .SetLogLevel(testHostSettings.LogLevel)
            .ConfigureServices((_, collection) =>
            {
                collection.AddSingleton(new ArmClient(new DefaultAzureCredential()));
                collection.Configure<PipelineOptions>(opt =>
                {
                    opt.DefaultCommandLogging = testHostSettings.CommandLogging;
                    opt.ShowProgressInConsole = false;
                    opt.PrintResults = false;
                    opt.PrintLogo = false;
                    opt.PrintDependencyChains = false;
                });
                
                if (testHostSettings.ClearLogProviders)
                {
                    collection.AddLogging(builder => builder.ClearProviders());
                }
            });
    }
}