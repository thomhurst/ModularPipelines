using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsShield
{
    public AwsShield(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateDrtLogBucket(AwsShieldAssociateDrtLogBucketOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateDrtRole(AwsShieldAssociateDrtRoleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateHealthCheck(AwsShieldAssociateHealthCheckOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateProactiveEngagementDetails(AwsShieldAssociateProactiveEngagementDetailsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateProtection(AwsShieldCreateProtectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateProtectionGroup(AwsShieldCreateProtectionGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSubscription(AwsShieldCreateSubscriptionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldCreateSubscriptionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteProtection(AwsShieldDeleteProtectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteProtectionGroup(AwsShieldDeleteProtectionGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAttack(AwsShieldDescribeAttackOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAttackStatistics(AwsShieldDescribeAttackStatisticsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDescribeAttackStatisticsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeDrtAccess(AwsShieldDescribeDrtAccessOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDescribeDrtAccessOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEmergencyContactSettings(AwsShieldDescribeEmergencyContactSettingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDescribeEmergencyContactSettingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeProtection(AwsShieldDescribeProtectionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDescribeProtectionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeProtectionGroup(AwsShieldDescribeProtectionGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSubscription(AwsShieldDescribeSubscriptionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDescribeSubscriptionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableApplicationLayerAutomaticResponse(AwsShieldDisableApplicationLayerAutomaticResponseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableProactiveEngagement(AwsShieldDisableProactiveEngagementOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDisableProactiveEngagementOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateDrtLogBucket(AwsShieldDisassociateDrtLogBucketOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateDrtRole(AwsShieldDisassociateDrtRoleOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDisassociateDrtRoleOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateHealthCheck(AwsShieldDisassociateHealthCheckOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableApplicationLayerAutomaticResponse(AwsShieldEnableApplicationLayerAutomaticResponseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableProactiveEngagement(AwsShieldEnableProactiveEngagementOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldEnableProactiveEngagementOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSubscriptionState(AwsShieldGetSubscriptionStateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldGetSubscriptionStateOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListAttacks(AwsShieldListAttacksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldListAttacksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListProtectionGroups(AwsShieldListProtectionGroupsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldListProtectionGroupsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListProtections(AwsShieldListProtectionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldListProtectionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListResourcesInProtectionGroup(AwsShieldListResourcesInProtectionGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsShieldListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsShieldTagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsShieldUntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateApplicationLayerAutomaticResponse(AwsShieldUpdateApplicationLayerAutomaticResponseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateEmergencyContactSettings(AwsShieldUpdateEmergencyContactSettingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldUpdateEmergencyContactSettingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateProtectionGroup(AwsShieldUpdateProtectionGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSubscription(AwsShieldUpdateSubscriptionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldUpdateSubscriptionOptions(), executionOptions, cancellationToken);
    }
}