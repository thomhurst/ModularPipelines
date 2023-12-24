using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsNetworkmanager
{
    public AwsNetworkmanager(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AcceptAttachment(AwsNetworkmanagerAcceptAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateConnectPeer(AwsNetworkmanagerAssociateConnectPeerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateCustomerGateway(AwsNetworkmanagerAssociateCustomerGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateLink(AwsNetworkmanagerAssociateLinkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateTransitGatewayConnectPeer(AwsNetworkmanagerAssociateTransitGatewayConnectPeerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConnectAttachment(AwsNetworkmanagerCreateConnectAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConnectPeer(AwsNetworkmanagerCreateConnectPeerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConnection(AwsNetworkmanagerCreateConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCoreNetwork(AwsNetworkmanagerCreateCoreNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDevice(AwsNetworkmanagerCreateDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGlobalNetwork(AwsNetworkmanagerCreateGlobalNetworkOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkmanagerCreateGlobalNetworkOptions(), token);
    }

    public async Task<CommandResult> CreateLink(AwsNetworkmanagerCreateLinkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSite(AwsNetworkmanagerCreateSiteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSiteToSiteVpnAttachment(AwsNetworkmanagerCreateSiteToSiteVpnAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTransitGatewayPeering(AwsNetworkmanagerCreateTransitGatewayPeeringOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTransitGatewayRouteTableAttachment(AwsNetworkmanagerCreateTransitGatewayRouteTableAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVpcAttachment(AwsNetworkmanagerCreateVpcAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAttachment(AwsNetworkmanagerDeleteAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConnectPeer(AwsNetworkmanagerDeleteConnectPeerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConnection(AwsNetworkmanagerDeleteConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCoreNetwork(AwsNetworkmanagerDeleteCoreNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCoreNetworkPolicyVersion(AwsNetworkmanagerDeleteCoreNetworkPolicyVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDevice(AwsNetworkmanagerDeleteDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGlobalNetwork(AwsNetworkmanagerDeleteGlobalNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLink(AwsNetworkmanagerDeleteLinkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePeering(AwsNetworkmanagerDeletePeeringOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsNetworkmanagerDeleteResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSite(AwsNetworkmanagerDeleteSiteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterTransitGateway(AwsNetworkmanagerDeregisterTransitGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeGlobalNetworks(AwsNetworkmanagerDescribeGlobalNetworksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkmanagerDescribeGlobalNetworksOptions(), token);
    }

    public async Task<CommandResult> DisassociateConnectPeer(AwsNetworkmanagerDisassociateConnectPeerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateCustomerGateway(AwsNetworkmanagerDisassociateCustomerGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateLink(AwsNetworkmanagerDisassociateLinkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateTransitGatewayConnectPeer(AwsNetworkmanagerDisassociateTransitGatewayConnectPeerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExecuteCoreNetworkChangeSet(AwsNetworkmanagerExecuteCoreNetworkChangeSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnectAttachment(AwsNetworkmanagerGetConnectAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnectPeer(AwsNetworkmanagerGetConnectPeerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnectPeerAssociations(AwsNetworkmanagerGetConnectPeerAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnections(AwsNetworkmanagerGetConnectionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCoreNetwork(AwsNetworkmanagerGetCoreNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCoreNetworkChangeEvents(AwsNetworkmanagerGetCoreNetworkChangeEventsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCoreNetworkChangeSet(AwsNetworkmanagerGetCoreNetworkChangeSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCoreNetworkPolicy(AwsNetworkmanagerGetCoreNetworkPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCustomerGatewayAssociations(AwsNetworkmanagerGetCustomerGatewayAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDevices(AwsNetworkmanagerGetDevicesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLinkAssociations(AwsNetworkmanagerGetLinkAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLinks(AwsNetworkmanagerGetLinksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNetworkResourceCounts(AwsNetworkmanagerGetNetworkResourceCountsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNetworkResourceRelationships(AwsNetworkmanagerGetNetworkResourceRelationshipsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNetworkResources(AwsNetworkmanagerGetNetworkResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNetworkRoutes(AwsNetworkmanagerGetNetworkRoutesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNetworkTelemetry(AwsNetworkmanagerGetNetworkTelemetryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourcePolicy(AwsNetworkmanagerGetResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRouteAnalysis(AwsNetworkmanagerGetRouteAnalysisOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSiteToSiteVpnAttachment(AwsNetworkmanagerGetSiteToSiteVpnAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSites(AwsNetworkmanagerGetSitesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTransitGatewayConnectPeerAssociations(AwsNetworkmanagerGetTransitGatewayConnectPeerAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTransitGatewayPeering(AwsNetworkmanagerGetTransitGatewayPeeringOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTransitGatewayRegistrations(AwsNetworkmanagerGetTransitGatewayRegistrationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTransitGatewayRouteTableAttachment(AwsNetworkmanagerGetTransitGatewayRouteTableAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVpcAttachment(AwsNetworkmanagerGetVpcAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAttachments(AwsNetworkmanagerListAttachmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkmanagerListAttachmentsOptions(), token);
    }

    public async Task<CommandResult> ListConnectPeers(AwsNetworkmanagerListConnectPeersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkmanagerListConnectPeersOptions(), token);
    }

    public async Task<CommandResult> ListCoreNetworkPolicyVersions(AwsNetworkmanagerListCoreNetworkPolicyVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCoreNetworks(AwsNetworkmanagerListCoreNetworksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkmanagerListCoreNetworksOptions(), token);
    }

    public async Task<CommandResult> ListOrganizationServiceAccessStatus(AwsNetworkmanagerListOrganizationServiceAccessStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkmanagerListOrganizationServiceAccessStatusOptions(), token);
    }

    public async Task<CommandResult> ListPeerings(AwsNetworkmanagerListPeeringsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkmanagerListPeeringsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsNetworkmanagerListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutCoreNetworkPolicy(AwsNetworkmanagerPutCoreNetworkPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsNetworkmanagerPutResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterTransitGateway(AwsNetworkmanagerRegisterTransitGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectAttachment(AwsNetworkmanagerRejectAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreCoreNetworkPolicyVersion(AwsNetworkmanagerRestoreCoreNetworkPolicyVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartOrganizationServiceAccessUpdate(AwsNetworkmanagerStartOrganizationServiceAccessUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartRouteAnalysis(AwsNetworkmanagerStartRouteAnalysisOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsNetworkmanagerTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsNetworkmanagerUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConnection(AwsNetworkmanagerUpdateConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCoreNetwork(AwsNetworkmanagerUpdateCoreNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDevice(AwsNetworkmanagerUpdateDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGlobalNetwork(AwsNetworkmanagerUpdateGlobalNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLink(AwsNetworkmanagerUpdateLinkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateNetworkResourceMetadata(AwsNetworkmanagerUpdateNetworkResourceMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSite(AwsNetworkmanagerUpdateSiteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVpcAttachment(AwsNetworkmanagerUpdateVpcAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}