using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests;

public class TestBase
{
    public async Task<T> RunModule<T>() where T : ModuleBase
    {
        var results = await PipelineHostBuilder.Create()
            .ConfigureServices((context, collection) => collection.Configure<PipelineOptions>(opt => opt.ShowProgressInConsole = false))
            .ConfigureServices((context, collection) =>
            {
                collection.AddModule<T>();
            })
            .ExecutePipelineAsync();

        return results.OfType<T>().Single();
    }
}
