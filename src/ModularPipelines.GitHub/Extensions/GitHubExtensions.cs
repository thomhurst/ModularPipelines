using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;

namespace ModularPipelines.GitHub.Extensions;

[ExcludeFromCodeCoverage]
public static class GitHubExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterGitHubContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterGitHubContext(collection));
    }

    public static IServiceCollection RegisterGitHubContext(this IServiceCollection services)
    {
        services.TryAddScoped<IGitHub, GitHub>();
        services.TryAddScoped<IGitHubEnvironmentVariables, GitHubEnvironmentVariables>();
        services.TryAddSingleton<IGitHubRepositoryInfo, GitHubRepositoryInfo>();
        services.AddPipelineGlobalHooks<GitHubMarkdownSummaryGenerator>();
        return services;
    }

    public static IGitHub GitHub(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<IGitHub>();
}