using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsVpcLattice
{
    public AwsVpcLattice(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchUpdateRule(AwsVpcLatticeBatchUpdateRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAccessLogSubscription(AwsVpcLatticeCreateAccessLogSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateListener(AwsVpcLatticeCreateListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRule(AwsVpcLatticeCreateRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateService(AwsVpcLatticeCreateServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateServiceNetwork(AwsVpcLatticeCreateServiceNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateServiceNetworkServiceAssociation(AwsVpcLatticeCreateServiceNetworkServiceAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateServiceNetworkVpcAssociation(AwsVpcLatticeCreateServiceNetworkVpcAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTargetGroup(AwsVpcLatticeCreateTargetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAccessLogSubscription(AwsVpcLatticeDeleteAccessLogSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAuthPolicy(AwsVpcLatticeDeleteAuthPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteListener(AwsVpcLatticeDeleteListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsVpcLatticeDeleteResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRule(AwsVpcLatticeDeleteRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteService(AwsVpcLatticeDeleteServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteServiceNetwork(AwsVpcLatticeDeleteServiceNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteServiceNetworkServiceAssociation(AwsVpcLatticeDeleteServiceNetworkServiceAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteServiceNetworkVpcAssociation(AwsVpcLatticeDeleteServiceNetworkVpcAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTargetGroup(AwsVpcLatticeDeleteTargetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterTargets(AwsVpcLatticeDeregisterTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccessLogSubscription(AwsVpcLatticeGetAccessLogSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAuthPolicy(AwsVpcLatticeGetAuthPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetListener(AwsVpcLatticeGetListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourcePolicy(AwsVpcLatticeGetResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRule(AwsVpcLatticeGetRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetService(AwsVpcLatticeGetServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServiceNetwork(AwsVpcLatticeGetServiceNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServiceNetworkServiceAssociation(AwsVpcLatticeGetServiceNetworkServiceAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServiceNetworkVpcAssociation(AwsVpcLatticeGetServiceNetworkVpcAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTargetGroup(AwsVpcLatticeGetTargetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAccessLogSubscriptions(AwsVpcLatticeListAccessLogSubscriptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListListeners(AwsVpcLatticeListListenersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRules(AwsVpcLatticeListRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListServiceNetworkServiceAssociations(AwsVpcLatticeListServiceNetworkServiceAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsVpcLatticeListServiceNetworkServiceAssociationsOptions(), token);
    }

    public async Task<CommandResult> ListServiceNetworkVpcAssociations(AwsVpcLatticeListServiceNetworkVpcAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsVpcLatticeListServiceNetworkVpcAssociationsOptions(), token);
    }

    public async Task<CommandResult> ListServiceNetworks(AwsVpcLatticeListServiceNetworksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsVpcLatticeListServiceNetworksOptions(), token);
    }

    public async Task<CommandResult> ListServices(AwsVpcLatticeListServicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsVpcLatticeListServicesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsVpcLatticeListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTargetGroups(AwsVpcLatticeListTargetGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsVpcLatticeListTargetGroupsOptions(), token);
    }

    public async Task<CommandResult> ListTargets(AwsVpcLatticeListTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAuthPolicy(AwsVpcLatticePutAuthPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsVpcLatticePutResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterTargets(AwsVpcLatticeRegisterTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsVpcLatticeTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsVpcLatticeUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAccessLogSubscription(AwsVpcLatticeUpdateAccessLogSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateListener(AwsVpcLatticeUpdateListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRule(AwsVpcLatticeUpdateRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateService(AwsVpcLatticeUpdateServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateServiceNetwork(AwsVpcLatticeUpdateServiceNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateServiceNetworkVpcAssociation(AwsVpcLatticeUpdateServiceNetworkVpcAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTargetGroup(AwsVpcLatticeUpdateTargetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}