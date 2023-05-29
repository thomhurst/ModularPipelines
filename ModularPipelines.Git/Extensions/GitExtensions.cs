using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Command.Extensions;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Extensions;

public static class GitExtensions
{
    public static IServiceCollection RegisterGitContext(this IServiceCollection services)
    {
        services.RegisterCommandContext();
        services.TryAddSingleton<IGit, Git>();
        return services;
    }
    
    public static IGit Git(this IModuleContext context) => context.Get<IGit>()!;
}