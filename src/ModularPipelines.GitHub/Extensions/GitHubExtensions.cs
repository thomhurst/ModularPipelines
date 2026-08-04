using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.GitHub.PipelineWriters;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;

namespace ModularPipelines.GitHub.Extensions;

[ExcludeFromCodeCoverage]
public static class GitHubExtensions
{
    /// <summary>
    /// Generates a distributed GitHub Actions workflow from registered module capability requirements.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="options">The workflow generation options.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder WriteDistributedWorkflow(
        this PipelineBuilder builder,
        DistributedWorkflowOptions options)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(options);

        builder.Services.AddSingleton<IBuildSystemPipelineFileWriter>(services =>
            new DistributedGitHubPipelineFileWriter(options, services.GetServices<IModule>()));

        return builder;
    }

    /// <summary>
    /// Generates a distributed GitHub Actions workflow from registered module capability requirements.
    /// </summary>
    /// <param name="registration">The current module registration.</param>
    /// <param name="options">The workflow generation options.</param>
    /// <typeparam name="TModule">The most recently registered module type.</typeparam>
    /// <returns>The pipeline builder for chaining.</returns>
    public static PipelineBuilder WriteDistributedWorkflow<TModule>(
        this ModuleRegistration<TModule> registration,
        DistributedWorkflowOptions options)
        where TModule : class, IModule
    {
        ArgumentNullException.ThrowIfNull(registration);
        return registration.Builder.WriteDistributedWorkflow(options);
    }

    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterGitHubContext(this IServiceCollection services)
    {
        services.TryAddScoped<IGitHub, GitHub>();
        services.TryAddScoped<IGitHubEnvironmentVariables, GitHubEnvironmentVariables>();
        services.TryAddSingleton<IGitHubRepositoryInfo, GitHubRepositoryInfo>();
        services.AddSingleton<IPipelineGlobalHooks, GitHubMarkdownSummaryGenerator>();
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IRunReportEnricher, GitHubRunReportEnricher>());
        services.AddGitHubHttpClient();
        return services;
    }

    public static IGitHub GitHub(this IPipelineContext context) => context.Services.Get<IGitHub>();
}
