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
        _internalHost.ConfigureServices(s =>
        {
            s.RemoveAll<IModuleEstimatedTimeProvider>()
                .AddSingleton<IModuleEstimatedTimeProvider, T>();
        });
        
        return this;
    }
}