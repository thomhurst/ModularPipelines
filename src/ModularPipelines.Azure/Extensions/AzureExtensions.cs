using System.Runtime.CompilerServices;
using Azure.Core;
using Azure.ResourceManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Azure.Provisioning;
using ModularPipelines.Azure.Provisioning.Compute;
using ModularPipelines.Azure.Provisioning.Containers;
using ModularPipelines.Azure.Provisioning.Cosmos;
using ModularPipelines.Azure.Provisioning.Gateways;
using ModularPipelines.Azure.Provisioning.Network;
using ModularPipelines.Azure.Provisioning.PubSub;
using ModularPipelines.Azure.Provisioning.Security;
using ModularPipelines.Azure.Provisioning.Storage;
using ModularPipelines.Azure.Services;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Azure.Extensions;

/// <summary>
/// Extension methods for Azure integration with ModularPipelines.
/// </summary>
/// <remarks>
/// Use <c>context.Azure()</c> to access Azure CLI commands from pipeline modules.
/// </remarks>
public static class AzureExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterAzureContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterAzureContext(collection));
    }

    /// <summary>
    /// Registers Azure services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to register services with.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection RegisterAzureContext(this IServiceCollection services)
    {
        // Core Azure services
        services.TryAddScoped<IAzure, Azure>();
        services.TryAddScoped<IAzureProvisioner, AzureProvisioner>();
        services.TryAddScoped<IAzureKeyVault, AzureKeyVault>();

        // Provisioning services
        services.TryAddScoped<AzureNetworkProvisioner>();
        services.TryAddScoped<AzureSecurityProvisioner>();
        services.TryAddScoped<AzureComputeProvisioner>();
        services.TryAddScoped<AzureServiceBusProvisioner>();
        services.TryAddScoped<AzureKubernetesProvisioner>();
        services.TryAddScoped<AzureCosmosProvisioner>();
        services.TryAddScoped<AzureCosmosSqlProvisioner>();
        services.TryAddScoped<AzureTrafficAndLoadBalancerProvisioner>();
        services.TryAddScoped<AzureStorageProvisioner>();
        services.TryAddScoped<AzureContainerProvisioner>();
        services.TryAddScoped<AzureGatewayProvisioner>();

        return services;
    }

    /// <summary>
    /// Adds an Azure ARM client to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="armClient">The ARM client to add.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddAzureArmClient(this IServiceCollection services, ArmClient armClient)
    {
        RegisterAzureContext(services);
        return services.AddSingleton(armClient);
    }

    /// <summary>
    /// Adds an Azure ARM client to the service collection using the specified credentials.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="tokenCredential">The token credential to use.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddAzureArmClient(this IServiceCollection services,
        TokenCredential tokenCredential)
    {
        RegisterAzureContext(services);
        return services.AddSingleton(new ArmClient(tokenCredential));
    }

    /// <summary>
    /// Gets the Azure services from the module context.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <returns>The Azure services.</returns>
    public static IAzure Azure(this IModuleContext context)
    {
        return context.ServiceProvider.GetRequiredService<IAzure>();
    }

    /// <summary>
    /// Gets the Azure services from the pipeline hook context.
    /// </summary>
    /// <param name="context">The pipeline hook context.</param>
    /// <returns>The Azure services.</returns>
    public static IAzure Azure(this IPipelineHookContext context)
    {
        return context.ServiceProvider.GetRequiredService<IAzure>();
    }
}
