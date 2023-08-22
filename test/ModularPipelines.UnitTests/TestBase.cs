using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class TestBase
{
    public async Task<T> RunModule<T>() where T : ModuleBase
    {
        var results = await TestPipelineHostBuilder.Create()
            .AddModule<T>()
            .ExecutePipelineAsync();

        return results.OfType<T>().Single();
    }
}
