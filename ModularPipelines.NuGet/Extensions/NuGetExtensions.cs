using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Command.Extensions;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;

namespace ModularPipelines.NuGet.Extensions;

public static class NuGetExtensions
{
    public static IServiceCollection RegisterNuGetContext(this IServiceCollection services)
    {
        services.RegisterCommandContext()
            .RegisterDotNetContext();

        services.TryAddSingleton<INuGet, NuGet>();
        return services;
    }
    
    public static INuGet NuGet(this IModuleContext context) => context.Get<INuGet>()!;
}