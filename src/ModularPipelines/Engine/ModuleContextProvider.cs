using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;

namespace ModularPipelines.Engine;

internal class ModuleContextProvider : IModuleContextProvider
{
    private readonly IServiceProvider _serviceProvider;

    public ModuleContextProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IPipelineContext GetModuleContext()
    {
        return _serviceProvider.GetRequiredService<IPipelineContext>();
    }
}