using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Interfaces;

namespace ModularPipelines.GitHub.Extensions;

[ExcludeFromCodeCoverage]
public static class GitHubExtensions
{
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterGitHubContext(this IServiceCollection services)
    {
        services.TryAddScoped<IGitHub, GitHub>();
        services.TryAddScoped<IGitHubEnvironmentVariables, GitHubEnvironmentVariables>();
        services.TryAddSingleton<IGitHubRepositoryInfo, GitHubRepositoryInfo>();
        services.AddSingleton<IPipelineGlobalHooks, GitHubMarkdownSummaryGenerator>();
        services.AddGitHubHttpClient();
        return services;
    }

    public static IGitHub GitHub(this IPipelineContext context) => context.Services.Get<IGitHub>();
}
