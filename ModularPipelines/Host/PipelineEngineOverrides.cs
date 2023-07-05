using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Engine;

namespace ModularPipelines.Host;

public class PipelineEngineOverrides
{
    private readonly IHostBuilder _internalHost;

    internal PipelineEngineOverrides(IHostBuilder internalHost)
    {
        _internalHost = internalHost;
    }

    public PipelineEngineOverrides OverrideModuleEstimatedTimeProvider<T>() where T : class, IModuleEstimatedTimeProvider
    {
        return OverrideGeneric<IModuleEstimatedTimeProvider, T>();
    }
    
    public PipelineEngineOverrides OverrideResultsRepository<T>() where T : class, IModuleResultRepository
    {
        return OverrideGeneric<IModuleResultRepository, T>();
    }
    
    private PipelineEngineOverrides OverrideGeneric<TBase, T>() where T : class, TBase where TBase : class
    {
        _internalHost.ConfigureServices(s =>
        {
            s.RemoveAll<TBase>()
                .AddSingleton<TBase, T>();
        });
        
        return this;
    }
}