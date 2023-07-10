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
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterGitContext(collection));
    }

    public static IServiceCollection RegisterGitContext(this IServiceCollection services)
    {
        services.TryAddTransient<IGit, Git>();
        services.TryAddTransient<IGitCommands, GitCommands>();
        services.TryAddTransient<IGitInformation, GitInformation>();
        services.TryAddSingleton<StaticGitInformation>();
        services.TryAddTransient<GitCommandRunner>();
        services.TryAddTransient<IGitCommitMapper, GitCommitMapper>();
        return services;
    }

    public static IGit Git(this IModuleContext context) => context.ServiceProvider.GetRequiredService<IGit>();
}
