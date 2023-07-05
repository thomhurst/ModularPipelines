using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Powershell.Extensions;

public static class PowershellExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterPowershellContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterPowershellContext(collection));
    }
    
    public static IServiceCollection RegisterPowershellContext(this IServiceCollection services)
    {
        services.TryAddTransient<IPowershell, Powershell>();
        
        return services;
    }
    
    public static IPowershell Powershell(this IModuleContext context) => context.ServiceProvider.GetRequiredService<IPowershell>();
}