using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Git.Extensions;

[ExcludeFromCodeCoverage]
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
        services.TryAddScoped<IGitVersioning, GitVersioning>();
        services.TryAddScoped<GitCommandRunner>();
        services.TryAddSingleton<StaticGitInformation>();
        services.TryAddSingleton<IGitCommitMapper, GitCommitMapper>();
        return services;
    }

    public static IGit Git(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<IGit>();
}