using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.Engine;

namespace ModularPipelines.NuGet.Extensions;

public static class NuGetExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterNuGetContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterNuGetContext(collection));
    }

    public static IServiceCollection RegisterNuGetContext(this IServiceCollection services)
    {
        services.RegisterDotNetContext();

        services.TryAddScoped<INuGet, NuGet>();

        return services;
    }

    public static INuGet NuGet(this IModuleContext context) => (INuGet) context.ServiceProvider.GetRequiredService<INuGet>();
}
