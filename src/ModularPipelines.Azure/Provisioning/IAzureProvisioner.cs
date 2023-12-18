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

public interface IAzureProvisioner
{
    Task<ArmOperation<ResourceGroupResource>> ResourceGroup(AzureSubscriptionIdentifier azureSubscriptionIdentifier, string resourceGroupName, ResourceGroupData properties);

    Task<ArmOperation<KeyVaultResource>> KeyVault(AzureResourceIdentifier azureResourceIdentifier, KeyVaultCreateOrUpdateContent properties);

    Task<ArmOperation<RedisResource>> Redis(AzureResourceIdentifier azureResourceIdentifier, RedisCreateOrUpdateContent properties);

    Task<ArmOperation<ContainerRegistryResource>> ContainerRegistry(AzureResourceIdentifier azureResourceIdentifier, ContainerRegistryData properties);

    Task<ArmOperation<AppConfigurationStoreResource>> AppConfiguration(AzureResourceIdentifier azureResourceIdentifier, AppConfigurationStoreData properties);

    Task<ArmOperation<OperationalInsightsWorkspaceResource>> OperationalInsightsWorkspace(AzureResourceIdentifier azureResourceIdentifier, OperationalInsightsWorkspaceData properties);

    Task<ArmOperation<OperationalInsightsClusterResource>> OperationalInsightsCluster(AzureResourceIdentifier azureResourceIdentifier, OperationalInsightsClusterData properties);

    Task<ArmOperation<ApplicationInsightsComponentResource>> ApplicationInsights(AzureResourceIdentifier azureResourceIdentifier, ApplicationInsightsComponentData properties);

    AzureComputeProvisioner Compute { get; }

    AzureTrafficAndLoadBalancerProvisioner TrafficAndLoadBalancers { get; }

    AzureKubernetesProvisioner Kubernetes { get; }

    AzureSecurityProvisioner Security { get; }

    AzureServiceBusProvisioner ServiceBus { get; }

    AzureCosmosProvisioner Cosmos { get; }

    AzureNetworkProvisioner Network { get; }

    AzureStorageProvisioner Storage { get; }

    AzureGatewayProvisioner Gateways { get; }

    ArmClient ArmClient { get; }

    ResourceGroupResource GetResourceGroup(AzureResourceIdentifier resourceIdentifier);
}