using System.Runtime.CompilerServices;
using Azure.Core;
using Azure.ResourceManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Azure.Provisioning;
using ModularPipelines.Azure.Provisioning.Compute;
using ModularPipelines.Azure.Provisioning.Cosmos;
using ModularPipelines.Azure.Provisioning.Network;
using ModularPipelines.Azure.Provisioning.Security;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Azure.Extensions;

public static class AzureExtensions
{
    public static IServiceCollection RegisterAzureContext(this IServiceCollection services)
    {
        services.TryAddScoped<IAzure, Azure>();
        services.TryAddScoped<IAzureProvisioner, AzureProvisioner>();
        services.TryAddScoped<IAzureKeyVault, AzureKeyVault>();
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
        return services;
    }

    public static IServiceCollection AddAzureArmClient(this IServiceCollection services, ArmClient armClient)
    {
        RegisterAzureContext(services);
        return services.AddSingleton(armClient);
    }

    public static IServiceCollection AddAzureArmClient(this IServiceCollection services, TokenCredential tokenCredential)
    {
        RegisterAzureContext(services);
        return services.AddSingleton(new ArmClient(tokenCredential));
    }

    public static IAzure Azure(this IPipelineContext context) => context.ServiceProvider.GetRequiredService<IAzure>();
}
