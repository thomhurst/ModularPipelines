using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> AcceptDirectConnectGatewayAssociationProposal(AwsDirectconnectAcceptDirectConnectGatewayAssociationProposalOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AllocateHostedConnection(AwsDirectconnectAllocateHostedConnectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AllocatePrivateVirtualInterface(AwsDirectconnectAllocatePrivateVirtualInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AllocatePublicVirtualInterface(AwsDirectconnectAllocatePublicVirtualInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AllocateTransitVirtualInterface(AwsDirectconnectAllocateTransitVirtualInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateConnectionWithLag(AwsDirectconnectAssociateConnectionWithLagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateHostedConnection(AwsDirectconnectAssociateHostedConnectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateMacSecKey(AwsDirectconnectAssociateMacSecKeyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateVirtualInterface(AwsDirectconnectAssociateVirtualInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ConfirmConnection(AwsDirectconnectConfirmConnectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ConfirmCustomerAgreement(AwsDirectconnectConfirmCustomerAgreementOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectConfirmCustomerAgreementOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ConfirmPrivateVirtualInterface(AwsDirectconnectConfirmPrivateVirtualInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ConfirmPublicVirtualInterface(AwsDirectconnectConfirmPublicVirtualInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ConfirmTransitVirtualInterface(AwsDirectconnectConfirmTransitVirtualInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateBgpPeer(AwsDirectconnectCreateBgpPeerOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectCreateBgpPeerOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateConnection(AwsDirectconnectCreateConnectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDirectConnectGateway(AwsDirectconnectCreateDirectConnectGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDirectConnectGatewayAssociation(AwsDirectconnectCreateDirectConnectGatewayAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDirectConnectGatewayAssociationProposal(AwsDirectconnectCreateDirectConnectGatewayAssociationProposalOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateInterconnect(AwsDirectconnectCreateInterconnectOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateLag(AwsDirectconnectCreateLagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreatePrivateVirtualInterface(AwsDirectconnectCreatePrivateVirtualInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreatePublicVirtualInterface(AwsDirectconnectCreatePublicVirtualInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTransitVirtualInterface(AwsDirectconnectCreateTransitVirtualInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteBgpPeer(AwsDirectconnectDeleteBgpPeerOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDeleteBgpPeerOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteConnection(AwsDirectconnectDeleteConnectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDirectConnectGateway(AwsDirectconnectDeleteDirectConnectGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDirectConnectGatewayAssociation(AwsDirectconnectDeleteDirectConnectGatewayAssociationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDeleteDirectConnectGatewayAssociationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDirectConnectGatewayAssociationProposal(AwsDirectconnectDeleteDirectConnectGatewayAssociationProposalOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteInterconnect(AwsDirectconnectDeleteInterconnectOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteLag(AwsDirectconnectDeleteLagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVirtualInterface(AwsDirectconnectDeleteVirtualInterfaceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConnections(AwsDirectconnectDescribeConnectionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeConnectionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeCustomerMetadata(AwsDirectconnectDescribeCustomerMetadataOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeCustomerMetadataOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeDirectConnectGatewayAssociationProposals(AwsDirectconnectDescribeDirectConnectGatewayAssociationProposalsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeDirectConnectGatewayAssociationProposalsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeDirectConnectGatewayAssociations(AwsDirectconnectDescribeDirectConnectGatewayAssociationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeDirectConnectGatewayAssociationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeDirectConnectGatewayAttachments(AwsDirectconnectDescribeDirectConnectGatewayAttachmentsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeDirectConnectGatewayAttachmentsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeDirectConnectGateways(AwsDirectconnectDescribeDirectConnectGatewaysOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeDirectConnectGatewaysOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeHostedConnections(AwsDirectconnectDescribeHostedConnectionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInterconnects(AwsDirectconnectDescribeInterconnectsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeInterconnectsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLags(AwsDirectconnectDescribeLagsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeLagsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLoa(AwsDirectconnectDescribeLoaOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLocations(AwsDirectconnectDescribeLocationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeLocationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRouterConfiguration(AwsDirectconnectDescribeRouterConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTags(AwsDirectconnectDescribeTagsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVirtualGateways(AwsDirectconnectDescribeVirtualGatewaysOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeVirtualGatewaysOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVirtualInterfaces(AwsDirectconnectDescribeVirtualInterfacesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectDescribeVirtualInterfacesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateConnectionFromLag(AwsDirectconnectDisassociateConnectionFromLagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateMacSecKey(AwsDirectconnectDisassociateMacSecKeyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListVirtualInterfaceTestHistory(AwsDirectconnectListVirtualInterfaceTestHistoryOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectListVirtualInterfaceTestHistoryOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartBgpFailoverTest(AwsDirectconnectStartBgpFailoverTestOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopBgpFailoverTest(AwsDirectconnectStopBgpFailoverTestOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsDirectconnectTagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsDirectconnectUntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateConnection(AwsDirectconnectUpdateConnectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateDirectConnectGateway(AwsDirectconnectUpdateDirectConnectGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateDirectConnectGatewayAssociation(AwsDirectconnectUpdateDirectConnectGatewayAssociationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDirectconnectUpdateDirectConnectGatewayAssociationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateLag(AwsDirectconnectUpdateLagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateVirtualInterfaceAttributes(AwsDirectconnectUpdateVirtualInterfaceAttributesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}