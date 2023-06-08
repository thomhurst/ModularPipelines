using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;

namespace ModularPipelines.Helpers;

internal class ModuleContextCreator : IModuleContextCreator
{
    private readonly IServiceProvider _serviceProvider;

    public ModuleContextCreator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public IModuleContext CreateContext(Type moduleType)
    {
        var moduleContextType = typeof(IModuleContext<>).MakeGenericType(moduleType);
        return (IModuleContext) _serviceProvider.GetRequiredService(moduleContextType);
    }
}