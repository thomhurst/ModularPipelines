using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.Network;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning.Gateways;

public class AzureGatewayProvisioner : BaseAzureProvisioner
{
    public AzureGatewayProvisioner(ArmClient armClient) : base(armClient)
    {
    }

    public async Task<ArmOperation<ApplicationGatewayResource>> ApplicationGateway(AzureResourceIdentifier azureResourceIdentifier, ApplicationGatewayData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetApplicationGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<VirtualNetworkGatewayResource>> VirtualNetworkGateway(AzureResourceIdentifier azureResourceIdentifier, VirtualNetworkGatewayData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetVirtualNetworkGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<NatGatewayResource>> NatGateway(AzureResourceIdentifier azureResourceIdentifier, NatGatewayData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetNatGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<VpnGatewayResource>> VpnGateway(AzureResourceIdentifier azureResourceIdentifier, VpnGatewayData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetVpnGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<VirtualNetworkGatewayConnectionResource>> VpnGateway(AzureResourceIdentifier azureResourceIdentifier, VirtualNetworkGatewayConnectionData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetVirtualNetworkGatewayConnections()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<ExpressRouteGatewayResource>> GetExpressRouteGateway(AzureResourceIdentifier azureResourceIdentifier, ExpressRouteGatewayData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetExpressRouteGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<P2SVpnGatewayResource>> P2SVpnGateway(AzureResourceIdentifier azureResourceIdentifier, P2SVpnGatewayData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetP2SVpnGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<LocalNetworkGatewayResource>> LocalNetworkGateway(AzureResourceIdentifier azureResourceIdentifier, LocalNetworkGatewayData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetLocalNetworkGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }
}