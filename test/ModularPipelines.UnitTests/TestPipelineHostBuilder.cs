using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Host;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests;

public class TestPipelineHostBuilder
{
    public static PipelineHostBuilder Create()
    {
        return new PipelineHostBuilder()
            .ConfigureServices((context, collection) =>
            {
                collection.Configure<PipelineOptions>(opt => opt.ShowProgressInConsole = false);
                collection.AddLogging(builder => builder.ClearProviders());
            });
    }
}