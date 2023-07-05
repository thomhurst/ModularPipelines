using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class TestBase
{
    public async Task<T> RunModule<T>() where T : ModuleBase
    {
        var results = await PipelineHostBuilder.Create()
            .ConfigureServices( ( context, collection ) =>
            {
                collection.AddModule<T>();
            } )
            .ExecutePipelineAsync();

        return results.OfType<T>().Single();
    }
}
