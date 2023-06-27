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
        ServiceContextRegistry.RegisterContext(collection => RegisterAzurePipelineContext(collection));
    }
    
    public static IServiceCollection RegisterAzurePipelineContext(this IServiceCollection services)
    {
        services.TryAddSingleton<IAzurePipeline, AzurePipeline>();
        services.TryAddSingleton<AzurePipelineVariables>();
        services.TryAddSingleton<AzurePipelineAgentVariables>();
        return services;
    }

    public static IAzurePipeline AzurePipeline(this IModuleContext context) => (IAzurePipeline) context.ServiceProvider.GetRequiredService<IAzurePipeline>();
}