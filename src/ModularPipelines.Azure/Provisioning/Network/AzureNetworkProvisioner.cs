using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.Network;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning.Network;

public class AzureNetworkProvisioner : BaseAzureProvisioner
{
    public AzureNetworkProvisioner(ArmClient armClient) : base(armClient)
    {
    }

    public async Task<ArmOperation<VirtualNetworkResource>> VirtualNetworkAsync(AzureResourceIdentifier azureResourceIdentifier, VirtualNetworkData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetVirtualNetworks()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<SubnetResource>> SubnetAsync(AzureResourceIdentifier azureResourceIdentifier, string subnetName, SubnetData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(subnetName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var virtualNetwork = await GetResourceGroup(azureResourceIdentifier).GetVirtualNetworkAsync(azureResourceIdentifier.ResourceName, cancellationToken: cancellationToken);

        return await virtualNetwork.Value.GetSubnets().CreateOrUpdateAsync(WaitUntil.Completed, subnetName, properties, cancellationToken);
    }

    public async Task<ArmOperation<PrivateLinkServiceResource>> PrivateLinkServiceAsync(AzureResourceIdentifier azureResourceIdentifier, string subnetName, PrivateLinkServiceData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetPrivateLinkServices()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<PrivateEndpointResource>> PrivateEndpointAsync(AzureResourceIdentifier azureResourceIdentifier, string subnetName, PrivateEndpointData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetPrivateEndpoints()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<WebApplicationFirewallPolicyResource>> WebApplicationFirewallPolicyAsync(AzureResourceIdentifier azureResourceIdentifier, string subnetName, WebApplicationFirewallPolicyData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetWebApplicationFirewallPolicies()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }
}
