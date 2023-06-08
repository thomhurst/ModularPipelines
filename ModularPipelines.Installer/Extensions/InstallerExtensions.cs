using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Installer.Extensions;

public static class InstallerExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterInstallerContext()
    {
        ServiceContextRegistry.RegisterContext(collection => RegisterInstallerContext(collection));
    }
    
    public static IServiceCollection RegisterInstallerContext(this IServiceCollection services)
    {
        services.TryAddSingleton(typeof(IInstaller<>), typeof(Installer<>));
        return services;
    }
    
    public static IInstaller Installer(this IModuleContext context) => (IInstaller) context.ServiceProvider.GetRequiredService(typeof(IInstaller<>).MakeGenericType(context.ModuleType));

}