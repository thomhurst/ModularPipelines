using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Git.Extensions;

public static class GitExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterGitContext()
    {
        ServiceContextRegistry.RegisterContext(collection => RegisterGitContext(collection));
    }
    
    public static IServiceCollection RegisterGitContext(this IServiceCollection services)
    {
        services.TryAddSingleton<IGit, Git>();
        services.TryAddSingleton<IGitOperations, GitOperations>();
        services.TryAddSingleton<IGitInformation, GitInformation>();
        services.TryAddSingleton<StaticGitInformation>();
        services.TryAddSingleton<GitCommandRunner>();
        services.TryAddSingleton<IGitCommitMapper, GitCommitMapper>();
        return services;
    }

    public static IGit Git(this IModuleContext context) => context.ServiceProvider.GetRequiredService<IGit>();
}