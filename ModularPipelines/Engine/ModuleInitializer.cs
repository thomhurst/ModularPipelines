using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleInitializer : IModuleInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public ModuleInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ModuleBase Initialize(ModuleBase module)
    {
        // Each context needs to be transient
        var context = _serviceProvider.GetRequiredService<IModuleContext>();
        return module.Initialize(context);
    }
}
