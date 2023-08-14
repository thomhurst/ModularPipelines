﻿using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Terraform.Extensions;

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
        services.TryAddTransient<ITerraform, Terraform>();

        return services;
    }

    public static ITerraform Terraform(this IModuleContext context) => context.ServiceProvider.GetRequiredService<ITerraform>();
}
