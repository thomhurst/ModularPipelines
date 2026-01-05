using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsEc2
{
    public AwsEc2(
        AwsEc2Wait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsEc2Wait Wait { get; }

    public async Task<CommandResult> AcceptAddressTransfer(AwsEc2AcceptAddressTransferOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AcceptReservedInstancesExchangeQuote(AwsEc2AcceptReservedInstancesExchangeQuoteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AcceptTransitGatewayMulticastDomainAssociations(AwsEc2AcceptTransitGatewayMulticastDomainAssociationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2AcceptTransitGatewayMulticastDomainAssociationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AcceptTransitGatewayPeeringAttachment(AwsEc2AcceptTransitGatewayPeeringAttachmentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AcceptTransitGatewayVpcAttachment(AwsEc2AcceptTransitGatewayVpcAttachmentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AcceptVpcEndpointConnections(AwsEc2AcceptVpcEndpointConnectionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AcceptVpcPeeringConnection(AwsEc2AcceptVpcPeeringConnectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AdvertiseByoipCidr(AwsEc2AdvertiseByoipCidrOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AllocateAddress(AwsEc2AllocateAddressOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2AllocateAddressOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AllocateHosts(AwsEc2AllocateHostsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AllocateIpamPoolCidr(AwsEc2AllocateIpamPoolCidrOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ApplySecurityGroupsToClientVpnTargetNetwork(AwsEc2ApplySecurityGroupsToClientVpnTargetNetworkOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssignIpv6Addresses(AwsEc2AssignIpv6AddressesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssignPrivateIpAddresses(AwsEc2AssignPrivateIpAddressesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssignPrivateNatGatewayAddress(AwsEc2AssignPrivateNatGatewayAddressOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateAddress(AwsEc2AssociateAddressOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2AssociateAddressOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateClientVpnTargetNetwork(AwsEc2AssociateClientVpnTargetNetworkOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateDhcpOptions(AwsEc2AssociateDhcpOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateEnclaveCertificateIamRole(AwsEc2AssociateEnclaveCertificateIamRoleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateIamInstanceProfile(AwsEc2AssociateIamInstanceProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateInstanceEventWindow(AwsEc2AssociateInstanceEventWindowOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateIpamByoasn(AwsEc2AssociateIpamByoasnOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateIpamResourceDiscovery(AwsEc2AssociateIpamResourceDiscoveryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateNatGatewayAddress(AwsEc2AssociateNatGatewayAddressOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateRouteTable(AwsEc2AssociateRouteTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateSubnetCidrBlock(AwsEc2AssociateSubnetCidrBlockOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateTransitGatewayMulticastDomain(AwsEc2AssociateTransitGatewayMulticastDomainOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateTransitGatewayPolicyTable(AwsEc2AssociateTransitGatewayPolicyTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateTransitGatewayRouteTable(AwsEc2AssociateTransitGatewayRouteTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateTrunkInterface(AwsEc2AssociateTrunkInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateVpcCidrBlock(AwsEc2AssociateVpcCidrBlockOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AttachClassicLinkVpc(AwsEc2AttachClassicLinkVpcOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AttachInternetGateway(AwsEc2AttachInternetGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AttachNetworkInterface(AwsEc2AttachNetworkInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AttachVerifiedAccessTrustProvider(AwsEc2AttachVerifiedAccessTrustProviderOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AttachVolume(AwsEc2AttachVolumeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AttachVpnGateway(AwsEc2AttachVpnGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AuthorizeClientVpnIngress(AwsEc2AuthorizeClientVpnIngressOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AuthorizeSecurityGroupEgress(AwsEc2AuthorizeSecurityGroupEgressOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AuthorizeSecurityGroupIngress(AwsEc2AuthorizeSecurityGroupIngressOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2AuthorizeSecurityGroupIngressOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BundleInstance(AwsEc2BundleInstanceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelBundleTask(AwsEc2CancelBundleTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelCapacityReservation(AwsEc2CancelCapacityReservationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelCapacityReservationFleets(AwsEc2CancelCapacityReservationFleetsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelConversionTask(AwsEc2CancelConversionTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelExportTask(AwsEc2CancelExportTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelImageLaunchPermission(AwsEc2CancelImageLaunchPermissionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelImportTask(AwsEc2CancelImportTaskOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CancelImportTaskOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelReservedInstancesListing(AwsEc2CancelReservedInstancesListingOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelSpotFleetRequests(AwsEc2CancelSpotFleetRequestsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelSpotInstanceRequests(AwsEc2CancelSpotInstanceRequestsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ConfirmProductInstance(AwsEc2ConfirmProductInstanceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CopyFpgaImage(AwsEc2CopyFpgaImageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CopyImage(AwsEc2CopyImageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CopySnapshot(AwsEc2CopySnapshotOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateCapacityReservation(AwsEc2CreateCapacityReservationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateCapacityReservationFleet(AwsEc2CreateCapacityReservationFleetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateCarrierGateway(AwsEc2CreateCarrierGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateClientVpnEndpoint(AwsEc2CreateClientVpnEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateClientVpnRoute(AwsEc2CreateClientVpnRouteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateCoipCidr(AwsEc2CreateCoipCidrOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateCoipPool(AwsEc2CreateCoipPoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateCustomerGateway(AwsEc2CreateCustomerGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDefaultSubnet(AwsEc2CreateDefaultSubnetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDefaultVpc(AwsEc2CreateDefaultVpcOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateDefaultVpcOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDhcpOptions(AwsEc2CreateDhcpOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateEgressOnlyInternetGateway(AwsEc2CreateEgressOnlyInternetGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateFleet(AwsEc2CreateFleetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateFlowLogs(AwsEc2CreateFlowLogsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateFpgaImage(AwsEc2CreateFpgaImageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateImage(AwsEc2CreateImageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateInstanceConnectEndpoint(AwsEc2CreateInstanceConnectEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateInstanceEventWindow(AwsEc2CreateInstanceEventWindowOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateInstanceEventWindowOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateInstanceExportTask(AwsEc2CreateInstanceExportTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateInternetGateway(AwsEc2CreateInternetGatewayOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateInternetGatewayOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateIpam(AwsEc2CreateIpamOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateIpamOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateIpamPool(AwsEc2CreateIpamPoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateIpamResourceDiscovery(AwsEc2CreateIpamResourceDiscoveryOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateIpamResourceDiscoveryOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateIpamScope(AwsEc2CreateIpamScopeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateKeyPair(AwsEc2CreateKeyPairOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateLaunchTemplate(AwsEc2CreateLaunchTemplateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateLaunchTemplateVersion(AwsEc2CreateLaunchTemplateVersionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateLocalGatewayRoute(AwsEc2CreateLocalGatewayRouteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateLocalGatewayRouteTable(AwsEc2CreateLocalGatewayRouteTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateLocalGatewayRouteTableVirtualInterfaceGroupAssociation(AwsEc2CreateLocalGatewayRouteTableVirtualInterfaceGroupAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateLocalGatewayRouteTableVpcAssociation(AwsEc2CreateLocalGatewayRouteTableVpcAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateManagedPrefixList(AwsEc2CreateManagedPrefixListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateNatGateway(AwsEc2CreateNatGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateNetworkAcl(AwsEc2CreateNetworkAclOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateNetworkAclEntry(AwsEc2CreateNetworkAclEntryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateNetworkInsightsAccessScope(AwsEc2CreateNetworkInsightsAccessScopeOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateNetworkInsightsAccessScopeOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateNetworkInsightsPath(AwsEc2CreateNetworkInsightsPathOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateNetworkInterface(AwsEc2CreateNetworkInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateNetworkInterfacePermission(AwsEc2CreateNetworkInterfacePermissionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreatePlacementGroup(AwsEc2CreatePlacementGroupOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreatePlacementGroupOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreatePublicIpv4Pool(AwsEc2CreatePublicIpv4PoolOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreatePublicIpv4PoolOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateReplaceRootVolumeTask(AwsEc2CreateReplaceRootVolumeTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateReservedInstancesListing(AwsEc2CreateReservedInstancesListingOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateRestoreImageTask(AwsEc2CreateRestoreImageTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateRoute(AwsEc2CreateRouteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateRouteTable(AwsEc2CreateRouteTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSecurityGroup(AwsEc2CreateSecurityGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSnapshot(AwsEc2CreateSnapshotOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSnapshots(AwsEc2CreateSnapshotsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSpotDatafeedSubscription(AwsEc2CreateSpotDatafeedSubscriptionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateStoreImageTask(AwsEc2CreateStoreImageTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSubnet(AwsEc2CreateSubnetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSubnetCidrReservation(AwsEc2CreateSubnetCidrReservationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTags(AwsEc2CreateTagsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTrafficMirrorFilter(AwsEc2CreateTrafficMirrorFilterOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateTrafficMirrorFilterOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTrafficMirrorFilterRule(AwsEc2CreateTrafficMirrorFilterRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTrafficMirrorSession(AwsEc2CreateTrafficMirrorSessionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTrafficMirrorTarget(AwsEc2CreateTrafficMirrorTargetOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateTrafficMirrorTargetOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTransitGateway(AwsEc2CreateTransitGatewayOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateTransitGatewayOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTransitGatewayConnect(AwsEc2CreateTransitGatewayConnectOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTransitGatewayConnectPeer(AwsEc2CreateTransitGatewayConnectPeerOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTransitGatewayMulticastDomain(AwsEc2CreateTransitGatewayMulticastDomainOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTransitGatewayPeeringAttachment(AwsEc2CreateTransitGatewayPeeringAttachmentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTransitGatewayPolicyTable(AwsEc2CreateTransitGatewayPolicyTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTransitGatewayPrefixListReference(AwsEc2CreateTransitGatewayPrefixListReferenceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTransitGatewayRoute(AwsEc2CreateTransitGatewayRouteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTransitGatewayRouteTable(AwsEc2CreateTransitGatewayRouteTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTransitGatewayRouteTableAnnouncement(AwsEc2CreateTransitGatewayRouteTableAnnouncementOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTransitGatewayVpcAttachment(AwsEc2CreateTransitGatewayVpcAttachmentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVerifiedAccessEndpoint(AwsEc2CreateVerifiedAccessEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVerifiedAccessGroup(AwsEc2CreateVerifiedAccessGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVerifiedAccessInstance(AwsEc2CreateVerifiedAccessInstanceOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateVerifiedAccessInstanceOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVerifiedAccessTrustProvider(AwsEc2CreateVerifiedAccessTrustProviderOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVolume(AwsEc2CreateVolumeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVpc(AwsEc2CreateVpcOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateVpcOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVpcEndpoint(AwsEc2CreateVpcEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVpcEndpointConnectionNotification(AwsEc2CreateVpcEndpointConnectionNotificationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVpcEndpointServiceConfiguration(AwsEc2CreateVpcEndpointServiceConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateVpcEndpointServiceConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVpcPeeringConnection(AwsEc2CreateVpcPeeringConnectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVpnConnection(AwsEc2CreateVpnConnectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVpnConnectionRoute(AwsEc2CreateVpnConnectionRouteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVpnGateway(AwsEc2CreateVpnGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteCarrierGateway(AwsEc2DeleteCarrierGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteClientVpnEndpoint(AwsEc2DeleteClientVpnEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteClientVpnRoute(AwsEc2DeleteClientVpnRouteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteCoipCidr(AwsEc2DeleteCoipCidrOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteCoipPool(AwsEc2DeleteCoipPoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteCustomerGateway(AwsEc2DeleteCustomerGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDhcpOptions(AwsEc2DeleteDhcpOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteEgressOnlyInternetGateway(AwsEc2DeleteEgressOnlyInternetGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteFleets(AwsEc2DeleteFleetsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteFlowLogs(AwsEc2DeleteFlowLogsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteFpgaImage(AwsEc2DeleteFpgaImageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteInstanceConnectEndpoint(AwsEc2DeleteInstanceConnectEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteInstanceEventWindow(AwsEc2DeleteInstanceEventWindowOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteInternetGateway(AwsEc2DeleteInternetGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteIpam(AwsEc2DeleteIpamOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteIpamPool(AwsEc2DeleteIpamPoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteIpamResourceDiscovery(AwsEc2DeleteIpamResourceDiscoveryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteIpamScope(AwsEc2DeleteIpamScopeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteKeyPair(AwsEc2DeleteKeyPairOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DeleteKeyPairOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteLaunchTemplate(AwsEc2DeleteLaunchTemplateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DeleteLaunchTemplateOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteLaunchTemplateVersions(AwsEc2DeleteLaunchTemplateVersionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteLocalGatewayRoute(AwsEc2DeleteLocalGatewayRouteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteLocalGatewayRouteTable(AwsEc2DeleteLocalGatewayRouteTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteLocalGatewayRouteTableVirtualInterfaceGroupAssociation(AwsEc2DeleteLocalGatewayRouteTableVirtualInterfaceGroupAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteLocalGatewayRouteTableVpcAssociation(AwsEc2DeleteLocalGatewayRouteTableVpcAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteManagedPrefixList(AwsEc2DeleteManagedPrefixListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteNatGateway(AwsEc2DeleteNatGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteNetworkAcl(AwsEc2DeleteNetworkAclOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteNetworkAclEntry(AwsEc2DeleteNetworkAclEntryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteNetworkInsightsAccessScope(AwsEc2DeleteNetworkInsightsAccessScopeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteNetworkInsightsAccessScopeAnalysis(AwsEc2DeleteNetworkInsightsAccessScopeAnalysisOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteNetworkInsightsAnalysis(AwsEc2DeleteNetworkInsightsAnalysisOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteNetworkInsightsPath(AwsEc2DeleteNetworkInsightsPathOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteNetworkInterface(AwsEc2DeleteNetworkInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteNetworkInterfacePermission(AwsEc2DeleteNetworkInterfacePermissionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeletePlacementGroup(AwsEc2DeletePlacementGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeletePublicIpv4Pool(AwsEc2DeletePublicIpv4PoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteQueuedReservedInstances(AwsEc2DeleteQueuedReservedInstancesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteRoute(AwsEc2DeleteRouteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteRouteTable(AwsEc2DeleteRouteTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSecurityGroup(AwsEc2DeleteSecurityGroupOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DeleteSecurityGroupOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSnapshot(AwsEc2DeleteSnapshotOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSpotDatafeedSubscription(AwsEc2DeleteSpotDatafeedSubscriptionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DeleteSpotDatafeedSubscriptionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSubnet(AwsEc2DeleteSubnetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSubnetCidrReservation(AwsEc2DeleteSubnetCidrReservationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTags(AwsEc2DeleteTagsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTrafficMirrorFilter(AwsEc2DeleteTrafficMirrorFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTrafficMirrorFilterRule(AwsEc2DeleteTrafficMirrorFilterRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTrafficMirrorSession(AwsEc2DeleteTrafficMirrorSessionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTrafficMirrorTarget(AwsEc2DeleteTrafficMirrorTargetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTransitGateway(AwsEc2DeleteTransitGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTransitGatewayConnect(AwsEc2DeleteTransitGatewayConnectOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTransitGatewayConnectPeer(AwsEc2DeleteTransitGatewayConnectPeerOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTransitGatewayMulticastDomain(AwsEc2DeleteTransitGatewayMulticastDomainOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTransitGatewayPeeringAttachment(AwsEc2DeleteTransitGatewayPeeringAttachmentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTransitGatewayPolicyTable(AwsEc2DeleteTransitGatewayPolicyTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTransitGatewayPrefixListReference(AwsEc2DeleteTransitGatewayPrefixListReferenceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTransitGatewayRoute(AwsEc2DeleteTransitGatewayRouteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTransitGatewayRouteTable(AwsEc2DeleteTransitGatewayRouteTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTransitGatewayRouteTableAnnouncement(AwsEc2DeleteTransitGatewayRouteTableAnnouncementOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTransitGatewayVpcAttachment(AwsEc2DeleteTransitGatewayVpcAttachmentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVerifiedAccessEndpoint(AwsEc2DeleteVerifiedAccessEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVerifiedAccessGroup(AwsEc2DeleteVerifiedAccessGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVerifiedAccessInstance(AwsEc2DeleteVerifiedAccessInstanceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVerifiedAccessTrustProvider(AwsEc2DeleteVerifiedAccessTrustProviderOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVolume(AwsEc2DeleteVolumeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVpc(AwsEc2DeleteVpcOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVpcEndpointConnectionNotifications(AwsEc2DeleteVpcEndpointConnectionNotificationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVpcEndpointServiceConfigurations(AwsEc2DeleteVpcEndpointServiceConfigurationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVpcEndpoints(AwsEc2DeleteVpcEndpointsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVpcPeeringConnection(AwsEc2DeleteVpcPeeringConnectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVpnConnection(AwsEc2DeleteVpnConnectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVpnConnectionRoute(AwsEc2DeleteVpnConnectionRouteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVpnGateway(AwsEc2DeleteVpnGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeprovisionByoipCidr(AwsEc2DeprovisionByoipCidrOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeprovisionIpamByoasn(AwsEc2DeprovisionIpamByoasnOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeprovisionIpamPoolCidr(AwsEc2DeprovisionIpamPoolCidrOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeprovisionPublicIpv4PoolCidr(AwsEc2DeprovisionPublicIpv4PoolCidrOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeregisterImage(AwsEc2DeregisterImageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeregisterInstanceEventNotificationAttributes(AwsEc2DeregisterInstanceEventNotificationAttributesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeregisterTransitGatewayMulticastGroupMembers(AwsEc2DeregisterTransitGatewayMulticastGroupMembersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DeregisterTransitGatewayMulticastGroupMembersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeregisterTransitGatewayMulticastGroupSources(AwsEc2DeregisterTransitGatewayMulticastGroupSourcesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DeregisterTransitGatewayMulticastGroupSourcesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAccountAttributes(AwsEc2DescribeAccountAttributesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAccountAttributesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAddressTransfers(AwsEc2DescribeAddressTransfersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAddressTransfersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAddresses(AwsEc2DescribeAddressesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAddressesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAddressesAttribute(AwsEc2DescribeAddressesAttributeOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAddressesAttributeOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAggregateIdFormat(AwsEc2DescribeAggregateIdFormatOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAggregateIdFormatOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAvailabilityZones(AwsEc2DescribeAvailabilityZonesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAvailabilityZonesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAwsNetworkPerformanceMetricSubscriptions(AwsEc2DescribeAwsNetworkPerformanceMetricSubscriptionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAwsNetworkPerformanceMetricSubscriptionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeBundleTasks(AwsEc2DescribeBundleTasksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeBundleTasksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeByoipCidrs(AwsEc2DescribeByoipCidrsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeByoipCidrsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeCapacityBlockOfferings(AwsEc2DescribeCapacityBlockOfferingsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeCapacityReservationFleets(AwsEc2DescribeCapacityReservationFleetsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeCapacityReservationFleetsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeCapacityReservations(AwsEc2DescribeCapacityReservationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeCapacityReservationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeCarrierGateways(AwsEc2DescribeCarrierGatewaysOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeCarrierGatewaysOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClassicLinkInstances(AwsEc2DescribeClassicLinkInstancesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeClassicLinkInstancesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClientVpnAuthorizationRules(AwsEc2DescribeClientVpnAuthorizationRulesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClientVpnConnections(AwsEc2DescribeClientVpnConnectionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClientVpnEndpoints(AwsEc2DescribeClientVpnEndpointsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeClientVpnEndpointsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClientVpnRoutes(AwsEc2DescribeClientVpnRoutesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClientVpnTargetNetworks(AwsEc2DescribeClientVpnTargetNetworksOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeCoipPools(AwsEc2DescribeCoipPoolsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeCoipPoolsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConversionTasks(AwsEc2DescribeConversionTasksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeConversionTasksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeCustomerGateways(AwsEc2DescribeCustomerGatewaysOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeCustomerGatewaysOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeDhcpOptions(AwsEc2DescribeDhcpOptionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeDhcpOptionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEgressOnlyInternetGateways(AwsEc2DescribeEgressOnlyInternetGatewaysOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeEgressOnlyInternetGatewaysOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeElasticGpus(AwsEc2DescribeElasticGpusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeElasticGpusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeExportImageTasks(AwsEc2DescribeExportImageTasksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeExportImageTasksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeExportTasks(AwsEc2DescribeExportTasksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeExportTasksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeFastLaunchImages(AwsEc2DescribeFastLaunchImagesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeFastLaunchImagesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeFastSnapshotRestores(AwsEc2DescribeFastSnapshotRestoresOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeFastSnapshotRestoresOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeFleetHistory(AwsEc2DescribeFleetHistoryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeFleetInstances(AwsEc2DescribeFleetInstancesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeFleets(AwsEc2DescribeFleetsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeFleetsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeFlowLogs(AwsEc2DescribeFlowLogsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeFlowLogsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeFpgaImageAttribute(AwsEc2DescribeFpgaImageAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeFpgaImages(AwsEc2DescribeFpgaImagesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeFpgaImagesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeHostReservationOfferings(AwsEc2DescribeHostReservationOfferingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeHostReservationOfferingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeHostReservations(AwsEc2DescribeHostReservationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeHostReservationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeHosts(AwsEc2DescribeHostsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeHostsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeIamInstanceProfileAssociations(AwsEc2DescribeIamInstanceProfileAssociationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIamInstanceProfileAssociationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeIdFormat(AwsEc2DescribeIdFormatOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIdFormatOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeIdentityIdFormat(AwsEc2DescribeIdentityIdFormatOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeImageAttribute(AwsEc2DescribeImageAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeImages(AwsEc2DescribeImagesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeImagesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeImportImageTasks(AwsEc2DescribeImportImageTasksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeImportImageTasksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeImportSnapshotTasks(AwsEc2DescribeImportSnapshotTasksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeImportSnapshotTasksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInstanceAttribute(AwsEc2DescribeInstanceAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInstanceConnectEndpoints(AwsEc2DescribeInstanceConnectEndpointsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceConnectEndpointsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInstanceCreditSpecifications(AwsEc2DescribeInstanceCreditSpecificationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceCreditSpecificationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInstanceEventNotificationAttributes(AwsEc2DescribeInstanceEventNotificationAttributesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceEventNotificationAttributesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInstanceEventWindows(AwsEc2DescribeInstanceEventWindowsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceEventWindowsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInstanceStatus(AwsEc2DescribeInstanceStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInstanceTopology(AwsEc2DescribeInstanceTopologyOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceTopologyOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInstanceTypeOfferings(AwsEc2DescribeInstanceTypeOfferingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceTypeOfferingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInstanceTypes(AwsEc2DescribeInstanceTypesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceTypesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInstances(AwsEc2DescribeInstancesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstancesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInternetGateways(AwsEc2DescribeInternetGatewaysOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInternetGatewaysOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeIpamByoasn(AwsEc2DescribeIpamByoasnOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpamByoasnOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeIpamPools(AwsEc2DescribeIpamPoolsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpamPoolsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeIpamResourceDiscoveries(AwsEc2DescribeIpamResourceDiscoveriesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpamResourceDiscoveriesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeIpamResourceDiscoveryAssociations(AwsEc2DescribeIpamResourceDiscoveryAssociationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpamResourceDiscoveryAssociationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeIpamScopes(AwsEc2DescribeIpamScopesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpamScopesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeIpams(AwsEc2DescribeIpamsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpamsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeIpv6Pools(AwsEc2DescribeIpv6PoolsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpv6PoolsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeKeyPairs(AwsEc2DescribeKeyPairsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeKeyPairsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLaunchTemplateVersions(AwsEc2DescribeLaunchTemplateVersionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLaunchTemplateVersionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLaunchTemplates(AwsEc2DescribeLaunchTemplatesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLaunchTemplatesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLocalGatewayRouteTableVirtualInterfaceGroupAssociations(AwsEc2DescribeLocalGatewayRouteTableVirtualInterfaceGroupAssociationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLocalGatewayRouteTableVirtualInterfaceGroupAssociationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLocalGatewayRouteTableVpcAssociations(AwsEc2DescribeLocalGatewayRouteTableVpcAssociationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLocalGatewayRouteTableVpcAssociationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLocalGatewayRouteTables(AwsEc2DescribeLocalGatewayRouteTablesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLocalGatewayRouteTablesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLocalGatewayVirtualInterfaceGroups(AwsEc2DescribeLocalGatewayVirtualInterfaceGroupsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLocalGatewayVirtualInterfaceGroupsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLocalGatewayVirtualInterfaces(AwsEc2DescribeLocalGatewayVirtualInterfacesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLocalGatewayVirtualInterfacesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLocalGateways(AwsEc2DescribeLocalGatewaysOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLocalGatewaysOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLockedSnapshots(AwsEc2DescribeLockedSnapshotsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLockedSnapshotsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeManagedPrefixLists(AwsEc2DescribeManagedPrefixListsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeManagedPrefixListsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeMovingAddresses(AwsEc2DescribeMovingAddressesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeMovingAddressesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeNatGateways(AwsEc2DescribeNatGatewaysOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNatGatewaysOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeNetworkAcls(AwsEc2DescribeNetworkAclsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkAclsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeNetworkInsightsAccessScopeAnalyses(AwsEc2DescribeNetworkInsightsAccessScopeAnalysesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkInsightsAccessScopeAnalysesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeNetworkInsightsAccessScopes(AwsEc2DescribeNetworkInsightsAccessScopesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkInsightsAccessScopesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeNetworkInsightsAnalyses(AwsEc2DescribeNetworkInsightsAnalysesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkInsightsAnalysesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeNetworkInsightsPaths(AwsEc2DescribeNetworkInsightsPathsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkInsightsPathsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeNetworkInterfaceAttribute(AwsEc2DescribeNetworkInterfaceAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeNetworkInterfacePermissions(AwsEc2DescribeNetworkInterfacePermissionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkInterfacePermissionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeNetworkInterfaces(AwsEc2DescribeNetworkInterfacesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkInterfacesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribePlacementGroups(AwsEc2DescribePlacementGroupsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribePlacementGroupsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribePrefixLists(AwsEc2DescribePrefixListsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribePrefixListsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribePrincipalIdFormat(AwsEc2DescribePrincipalIdFormatOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribePrincipalIdFormatOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribePublicIpv4Pools(AwsEc2DescribePublicIpv4PoolsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribePublicIpv4PoolsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRegions(AwsEc2DescribeRegionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeRegionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeReplaceRootVolumeTasks(AwsEc2DescribeReplaceRootVolumeTasksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeReplaceRootVolumeTasksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeReservedInstances(AwsEc2DescribeReservedInstancesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeReservedInstancesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeReservedInstancesListings(AwsEc2DescribeReservedInstancesListingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeReservedInstancesListingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeReservedInstancesModifications(AwsEc2DescribeReservedInstancesModificationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeReservedInstancesModificationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeReservedInstancesOfferings(AwsEc2DescribeReservedInstancesOfferingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeReservedInstancesOfferingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRouteTables(AwsEc2DescribeRouteTablesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeRouteTablesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeScheduledInstanceAvailability(AwsEc2DescribeScheduledInstanceAvailabilityOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeScheduledInstances(AwsEc2DescribeScheduledInstancesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeScheduledInstancesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSecurityGroupReferences(AwsEc2DescribeSecurityGroupReferencesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSecurityGroupRules(AwsEc2DescribeSecurityGroupRulesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSecurityGroupRulesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSecurityGroups(AwsEc2DescribeSecurityGroupsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSecurityGroupsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSnapshotAttribute(AwsEc2DescribeSnapshotAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSnapshotTierStatus(AwsEc2DescribeSnapshotTierStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSnapshotTierStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSnapshots(AwsEc2DescribeSnapshotsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSnapshotsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSpotDatafeedSubscription(AwsEc2DescribeSpotDatafeedSubscriptionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSpotDatafeedSubscriptionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSpotFleetInstances(AwsEc2DescribeSpotFleetInstancesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSpotFleetRequestHistory(AwsEc2DescribeSpotFleetRequestHistoryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSpotFleetRequests(AwsEc2DescribeSpotFleetRequestsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSpotFleetRequestsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSpotInstanceRequests(AwsEc2DescribeSpotInstanceRequestsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSpotInstanceRequestsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSpotPriceHistory(AwsEc2DescribeSpotPriceHistoryOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSpotPriceHistoryOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeStaleSecurityGroups(AwsEc2DescribeStaleSecurityGroupsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeStoreImageTasks(AwsEc2DescribeStoreImageTasksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeStoreImageTasksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSubnets(AwsEc2DescribeSubnetsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSubnetsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTags(AwsEc2DescribeTagsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTagsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTrafficMirrorFilters(AwsEc2DescribeTrafficMirrorFiltersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTrafficMirrorFiltersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTrafficMirrorSessions(AwsEc2DescribeTrafficMirrorSessionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTrafficMirrorSessionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTrafficMirrorTargets(AwsEc2DescribeTrafficMirrorTargetsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTrafficMirrorTargetsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTransitGatewayAttachments(AwsEc2DescribeTransitGatewayAttachmentsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayAttachmentsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTransitGatewayConnectPeers(AwsEc2DescribeTransitGatewayConnectPeersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayConnectPeersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTransitGatewayConnects(AwsEc2DescribeTransitGatewayConnectsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayConnectsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTransitGatewayMulticastDomains(AwsEc2DescribeTransitGatewayMulticastDomainsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayMulticastDomainsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTransitGatewayPeeringAttachments(AwsEc2DescribeTransitGatewayPeeringAttachmentsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayPeeringAttachmentsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTransitGatewayPolicyTables(AwsEc2DescribeTransitGatewayPolicyTablesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayPolicyTablesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTransitGatewayRouteTableAnnouncements(AwsEc2DescribeTransitGatewayRouteTableAnnouncementsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayRouteTableAnnouncementsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTransitGatewayRouteTables(AwsEc2DescribeTransitGatewayRouteTablesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayRouteTablesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTransitGatewayVpcAttachments(AwsEc2DescribeTransitGatewayVpcAttachmentsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayVpcAttachmentsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTransitGateways(AwsEc2DescribeTransitGatewaysOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewaysOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTrunkInterfaceAssociations(AwsEc2DescribeTrunkInterfaceAssociationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTrunkInterfaceAssociationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVerifiedAccessEndpoints(AwsEc2DescribeVerifiedAccessEndpointsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVerifiedAccessEndpointsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVerifiedAccessGroups(AwsEc2DescribeVerifiedAccessGroupsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVerifiedAccessGroupsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVerifiedAccessInstanceLoggingConfigurations(AwsEc2DescribeVerifiedAccessInstanceLoggingConfigurationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVerifiedAccessInstanceLoggingConfigurationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVerifiedAccessInstances(AwsEc2DescribeVerifiedAccessInstancesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVerifiedAccessInstancesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVerifiedAccessTrustProviders(AwsEc2DescribeVerifiedAccessTrustProvidersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVerifiedAccessTrustProvidersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVolumeAttribute(AwsEc2DescribeVolumeAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVolumeStatus(AwsEc2DescribeVolumeStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVolumeStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVolumes(AwsEc2DescribeVolumesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVolumesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVolumesModifications(AwsEc2DescribeVolumesModificationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVolumesModificationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVpcAttribute(AwsEc2DescribeVpcAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVpcClassicLink(AwsEc2DescribeVpcClassicLinkOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcClassicLinkOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVpcClassicLinkDnsSupport(AwsEc2DescribeVpcClassicLinkDnsSupportOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcClassicLinkDnsSupportOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVpcEndpointConnectionNotifications(AwsEc2DescribeVpcEndpointConnectionNotificationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcEndpointConnectionNotificationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVpcEndpointConnections(AwsEc2DescribeVpcEndpointConnectionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcEndpointConnectionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVpcEndpointServiceConfigurations(AwsEc2DescribeVpcEndpointServiceConfigurationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcEndpointServiceConfigurationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVpcEndpointServicePermissions(AwsEc2DescribeVpcEndpointServicePermissionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVpcEndpointServices(AwsEc2DescribeVpcEndpointServicesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcEndpointServicesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVpcEndpoints(AwsEc2DescribeVpcEndpointsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcEndpointsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVpcPeeringConnections(AwsEc2DescribeVpcPeeringConnectionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcPeeringConnectionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVpcs(AwsEc2DescribeVpcsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVpnConnections(AwsEc2DescribeVpnConnectionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpnConnectionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVpnGateways(AwsEc2DescribeVpnGatewaysOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpnGatewaysOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DetachClassicLinkVpc(AwsEc2DetachClassicLinkVpcOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DetachInternetGateway(AwsEc2DetachInternetGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DetachNetworkInterface(AwsEc2DetachNetworkInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DetachVerifiedAccessTrustProvider(AwsEc2DetachVerifiedAccessTrustProviderOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DetachVolume(AwsEc2DetachVolumeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DetachVpnGateway(AwsEc2DetachVpnGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableAddressTransfer(AwsEc2DisableAddressTransferOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableAwsNetworkPerformanceMetricSubscription(AwsEc2DisableAwsNetworkPerformanceMetricSubscriptionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisableAwsNetworkPerformanceMetricSubscriptionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableEbsEncryptionByDefault(AwsEc2DisableEbsEncryptionByDefaultOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisableEbsEncryptionByDefaultOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableFastLaunch(AwsEc2DisableFastLaunchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableFastSnapshotRestores(AwsEc2DisableFastSnapshotRestoresOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableImage(AwsEc2DisableImageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableImageBlockPublicAccess(AwsEc2DisableImageBlockPublicAccessOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisableImageBlockPublicAccessOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableImageDeprecation(AwsEc2DisableImageDeprecationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableIpamOrganizationAdminAccount(AwsEc2DisableIpamOrganizationAdminAccountOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableSerialConsoleAccess(AwsEc2DisableSerialConsoleAccessOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisableSerialConsoleAccessOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableSnapshotBlockPublicAccess(AwsEc2DisableSnapshotBlockPublicAccessOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisableSnapshotBlockPublicAccessOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableTransitGatewayRouteTablePropagation(AwsEc2DisableTransitGatewayRouteTablePropagationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableVgwRoutePropagation(AwsEc2DisableVgwRoutePropagationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableVpcClassicLink(AwsEc2DisableVpcClassicLinkOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableVpcClassicLinkDnsSupport(AwsEc2DisableVpcClassicLinkDnsSupportOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisableVpcClassicLinkDnsSupportOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateAddress(AwsEc2DisassociateAddressOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisassociateAddressOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateClientVpnTargetNetwork(AwsEc2DisassociateClientVpnTargetNetworkOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateEnclaveCertificateIamRole(AwsEc2DisassociateEnclaveCertificateIamRoleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateIamInstanceProfile(AwsEc2DisassociateIamInstanceProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateInstanceEventWindow(AwsEc2DisassociateInstanceEventWindowOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateIpamByoasn(AwsEc2DisassociateIpamByoasnOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateIpamResourceDiscovery(AwsEc2DisassociateIpamResourceDiscoveryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateNatGatewayAddress(AwsEc2DisassociateNatGatewayAddressOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateRouteTable(AwsEc2DisassociateRouteTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateSubnetCidrBlock(AwsEc2DisassociateSubnetCidrBlockOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateTransitGatewayMulticastDomain(AwsEc2DisassociateTransitGatewayMulticastDomainOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateTransitGatewayPolicyTable(AwsEc2DisassociateTransitGatewayPolicyTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateTransitGatewayRouteTable(AwsEc2DisassociateTransitGatewayRouteTableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateTrunkInterface(AwsEc2DisassociateTrunkInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateVpcCidrBlock(AwsEc2DisassociateVpcCidrBlockOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableAddressTransfer(AwsEc2EnableAddressTransferOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableAwsNetworkPerformanceMetricSubscription(AwsEc2EnableAwsNetworkPerformanceMetricSubscriptionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2EnableAwsNetworkPerformanceMetricSubscriptionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableEbsEncryptionByDefault(AwsEc2EnableEbsEncryptionByDefaultOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2EnableEbsEncryptionByDefaultOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableFastLaunch(AwsEc2EnableFastLaunchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableFastSnapshotRestores(AwsEc2EnableFastSnapshotRestoresOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableImage(AwsEc2EnableImageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableImageBlockPublicAccess(AwsEc2EnableImageBlockPublicAccessOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableImageDeprecation(AwsEc2EnableImageDeprecationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableIpamOrganizationAdminAccount(AwsEc2EnableIpamOrganizationAdminAccountOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableReachabilityAnalyzerOrganizationSharing(AwsEc2EnableReachabilityAnalyzerOrganizationSharingOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2EnableReachabilityAnalyzerOrganizationSharingOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableSerialConsoleAccess(AwsEc2EnableSerialConsoleAccessOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2EnableSerialConsoleAccessOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableSnapshotBlockPublicAccess(AwsEc2EnableSnapshotBlockPublicAccessOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableTransitGatewayRouteTablePropagation(AwsEc2EnableTransitGatewayRouteTablePropagationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableVgwRoutePropagation(AwsEc2EnableVgwRoutePropagationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableVolumeIo(AwsEc2EnableVolumeIoOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableVpcClassicLink(AwsEc2EnableVpcClassicLinkOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableVpcClassicLinkDnsSupport(AwsEc2EnableVpcClassicLinkDnsSupportOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2EnableVpcClassicLinkDnsSupportOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExportClientVpnClientCertificateRevocationList(AwsEc2ExportClientVpnClientCertificateRevocationListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExportClientVpnClientConfiguration(AwsEc2ExportClientVpnClientConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExportImage(AwsEc2ExportImageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExportTransitGatewayRoutes(AwsEc2ExportTransitGatewayRoutesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAssociatedEnclaveCertificateIamRoles(AwsEc2GetAssociatedEnclaveCertificateIamRolesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAssociatedIpv6PoolCidrs(AwsEc2GetAssociatedIpv6PoolCidrsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAwsNetworkPerformanceData(AwsEc2GetAwsNetworkPerformanceDataOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetAwsNetworkPerformanceDataOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetCapacityReservationUsage(AwsEc2GetCapacityReservationUsageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetCoipPoolUsage(AwsEc2GetCoipPoolUsageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetConsoleOutput(AwsEc2GetConsoleOutputOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetConsoleScreenshot(AwsEc2GetConsoleScreenshotOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDefaultCreditSpecification(AwsEc2GetDefaultCreditSpecificationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEbsDefaultKmsKeyId(AwsEc2GetEbsDefaultKmsKeyIdOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetEbsDefaultKmsKeyIdOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEbsEncryptionByDefault(AwsEc2GetEbsEncryptionByDefaultOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetEbsEncryptionByDefaultOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetFlowLogsIntegrationTemplate(AwsEc2GetFlowLogsIntegrationTemplateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetGroupsForCapacityReservation(AwsEc2GetGroupsForCapacityReservationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetHostReservationPurchasePreview(AwsEc2GetHostReservationPurchasePreviewOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetImageBlockPublicAccessState(AwsEc2GetImageBlockPublicAccessStateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetImageBlockPublicAccessStateOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetInstanceTypesFromInstanceRequirements(AwsEc2GetInstanceTypesFromInstanceRequirementsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetInstanceUefiData(AwsEc2GetInstanceUefiDataOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetIpamAddressHistory(AwsEc2GetIpamAddressHistoryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetIpamDiscoveredAccounts(AwsEc2GetIpamDiscoveredAccountsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetIpamDiscoveredPublicAddresses(AwsEc2GetIpamDiscoveredPublicAddressesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetIpamDiscoveredResourceCidrs(AwsEc2GetIpamDiscoveredResourceCidrsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetIpamPoolAllocations(AwsEc2GetIpamPoolAllocationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetIpamPoolCidrs(AwsEc2GetIpamPoolCidrsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetIpamResourceCidrs(AwsEc2GetIpamResourceCidrsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetLaunchTemplateData(AwsEc2GetLaunchTemplateDataOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetManagedPrefixListAssociations(AwsEc2GetManagedPrefixListAssociationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetManagedPrefixListEntries(AwsEc2GetManagedPrefixListEntriesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetNetworkInsightsAccessScopeAnalysisFindings(AwsEc2GetNetworkInsightsAccessScopeAnalysisFindingsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetNetworkInsightsAccessScopeContent(AwsEc2GetNetworkInsightsAccessScopeContentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetPasswordData(AwsEc2GetPasswordDataOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetReservedInstancesExchangeQuote(AwsEc2GetReservedInstancesExchangeQuoteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSecurityGroupsForVpc(AwsEc2GetSecurityGroupsForVpcOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSerialConsoleAccessStatus(AwsEc2GetSerialConsoleAccessStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetSerialConsoleAccessStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSnapshotBlockPublicAccessState(AwsEc2GetSnapshotBlockPublicAccessStateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetSnapshotBlockPublicAccessStateOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSpotPlacementScores(AwsEc2GetSpotPlacementScoresOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSubnetCidrReservations(AwsEc2GetSubnetCidrReservationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTransitGatewayAttachmentPropagations(AwsEc2GetTransitGatewayAttachmentPropagationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTransitGatewayMulticastDomainAssociations(AwsEc2GetTransitGatewayMulticastDomainAssociationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTransitGatewayPolicyTableAssociations(AwsEc2GetTransitGatewayPolicyTableAssociationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTransitGatewayPolicyTableEntries(AwsEc2GetTransitGatewayPolicyTableEntriesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTransitGatewayPrefixListReferences(AwsEc2GetTransitGatewayPrefixListReferencesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTransitGatewayRouteTableAssociations(AwsEc2GetTransitGatewayRouteTableAssociationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTransitGatewayRouteTablePropagations(AwsEc2GetTransitGatewayRouteTablePropagationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVerifiedAccessEndpointPolicy(AwsEc2GetVerifiedAccessEndpointPolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVerifiedAccessGroupPolicy(AwsEc2GetVerifiedAccessGroupPolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVpnConnectionDeviceSampleConfiguration(AwsEc2GetVpnConnectionDeviceSampleConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVpnConnectionDeviceTypes(AwsEc2GetVpnConnectionDeviceTypesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetVpnConnectionDeviceTypesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVpnTunnelReplacementStatus(AwsEc2GetVpnTunnelReplacementStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ImportClientVpnClientCertificateRevocationList(AwsEc2ImportClientVpnClientCertificateRevocationListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ImportImage(AwsEc2ImportImageOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ImportImageOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ImportKeyPair(AwsEc2ImportKeyPairOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ImportSnapshot(AwsEc2ImportSnapshotOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ImportSnapshotOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListImagesInRecycleBin(AwsEc2ListImagesInRecycleBinOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ListImagesInRecycleBinOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListSnapshotsInRecycleBin(AwsEc2ListSnapshotsInRecycleBinOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ListSnapshotsInRecycleBinOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> LockSnapshot(AwsEc2LockSnapshotOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyAddressAttribute(AwsEc2ModifyAddressAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyAvailabilityZoneGroup(AwsEc2ModifyAvailabilityZoneGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyCapacityReservation(AwsEc2ModifyCapacityReservationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyCapacityReservationFleet(AwsEc2ModifyCapacityReservationFleetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyClientVpnEndpoint(AwsEc2ModifyClientVpnEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyDefaultCreditSpecification(AwsEc2ModifyDefaultCreditSpecificationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyEbsDefaultKmsKeyId(AwsEc2ModifyEbsDefaultKmsKeyIdOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyFleet(AwsEc2ModifyFleetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyFpgaImageAttribute(AwsEc2ModifyFpgaImageAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyHosts(AwsEc2ModifyHostsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyIdFormat(AwsEc2ModifyIdFormatOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyIdentityIdFormat(AwsEc2ModifyIdentityIdFormatOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyImageAttribute(AwsEc2ModifyImageAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyInstanceAttribute(AwsEc2ModifyInstanceAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyInstanceCapacityReservationAttributes(AwsEc2ModifyInstanceCapacityReservationAttributesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyInstanceCreditSpecification(AwsEc2ModifyInstanceCreditSpecificationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyInstanceEventStartTime(AwsEc2ModifyInstanceEventStartTimeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyInstanceEventWindow(AwsEc2ModifyInstanceEventWindowOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyInstanceMaintenanceOptions(AwsEc2ModifyInstanceMaintenanceOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyInstanceMetadataOptions(AwsEc2ModifyInstanceMetadataOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyInstancePlacement(AwsEc2ModifyInstancePlacementOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyIpam(AwsEc2ModifyIpamOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyIpamPool(AwsEc2ModifyIpamPoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyIpamResourceCidr(AwsEc2ModifyIpamResourceCidrOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyIpamResourceDiscovery(AwsEc2ModifyIpamResourceDiscoveryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyIpamScope(AwsEc2ModifyIpamScopeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyLaunchTemplate(AwsEc2ModifyLaunchTemplateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ModifyLaunchTemplateOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyLocalGatewayRoute(AwsEc2ModifyLocalGatewayRouteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyManagedPrefixList(AwsEc2ModifyManagedPrefixListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyNetworkInterfaceAttribute(AwsEc2ModifyNetworkInterfaceAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyPrivateDnsNameOptions(AwsEc2ModifyPrivateDnsNameOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyReservedInstances(AwsEc2ModifyReservedInstancesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifySecurityGroupRules(AwsEc2ModifySecurityGroupRulesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifySnapshotAttribute(AwsEc2ModifySnapshotAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifySnapshotTier(AwsEc2ModifySnapshotTierOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifySpotFleetRequest(AwsEc2ModifySpotFleetRequestOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifySubnetAttribute(AwsEc2ModifySubnetAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyTrafficMirrorFilterNetworkServices(AwsEc2ModifyTrafficMirrorFilterNetworkServicesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyTrafficMirrorFilterRule(AwsEc2ModifyTrafficMirrorFilterRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyTrafficMirrorSession(AwsEc2ModifyTrafficMirrorSessionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyTransitGateway(AwsEc2ModifyTransitGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyTransitGatewayPrefixListReference(AwsEc2ModifyTransitGatewayPrefixListReferenceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyTransitGatewayVpcAttachment(AwsEc2ModifyTransitGatewayVpcAttachmentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVerifiedAccessEndpoint(AwsEc2ModifyVerifiedAccessEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVerifiedAccessEndpointPolicy(AwsEc2ModifyVerifiedAccessEndpointPolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVerifiedAccessGroup(AwsEc2ModifyVerifiedAccessGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVerifiedAccessGroupPolicy(AwsEc2ModifyVerifiedAccessGroupPolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVerifiedAccessInstance(AwsEc2ModifyVerifiedAccessInstanceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVerifiedAccessInstanceLoggingConfiguration(AwsEc2ModifyVerifiedAccessInstanceLoggingConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVerifiedAccessTrustProvider(AwsEc2ModifyVerifiedAccessTrustProviderOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVolume(AwsEc2ModifyVolumeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVolumeAttribute(AwsEc2ModifyVolumeAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVpcAttribute(AwsEc2ModifyVpcAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVpcEndpoint(AwsEc2ModifyVpcEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVpcEndpointConnectionNotification(AwsEc2ModifyVpcEndpointConnectionNotificationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVpcEndpointServiceConfiguration(AwsEc2ModifyVpcEndpointServiceConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVpcEndpointServicePayerResponsibility(AwsEc2ModifyVpcEndpointServicePayerResponsibilityOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVpcEndpointServicePermissions(AwsEc2ModifyVpcEndpointServicePermissionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVpcPeeringConnectionOptions(AwsEc2ModifyVpcPeeringConnectionOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVpcTenancy(AwsEc2ModifyVpcTenancyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVpnConnection(AwsEc2ModifyVpnConnectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVpnConnectionOptions(AwsEc2ModifyVpnConnectionOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVpnTunnelCertificate(AwsEc2ModifyVpnTunnelCertificateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyVpnTunnelOptions(AwsEc2ModifyVpnTunnelOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> MonitorInstances(AwsEc2MonitorInstancesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> MoveAddressToVpc(AwsEc2MoveAddressToVpcOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> MoveByoipCidrToIpam(AwsEc2MoveByoipCidrToIpamOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ProvisionByoipCidr(AwsEc2ProvisionByoipCidrOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ProvisionIpamByoasn(AwsEc2ProvisionIpamByoasnOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ProvisionIpamPoolCidr(AwsEc2ProvisionIpamPoolCidrOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ProvisionPublicIpv4PoolCidr(AwsEc2ProvisionPublicIpv4PoolCidrOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PurchaseCapacityBlock(AwsEc2PurchaseCapacityBlockOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PurchaseHostReservation(AwsEc2PurchaseHostReservationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PurchaseReservedInstancesOffering(AwsEc2PurchaseReservedInstancesOfferingOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PurchaseScheduledInstances(AwsEc2PurchaseScheduledInstancesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RebootInstances(AwsEc2RebootInstancesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RegisterImage(AwsEc2RegisterImageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RegisterInstanceEventNotificationAttributes(AwsEc2RegisterInstanceEventNotificationAttributesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RegisterTransitGatewayMulticastGroupMembers(AwsEc2RegisterTransitGatewayMulticastGroupMembersOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RegisterTransitGatewayMulticastGroupSources(AwsEc2RegisterTransitGatewayMulticastGroupSourcesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RejectTransitGatewayMulticastDomainAssociations(AwsEc2RejectTransitGatewayMulticastDomainAssociationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2RejectTransitGatewayMulticastDomainAssociationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RejectTransitGatewayPeeringAttachment(AwsEc2RejectTransitGatewayPeeringAttachmentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RejectTransitGatewayVpcAttachment(AwsEc2RejectTransitGatewayVpcAttachmentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RejectVpcEndpointConnections(AwsEc2RejectVpcEndpointConnectionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RejectVpcPeeringConnection(AwsEc2RejectVpcPeeringConnectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReleaseAddress(AwsEc2ReleaseAddressOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ReleaseAddressOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReleaseHosts(AwsEc2ReleaseHostsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReleaseIpamPoolAllocation(AwsEc2ReleaseIpamPoolAllocationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplaceIamInstanceProfileAssociation(AwsEc2ReplaceIamInstanceProfileAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplaceNetworkAclAssociation(AwsEc2ReplaceNetworkAclAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplaceNetworkAclEntry(AwsEc2ReplaceNetworkAclEntryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplaceRoute(AwsEc2ReplaceRouteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplaceRouteTableAssociation(AwsEc2ReplaceRouteTableAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplaceTransitGatewayRoute(AwsEc2ReplaceTransitGatewayRouteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplaceVpnTunnel(AwsEc2ReplaceVpnTunnelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReportInstanceStatus(AwsEc2ReportInstanceStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RequestSpotFleet(AwsEc2RequestSpotFleetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RequestSpotInstances(AwsEc2RequestSpotInstancesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2RequestSpotInstancesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ResetAddressAttribute(AwsEc2ResetAddressAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ResetEbsDefaultKmsKeyId(AwsEc2ResetEbsDefaultKmsKeyIdOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ResetEbsDefaultKmsKeyIdOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ResetFpgaImageAttribute(AwsEc2ResetFpgaImageAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ResetImageAttribute(AwsEc2ResetImageAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ResetInstanceAttribute(AwsEc2ResetInstanceAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ResetNetworkInterfaceAttribute(AwsEc2ResetNetworkInterfaceAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ResetSnapshotAttribute(AwsEc2ResetSnapshotAttributeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RestoreAddressToClassic(AwsEc2RestoreAddressToClassicOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RestoreImageFromRecycleBin(AwsEc2RestoreImageFromRecycleBinOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RestoreManagedPrefixListVersion(AwsEc2RestoreManagedPrefixListVersionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RestoreSnapshotFromRecycleBin(AwsEc2RestoreSnapshotFromRecycleBinOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RestoreSnapshotTier(AwsEc2RestoreSnapshotTierOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RevokeClientVpnIngress(AwsEc2RevokeClientVpnIngressOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RevokeSecurityGroupEgress(AwsEc2RevokeSecurityGroupEgressOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RevokeSecurityGroupIngress(AwsEc2RevokeSecurityGroupIngressOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2RevokeSecurityGroupIngressOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RunInstances(AwsEc2RunInstancesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2RunInstancesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RunScheduledInstances(AwsEc2RunScheduledInstancesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchLocalGatewayRoutes(AwsEc2SearchLocalGatewayRoutesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchTransitGatewayMulticastGroups(AwsEc2SearchTransitGatewayMulticastGroupsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchTransitGatewayRoutes(AwsEc2SearchTransitGatewayRoutesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SendDiagnosticInterrupt(AwsEc2SendDiagnosticInterruptOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartInstances(AwsEc2StartInstancesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartNetworkInsightsAccessScopeAnalysis(AwsEc2StartNetworkInsightsAccessScopeAnalysisOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartNetworkInsightsAnalysis(AwsEc2StartNetworkInsightsAnalysisOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartVpcEndpointServicePrivateDnsVerification(AwsEc2StartVpcEndpointServicePrivateDnsVerificationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopInstances(AwsEc2StopInstancesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TerminateClientVpnConnections(AwsEc2TerminateClientVpnConnectionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TerminateInstances(AwsEc2TerminateInstancesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UnassignIpv6Addresses(AwsEc2UnassignIpv6AddressesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UnassignPrivateIpAddresses(AwsEc2UnassignPrivateIpAddressesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UnassignPrivateNatGatewayAddress(AwsEc2UnassignPrivateNatGatewayAddressOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UnlockSnapshot(AwsEc2UnlockSnapshotOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UnmonitorInstances(AwsEc2UnmonitorInstancesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSecurityGroupRuleDescriptionsEgress(AwsEc2UpdateSecurityGroupRuleDescriptionsEgressOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2UpdateSecurityGroupRuleDescriptionsEgressOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSecurityGroupRuleDescriptionsIngress(AwsEc2UpdateSecurityGroupRuleDescriptionsIngressOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2UpdateSecurityGroupRuleDescriptionsIngressOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> WithdrawByoipCidr(AwsEc2WithdrawByoipCidrOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}