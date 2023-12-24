using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsElbv2
{
    public AwsElbv2(
        AwsElbv2Wait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsElbv2Wait Wait { get; }

    public async Task<CommandResult> AddListenerCertificates(AwsElbv2AddListenerCertificatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddTags(AwsElbv2AddTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddTrustStoreRevocations(AwsElbv2AddTrustStoreRevocationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateListener(AwsElbv2CreateListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLoadBalancer(AwsElbv2CreateLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRule(AwsElbv2CreateRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTargetGroup(AwsElbv2CreateTargetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTrustStore(AwsElbv2CreateTrustStoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteListener(AwsElbv2DeleteListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLoadBalancer(AwsElbv2DeleteLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRule(AwsElbv2DeleteRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTargetGroup(AwsElbv2DeleteTargetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTrustStore(AwsElbv2DeleteTrustStoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterTargets(AwsElbv2DeregisterTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccountLimits(AwsElbv2DescribeAccountLimitsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbv2DescribeAccountLimitsOptions(), token);
    }

    public async Task<CommandResult> DescribeListenerCertificates(AwsElbv2DescribeListenerCertificatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeListeners(AwsElbv2DescribeListenersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbv2DescribeListenersOptions(), token);
    }

    public async Task<CommandResult> DescribeLoadBalancerAttributes(AwsElbv2DescribeLoadBalancerAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeLoadBalancers(AwsElbv2DescribeLoadBalancersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbv2DescribeLoadBalancersOptions(), token);
    }

    public async Task<CommandResult> DescribeRules(AwsElbv2DescribeRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbv2DescribeRulesOptions(), token);
    }

    public async Task<CommandResult> DescribeSslPolicies(AwsElbv2DescribeSslPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbv2DescribeSslPoliciesOptions(), token);
    }

    public async Task<CommandResult> DescribeTags(AwsElbv2DescribeTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTargetGroupAttributes(AwsElbv2DescribeTargetGroupAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTargetGroups(AwsElbv2DescribeTargetGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbv2DescribeTargetGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeTargetHealth(AwsElbv2DescribeTargetHealthOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTrustStoreAssociations(AwsElbv2DescribeTrustStoreAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTrustStoreRevocations(AwsElbv2DescribeTrustStoreRevocationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTrustStores(AwsElbv2DescribeTrustStoresOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbv2DescribeTrustStoresOptions(), token);
    }

    public async Task<CommandResult> GetTrustStoreCaCertificatesBundle(AwsElbv2GetTrustStoreCaCertificatesBundleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTrustStoreRevocationContent(AwsElbv2GetTrustStoreRevocationContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyListener(AwsElbv2ModifyListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyLoadBalancerAttributes(AwsElbv2ModifyLoadBalancerAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyRule(AwsElbv2ModifyRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyTargetGroup(AwsElbv2ModifyTargetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyTargetGroupAttributes(AwsElbv2ModifyTargetGroupAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyTrustStore(AwsElbv2ModifyTrustStoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterTargets(AwsElbv2RegisterTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveListenerCertificates(AwsElbv2RemoveListenerCertificatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTags(AwsElbv2RemoveTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTrustStoreRevocations(AwsElbv2RemoveTrustStoreRevocationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIpAddressType(AwsElbv2SetIpAddressTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetRulePriorities(AwsElbv2SetRulePrioritiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetSecurityGroups(AwsElbv2SetSecurityGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetSubnets(AwsElbv2SetSubnetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}