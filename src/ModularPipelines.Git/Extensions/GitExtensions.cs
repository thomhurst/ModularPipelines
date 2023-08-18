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
        services.TryAddScoped<IGit, Git>();
        services.TryAddScoped<IGitCommands, GitCommands>();
        services.TryAddScoped<IGitInformation, GitInformation>();
        services.TryAddSingleton<IGitVersioning, GitVersioning>();
        services.TryAddSingleton<StaticGitInformation>();
        services.TryAddScoped<GitCommandRunner>();
        services.TryAddScoped<IGitCommitMapper, GitCommitMapper>();
        return services;
    }

    public static IGit Git(this IModuleContext context) => context.ServiceProvider.GetRequiredService<IGit>();
}
