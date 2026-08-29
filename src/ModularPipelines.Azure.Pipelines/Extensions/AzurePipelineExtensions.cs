using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Azure.Pipelines.Extensions;

[ExcludeFromCodeCoverage]
public static class AzurePipelineExtensions
{
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterAzurePipelineContext(this IServiceCollection services)
    {
        services.TryAddScoped<IAzurePipeline, AzurePipeline>();
        services.TryAddScoped<AzurePipelineVariables>();
        services.TryAddScoped<AzurePipelineAgentVariables>();
        return services;
    }

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]

    [global::System.Obsolete("Use context.Tools.Get<global::ModularPipelines.Azure.Pipelines.IAzurePipeline>().")]

    public static IAzurePipeline AzurePipeline(this IPipelineContext context) => context.Services.GetRequiredService<IAzurePipeline>();
}
