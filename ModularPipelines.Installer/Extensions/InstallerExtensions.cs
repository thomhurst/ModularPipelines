using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Command.Extensions;
using ModularPipelines.Context;

namespace ModularPipelines.Installer.Extensions;

public static class InstallerExtensions
{
    public static IServiceCollection RegisterInstallerContext(this IServiceCollection services)
    {
        services.RegisterCommandContext();
        
        services.TryAddSingleton<IInstaller, Installer>();
        return services;
    }
    
    public static IInstaller Installer(this IModuleContext context) => context.Get<IInstaller>()!;
}