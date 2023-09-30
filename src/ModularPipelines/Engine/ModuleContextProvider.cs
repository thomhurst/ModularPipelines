using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using Initialization.Microsoft.Extensions.DependencyInjection.Extensions;

namespace ModularPipelines.Engine;

internal class ModuleContextProvider : IPipelineContextProvider
{
    private readonly IServiceProvider _serviceProvider;

    public ModuleContextProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IPipelineContext> GetModuleContext()
    {
        var serviceScope = _serviceProvider.CreateAsyncScope();

        await serviceScope.ServiceProvider.InitializeAsync();

        return serviceScope.ServiceProvider.GetRequiredService<IPipelineContext>();
    }
}