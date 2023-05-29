using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;

namespace ModularPipelines.Command.Extensions;

public static class CommandExtensions
{
    public static IServiceCollection RegisterCommandContext(this IServiceCollection services)
    {
        services.TryAddSingleton<ICommand, Command>();
        return services;
    }
    
    public static ICommand Command(this IModuleContext context) => context.Get<ICommand>()!;
}