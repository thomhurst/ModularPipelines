using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Ftp.Extensions;

public static class FtpExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterFtpContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterFtpContext(collection));
    }

    public static IServiceCollection RegisterFtpContext(this IServiceCollection services)
    {
        services.TryAddScoped<IFtp, Ftp>();
        return services;
    }

    public static IFtp Ftp(this IModuleContext context) => context.ServiceProvider.GetRequiredService<IFtp>();
}
