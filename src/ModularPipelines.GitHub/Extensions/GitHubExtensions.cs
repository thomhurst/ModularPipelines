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

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]

    [global::System.Obsolete("Use context.Tools.Get<global::ModularPipelines.GitHub.IGitHub>().")]

    public static IGitHub GitHub(this IPipelineContext context) => context.Services.GetRequiredService<IGitHub>();
}
