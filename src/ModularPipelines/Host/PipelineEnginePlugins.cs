using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Engine;

namespace ModularPipelines.Host;

public class PipelineEnginePlugins
{
    private readonly IHostBuilder _internalHost;

    internal PipelineEnginePlugins(IHostBuilder internalHost)
    {
        _internalHost = internalHost;
    }

    public PipelineEnginePlugins SetModuleEstimatedTimeProvider<T>() where T : class, IModuleEstimatedTimeProvider
    {
        return OverrideGeneric<IModuleEstimatedTimeProvider, T>();
    }

    public PipelineEnginePlugins SetResultsRepository<T>() where T : class, IModuleResultRepository
    {
        return OverrideGeneric<IModuleResultRepository, T>();
    }

    private PipelineEnginePlugins OverrideGeneric<TBase, T>() where T : class, TBase where TBase : class
    {
        _internalHost.ConfigureServices(s =>
        {
            s.RemoveAll<TBase>()
                .AddSingleton<TBase, T>();
        });

        return this;
    }
}
