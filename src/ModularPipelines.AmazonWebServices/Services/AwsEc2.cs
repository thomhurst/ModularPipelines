using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> AcceptAddressTransfer(AwsEc2AcceptAddressTransferOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AcceptReservedInstancesExchangeQuote(AwsEc2AcceptReservedInstancesExchangeQuoteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AcceptTransitGatewayMulticastDomainAssociations(AwsEc2AcceptTransitGatewayMulticastDomainAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2AcceptTransitGatewayMulticastDomainAssociationsOptions(), token);
    }

    public async Task<CommandResult> AcceptTransitGatewayPeeringAttachment(AwsEc2AcceptTransitGatewayPeeringAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AcceptTransitGatewayVpcAttachment(AwsEc2AcceptTransitGatewayVpcAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AcceptVpcEndpointConnections(AwsEc2AcceptVpcEndpointConnectionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AcceptVpcPeeringConnection(AwsEc2AcceptVpcPeeringConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdvertiseByoipCidr(AwsEc2AdvertiseByoipCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AllocateAddress(AwsEc2AllocateAddressOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2AllocateAddressOptions(), token);
    }

    public async Task<CommandResult> AllocateHosts(AwsEc2AllocateHostsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AllocateIpamPoolCidr(AwsEc2AllocateIpamPoolCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ApplySecurityGroupsToClientVpnTargetNetwork(AwsEc2ApplySecurityGroupsToClientVpnTargetNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssignIpv6Addresses(AwsEc2AssignIpv6AddressesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssignPrivateIpAddresses(AwsEc2AssignPrivateIpAddressesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssignPrivateNatGatewayAddress(AwsEc2AssignPrivateNatGatewayAddressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateAddress(AwsEc2AssociateAddressOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2AssociateAddressOptions(), token);
    }

    public async Task<CommandResult> AssociateClientVpnTargetNetwork(AwsEc2AssociateClientVpnTargetNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateDhcpOptions(AwsEc2AssociateDhcpOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateEnclaveCertificateIamRole(AwsEc2AssociateEnclaveCertificateIamRoleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateIamInstanceProfile(AwsEc2AssociateIamInstanceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateInstanceEventWindow(AwsEc2AssociateInstanceEventWindowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateIpamByoasn(AwsEc2AssociateIpamByoasnOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateIpamResourceDiscovery(AwsEc2AssociateIpamResourceDiscoveryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateNatGatewayAddress(AwsEc2AssociateNatGatewayAddressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateRouteTable(AwsEc2AssociateRouteTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateSubnetCidrBlock(AwsEc2AssociateSubnetCidrBlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateTransitGatewayMulticastDomain(AwsEc2AssociateTransitGatewayMulticastDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateTransitGatewayPolicyTable(AwsEc2AssociateTransitGatewayPolicyTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateTransitGatewayRouteTable(AwsEc2AssociateTransitGatewayRouteTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateTrunkInterface(AwsEc2AssociateTrunkInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateVpcCidrBlock(AwsEc2AssociateVpcCidrBlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachClassicLinkVpc(AwsEc2AttachClassicLinkVpcOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachInternetGateway(AwsEc2AttachInternetGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachNetworkInterface(AwsEc2AttachNetworkInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachVerifiedAccessTrustProvider(AwsEc2AttachVerifiedAccessTrustProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachVolume(AwsEc2AttachVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachVpnGateway(AwsEc2AttachVpnGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AuthorizeClientVpnIngress(AwsEc2AuthorizeClientVpnIngressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AuthorizeSecurityGroupEgress(AwsEc2AuthorizeSecurityGroupEgressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AuthorizeSecurityGroupIngress(AwsEc2AuthorizeSecurityGroupIngressOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2AuthorizeSecurityGroupIngressOptions(), token);
    }

    public async Task<CommandResult> BundleInstance(AwsEc2BundleInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelBundleTask(AwsEc2CancelBundleTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelCapacityReservation(AwsEc2CancelCapacityReservationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelCapacityReservationFleets(AwsEc2CancelCapacityReservationFleetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelConversionTask(AwsEc2CancelConversionTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelExportTask(AwsEc2CancelExportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelImageLaunchPermission(AwsEc2CancelImageLaunchPermissionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelImportTask(AwsEc2CancelImportTaskOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CancelImportTaskOptions(), token);
    }

    public async Task<CommandResult> CancelReservedInstancesListing(AwsEc2CancelReservedInstancesListingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelSpotFleetRequests(AwsEc2CancelSpotFleetRequestsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelSpotInstanceRequests(AwsEc2CancelSpotInstanceRequestsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfirmProductInstance(AwsEc2ConfirmProductInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyFpgaImage(AwsEc2CopyFpgaImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyImage(AwsEc2CopyImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopySnapshot(AwsEc2CopySnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCapacityReservation(AwsEc2CreateCapacityReservationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCapacityReservationFleet(AwsEc2CreateCapacityReservationFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCarrierGateway(AwsEc2CreateCarrierGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateClientVpnEndpoint(AwsEc2CreateClientVpnEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateClientVpnRoute(AwsEc2CreateClientVpnRouteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCoipCidr(AwsEc2CreateCoipCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCoipPool(AwsEc2CreateCoipPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomerGateway(AwsEc2CreateCustomerGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDefaultSubnet(AwsEc2CreateDefaultSubnetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDefaultVpc(AwsEc2CreateDefaultVpcOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateDefaultVpcOptions(), token);
    }

    public async Task<CommandResult> CreateDhcpOptions(AwsEc2CreateDhcpOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEgressOnlyInternetGateway(AwsEc2CreateEgressOnlyInternetGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFleet(AwsEc2CreateFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFlowLogs(AwsEc2CreateFlowLogsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFpgaImage(AwsEc2CreateFpgaImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateImage(AwsEc2CreateImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInstanceConnectEndpoint(AwsEc2CreateInstanceConnectEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInstanceEventWindow(AwsEc2CreateInstanceEventWindowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateInstanceEventWindowOptions(), token);
    }

    public async Task<CommandResult> CreateInstanceExportTask(AwsEc2CreateInstanceExportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInternetGateway(AwsEc2CreateInternetGatewayOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateInternetGatewayOptions(), token);
    }

    public async Task<CommandResult> CreateIpam(AwsEc2CreateIpamOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateIpamOptions(), token);
    }

    public async Task<CommandResult> CreateIpamPool(AwsEc2CreateIpamPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateIpamResourceDiscovery(AwsEc2CreateIpamResourceDiscoveryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateIpamResourceDiscoveryOptions(), token);
    }

    public async Task<CommandResult> CreateIpamScope(AwsEc2CreateIpamScopeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateKeyPair(AwsEc2CreateKeyPairOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLaunchTemplate(AwsEc2CreateLaunchTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLaunchTemplateVersion(AwsEc2CreateLaunchTemplateVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLocalGatewayRoute(AwsEc2CreateLocalGatewayRouteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLocalGatewayRouteTable(AwsEc2CreateLocalGatewayRouteTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLocalGatewayRouteTableVirtualInterfaceGroupAssociation(AwsEc2CreateLocalGatewayRouteTableVirtualInterfaceGroupAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLocalGatewayRouteTableVpcAssociation(AwsEc2CreateLocalGatewayRouteTableVpcAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateManagedPrefixList(AwsEc2CreateManagedPrefixListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateNatGateway(AwsEc2CreateNatGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateNetworkAcl(AwsEc2CreateNetworkAclOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateNetworkAclEntry(AwsEc2CreateNetworkAclEntryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateNetworkInsightsAccessScope(AwsEc2CreateNetworkInsightsAccessScopeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateNetworkInsightsAccessScopeOptions(), token);
    }

    public async Task<CommandResult> CreateNetworkInsightsPath(AwsEc2CreateNetworkInsightsPathOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateNetworkInterface(AwsEc2CreateNetworkInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateNetworkInterfacePermission(AwsEc2CreateNetworkInterfacePermissionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePlacementGroup(AwsEc2CreatePlacementGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreatePlacementGroupOptions(), token);
    }

    public async Task<CommandResult> CreatePublicIpv4Pool(AwsEc2CreatePublicIpv4PoolOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreatePublicIpv4PoolOptions(), token);
    }

    public async Task<CommandResult> CreateReplaceRootVolumeTask(AwsEc2CreateReplaceRootVolumeTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReservedInstancesListing(AwsEc2CreateReservedInstancesListingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRestoreImageTask(AwsEc2CreateRestoreImageTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRoute(AwsEc2CreateRouteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRouteTable(AwsEc2CreateRouteTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSecurityGroup(AwsEc2CreateSecurityGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSnapshot(AwsEc2CreateSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSnapshots(AwsEc2CreateSnapshotsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSpotDatafeedSubscription(AwsEc2CreateSpotDatafeedSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStoreImageTask(AwsEc2CreateStoreImageTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSubnet(AwsEc2CreateSubnetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSubnetCidrReservation(AwsEc2CreateSubnetCidrReservationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTags(AwsEc2CreateTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTrafficMirrorFilter(AwsEc2CreateTrafficMirrorFilterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateTrafficMirrorFilterOptions(), token);
    }

    public async Task<CommandResult> CreateTrafficMirrorFilterRule(AwsEc2CreateTrafficMirrorFilterRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTrafficMirrorSession(AwsEc2CreateTrafficMirrorSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTrafficMirrorTarget(AwsEc2CreateTrafficMirrorTargetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateTrafficMirrorTargetOptions(), token);
    }

    public async Task<CommandResult> CreateTransitGateway(AwsEc2CreateTransitGatewayOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateTransitGatewayOptions(), token);
    }

    public async Task<CommandResult> CreateTransitGatewayConnect(AwsEc2CreateTransitGatewayConnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTransitGatewayConnectPeer(AwsEc2CreateTransitGatewayConnectPeerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTransitGatewayMulticastDomain(AwsEc2CreateTransitGatewayMulticastDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTransitGatewayPeeringAttachment(AwsEc2CreateTransitGatewayPeeringAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTransitGatewayPolicyTable(AwsEc2CreateTransitGatewayPolicyTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTransitGatewayPrefixListReference(AwsEc2CreateTransitGatewayPrefixListReferenceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTransitGatewayRoute(AwsEc2CreateTransitGatewayRouteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTransitGatewayRouteTable(AwsEc2CreateTransitGatewayRouteTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTransitGatewayRouteTableAnnouncement(AwsEc2CreateTransitGatewayRouteTableAnnouncementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTransitGatewayVpcAttachment(AwsEc2CreateTransitGatewayVpcAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVerifiedAccessEndpoint(AwsEc2CreateVerifiedAccessEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVerifiedAccessGroup(AwsEc2CreateVerifiedAccessGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVerifiedAccessInstance(AwsEc2CreateVerifiedAccessInstanceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateVerifiedAccessInstanceOptions(), token);
    }

    public async Task<CommandResult> CreateVerifiedAccessTrustProvider(AwsEc2CreateVerifiedAccessTrustProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVolume(AwsEc2CreateVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVpc(AwsEc2CreateVpcOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateVpcOptions(), token);
    }

    public async Task<CommandResult> CreateVpcEndpoint(AwsEc2CreateVpcEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVpcEndpointConnectionNotification(AwsEc2CreateVpcEndpointConnectionNotificationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVpcEndpointServiceConfiguration(AwsEc2CreateVpcEndpointServiceConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2CreateVpcEndpointServiceConfigurationOptions(), token);
    }

    public async Task<CommandResult> CreateVpcPeeringConnection(AwsEc2CreateVpcPeeringConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVpnConnection(AwsEc2CreateVpnConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVpnConnectionRoute(AwsEc2CreateVpnConnectionRouteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVpnGateway(AwsEc2CreateVpnGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCarrierGateway(AwsEc2DeleteCarrierGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteClientVpnEndpoint(AwsEc2DeleteClientVpnEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteClientVpnRoute(AwsEc2DeleteClientVpnRouteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCoipCidr(AwsEc2DeleteCoipCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCoipPool(AwsEc2DeleteCoipPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomerGateway(AwsEc2DeleteCustomerGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDhcpOptions(AwsEc2DeleteDhcpOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEgressOnlyInternetGateway(AwsEc2DeleteEgressOnlyInternetGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFleets(AwsEc2DeleteFleetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFlowLogs(AwsEc2DeleteFlowLogsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFpgaImage(AwsEc2DeleteFpgaImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInstanceConnectEndpoint(AwsEc2DeleteInstanceConnectEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInstanceEventWindow(AwsEc2DeleteInstanceEventWindowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInternetGateway(AwsEc2DeleteInternetGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIpam(AwsEc2DeleteIpamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIpamPool(AwsEc2DeleteIpamPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIpamResourceDiscovery(AwsEc2DeleteIpamResourceDiscoveryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIpamScope(AwsEc2DeleteIpamScopeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteKeyPair(AwsEc2DeleteKeyPairOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DeleteKeyPairOptions(), token);
    }

    public async Task<CommandResult> DeleteLaunchTemplate(AwsEc2DeleteLaunchTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DeleteLaunchTemplateOptions(), token);
    }

    public async Task<CommandResult> DeleteLaunchTemplateVersions(AwsEc2DeleteLaunchTemplateVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLocalGatewayRoute(AwsEc2DeleteLocalGatewayRouteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLocalGatewayRouteTable(AwsEc2DeleteLocalGatewayRouteTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLocalGatewayRouteTableVirtualInterfaceGroupAssociation(AwsEc2DeleteLocalGatewayRouteTableVirtualInterfaceGroupAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLocalGatewayRouteTableVpcAssociation(AwsEc2DeleteLocalGatewayRouteTableVpcAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteManagedPrefixList(AwsEc2DeleteManagedPrefixListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNatGateway(AwsEc2DeleteNatGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNetworkAcl(AwsEc2DeleteNetworkAclOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNetworkAclEntry(AwsEc2DeleteNetworkAclEntryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNetworkInsightsAccessScope(AwsEc2DeleteNetworkInsightsAccessScopeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNetworkInsightsAccessScopeAnalysis(AwsEc2DeleteNetworkInsightsAccessScopeAnalysisOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNetworkInsightsAnalysis(AwsEc2DeleteNetworkInsightsAnalysisOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNetworkInsightsPath(AwsEc2DeleteNetworkInsightsPathOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNetworkInterface(AwsEc2DeleteNetworkInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNetworkInterfacePermission(AwsEc2DeleteNetworkInterfacePermissionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePlacementGroup(AwsEc2DeletePlacementGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePublicIpv4Pool(AwsEc2DeletePublicIpv4PoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteQueuedReservedInstances(AwsEc2DeleteQueuedReservedInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRoute(AwsEc2DeleteRouteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRouteTable(AwsEc2DeleteRouteTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSecurityGroup(AwsEc2DeleteSecurityGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DeleteSecurityGroupOptions(), token);
    }

    public async Task<CommandResult> DeleteSnapshot(AwsEc2DeleteSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSpotDatafeedSubscription(AwsEc2DeleteSpotDatafeedSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DeleteSpotDatafeedSubscriptionOptions(), token);
    }

    public async Task<CommandResult> DeleteSubnet(AwsEc2DeleteSubnetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSubnetCidrReservation(AwsEc2DeleteSubnetCidrReservationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTags(AwsEc2DeleteTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTrafficMirrorFilter(AwsEc2DeleteTrafficMirrorFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTrafficMirrorFilterRule(AwsEc2DeleteTrafficMirrorFilterRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTrafficMirrorSession(AwsEc2DeleteTrafficMirrorSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTrafficMirrorTarget(AwsEc2DeleteTrafficMirrorTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTransitGateway(AwsEc2DeleteTransitGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTransitGatewayConnect(AwsEc2DeleteTransitGatewayConnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTransitGatewayConnectPeer(AwsEc2DeleteTransitGatewayConnectPeerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTransitGatewayMulticastDomain(AwsEc2DeleteTransitGatewayMulticastDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTransitGatewayPeeringAttachment(AwsEc2DeleteTransitGatewayPeeringAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTransitGatewayPolicyTable(AwsEc2DeleteTransitGatewayPolicyTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTransitGatewayPrefixListReference(AwsEc2DeleteTransitGatewayPrefixListReferenceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTransitGatewayRoute(AwsEc2DeleteTransitGatewayRouteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTransitGatewayRouteTable(AwsEc2DeleteTransitGatewayRouteTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTransitGatewayRouteTableAnnouncement(AwsEc2DeleteTransitGatewayRouteTableAnnouncementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTransitGatewayVpcAttachment(AwsEc2DeleteTransitGatewayVpcAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVerifiedAccessEndpoint(AwsEc2DeleteVerifiedAccessEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVerifiedAccessGroup(AwsEc2DeleteVerifiedAccessGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVerifiedAccessInstance(AwsEc2DeleteVerifiedAccessInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVerifiedAccessTrustProvider(AwsEc2DeleteVerifiedAccessTrustProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVolume(AwsEc2DeleteVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpc(AwsEc2DeleteVpcOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpcEndpointConnectionNotifications(AwsEc2DeleteVpcEndpointConnectionNotificationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpcEndpointServiceConfigurations(AwsEc2DeleteVpcEndpointServiceConfigurationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpcEndpoints(AwsEc2DeleteVpcEndpointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpcPeeringConnection(AwsEc2DeleteVpcPeeringConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpnConnection(AwsEc2DeleteVpnConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpnConnectionRoute(AwsEc2DeleteVpnConnectionRouteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpnGateway(AwsEc2DeleteVpnGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeprovisionByoipCidr(AwsEc2DeprovisionByoipCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeprovisionIpamByoasn(AwsEc2DeprovisionIpamByoasnOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeprovisionIpamPoolCidr(AwsEc2DeprovisionIpamPoolCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeprovisionPublicIpv4PoolCidr(AwsEc2DeprovisionPublicIpv4PoolCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterImage(AwsEc2DeregisterImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterInstanceEventNotificationAttributes(AwsEc2DeregisterInstanceEventNotificationAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterTransitGatewayMulticastGroupMembers(AwsEc2DeregisterTransitGatewayMulticastGroupMembersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DeregisterTransitGatewayMulticastGroupMembersOptions(), token);
    }

    public async Task<CommandResult> DeregisterTransitGatewayMulticastGroupSources(AwsEc2DeregisterTransitGatewayMulticastGroupSourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DeregisterTransitGatewayMulticastGroupSourcesOptions(), token);
    }

    public async Task<CommandResult> DescribeAccountAttributes(AwsEc2DescribeAccountAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAccountAttributesOptions(), token);
    }

    public async Task<CommandResult> DescribeAddressTransfers(AwsEc2DescribeAddressTransfersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAddressTransfersOptions(), token);
    }

    public async Task<CommandResult> DescribeAddresses(AwsEc2DescribeAddressesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAddressesOptions(), token);
    }

    public async Task<CommandResult> DescribeAddressesAttribute(AwsEc2DescribeAddressesAttributeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAddressesAttributeOptions(), token);
    }

    public async Task<CommandResult> DescribeAggregateIdFormat(AwsEc2DescribeAggregateIdFormatOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAggregateIdFormatOptions(), token);
    }

    public async Task<CommandResult> DescribeAvailabilityZones(AwsEc2DescribeAvailabilityZonesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAvailabilityZonesOptions(), token);
    }

    public async Task<CommandResult> DescribeAwsNetworkPerformanceMetricSubscriptions(AwsEc2DescribeAwsNetworkPerformanceMetricSubscriptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeAwsNetworkPerformanceMetricSubscriptionsOptions(), token);
    }

    public async Task<CommandResult> DescribeBundleTasks(AwsEc2DescribeBundleTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeBundleTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeByoipCidrs(AwsEc2DescribeByoipCidrsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeByoipCidrsOptions(), token);
    }

    public async Task<CommandResult> DescribeCapacityBlockOfferings(AwsEc2DescribeCapacityBlockOfferingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCapacityReservationFleets(AwsEc2DescribeCapacityReservationFleetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeCapacityReservationFleetsOptions(), token);
    }

    public async Task<CommandResult> DescribeCapacityReservations(AwsEc2DescribeCapacityReservationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeCapacityReservationsOptions(), token);
    }

    public async Task<CommandResult> DescribeCarrierGateways(AwsEc2DescribeCarrierGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeCarrierGatewaysOptions(), token);
    }

    public async Task<CommandResult> DescribeClassicLinkInstances(AwsEc2DescribeClassicLinkInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeClassicLinkInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeClientVpnAuthorizationRules(AwsEc2DescribeClientVpnAuthorizationRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeClientVpnConnections(AwsEc2DescribeClientVpnConnectionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeClientVpnEndpoints(AwsEc2DescribeClientVpnEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeClientVpnEndpointsOptions(), token);
    }

    public async Task<CommandResult> DescribeClientVpnRoutes(AwsEc2DescribeClientVpnRoutesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeClientVpnTargetNetworks(AwsEc2DescribeClientVpnTargetNetworksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCoipPools(AwsEc2DescribeCoipPoolsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeCoipPoolsOptions(), token);
    }

    public async Task<CommandResult> DescribeConversionTasks(AwsEc2DescribeConversionTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeConversionTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeCustomerGateways(AwsEc2DescribeCustomerGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeCustomerGatewaysOptions(), token);
    }

    public async Task<CommandResult> DescribeDhcpOptions(AwsEc2DescribeDhcpOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeDhcpOptionsOptions(), token);
    }

    public async Task<CommandResult> DescribeEgressOnlyInternetGateways(AwsEc2DescribeEgressOnlyInternetGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeEgressOnlyInternetGatewaysOptions(), token);
    }

    public async Task<CommandResult> DescribeElasticGpus(AwsEc2DescribeElasticGpusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeElasticGpusOptions(), token);
    }

    public async Task<CommandResult> DescribeExportImageTasks(AwsEc2DescribeExportImageTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeExportImageTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeExportTasks(AwsEc2DescribeExportTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeExportTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeFastLaunchImages(AwsEc2DescribeFastLaunchImagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeFastLaunchImagesOptions(), token);
    }

    public async Task<CommandResult> DescribeFastSnapshotRestores(AwsEc2DescribeFastSnapshotRestoresOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeFastSnapshotRestoresOptions(), token);
    }

    public async Task<CommandResult> DescribeFleetHistory(AwsEc2DescribeFleetHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFleetInstances(AwsEc2DescribeFleetInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFleets(AwsEc2DescribeFleetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeFleetsOptions(), token);
    }

    public async Task<CommandResult> DescribeFlowLogs(AwsEc2DescribeFlowLogsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeFlowLogsOptions(), token);
    }

    public async Task<CommandResult> DescribeFpgaImageAttribute(AwsEc2DescribeFpgaImageAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFpgaImages(AwsEc2DescribeFpgaImagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeFpgaImagesOptions(), token);
    }

    public async Task<CommandResult> DescribeHostReservationOfferings(AwsEc2DescribeHostReservationOfferingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeHostReservationOfferingsOptions(), token);
    }

    public async Task<CommandResult> DescribeHostReservations(AwsEc2DescribeHostReservationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeHostReservationsOptions(), token);
    }

    public async Task<CommandResult> DescribeHosts(AwsEc2DescribeHostsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeHostsOptions(), token);
    }

    public async Task<CommandResult> DescribeIamInstanceProfileAssociations(AwsEc2DescribeIamInstanceProfileAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIamInstanceProfileAssociationsOptions(), token);
    }

    public async Task<CommandResult> DescribeIdFormat(AwsEc2DescribeIdFormatOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIdFormatOptions(), token);
    }

    public async Task<CommandResult> DescribeIdentityIdFormat(AwsEc2DescribeIdentityIdFormatOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeImageAttribute(AwsEc2DescribeImageAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeImages(AwsEc2DescribeImagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeImagesOptions(), token);
    }

    public async Task<CommandResult> DescribeImportImageTasks(AwsEc2DescribeImportImageTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeImportImageTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeImportSnapshotTasks(AwsEc2DescribeImportSnapshotTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeImportSnapshotTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeInstanceAttribute(AwsEc2DescribeInstanceAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInstanceConnectEndpoints(AwsEc2DescribeInstanceConnectEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceConnectEndpointsOptions(), token);
    }

    public async Task<CommandResult> DescribeInstanceCreditSpecifications(AwsEc2DescribeInstanceCreditSpecificationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceCreditSpecificationsOptions(), token);
    }

    public async Task<CommandResult> DescribeInstanceEventNotificationAttributes(AwsEc2DescribeInstanceEventNotificationAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceEventNotificationAttributesOptions(), token);
    }

    public async Task<CommandResult> DescribeInstanceEventWindows(AwsEc2DescribeInstanceEventWindowsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceEventWindowsOptions(), token);
    }

    public async Task<CommandResult> DescribeInstanceStatus(AwsEc2DescribeInstanceStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceStatusOptions(), token);
    }

    public async Task<CommandResult> DescribeInstanceTopology(AwsEc2DescribeInstanceTopologyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceTopologyOptions(), token);
    }

    public async Task<CommandResult> DescribeInstanceTypeOfferings(AwsEc2DescribeInstanceTypeOfferingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceTypeOfferingsOptions(), token);
    }

    public async Task<CommandResult> DescribeInstanceTypes(AwsEc2DescribeInstanceTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstanceTypesOptions(), token);
    }

    public async Task<CommandResult> DescribeInstances(AwsEc2DescribeInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeInternetGateways(AwsEc2DescribeInternetGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeInternetGatewaysOptions(), token);
    }

    public async Task<CommandResult> DescribeIpamByoasn(AwsEc2DescribeIpamByoasnOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpamByoasnOptions(), token);
    }

    public async Task<CommandResult> DescribeIpamPools(AwsEc2DescribeIpamPoolsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpamPoolsOptions(), token);
    }

    public async Task<CommandResult> DescribeIpamResourceDiscoveries(AwsEc2DescribeIpamResourceDiscoveriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpamResourceDiscoveriesOptions(), token);
    }

    public async Task<CommandResult> DescribeIpamResourceDiscoveryAssociations(AwsEc2DescribeIpamResourceDiscoveryAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpamResourceDiscoveryAssociationsOptions(), token);
    }

    public async Task<CommandResult> DescribeIpamScopes(AwsEc2DescribeIpamScopesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpamScopesOptions(), token);
    }

    public async Task<CommandResult> DescribeIpams(AwsEc2DescribeIpamsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpamsOptions(), token);
    }

    public async Task<CommandResult> DescribeIpv6Pools(AwsEc2DescribeIpv6PoolsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeIpv6PoolsOptions(), token);
    }

    public async Task<CommandResult> DescribeKeyPairs(AwsEc2DescribeKeyPairsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeKeyPairsOptions(), token);
    }

    public async Task<CommandResult> DescribeLaunchTemplateVersions(AwsEc2DescribeLaunchTemplateVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLaunchTemplateVersionsOptions(), token);
    }

    public async Task<CommandResult> DescribeLaunchTemplates(AwsEc2DescribeLaunchTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLaunchTemplatesOptions(), token);
    }

    public async Task<CommandResult> DescribeLocalGatewayRouteTableVirtualInterfaceGroupAssociations(AwsEc2DescribeLocalGatewayRouteTableVirtualInterfaceGroupAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLocalGatewayRouteTableVirtualInterfaceGroupAssociationsOptions(), token);
    }

    public async Task<CommandResult> DescribeLocalGatewayRouteTableVpcAssociations(AwsEc2DescribeLocalGatewayRouteTableVpcAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLocalGatewayRouteTableVpcAssociationsOptions(), token);
    }

    public async Task<CommandResult> DescribeLocalGatewayRouteTables(AwsEc2DescribeLocalGatewayRouteTablesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLocalGatewayRouteTablesOptions(), token);
    }

    public async Task<CommandResult> DescribeLocalGatewayVirtualInterfaceGroups(AwsEc2DescribeLocalGatewayVirtualInterfaceGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLocalGatewayVirtualInterfaceGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeLocalGatewayVirtualInterfaces(AwsEc2DescribeLocalGatewayVirtualInterfacesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLocalGatewayVirtualInterfacesOptions(), token);
    }

    public async Task<CommandResult> DescribeLocalGateways(AwsEc2DescribeLocalGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLocalGatewaysOptions(), token);
    }

    public async Task<CommandResult> DescribeLockedSnapshots(AwsEc2DescribeLockedSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeLockedSnapshotsOptions(), token);
    }

    public async Task<CommandResult> DescribeManagedPrefixLists(AwsEc2DescribeManagedPrefixListsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeManagedPrefixListsOptions(), token);
    }

    public async Task<CommandResult> DescribeMovingAddresses(AwsEc2DescribeMovingAddressesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeMovingAddressesOptions(), token);
    }

    public async Task<CommandResult> DescribeNatGateways(AwsEc2DescribeNatGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNatGatewaysOptions(), token);
    }

    public async Task<CommandResult> DescribeNetworkAcls(AwsEc2DescribeNetworkAclsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkAclsOptions(), token);
    }

    public async Task<CommandResult> DescribeNetworkInsightsAccessScopeAnalyses(AwsEc2DescribeNetworkInsightsAccessScopeAnalysesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkInsightsAccessScopeAnalysesOptions(), token);
    }

    public async Task<CommandResult> DescribeNetworkInsightsAccessScopes(AwsEc2DescribeNetworkInsightsAccessScopesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkInsightsAccessScopesOptions(), token);
    }

    public async Task<CommandResult> DescribeNetworkInsightsAnalyses(AwsEc2DescribeNetworkInsightsAnalysesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkInsightsAnalysesOptions(), token);
    }

    public async Task<CommandResult> DescribeNetworkInsightsPaths(AwsEc2DescribeNetworkInsightsPathsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkInsightsPathsOptions(), token);
    }

    public async Task<CommandResult> DescribeNetworkInterfaceAttribute(AwsEc2DescribeNetworkInterfaceAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeNetworkInterfacePermissions(AwsEc2DescribeNetworkInterfacePermissionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkInterfacePermissionsOptions(), token);
    }

    public async Task<CommandResult> DescribeNetworkInterfaces(AwsEc2DescribeNetworkInterfacesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeNetworkInterfacesOptions(), token);
    }

    public async Task<CommandResult> DescribePlacementGroups(AwsEc2DescribePlacementGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribePlacementGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribePrefixLists(AwsEc2DescribePrefixListsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribePrefixListsOptions(), token);
    }

    public async Task<CommandResult> DescribePrincipalIdFormat(AwsEc2DescribePrincipalIdFormatOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribePrincipalIdFormatOptions(), token);
    }

    public async Task<CommandResult> DescribePublicIpv4Pools(AwsEc2DescribePublicIpv4PoolsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribePublicIpv4PoolsOptions(), token);
    }

    public async Task<CommandResult> DescribeRegions(AwsEc2DescribeRegionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeRegionsOptions(), token);
    }

    public async Task<CommandResult> DescribeReplaceRootVolumeTasks(AwsEc2DescribeReplaceRootVolumeTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeReplaceRootVolumeTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeReservedInstances(AwsEc2DescribeReservedInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeReservedInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeReservedInstancesListings(AwsEc2DescribeReservedInstancesListingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeReservedInstancesListingsOptions(), token);
    }

    public async Task<CommandResult> DescribeReservedInstancesModifications(AwsEc2DescribeReservedInstancesModificationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeReservedInstancesModificationsOptions(), token);
    }

    public async Task<CommandResult> DescribeReservedInstancesOfferings(AwsEc2DescribeReservedInstancesOfferingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeReservedInstancesOfferingsOptions(), token);
    }

    public async Task<CommandResult> DescribeRouteTables(AwsEc2DescribeRouteTablesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeRouteTablesOptions(), token);
    }

    public async Task<CommandResult> DescribeScheduledInstanceAvailability(AwsEc2DescribeScheduledInstanceAvailabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeScheduledInstances(AwsEc2DescribeScheduledInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeScheduledInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeSecurityGroupReferences(AwsEc2DescribeSecurityGroupReferencesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSecurityGroupRules(AwsEc2DescribeSecurityGroupRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSecurityGroupRulesOptions(), token);
    }

    public async Task<CommandResult> DescribeSecurityGroups(AwsEc2DescribeSecurityGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSecurityGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeSnapshotAttribute(AwsEc2DescribeSnapshotAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSnapshotTierStatus(AwsEc2DescribeSnapshotTierStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSnapshotTierStatusOptions(), token);
    }

    public async Task<CommandResult> DescribeSnapshots(AwsEc2DescribeSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSnapshotsOptions(), token);
    }

    public async Task<CommandResult> DescribeSpotDatafeedSubscription(AwsEc2DescribeSpotDatafeedSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSpotDatafeedSubscriptionOptions(), token);
    }

    public async Task<CommandResult> DescribeSpotFleetInstances(AwsEc2DescribeSpotFleetInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSpotFleetRequestHistory(AwsEc2DescribeSpotFleetRequestHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSpotFleetRequests(AwsEc2DescribeSpotFleetRequestsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSpotFleetRequestsOptions(), token);
    }

    public async Task<CommandResult> DescribeSpotInstanceRequests(AwsEc2DescribeSpotInstanceRequestsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSpotInstanceRequestsOptions(), token);
    }

    public async Task<CommandResult> DescribeSpotPriceHistory(AwsEc2DescribeSpotPriceHistoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSpotPriceHistoryOptions(), token);
    }

    public async Task<CommandResult> DescribeStaleSecurityGroups(AwsEc2DescribeStaleSecurityGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStoreImageTasks(AwsEc2DescribeStoreImageTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeStoreImageTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeSubnets(AwsEc2DescribeSubnetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeSubnetsOptions(), token);
    }

    public async Task<CommandResult> DescribeTags(AwsEc2DescribeTagsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTagsOptions(), token);
    }

    public async Task<CommandResult> DescribeTrafficMirrorFilters(AwsEc2DescribeTrafficMirrorFiltersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTrafficMirrorFiltersOptions(), token);
    }

    public async Task<CommandResult> DescribeTrafficMirrorSessions(AwsEc2DescribeTrafficMirrorSessionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTrafficMirrorSessionsOptions(), token);
    }

    public async Task<CommandResult> DescribeTrafficMirrorTargets(AwsEc2DescribeTrafficMirrorTargetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTrafficMirrorTargetsOptions(), token);
    }

    public async Task<CommandResult> DescribeTransitGatewayAttachments(AwsEc2DescribeTransitGatewayAttachmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayAttachmentsOptions(), token);
    }

    public async Task<CommandResult> DescribeTransitGatewayConnectPeers(AwsEc2DescribeTransitGatewayConnectPeersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayConnectPeersOptions(), token);
    }

    public async Task<CommandResult> DescribeTransitGatewayConnects(AwsEc2DescribeTransitGatewayConnectsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayConnectsOptions(), token);
    }

    public async Task<CommandResult> DescribeTransitGatewayMulticastDomains(AwsEc2DescribeTransitGatewayMulticastDomainsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayMulticastDomainsOptions(), token);
    }

    public async Task<CommandResult> DescribeTransitGatewayPeeringAttachments(AwsEc2DescribeTransitGatewayPeeringAttachmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayPeeringAttachmentsOptions(), token);
    }

    public async Task<CommandResult> DescribeTransitGatewayPolicyTables(AwsEc2DescribeTransitGatewayPolicyTablesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayPolicyTablesOptions(), token);
    }

    public async Task<CommandResult> DescribeTransitGatewayRouteTableAnnouncements(AwsEc2DescribeTransitGatewayRouteTableAnnouncementsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayRouteTableAnnouncementsOptions(), token);
    }

    public async Task<CommandResult> DescribeTransitGatewayRouteTables(AwsEc2DescribeTransitGatewayRouteTablesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayRouteTablesOptions(), token);
    }

    public async Task<CommandResult> DescribeTransitGatewayVpcAttachments(AwsEc2DescribeTransitGatewayVpcAttachmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewayVpcAttachmentsOptions(), token);
    }

    public async Task<CommandResult> DescribeTransitGateways(AwsEc2DescribeTransitGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTransitGatewaysOptions(), token);
    }

    public async Task<CommandResult> DescribeTrunkInterfaceAssociations(AwsEc2DescribeTrunkInterfaceAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeTrunkInterfaceAssociationsOptions(), token);
    }

    public async Task<CommandResult> DescribeVerifiedAccessEndpoints(AwsEc2DescribeVerifiedAccessEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVerifiedAccessEndpointsOptions(), token);
    }

    public async Task<CommandResult> DescribeVerifiedAccessGroups(AwsEc2DescribeVerifiedAccessGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVerifiedAccessGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeVerifiedAccessInstanceLoggingConfigurations(AwsEc2DescribeVerifiedAccessInstanceLoggingConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVerifiedAccessInstanceLoggingConfigurationsOptions(), token);
    }

    public async Task<CommandResult> DescribeVerifiedAccessInstances(AwsEc2DescribeVerifiedAccessInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVerifiedAccessInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeVerifiedAccessTrustProviders(AwsEc2DescribeVerifiedAccessTrustProvidersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVerifiedAccessTrustProvidersOptions(), token);
    }

    public async Task<CommandResult> DescribeVolumeAttribute(AwsEc2DescribeVolumeAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeVolumeStatus(AwsEc2DescribeVolumeStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVolumeStatusOptions(), token);
    }

    public async Task<CommandResult> DescribeVolumes(AwsEc2DescribeVolumesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVolumesOptions(), token);
    }

    public async Task<CommandResult> DescribeVolumesModifications(AwsEc2DescribeVolumesModificationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVolumesModificationsOptions(), token);
    }

    public async Task<CommandResult> DescribeVpcAttribute(AwsEc2DescribeVpcAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeVpcClassicLink(AwsEc2DescribeVpcClassicLinkOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcClassicLinkOptions(), token);
    }

    public async Task<CommandResult> DescribeVpcClassicLinkDnsSupport(AwsEc2DescribeVpcClassicLinkDnsSupportOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcClassicLinkDnsSupportOptions(), token);
    }

    public async Task<CommandResult> DescribeVpcEndpointConnectionNotifications(AwsEc2DescribeVpcEndpointConnectionNotificationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcEndpointConnectionNotificationsOptions(), token);
    }

    public async Task<CommandResult> DescribeVpcEndpointConnections(AwsEc2DescribeVpcEndpointConnectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcEndpointConnectionsOptions(), token);
    }

    public async Task<CommandResult> DescribeVpcEndpointServiceConfigurations(AwsEc2DescribeVpcEndpointServiceConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcEndpointServiceConfigurationsOptions(), token);
    }

    public async Task<CommandResult> DescribeVpcEndpointServicePermissions(AwsEc2DescribeVpcEndpointServicePermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeVpcEndpointServices(AwsEc2DescribeVpcEndpointServicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcEndpointServicesOptions(), token);
    }

    public async Task<CommandResult> DescribeVpcEndpoints(AwsEc2DescribeVpcEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcEndpointsOptions(), token);
    }

    public async Task<CommandResult> DescribeVpcPeeringConnections(AwsEc2DescribeVpcPeeringConnectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcPeeringConnectionsOptions(), token);
    }

    public async Task<CommandResult> DescribeVpcs(AwsEc2DescribeVpcsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpcsOptions(), token);
    }

    public async Task<CommandResult> DescribeVpnConnections(AwsEc2DescribeVpnConnectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpnConnectionsOptions(), token);
    }

    public async Task<CommandResult> DescribeVpnGateways(AwsEc2DescribeVpnGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DescribeVpnGatewaysOptions(), token);
    }

    public async Task<CommandResult> DetachClassicLinkVpc(AwsEc2DetachClassicLinkVpcOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachInternetGateway(AwsEc2DetachInternetGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachNetworkInterface(AwsEc2DetachNetworkInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachVerifiedAccessTrustProvider(AwsEc2DetachVerifiedAccessTrustProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachVolume(AwsEc2DetachVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachVpnGateway(AwsEc2DetachVpnGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableAddressTransfer(AwsEc2DisableAddressTransferOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableAwsNetworkPerformanceMetricSubscription(AwsEc2DisableAwsNetworkPerformanceMetricSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisableAwsNetworkPerformanceMetricSubscriptionOptions(), token);
    }

    public async Task<CommandResult> DisableEbsEncryptionByDefault(AwsEc2DisableEbsEncryptionByDefaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisableEbsEncryptionByDefaultOptions(), token);
    }

    public async Task<CommandResult> DisableFastLaunch(AwsEc2DisableFastLaunchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableFastSnapshotRestores(AwsEc2DisableFastSnapshotRestoresOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableImage(AwsEc2DisableImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableImageBlockPublicAccess(AwsEc2DisableImageBlockPublicAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisableImageBlockPublicAccessOptions(), token);
    }

    public async Task<CommandResult> DisableImageDeprecation(AwsEc2DisableImageDeprecationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableIpamOrganizationAdminAccount(AwsEc2DisableIpamOrganizationAdminAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableSerialConsoleAccess(AwsEc2DisableSerialConsoleAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisableSerialConsoleAccessOptions(), token);
    }

    public async Task<CommandResult> DisableSnapshotBlockPublicAccess(AwsEc2DisableSnapshotBlockPublicAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisableSnapshotBlockPublicAccessOptions(), token);
    }

    public async Task<CommandResult> DisableTransitGatewayRouteTablePropagation(AwsEc2DisableTransitGatewayRouteTablePropagationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableVgwRoutePropagation(AwsEc2DisableVgwRoutePropagationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableVpcClassicLink(AwsEc2DisableVpcClassicLinkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableVpcClassicLinkDnsSupport(AwsEc2DisableVpcClassicLinkDnsSupportOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisableVpcClassicLinkDnsSupportOptions(), token);
    }

    public async Task<CommandResult> DisassociateAddress(AwsEc2DisassociateAddressOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2DisassociateAddressOptions(), token);
    }

    public async Task<CommandResult> DisassociateClientVpnTargetNetwork(AwsEc2DisassociateClientVpnTargetNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateEnclaveCertificateIamRole(AwsEc2DisassociateEnclaveCertificateIamRoleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateIamInstanceProfile(AwsEc2DisassociateIamInstanceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateInstanceEventWindow(AwsEc2DisassociateInstanceEventWindowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateIpamByoasn(AwsEc2DisassociateIpamByoasnOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateIpamResourceDiscovery(AwsEc2DisassociateIpamResourceDiscoveryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateNatGatewayAddress(AwsEc2DisassociateNatGatewayAddressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateRouteTable(AwsEc2DisassociateRouteTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateSubnetCidrBlock(AwsEc2DisassociateSubnetCidrBlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateTransitGatewayMulticastDomain(AwsEc2DisassociateTransitGatewayMulticastDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateTransitGatewayPolicyTable(AwsEc2DisassociateTransitGatewayPolicyTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateTransitGatewayRouteTable(AwsEc2DisassociateTransitGatewayRouteTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateTrunkInterface(AwsEc2DisassociateTrunkInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateVpcCidrBlock(AwsEc2DisassociateVpcCidrBlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableAddressTransfer(AwsEc2EnableAddressTransferOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableAwsNetworkPerformanceMetricSubscription(AwsEc2EnableAwsNetworkPerformanceMetricSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2EnableAwsNetworkPerformanceMetricSubscriptionOptions(), token);
    }

    public async Task<CommandResult> EnableEbsEncryptionByDefault(AwsEc2EnableEbsEncryptionByDefaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2EnableEbsEncryptionByDefaultOptions(), token);
    }

    public async Task<CommandResult> EnableFastLaunch(AwsEc2EnableFastLaunchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableFastSnapshotRestores(AwsEc2EnableFastSnapshotRestoresOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableImage(AwsEc2EnableImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableImageBlockPublicAccess(AwsEc2EnableImageBlockPublicAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableImageDeprecation(AwsEc2EnableImageDeprecationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableIpamOrganizationAdminAccount(AwsEc2EnableIpamOrganizationAdminAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableReachabilityAnalyzerOrganizationSharing(AwsEc2EnableReachabilityAnalyzerOrganizationSharingOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2EnableReachabilityAnalyzerOrganizationSharingOptions(), token);
    }

    public async Task<CommandResult> EnableSerialConsoleAccess(AwsEc2EnableSerialConsoleAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2EnableSerialConsoleAccessOptions(), token);
    }

    public async Task<CommandResult> EnableSnapshotBlockPublicAccess(AwsEc2EnableSnapshotBlockPublicAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableTransitGatewayRouteTablePropagation(AwsEc2EnableTransitGatewayRouteTablePropagationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableVgwRoutePropagation(AwsEc2EnableVgwRoutePropagationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableVolumeIo(AwsEc2EnableVolumeIoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableVpcClassicLink(AwsEc2EnableVpcClassicLinkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableVpcClassicLinkDnsSupport(AwsEc2EnableVpcClassicLinkDnsSupportOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2EnableVpcClassicLinkDnsSupportOptions(), token);
    }

    public async Task<CommandResult> ExportClientVpnClientCertificateRevocationList(AwsEc2ExportClientVpnClientCertificateRevocationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportClientVpnClientConfiguration(AwsEc2ExportClientVpnClientConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportImage(AwsEc2ExportImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportTransitGatewayRoutes(AwsEc2ExportTransitGatewayRoutesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAssociatedEnclaveCertificateIamRoles(AwsEc2GetAssociatedEnclaveCertificateIamRolesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAssociatedIpv6PoolCidrs(AwsEc2GetAssociatedIpv6PoolCidrsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAwsNetworkPerformanceData(AwsEc2GetAwsNetworkPerformanceDataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetAwsNetworkPerformanceDataOptions(), token);
    }

    public async Task<CommandResult> GetCapacityReservationUsage(AwsEc2GetCapacityReservationUsageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCoipPoolUsage(AwsEc2GetCoipPoolUsageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConsoleOutput(AwsEc2GetConsoleOutputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConsoleScreenshot(AwsEc2GetConsoleScreenshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDefaultCreditSpecification(AwsEc2GetDefaultCreditSpecificationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEbsDefaultKmsKeyId(AwsEc2GetEbsDefaultKmsKeyIdOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetEbsDefaultKmsKeyIdOptions(), token);
    }

    public async Task<CommandResult> GetEbsEncryptionByDefault(AwsEc2GetEbsEncryptionByDefaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetEbsEncryptionByDefaultOptions(), token);
    }

    public async Task<CommandResult> GetFlowLogsIntegrationTemplate(AwsEc2GetFlowLogsIntegrationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGroupsForCapacityReservation(AwsEc2GetGroupsForCapacityReservationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetHostReservationPurchasePreview(AwsEc2GetHostReservationPurchasePreviewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetImageBlockPublicAccessState(AwsEc2GetImageBlockPublicAccessStateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetImageBlockPublicAccessStateOptions(), token);
    }

    public async Task<CommandResult> GetInstanceTypesFromInstanceRequirements(AwsEc2GetInstanceTypesFromInstanceRequirementsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInstanceUefiData(AwsEc2GetInstanceUefiDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIpamAddressHistory(AwsEc2GetIpamAddressHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIpamDiscoveredAccounts(AwsEc2GetIpamDiscoveredAccountsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIpamDiscoveredPublicAddresses(AwsEc2GetIpamDiscoveredPublicAddressesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIpamDiscoveredResourceCidrs(AwsEc2GetIpamDiscoveredResourceCidrsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIpamPoolAllocations(AwsEc2GetIpamPoolAllocationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIpamPoolCidrs(AwsEc2GetIpamPoolCidrsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIpamResourceCidrs(AwsEc2GetIpamResourceCidrsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLaunchTemplateData(AwsEc2GetLaunchTemplateDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetManagedPrefixListAssociations(AwsEc2GetManagedPrefixListAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetManagedPrefixListEntries(AwsEc2GetManagedPrefixListEntriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNetworkInsightsAccessScopeAnalysisFindings(AwsEc2GetNetworkInsightsAccessScopeAnalysisFindingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNetworkInsightsAccessScopeContent(AwsEc2GetNetworkInsightsAccessScopeContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPasswordData(AwsEc2GetPasswordDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetReservedInstancesExchangeQuote(AwsEc2GetReservedInstancesExchangeQuoteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSecurityGroupsForVpc(AwsEc2GetSecurityGroupsForVpcOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSerialConsoleAccessStatus(AwsEc2GetSerialConsoleAccessStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetSerialConsoleAccessStatusOptions(), token);
    }

    public async Task<CommandResult> GetSnapshotBlockPublicAccessState(AwsEc2GetSnapshotBlockPublicAccessStateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetSnapshotBlockPublicAccessStateOptions(), token);
    }

    public async Task<CommandResult> GetSpotPlacementScores(AwsEc2GetSpotPlacementScoresOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSubnetCidrReservations(AwsEc2GetSubnetCidrReservationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTransitGatewayAttachmentPropagations(AwsEc2GetTransitGatewayAttachmentPropagationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTransitGatewayMulticastDomainAssociations(AwsEc2GetTransitGatewayMulticastDomainAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTransitGatewayPolicyTableAssociations(AwsEc2GetTransitGatewayPolicyTableAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTransitGatewayPolicyTableEntries(AwsEc2GetTransitGatewayPolicyTableEntriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTransitGatewayPrefixListReferences(AwsEc2GetTransitGatewayPrefixListReferencesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTransitGatewayRouteTableAssociations(AwsEc2GetTransitGatewayRouteTableAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTransitGatewayRouteTablePropagations(AwsEc2GetTransitGatewayRouteTablePropagationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVerifiedAccessEndpointPolicy(AwsEc2GetVerifiedAccessEndpointPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVerifiedAccessGroupPolicy(AwsEc2GetVerifiedAccessGroupPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVpnConnectionDeviceSampleConfiguration(AwsEc2GetVpnConnectionDeviceSampleConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVpnConnectionDeviceTypes(AwsEc2GetVpnConnectionDeviceTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2GetVpnConnectionDeviceTypesOptions(), token);
    }

    public async Task<CommandResult> GetVpnTunnelReplacementStatus(AwsEc2GetVpnTunnelReplacementStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportClientVpnClientCertificateRevocationList(AwsEc2ImportClientVpnClientCertificateRevocationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportImage(AwsEc2ImportImageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ImportImageOptions(), token);
    }

    public async Task<CommandResult> ImportKeyPair(AwsEc2ImportKeyPairOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportSnapshot(AwsEc2ImportSnapshotOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ImportSnapshotOptions(), token);
    }

    public async Task<CommandResult> ListImagesInRecycleBin(AwsEc2ListImagesInRecycleBinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ListImagesInRecycleBinOptions(), token);
    }

    public async Task<CommandResult> ListSnapshotsInRecycleBin(AwsEc2ListSnapshotsInRecycleBinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ListSnapshotsInRecycleBinOptions(), token);
    }

    public async Task<CommandResult> LockSnapshot(AwsEc2LockSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyAddressAttribute(AwsEc2ModifyAddressAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyAvailabilityZoneGroup(AwsEc2ModifyAvailabilityZoneGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyCapacityReservation(AwsEc2ModifyCapacityReservationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyCapacityReservationFleet(AwsEc2ModifyCapacityReservationFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyClientVpnEndpoint(AwsEc2ModifyClientVpnEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDefaultCreditSpecification(AwsEc2ModifyDefaultCreditSpecificationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyEbsDefaultKmsKeyId(AwsEc2ModifyEbsDefaultKmsKeyIdOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyFleet(AwsEc2ModifyFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyFpgaImageAttribute(AwsEc2ModifyFpgaImageAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyHosts(AwsEc2ModifyHostsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyIdFormat(AwsEc2ModifyIdFormatOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyIdentityIdFormat(AwsEc2ModifyIdentityIdFormatOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyImageAttribute(AwsEc2ModifyImageAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyInstanceAttribute(AwsEc2ModifyInstanceAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyInstanceCapacityReservationAttributes(AwsEc2ModifyInstanceCapacityReservationAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyInstanceCreditSpecification(AwsEc2ModifyInstanceCreditSpecificationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyInstanceEventStartTime(AwsEc2ModifyInstanceEventStartTimeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyInstanceEventWindow(AwsEc2ModifyInstanceEventWindowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyInstanceMaintenanceOptions(AwsEc2ModifyInstanceMaintenanceOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyInstanceMetadataOptions(AwsEc2ModifyInstanceMetadataOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyInstancePlacement(AwsEc2ModifyInstancePlacementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyIpam(AwsEc2ModifyIpamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyIpamPool(AwsEc2ModifyIpamPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyIpamResourceCidr(AwsEc2ModifyIpamResourceCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyIpamResourceDiscovery(AwsEc2ModifyIpamResourceDiscoveryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyIpamScope(AwsEc2ModifyIpamScopeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyLaunchTemplate(AwsEc2ModifyLaunchTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ModifyLaunchTemplateOptions(), token);
    }

    public async Task<CommandResult> ModifyLocalGatewayRoute(AwsEc2ModifyLocalGatewayRouteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyManagedPrefixList(AwsEc2ModifyManagedPrefixListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyNetworkInterfaceAttribute(AwsEc2ModifyNetworkInterfaceAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyPrivateDnsNameOptions(AwsEc2ModifyPrivateDnsNameOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyReservedInstances(AwsEc2ModifyReservedInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifySecurityGroupRules(AwsEc2ModifySecurityGroupRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifySnapshotAttribute(AwsEc2ModifySnapshotAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifySnapshotTier(AwsEc2ModifySnapshotTierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifySpotFleetRequest(AwsEc2ModifySpotFleetRequestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifySubnetAttribute(AwsEc2ModifySubnetAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyTrafficMirrorFilterNetworkServices(AwsEc2ModifyTrafficMirrorFilterNetworkServicesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyTrafficMirrorFilterRule(AwsEc2ModifyTrafficMirrorFilterRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyTrafficMirrorSession(AwsEc2ModifyTrafficMirrorSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyTransitGateway(AwsEc2ModifyTransitGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyTransitGatewayPrefixListReference(AwsEc2ModifyTransitGatewayPrefixListReferenceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyTransitGatewayVpcAttachment(AwsEc2ModifyTransitGatewayVpcAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVerifiedAccessEndpoint(AwsEc2ModifyVerifiedAccessEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVerifiedAccessEndpointPolicy(AwsEc2ModifyVerifiedAccessEndpointPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVerifiedAccessGroup(AwsEc2ModifyVerifiedAccessGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVerifiedAccessGroupPolicy(AwsEc2ModifyVerifiedAccessGroupPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVerifiedAccessInstance(AwsEc2ModifyVerifiedAccessInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVerifiedAccessInstanceLoggingConfiguration(AwsEc2ModifyVerifiedAccessInstanceLoggingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVerifiedAccessTrustProvider(AwsEc2ModifyVerifiedAccessTrustProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVolume(AwsEc2ModifyVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVolumeAttribute(AwsEc2ModifyVolumeAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVpcAttribute(AwsEc2ModifyVpcAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVpcEndpoint(AwsEc2ModifyVpcEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVpcEndpointConnectionNotification(AwsEc2ModifyVpcEndpointConnectionNotificationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVpcEndpointServiceConfiguration(AwsEc2ModifyVpcEndpointServiceConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVpcEndpointServicePayerResponsibility(AwsEc2ModifyVpcEndpointServicePayerResponsibilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVpcEndpointServicePermissions(AwsEc2ModifyVpcEndpointServicePermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVpcPeeringConnectionOptions(AwsEc2ModifyVpcPeeringConnectionOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVpcTenancy(AwsEc2ModifyVpcTenancyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVpnConnection(AwsEc2ModifyVpnConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVpnConnectionOptions(AwsEc2ModifyVpnConnectionOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVpnTunnelCertificate(AwsEc2ModifyVpnTunnelCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyVpnTunnelOptions(AwsEc2ModifyVpnTunnelOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MonitorInstances(AwsEc2MonitorInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MoveAddressToVpc(AwsEc2MoveAddressToVpcOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MoveByoipCidrToIpam(AwsEc2MoveByoipCidrToIpamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ProvisionByoipCidr(AwsEc2ProvisionByoipCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ProvisionIpamByoasn(AwsEc2ProvisionIpamByoasnOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ProvisionIpamPoolCidr(AwsEc2ProvisionIpamPoolCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ProvisionPublicIpv4PoolCidr(AwsEc2ProvisionPublicIpv4PoolCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PurchaseCapacityBlock(AwsEc2PurchaseCapacityBlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PurchaseHostReservation(AwsEc2PurchaseHostReservationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PurchaseReservedInstancesOffering(AwsEc2PurchaseReservedInstancesOfferingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PurchaseScheduledInstances(AwsEc2PurchaseScheduledInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebootInstances(AwsEc2RebootInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterImage(AwsEc2RegisterImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterInstanceEventNotificationAttributes(AwsEc2RegisterInstanceEventNotificationAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterTransitGatewayMulticastGroupMembers(AwsEc2RegisterTransitGatewayMulticastGroupMembersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterTransitGatewayMulticastGroupSources(AwsEc2RegisterTransitGatewayMulticastGroupSourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectTransitGatewayMulticastDomainAssociations(AwsEc2RejectTransitGatewayMulticastDomainAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2RejectTransitGatewayMulticastDomainAssociationsOptions(), token);
    }

    public async Task<CommandResult> RejectTransitGatewayPeeringAttachment(AwsEc2RejectTransitGatewayPeeringAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectTransitGatewayVpcAttachment(AwsEc2RejectTransitGatewayVpcAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectVpcEndpointConnections(AwsEc2RejectVpcEndpointConnectionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectVpcPeeringConnection(AwsEc2RejectVpcPeeringConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReleaseAddress(AwsEc2ReleaseAddressOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ReleaseAddressOptions(), token);
    }

    public async Task<CommandResult> ReleaseHosts(AwsEc2ReleaseHostsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReleaseIpamPoolAllocation(AwsEc2ReleaseIpamPoolAllocationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReplaceIamInstanceProfileAssociation(AwsEc2ReplaceIamInstanceProfileAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReplaceNetworkAclAssociation(AwsEc2ReplaceNetworkAclAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReplaceNetworkAclEntry(AwsEc2ReplaceNetworkAclEntryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReplaceRoute(AwsEc2ReplaceRouteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReplaceRouteTableAssociation(AwsEc2ReplaceRouteTableAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReplaceTransitGatewayRoute(AwsEc2ReplaceTransitGatewayRouteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReplaceVpnTunnel(AwsEc2ReplaceVpnTunnelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReportInstanceStatus(AwsEc2ReportInstanceStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RequestSpotFleet(AwsEc2RequestSpotFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RequestSpotInstances(AwsEc2RequestSpotInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2RequestSpotInstancesOptions(), token);
    }

    public async Task<CommandResult> ResetAddressAttribute(AwsEc2ResetAddressAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetEbsDefaultKmsKeyId(AwsEc2ResetEbsDefaultKmsKeyIdOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2ResetEbsDefaultKmsKeyIdOptions(), token);
    }

    public async Task<CommandResult> ResetFpgaImageAttribute(AwsEc2ResetFpgaImageAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetImageAttribute(AwsEc2ResetImageAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetInstanceAttribute(AwsEc2ResetInstanceAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetNetworkInterfaceAttribute(AwsEc2ResetNetworkInterfaceAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetSnapshotAttribute(AwsEc2ResetSnapshotAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreAddressToClassic(AwsEc2RestoreAddressToClassicOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreImageFromRecycleBin(AwsEc2RestoreImageFromRecycleBinOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreManagedPrefixListVersion(AwsEc2RestoreManagedPrefixListVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreSnapshotFromRecycleBin(AwsEc2RestoreSnapshotFromRecycleBinOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreSnapshotTier(AwsEc2RestoreSnapshotTierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RevokeClientVpnIngress(AwsEc2RevokeClientVpnIngressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RevokeSecurityGroupEgress(AwsEc2RevokeSecurityGroupEgressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RevokeSecurityGroupIngress(AwsEc2RevokeSecurityGroupIngressOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2RevokeSecurityGroupIngressOptions(), token);
    }

    public async Task<CommandResult> RunInstances(AwsEc2RunInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2RunInstancesOptions(), token);
    }

    public async Task<CommandResult> RunScheduledInstances(AwsEc2RunScheduledInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchLocalGatewayRoutes(AwsEc2SearchLocalGatewayRoutesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchTransitGatewayMulticastGroups(AwsEc2SearchTransitGatewayMulticastGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchTransitGatewayRoutes(AwsEc2SearchTransitGatewayRoutesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendDiagnosticInterrupt(AwsEc2SendDiagnosticInterruptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartInstances(AwsEc2StartInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartNetworkInsightsAccessScopeAnalysis(AwsEc2StartNetworkInsightsAccessScopeAnalysisOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartNetworkInsightsAnalysis(AwsEc2StartNetworkInsightsAnalysisOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartVpcEndpointServicePrivateDnsVerification(AwsEc2StartVpcEndpointServicePrivateDnsVerificationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopInstances(AwsEc2StopInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TerminateClientVpnConnections(AwsEc2TerminateClientVpnConnectionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TerminateInstances(AwsEc2TerminateInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnassignIpv6Addresses(AwsEc2UnassignIpv6AddressesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnassignPrivateIpAddresses(AwsEc2UnassignPrivateIpAddressesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnassignPrivateNatGatewayAddress(AwsEc2UnassignPrivateNatGatewayAddressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnlockSnapshot(AwsEc2UnlockSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnmonitorInstances(AwsEc2UnmonitorInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSecurityGroupRuleDescriptionsEgress(AwsEc2UpdateSecurityGroupRuleDescriptionsEgressOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2UpdateSecurityGroupRuleDescriptionsEgressOptions(), token);
    }

    public async Task<CommandResult> UpdateSecurityGroupRuleDescriptionsIngress(AwsEc2UpdateSecurityGroupRuleDescriptionsIngressOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2UpdateSecurityGroupRuleDescriptionsIngressOptions(), token);
    }

    public async Task<CommandResult> WithdrawByoipCidr(AwsEc2WithdrawByoipCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}