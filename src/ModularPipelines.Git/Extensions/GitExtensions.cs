using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Git.Extensions;

/// <summary>
/// Provides extension methods for integrating Git functionality into the ModularPipelines framework.
/// </summary>
[ExcludeFromCodeCoverage]
public static class GitExtensions
{
    /// <summary>
    /// Automatically registers the Git context services with the ModularPipelines framework.
    /// This method is called by the module initializer and should not be called directly.
    /// </summary>
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterGitContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterGitContext(collection));
    }

    /// <summary>
    /// Registers Git services with the dependency injection container.
    /// This includes services for running Git commands and accessing repository information.
    /// </summary>
    /// <param name="services">The service collection to add the Git services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection RegisterGitContext(this IServiceCollection services)
    {
        services.TryAddScoped<IGit, Git>();
        services.TryAddScoped<IGitCommands, GitCommands>();
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