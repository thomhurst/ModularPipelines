using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Git.Options;
using ModularPipelines.Reporting;

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
        services.AddOptions<GitVersioningOptions>();
        services.TryAddScoped<IGit, Git>();
        services.TryAddScoped<IGitCommands, GitCommands>();
        services.TryAddSingleton<IGitChanges, GitChanges>();
        services.TryAddSingleton<IGitInformation, GitInformation>();
        services.TryAddScoped<IGitVersioning, GitVersioning>();
        services.TryAddScoped<IGitCommandRunner, GitCommandRunner>();
        services.TryAddSingleton<IGitCommitMapper, GitCommitMapper>();
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IRunReportEnricher, GitRunReportEnricher>());
        return services;
    }

    /// <summary>
    /// Registers Git services and configures the GitVersion.Tool package used for version calculation.
    /// </summary>
    /// <param name="services">The service collection to add the Git services to.</param>
    /// <param name="configureVersioning">Configures version calculation, including the tool package version.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection RegisterGitContext(
        this IServiceCollection services,
        Action<GitVersioningOptions> configureVersioning)
    {
        services.Configure(configureVersioning);
        return services.RegisterGitContext();
    }
}
