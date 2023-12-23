using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsNetworkFirewall
{
    public AwsNetworkFirewall(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateFirewallPolicy(AwsNetworkFirewallAssociateFirewallPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateSubnets(AwsNetworkFirewallAssociateSubnetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFirewall(AwsNetworkFirewallCreateFirewallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFirewallPolicy(AwsNetworkFirewallCreateFirewallPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRuleGroup(AwsNetworkFirewallCreateRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTlsInspectionConfiguration(AwsNetworkFirewallCreateTlsInspectionConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFirewall(AwsNetworkFirewallDeleteFirewallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDeleteFirewallOptions(), token);
    }

    public async Task<CommandResult> DeleteFirewallPolicy(AwsNetworkFirewallDeleteFirewallPolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDeleteFirewallPolicyOptions(), token);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsNetworkFirewallDeleteResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRuleGroup(AwsNetworkFirewallDeleteRuleGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDeleteRuleGroupOptions(), token);
    }

    public async Task<CommandResult> DeleteTlsInspectionConfiguration(AwsNetworkFirewallDeleteTlsInspectionConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDeleteTlsInspectionConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeFirewall(AwsNetworkFirewallDescribeFirewallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDescribeFirewallOptions(), token);
    }

    public async Task<CommandResult> DescribeFirewallPolicy(AwsNetworkFirewallDescribeFirewallPolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDescribeFirewallPolicyOptions(), token);
    }

    public async Task<CommandResult> DescribeLoggingConfiguration(AwsNetworkFirewallDescribeLoggingConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDescribeLoggingConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeResourcePolicy(AwsNetworkFirewallDescribeResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRuleGroup(AwsNetworkFirewallDescribeRuleGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDescribeRuleGroupOptions(), token);
    }

    public async Task<CommandResult> DescribeRuleGroupMetadata(AwsNetworkFirewallDescribeRuleGroupMetadataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDescribeRuleGroupMetadataOptions(), token);
    }

    public async Task<CommandResult> DescribeTlsInspectionConfiguration(AwsNetworkFirewallDescribeTlsInspectionConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDescribeTlsInspectionConfigurationOptions(), token);
    }

    public async Task<CommandResult> DisassociateSubnets(AwsNetworkFirewallDisassociateSubnetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFirewallPolicies(AwsNetworkFirewallListFirewallPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallListFirewallPoliciesOptions(), token);
    }

    public async Task<CommandResult> ListFirewalls(AwsNetworkFirewallListFirewallsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallListFirewallsOptions(), token);
    }

    public async Task<CommandResult> ListRuleGroups(AwsNetworkFirewallListRuleGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallListRuleGroupsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsNetworkFirewallListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTlsInspectionConfigurations(AwsNetworkFirewallListTlsInspectionConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallListTlsInspectionConfigurationsOptions(), token);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsNetworkFirewallPutResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsNetworkFirewallTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsNetworkFirewallUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFirewallDeleteProtection(AwsNetworkFirewallUpdateFirewallDeleteProtectionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallUpdateFirewallDeleteProtectionOptions(), token);
    }

    public async Task<CommandResult> UpdateFirewallDescription(AwsNetworkFirewallUpdateFirewallDescriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallUpdateFirewallDescriptionOptions(), token);
    }

    public async Task<CommandResult> UpdateFirewallEncryptionConfiguration(AwsNetworkFirewallUpdateFirewallEncryptionConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallUpdateFirewallEncryptionConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdateFirewallPolicy(AwsNetworkFirewallUpdateFirewallPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFirewallPolicyChangeProtection(AwsNetworkFirewallUpdateFirewallPolicyChangeProtectionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallUpdateFirewallPolicyChangeProtectionOptions(), token);
    }

    public async Task<CommandResult> UpdateLoggingConfiguration(AwsNetworkFirewallUpdateLoggingConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallUpdateLoggingConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdateRuleGroup(AwsNetworkFirewallUpdateRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSubnetChangeProtection(AwsNetworkFirewallUpdateSubnetChangeProtectionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallUpdateSubnetChangeProtectionOptions(), token);
    }

    public async Task<CommandResult> UpdateTlsInspectionConfiguration(AwsNetworkFirewallUpdateTlsInspectionConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}