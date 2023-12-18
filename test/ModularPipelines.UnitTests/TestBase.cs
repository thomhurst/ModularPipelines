using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Host;
using ModularPipelines.Modules;
using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.UnitTests.Extensions;

namespace ModularPipelines.UnitTests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[Parallelizable(ParallelScope.All)]
public abstract class TestBase
{
    private readonly List<IPipelineHost> _hosts = new();

    public async Task<T> RunModule<T>()
        where T : ModuleBase
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<T>()
            .BuildHostAsync();

        _hosts.Add(host);

        var results = await host.ExecutePipelineAsync();

        return results.Modules.OfType<T>().Single();
    }

    public async Task<(T, T2)> RunModule<T, T2>()
        where T : ModuleBase
        where T2 : ModuleBase
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<T>()
            .AddModule<T2>()
            .BuildHostAsync();

        _hosts.Add(host);

        var results = await host.ExecuteTest();

        return (
            results.GetServices<ModuleBase>().OfType<T>().Single(),
            results.GetServices<ModuleBase>().OfType<T2>().Single()
        );
    }

    public async Task<(T, T2, T3)> RunModules<T, T2, T3>()
        where T : ModuleBase
        where T2 : ModuleBase
        where T3 : ModuleBase
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<T>()
            .AddModule<T2>()
            .AddModule<T3>()
            .BuildHostAsync();

        _hosts.Add(host);

        var results = await host.ExecuteTest();

        return (
            results.GetServices<ModuleBase>().OfType<T>().Single(),
            results.GetServices<ModuleBase>().OfType<T2>().Single(),
            results.GetServices<ModuleBase>().OfType<T3>().Single()
        );
    }

    public async Task<(T, T2, T3, T4)> RunModules<T, T2, T3, T4>()
        where T : ModuleBase
        where T2 : ModuleBase
        where T3 : ModuleBase
        where T4 : ModuleBase
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<T>()
            .AddModule<T2>()
            .AddModule<T3>()
            .AddModule<T4>()
            .BuildHostAsync();

        _hosts.Add(host);

        var results = await host.ExecuteTest();

        return (
            results.GetServices<ModuleBase>().OfType<T>().Single(),
            results.GetServices<ModuleBase>().OfType<T2>().Single(),
            results.GetServices<ModuleBase>().OfType<T3>().Single(),
            results.GetServices<ModuleBase>().OfType<T4>().Single()
        );
    }

    public async Task<T> GetService<T>()
        where T : notnull
    {
        var valueTuple = await GetService<T>(null);
        return valueTuple.T;
    }

    public async Task<(T T, IPipelineHost Host)> GetService<T>(Action<HostBuilderContext, IServiceCollection>? configureServices)
        where T : notnull
    {
        var host = await TestPipelineHostBuilder
            .Create()
            .ConfigureServices((context, collection) => configureServices?.Invoke(context, collection))
            .BuildHostAsync();

        _hosts.Add(host);

        var serviceProvider = host.Services;

        return (serviceProvider.GetRequiredService<T>(), host);
    }

    [TearDown]
    public async Task DisposeCreatedHost()
    {
        await _hosts.ToAsyncProcessorBuilder()
            .ForEachAsync(Disposer.DisposeObjectAsync)
            .ProcessInParallel();

        _hosts.Clear();
    }
}
