using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.AppConfiguration;
using Azure.ResourceManager.ApplicationInsights;
using Azure.ResourceManager.ContainerRegistry;
using Azure.ResourceManager.KeyVault;
using Azure.ResourceManager.KeyVault.Models;
using Azure.ResourceManager.OperationalInsights;
using Azure.ResourceManager.Redis;
using Azure.ResourceManager.Redis.Models;
using Azure.ResourceManager.Resources;
using ModularPipelines.Azure.Provisioning.Compute;
using ModularPipelines.Azure.Provisioning.Cosmos;
using ModularPipelines.Azure.Provisioning.Gateways;
using ModularPipelines.Azure.Provisioning.Network;
using ModularPipelines.Azure.Provisioning.PubSub;
using ModularPipelines.Azure.Provisioning.Security;
using ModularPipelines.Azure.Provisioning.Storage;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning;

internal class AzureProvisioner : BaseAzureProvisioner, IAzureProvisioner
{
    public AzureProvisioner(ArmClient armClient, AzureComputeProvisioner compute,
        AzureTrafficAndLoadBalancerProvisioner trafficAndLoadBalancers, AzureKubernetesProvisioner kubernetes,
        AzureSecurityProvisioner security, AzureServiceBusProvisioner serviceBus, AzureCosmosProvisioner cosmos,
        AzureNetworkProvisioner network, AzureStorageProvisioner storage, AzureGatewayProvisioner gateways) : base(armClient)
    {
        Compute = compute;
        TrafficAndLoadBalancers = trafficAndLoadBalancers;
        Kubernetes = kubernetes;
        Security = security;
        ServiceBus = serviceBus;
        Cosmos = cosmos;
        Network = network;
        Storage = storage;
        Gateways = gateways;
    }

    public async Task<ArmOperation<ResourceGroupResource>> ResourceGroup(AzureSubscriptionIdentifier azureSubscriptionIdentifier, string resourceGroupName, ResourceGroupData properties, CancellationToken cancellationToken = default)
    {
        var subscriptionCollection = ArmClient.GetSubscriptionResource(azureSubscriptionIdentifier.ToSubscriptionIdentifier());

        return await subscriptionCollection.GetResourceGroups()
            .CreateOrUpdateAsync(WaitUntil.Completed, resourceGroupName, properties, cancellationToken);
    }

    public async Task<ArmOperation<KeyVaultResource>> KeyVault(AzureResourceIdentifier azureResourceIdentifier, KeyVaultCreateOrUpdateContent properties, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetKeyVaults()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<RedisResource>> Redis(AzureResourceIdentifier azureResourceIdentifier, RedisCreateOrUpdateContent properties, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetAllRedis()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<ContainerRegistryResource>> ContainerRegistry(AzureResourceIdentifier azureResourceIdentifier, ContainerRegistryData properties, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetContainerRegistries()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<AppConfigurationStoreResource>> AppConfiguration(AzureResourceIdentifier azureResourceIdentifier, AppConfigurationStoreData properties, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetAppConfigurationStores()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<OperationalInsightsWorkspaceResource>> OperationalInsightsWorkspace(AzureResourceIdentifier azureResourceIdentifier, OperationalInsightsWorkspaceData properties, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetOperationalInsightsWorkspaces()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<OperationalInsightsClusterResource>> OperationalInsightsCluster(AzureResourceIdentifier azureResourceIdentifier, OperationalInsightsClusterData properties, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetOperationalInsightsClusters()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<ApplicationInsightsComponentResource>> ApplicationInsights(AzureResourceIdentifier azureResourceIdentifier, ApplicationInsightsComponentData properties, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetApplicationInsightsComponents()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public AzureComputeProvisioner Compute { get; }

    public AzureTrafficAndLoadBalancerProvisioner TrafficAndLoadBalancers { get; }

    public AzureKubernetesProvisioner Kubernetes { get; }

    public AzureSecurityProvisioner Security { get; }

    public AzureServiceBusProvisioner ServiceBus { get; }

    public AzureCosmosProvisioner Cosmos { get; }

    public AzureNetworkProvisioner Network { get; }

    public AzureStorageProvisioner Storage { get; }

    public AzureGatewayProvisioner Gateways { get; }
}