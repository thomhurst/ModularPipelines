using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Command.Extensions;
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
        ServiceContextRegistry.RegisterContext(collection => RegisterPowershellContext(collection));
    }
    
    public static IServiceCollection RegisterPowershellContext(this IServiceCollection services)
    {
        services.RegisterCommandContext();
        services.TryAddSingleton(typeof(IPowershell<>), typeof(Powershell<>));
        
        return services;
    }
    
    public static IPowershell Powershell(this IModuleContext context) => (IPowershell) context.ServiceProvider.GetRequiredService(typeof(IPowershell<>).MakeGenericType(context.ModuleType));
}