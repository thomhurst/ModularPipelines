using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Engine;

internal class ModuleContextProvider : IModuleContextProvider
{
    private readonly IServiceProvider _serviceProvider;

    public ModuleContextProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<IModuleContext> GetModuleContext()
    {
        var serviceProvider = _serviceProvider.CreateScope().ServiceProvider;
        
        await serviceProvider.InitializeAsync();
        
        return serviceProvider.GetRequiredService<IModuleContext>();
    }
}