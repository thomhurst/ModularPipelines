using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Extensions;

/// <summary>
/// Provides extension methods for integrating Git functionality into the ModularPipelines framework.
/// </summary>
[ExcludeFromCodeCoverage]
public static class GitExtensions
{
    /// <summary>
    /// Registers Git services with the dependency injection container.
    /// This includes services for running Git commands and accessing repository information.
    /// </summary>
    /// <param name="services">The service collection to add the Git services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterGitContext(this IServiceCollection services)
    {
        services.TryAddScoped<IGit, Git>();
        services.TryAddScoped<IGitCommands, GitCommands>();
        services.TryAddSingleton<IGitChanges, GitChanges>();
        services.TryAddSingleton<IGitInformation, GitInformation>();
        services.TryAddScoped<IGitVersioning, GitVersioning>();
        services.TryAddScoped<IGitCommandRunner, GitCommandRunner>();
        services.TryAddSingleton<IGitCommitMapper, GitCommitMapper>();
        return services;
    }

    /// <summary>
    /// Gets the Git service from the pipeline context.
    /// This provides access to Git commands and repository information.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>The <see cref="IGit"/> service for executing Git commands and accessing repository information.</returns>
    public static IGit Git(this IPipelineContext context) => context.Services.Get<IGit>();
}
