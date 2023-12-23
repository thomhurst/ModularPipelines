using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsGlobalaccelerator
{
    public AwsGlobalaccelerator(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddCustomRoutingEndpoints(AwsGlobalacceleratorAddCustomRoutingEndpointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddEndpoints(AwsGlobalacceleratorAddEndpointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdvertiseByoipCidr(AwsGlobalacceleratorAdvertiseByoipCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AllowCustomRoutingTraffic(AwsGlobalacceleratorAllowCustomRoutingTrafficOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAccelerator(AwsGlobalacceleratorCreateAcceleratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCrossAccountAttachment(AwsGlobalacceleratorCreateCrossAccountAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomRoutingAccelerator(AwsGlobalacceleratorCreateCustomRoutingAcceleratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomRoutingEndpointGroup(AwsGlobalacceleratorCreateCustomRoutingEndpointGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomRoutingListener(AwsGlobalacceleratorCreateCustomRoutingListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEndpointGroup(AwsGlobalacceleratorCreateEndpointGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateListener(AwsGlobalacceleratorCreateListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAccelerator(AwsGlobalacceleratorDeleteAcceleratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCrossAccountAttachment(AwsGlobalacceleratorDeleteCrossAccountAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomRoutingAccelerator(AwsGlobalacceleratorDeleteCustomRoutingAcceleratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomRoutingEndpointGroup(AwsGlobalacceleratorDeleteCustomRoutingEndpointGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomRoutingListener(AwsGlobalacceleratorDeleteCustomRoutingListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEndpointGroup(AwsGlobalacceleratorDeleteEndpointGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteListener(AwsGlobalacceleratorDeleteListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DenyCustomRoutingTraffic(AwsGlobalacceleratorDenyCustomRoutingTrafficOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeprovisionByoipCidr(AwsGlobalacceleratorDeprovisionByoipCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccelerator(AwsGlobalacceleratorDescribeAcceleratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAcceleratorAttributes(AwsGlobalacceleratorDescribeAcceleratorAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCrossAccountAttachment(AwsGlobalacceleratorDescribeCrossAccountAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCustomRoutingAccelerator(AwsGlobalacceleratorDescribeCustomRoutingAcceleratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCustomRoutingAcceleratorAttributes(AwsGlobalacceleratorDescribeCustomRoutingAcceleratorAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCustomRoutingEndpointGroup(AwsGlobalacceleratorDescribeCustomRoutingEndpointGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCustomRoutingListener(AwsGlobalacceleratorDescribeCustomRoutingListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEndpointGroup(AwsGlobalacceleratorDescribeEndpointGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeListener(AwsGlobalacceleratorDescribeListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAccelerators(AwsGlobalacceleratorListAcceleratorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlobalacceleratorListAcceleratorsOptions(), token);
    }

    public async Task<CommandResult> ListByoipCidrs(AwsGlobalacceleratorListByoipCidrsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlobalacceleratorListByoipCidrsOptions(), token);
    }

    public async Task<CommandResult> ListCrossAccountAttachments(AwsGlobalacceleratorListCrossAccountAttachmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlobalacceleratorListCrossAccountAttachmentsOptions(), token);
    }

    public async Task<CommandResult> ListCrossAccountResourceAccounts(AwsGlobalacceleratorListCrossAccountResourceAccountsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlobalacceleratorListCrossAccountResourceAccountsOptions(), token);
    }

    public async Task<CommandResult> ListCrossAccountResources(AwsGlobalacceleratorListCrossAccountResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCustomRoutingAccelerators(AwsGlobalacceleratorListCustomRoutingAcceleratorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlobalacceleratorListCustomRoutingAcceleratorsOptions(), token);
    }

    public async Task<CommandResult> ListCustomRoutingEndpointGroups(AwsGlobalacceleratorListCustomRoutingEndpointGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCustomRoutingListeners(AwsGlobalacceleratorListCustomRoutingListenersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCustomRoutingPortMappings(AwsGlobalacceleratorListCustomRoutingPortMappingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCustomRoutingPortMappingsByDestination(AwsGlobalacceleratorListCustomRoutingPortMappingsByDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEndpointGroups(AwsGlobalacceleratorListEndpointGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListListeners(AwsGlobalacceleratorListListenersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsGlobalacceleratorListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ProvisionByoipCidr(AwsGlobalacceleratorProvisionByoipCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveCustomRoutingEndpoints(AwsGlobalacceleratorRemoveCustomRoutingEndpointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveEndpoints(AwsGlobalacceleratorRemoveEndpointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsGlobalacceleratorTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsGlobalacceleratorUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAccelerator(AwsGlobalacceleratorUpdateAcceleratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAcceleratorAttributes(AwsGlobalacceleratorUpdateAcceleratorAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCrossAccountAttachment(AwsGlobalacceleratorUpdateCrossAccountAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCustomRoutingAccelerator(AwsGlobalacceleratorUpdateCustomRoutingAcceleratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCustomRoutingAcceleratorAttributes(AwsGlobalacceleratorUpdateCustomRoutingAcceleratorAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCustomRoutingListener(AwsGlobalacceleratorUpdateCustomRoutingListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEndpointGroup(AwsGlobalacceleratorUpdateEndpointGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateListener(AwsGlobalacceleratorUpdateListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> WithdrawByoipCidr(AwsGlobalacceleratorWithdrawByoipCidrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}