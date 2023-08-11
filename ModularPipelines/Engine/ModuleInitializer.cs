using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleInitializer : IModuleInitializer, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScope _serviceScope;

    public ModuleInitializer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScope = serviceScopeFactory.CreateScope();
        _serviceProvider = _serviceScope.ServiceProvider;
    }

    public ModuleBase Initialize(ModuleBase module)
    {
        // Each context needs to be transient
        var context = _serviceProvider.GetRequiredService<IModuleContext>();
        return module.Initialize(context);
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
