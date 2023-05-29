using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Command.Extensions;
using ModularPipelines.Context;

namespace ModularPipelines.DotNet.Extensions;

public static class DotNetExtensions
{
    public static IServiceCollection RegisterDotNetContext(this IServiceCollection services)
    {
        services.RegisterCommandContext();
        services.TryAddSingleton<IDotNet, DotNet>();
        return services;
    }
    
    public static IDotNet DotNet(this IModuleContext context) => context.Get<IDotNet>()!;
}