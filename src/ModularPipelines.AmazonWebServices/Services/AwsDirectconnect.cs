using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsDirectconnect
{
    public AwsDirectconnect(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AcceptDirectConnectGatewayAssociationProposal(AwsDirectconnectAcceptDirectConnectGatewayAssociationProposalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AllocateHostedConnection(AwsDirectconnectAllocateHostedConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AllocatePrivateVirtualInterface(AwsDirectconnectAllocatePrivateVirtualInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AllocatePublicVirtualInterface(AwsDirectconnectAllocatePublicVirtualInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AllocateTransitVirtualInterface(AwsDirectconnectAllocateTransitVirtualInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateConnectionWithLag(AwsDirectconnectAssociateConnectionWithLagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateHostedConnection(AwsDirectconnectAssociateHostedConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateMacSecKey(AwsDirectconnectAssociateMacSecKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateVirtualInterface(AwsDirectconnectAssociateVirtualInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfirmConnection(AwsDirectconnectConfirmConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfirmCustomerAgreement(AwsDirectconnectConfirmCustomerAgreementOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectConfirmCustomerAgreementOptions(), token);
    }

    public async Task<CommandResult> ConfirmPrivateVirtualInterface(AwsDirectconnectConfirmPrivateVirtualInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfirmPublicVirtualInterface(AwsDirectconnectConfirmPublicVirtualInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfirmTransitVirtualInterface(AwsDirectconnectConfirmTransitVirtualInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBgpPeer(AwsDirectconnectCreateBgpPeerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectCreateBgpPeerOptions(), token);
    }

    public async Task<CommandResult> CreateConnection(AwsDirectconnectCreateConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDirectConnectGateway(AwsDirectconnectCreateDirectConnectGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDirectConnectGatewayAssociation(AwsDirectconnectCreateDirectConnectGatewayAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDirectConnectGatewayAssociationProposal(AwsDirectconnectCreateDirectConnectGatewayAssociationProposalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInterconnect(AwsDirectconnectCreateInterconnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLag(AwsDirectconnectCreateLagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePrivateVirtualInterface(AwsDirectconnectCreatePrivateVirtualInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePublicVirtualInterface(AwsDirectconnectCreatePublicVirtualInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTransitVirtualInterface(AwsDirectconnectCreateTransitVirtualInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBgpPeer(AwsDirectconnectDeleteBgpPeerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDeleteBgpPeerOptions(), token);
    }

    public async Task<CommandResult> DeleteConnection(AwsDirectconnectDeleteConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDirectConnectGateway(AwsDirectconnectDeleteDirectConnectGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDirectConnectGatewayAssociation(AwsDirectconnectDeleteDirectConnectGatewayAssociationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDeleteDirectConnectGatewayAssociationOptions(), token);
    }

    public async Task<CommandResult> DeleteDirectConnectGatewayAssociationProposal(AwsDirectconnectDeleteDirectConnectGatewayAssociationProposalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInterconnect(AwsDirectconnectDeleteInterconnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLag(AwsDirectconnectDeleteLagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVirtualInterface(AwsDirectconnectDeleteVirtualInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeConnections(AwsDirectconnectDescribeConnectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeConnectionsOptions(), token);
    }

    public async Task<CommandResult> DescribeCustomerMetadata(AwsDirectconnectDescribeCustomerMetadataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeCustomerMetadataOptions(), token);
    }

    public async Task<CommandResult> DescribeDirectConnectGatewayAssociationProposals(AwsDirectconnectDescribeDirectConnectGatewayAssociationProposalsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeDirectConnectGatewayAssociationProposalsOptions(), token);
    }

    public async Task<CommandResult> DescribeDirectConnectGatewayAssociations(AwsDirectconnectDescribeDirectConnectGatewayAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeDirectConnectGatewayAssociationsOptions(), token);
    }

    public async Task<CommandResult> DescribeDirectConnectGatewayAttachments(AwsDirectconnectDescribeDirectConnectGatewayAttachmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeDirectConnectGatewayAttachmentsOptions(), token);
    }

    public async Task<CommandResult> DescribeDirectConnectGateways(AwsDirectconnectDescribeDirectConnectGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeDirectConnectGatewaysOptions(), token);
    }

    public async Task<CommandResult> DescribeHostedConnections(AwsDirectconnectDescribeHostedConnectionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInterconnects(AwsDirectconnectDescribeInterconnectsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeInterconnectsOptions(), token);
    }

    public async Task<CommandResult> DescribeLags(AwsDirectconnectDescribeLagsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeLagsOptions(), token);
    }

    public async Task<CommandResult> DescribeLoa(AwsDirectconnectDescribeLoaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeLocations(AwsDirectconnectDescribeLocationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeLocationsOptions(), token);
    }

    public async Task<CommandResult> DescribeRouterConfiguration(AwsDirectconnectDescribeRouterConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTags(AwsDirectconnectDescribeTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeVirtualGateways(AwsDirectconnectDescribeVirtualGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeVirtualGatewaysOptions(), token);
    }

    public async Task<CommandResult> DescribeVirtualInterfaces(AwsDirectconnectDescribeVirtualInterfacesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeVirtualInterfacesOptions(), token);
    }

    public async Task<CommandResult> DisassociateConnectionFromLag(AwsDirectconnectDisassociateConnectionFromLagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateMacSecKey(AwsDirectconnectDisassociateMacSecKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVirtualInterfaceTestHistory(AwsDirectconnectListVirtualInterfaceTestHistoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectListVirtualInterfaceTestHistoryOptions(), token);
    }

    public async Task<CommandResult> StartBgpFailoverTest(AwsDirectconnectStartBgpFailoverTestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopBgpFailoverTest(AwsDirectconnectStopBgpFailoverTestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsDirectconnectTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsDirectconnectUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConnection(AwsDirectconnectUpdateConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDirectConnectGateway(AwsDirectconnectUpdateDirectConnectGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDirectConnectGatewayAssociation(AwsDirectconnectUpdateDirectConnectGatewayAssociationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectUpdateDirectConnectGatewayAssociationOptions(), token);
    }

    public async Task<CommandResult> UpdateLag(AwsDirectconnectUpdateLagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVirtualInterfaceAttributes(AwsDirectconnectUpdateVirtualInterfaceAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}