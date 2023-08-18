using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Azure.Pipelines.Extensions;

public static class AzurePipelineExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterAzurePipelineContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterAzurePipelineContext(collection));
    }

    public static IServiceCollection RegisterAzurePipelineContext(this IServiceCollection services)
    {
        services.TryAddScoped<IAzurePipeline, AzurePipeline>();
        services.TryAddScoped<AzurePipelineVariables>();
        services.TryAddScoped<AzurePipelineAgentVariables>();
        return services;
    }

    public static IAzurePipeline AzurePipeline(this IModuleContext context) => (IAzurePipeline) context.ServiceProvider.GetRequiredService<IAzurePipeline>();
}
