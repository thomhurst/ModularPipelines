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

    public async Task<ArmOperation<ApplicationGatewayResource>> ApplicationGatewayAsync(AzureResourceIdentifier azureResourceIdentifier, ApplicationGatewayData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetApplicationGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<VirtualNetworkGatewayResource>> VirtualNetworkGatewayAsync(AzureResourceIdentifier azureResourceIdentifier, VirtualNetworkGatewayData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetVirtualNetworkGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<NatGatewayResource>> NatGatewayAsync(AzureResourceIdentifier azureResourceIdentifier, NatGatewayData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetNatGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<VpnGatewayResource>> VpnGatewayAsync(AzureResourceIdentifier azureResourceIdentifier, VpnGatewayData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetVpnGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<VirtualNetworkGatewayConnectionResource>> VpnGatewayAsync(AzureResourceIdentifier azureResourceIdentifier, VirtualNetworkGatewayConnectionData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetVirtualNetworkGatewayConnections()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<ExpressRouteGatewayResource>> GetExpressRouteGatewayAsync(AzureResourceIdentifier azureResourceIdentifier, ExpressRouteGatewayData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetExpressRouteGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<P2SVpnGatewayResource>> P2SVpnGatewayAsync(AzureResourceIdentifier azureResourceIdentifier, P2SVpnGatewayData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetP2SVpnGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<LocalNetworkGatewayResource>> LocalNetworkGatewayAsync(AzureResourceIdentifier azureResourceIdentifier, LocalNetworkGatewayData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetLocalNetworkGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }
}
