using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> AssociateFirewallPolicy(AwsNetworkFirewallAssociateFirewallPolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateSubnets(AwsNetworkFirewallAssociateSubnetsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateFirewall(AwsNetworkFirewallCreateFirewallOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateFirewallPolicy(AwsNetworkFirewallCreateFirewallPolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateRuleGroup(AwsNetworkFirewallCreateRuleGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTlsInspectionConfiguration(AwsNetworkFirewallCreateTlsInspectionConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteFirewall(AwsNetworkFirewallDeleteFirewallOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDeleteFirewallOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteFirewallPolicy(AwsNetworkFirewallDeleteFirewallPolicyOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDeleteFirewallPolicyOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsNetworkFirewallDeleteResourcePolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteRuleGroup(AwsNetworkFirewallDeleteRuleGroupOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDeleteRuleGroupOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTlsInspectionConfiguration(AwsNetworkFirewallDeleteTlsInspectionConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDeleteTlsInspectionConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeFirewall(AwsNetworkFirewallDescribeFirewallOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDescribeFirewallOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeFirewallPolicy(AwsNetworkFirewallDescribeFirewallPolicyOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDescribeFirewallPolicyOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLoggingConfiguration(AwsNetworkFirewallDescribeLoggingConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDescribeLoggingConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeResourcePolicy(AwsNetworkFirewallDescribeResourcePolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRuleGroup(AwsNetworkFirewallDescribeRuleGroupOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDescribeRuleGroupOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRuleGroupMetadata(AwsNetworkFirewallDescribeRuleGroupMetadataOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDescribeRuleGroupMetadataOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTlsInspectionConfiguration(AwsNetworkFirewallDescribeTlsInspectionConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallDescribeTlsInspectionConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateSubnets(AwsNetworkFirewallDisassociateSubnetsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListFirewallPolicies(AwsNetworkFirewallListFirewallPoliciesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallListFirewallPoliciesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListFirewalls(AwsNetworkFirewallListFirewallsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallListFirewallsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListRuleGroups(AwsNetworkFirewallListRuleGroupsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallListRuleGroupsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsNetworkFirewallListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTlsInspectionConfigurations(AwsNetworkFirewallListTlsInspectionConfigurationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallListTlsInspectionConfigurationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsNetworkFirewallPutResourcePolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsNetworkFirewallTagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsNetworkFirewallUntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateFirewallDeleteProtection(AwsNetworkFirewallUpdateFirewallDeleteProtectionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallUpdateFirewallDeleteProtectionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateFirewallDescription(AwsNetworkFirewallUpdateFirewallDescriptionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallUpdateFirewallDescriptionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateFirewallEncryptionConfiguration(AwsNetworkFirewallUpdateFirewallEncryptionConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallUpdateFirewallEncryptionConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateFirewallPolicy(AwsNetworkFirewallUpdateFirewallPolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateFirewallPolicyChangeProtection(AwsNetworkFirewallUpdateFirewallPolicyChangeProtectionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallUpdateFirewallPolicyChangeProtectionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateLoggingConfiguration(AwsNetworkFirewallUpdateLoggingConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallUpdateLoggingConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateRuleGroup(AwsNetworkFirewallUpdateRuleGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSubnetChangeProtection(AwsNetworkFirewallUpdateSubnetChangeProtectionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNetworkFirewallUpdateSubnetChangeProtectionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateTlsInspectionConfiguration(AwsNetworkFirewallUpdateTlsInspectionConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}