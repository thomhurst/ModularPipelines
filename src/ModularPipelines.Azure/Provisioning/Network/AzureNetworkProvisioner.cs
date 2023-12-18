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

    public async Task<ArmOperation<VirtualNetworkResource>> VirtualNetwork(AzureResourceIdentifier azureResourceIdentifier, VirtualNetworkData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetVirtualNetworks()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<SubnetResource>> Subnet(AzureResourceIdentifier azureResourceIdentifier, string subnetName, SubnetData properties)
    {
        var virtualNetwork = await GetResourceGroup(azureResourceIdentifier).GetVirtualNetworkAsync(azureResourceIdentifier.ResourceName);

        return await virtualNetwork.Value.GetSubnets().CreateOrUpdateAsync(WaitUntil.Completed, subnetName, properties);
    }

    public async Task<ArmOperation<PrivateLinkServiceResource>> PrivateLinkService(AzureResourceIdentifier azureResourceIdentifier, string subnetName, PrivateLinkServiceData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetPrivateLinkServices()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<PrivateEndpointResource>> PrivateEndpoint(AzureResourceIdentifier azureResourceIdentifier, string subnetName, PrivateEndpointData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetPrivateEndpoints()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<WebApplicationFirewallPolicyResource>> WebApplicationFirewallPolicy(AzureResourceIdentifier azureResourceIdentifier, string subnetName, WebApplicationFirewallPolicyData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetWebApplicationFirewallPolicies()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }
}