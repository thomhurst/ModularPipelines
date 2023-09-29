using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Terraform.Extensions;

[ExcludeFromCodeCoverage]
public static class TerraformExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterTerraformContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterTerraformContext(collection));
    }

    public static IServiceCollection RegisterTerraformContext(this IServiceCollection services)
    {
        services.TryAddScoped<ITerraform, Terraform>();

        return services;
    }

    public static ITerraform Terraform(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<ITerraform>();
}
