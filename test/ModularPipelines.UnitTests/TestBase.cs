using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Modules;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

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

    public async Task<T> GetService<T>() where T : notnull
    {
        var serviceProvider = TestPipelineHostBuilder.Create().BuildHost().Services;
        
        await serviceProvider.InitializeAsync();
        
        return serviceProvider.GetRequiredService<T>();
    }
}
