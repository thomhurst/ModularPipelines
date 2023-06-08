using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Cmd.Extensions;

public static class CmdExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterCmdContext()
    {
        ServiceContextRegistry.RegisterContext(collection => RegisterCmdContext(collection));
    }
    
    public static IServiceCollection RegisterCmdContext(this IServiceCollection services)
    {
        services.TryAddSingleton(typeof(ICmd<>), typeof(Cmd<>));
        
        return services;
    }
    
    public static ICmd Cmd(this IModuleContext context) => (ICmd) context.ServiceProvider.GetRequiredService(typeof(ICmd<>).MakeGenericType(context.ModuleType));
}