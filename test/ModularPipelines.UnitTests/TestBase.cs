using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.Modules;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;

namespace ModularPipelines.UnitTests;

[TestFixture, FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class TestBase
{
    private readonly List<IPipelineHost> _hosts = new();

    public async Task<T> RunModule<T>() where T : ModuleBase
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<T>()
            .BuildHostAsync();

        _hosts.Add(host);
        
        var results = await host.ExecutePipelineAsync();
        
        return results.Modules.OfType<T>().Single();
    }

    public async Task<T> GetService<T>() where T : notnull
    {
        var valueTuple = await GetService<T>(null);
        return valueTuple.T;
    }
    
    public async Task<(T T, IHost Host)> GetService<T>(Action<HostBuilderContext, IServiceCollection>? configureServices) where T : notnull
    {
        var host = await TestPipelineHostBuilder
            .Create()
            .ConfigureServices((context, collection) => configureServices?.Invoke(context, collection))
            .BuildHostAsync();
        
        _hosts.Add(host);
        
        var serviceProvider = host.Services;
        
        return (serviceProvider.GetRequiredService<T>(), host);
    }

    protected async Task Dispose(object obj)
    {
        if (obj is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync();
        }

        if (obj is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    [TearDown]
    public async Task DisposeCreatedHost()
    {
        await _hosts.ToAsyncProcessorBuilder()
            .ForEachAsync(Dispose)
            .ProcessInParallel();
    }
}
