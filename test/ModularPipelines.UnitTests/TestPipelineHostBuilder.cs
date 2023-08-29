using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Host;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests;

public static class TestPipelineHostBuilder
{
    public static PipelineHostBuilder Create()
    {
        return new PipelineHostBuilder()
            .ConfigureServices((_, collection) =>
            {
                collection.Configure<PipelineOptions>(opt => opt.ShowProgressInConsole = false);
                collection.AddLogging(builder => builder.ClearProviders());
            });
    }
}