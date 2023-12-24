using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsMediaconnect
{
    public AwsMediaconnect(
        AwsMediaconnectWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsMediaconnectWait Wait { get; }

    public async Task<CommandResult> AddBridgeOutputs(AwsMediaconnectAddBridgeOutputsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddBridgeSources(AwsMediaconnectAddBridgeSourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddFlowMediaStreams(AwsMediaconnectAddFlowMediaStreamsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddFlowOutputs(AwsMediaconnectAddFlowOutputsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddFlowSources(AwsMediaconnectAddFlowSourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddFlowVpcInterfaces(AwsMediaconnectAddFlowVpcInterfacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBridge(AwsMediaconnectCreateBridgeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFlow(AwsMediaconnectCreateFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGateway(AwsMediaconnectCreateGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBridge(AwsMediaconnectDeleteBridgeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFlow(AwsMediaconnectDeleteFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGateway(AwsMediaconnectDeleteGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterGatewayInstance(AwsMediaconnectDeregisterGatewayInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeBridge(AwsMediaconnectDescribeBridgeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFlow(AwsMediaconnectDescribeFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeGateway(AwsMediaconnectDescribeGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeGatewayInstance(AwsMediaconnectDescribeGatewayInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeOffering(AwsMediaconnectDescribeOfferingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeReservation(AwsMediaconnectDescribeReservationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GrantFlowEntitlements(AwsMediaconnectGrantFlowEntitlementsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListBridges(AwsMediaconnectListBridgesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMediaconnectListBridgesOptions(), token);
    }

    public async Task<CommandResult> ListEntitlements(AwsMediaconnectListEntitlementsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMediaconnectListEntitlementsOptions(), token);
    }

    public async Task<CommandResult> ListFlows(AwsMediaconnectListFlowsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMediaconnectListFlowsOptions(), token);
    }

    public async Task<CommandResult> ListGatewayInstances(AwsMediaconnectListGatewayInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMediaconnectListGatewayInstancesOptions(), token);
    }

    public async Task<CommandResult> ListGateways(AwsMediaconnectListGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMediaconnectListGatewaysOptions(), token);
    }

    public async Task<CommandResult> ListOfferings(AwsMediaconnectListOfferingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMediaconnectListOfferingsOptions(), token);
    }

    public async Task<CommandResult> ListReservations(AwsMediaconnectListReservationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMediaconnectListReservationsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsMediaconnectListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PurchaseOffering(AwsMediaconnectPurchaseOfferingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveBridgeOutput(AwsMediaconnectRemoveBridgeOutputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveBridgeSource(AwsMediaconnectRemoveBridgeSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveFlowMediaStream(AwsMediaconnectRemoveFlowMediaStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveFlowOutput(AwsMediaconnectRemoveFlowOutputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveFlowSource(AwsMediaconnectRemoveFlowSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveFlowVpcInterface(AwsMediaconnectRemoveFlowVpcInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RevokeFlowEntitlement(AwsMediaconnectRevokeFlowEntitlementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartFlow(AwsMediaconnectStartFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopFlow(AwsMediaconnectStopFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsMediaconnectTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsMediaconnectUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBridge(AwsMediaconnectUpdateBridgeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBridgeOutput(AwsMediaconnectUpdateBridgeOutputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBridgeSource(AwsMediaconnectUpdateBridgeSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBridgeState(AwsMediaconnectUpdateBridgeStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFlow(AwsMediaconnectUpdateFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFlowEntitlement(AwsMediaconnectUpdateFlowEntitlementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFlowMediaStream(AwsMediaconnectUpdateFlowMediaStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFlowOutput(AwsMediaconnectUpdateFlowOutputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFlowSource(AwsMediaconnectUpdateFlowSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGatewayInstance(AwsMediaconnectUpdateGatewayInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}