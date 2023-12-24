using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> AssociateDrtLogBucket(AwsShieldAssociateDrtLogBucketOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateDrtRole(AwsShieldAssociateDrtRoleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateHealthCheck(AwsShieldAssociateHealthCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateProactiveEngagementDetails(AwsShieldAssociateProactiveEngagementDetailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProtection(AwsShieldCreateProtectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProtectionGroup(AwsShieldCreateProtectionGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSubscription(AwsShieldCreateSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldCreateSubscriptionOptions(), token);
    }

    public async Task<CommandResult> DeleteProtection(AwsShieldDeleteProtectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProtectionGroup(AwsShieldDeleteProtectionGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAttack(AwsShieldDescribeAttackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAttackStatistics(AwsShieldDescribeAttackStatisticsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDescribeAttackStatisticsOptions(), token);
    }

    public async Task<CommandResult> DescribeDrtAccess(AwsShieldDescribeDrtAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDescribeDrtAccessOptions(), token);
    }

    public async Task<CommandResult> DescribeEmergencyContactSettings(AwsShieldDescribeEmergencyContactSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDescribeEmergencyContactSettingsOptions(), token);
    }

    public async Task<CommandResult> DescribeProtection(AwsShieldDescribeProtectionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDescribeProtectionOptions(), token);
    }

    public async Task<CommandResult> DescribeProtectionGroup(AwsShieldDescribeProtectionGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSubscription(AwsShieldDescribeSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDescribeSubscriptionOptions(), token);
    }

    public async Task<CommandResult> DisableApplicationLayerAutomaticResponse(AwsShieldDisableApplicationLayerAutomaticResponseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableProactiveEngagement(AwsShieldDisableProactiveEngagementOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDisableProactiveEngagementOptions(), token);
    }

    public async Task<CommandResult> DisassociateDrtLogBucket(AwsShieldDisassociateDrtLogBucketOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateDrtRole(AwsShieldDisassociateDrtRoleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldDisassociateDrtRoleOptions(), token);
    }

    public async Task<CommandResult> DisassociateHealthCheck(AwsShieldDisassociateHealthCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableApplicationLayerAutomaticResponse(AwsShieldEnableApplicationLayerAutomaticResponseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableProactiveEngagement(AwsShieldEnableProactiveEngagementOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldEnableProactiveEngagementOptions(), token);
    }

    public async Task<CommandResult> GetSubscriptionState(AwsShieldGetSubscriptionStateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldGetSubscriptionStateOptions(), token);
    }

    public async Task<CommandResult> ListAttacks(AwsShieldListAttacksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldListAttacksOptions(), token);
    }

    public async Task<CommandResult> ListProtectionGroups(AwsShieldListProtectionGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldListProtectionGroupsOptions(), token);
    }

    public async Task<CommandResult> ListProtections(AwsShieldListProtectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldListProtectionsOptions(), token);
    }

    public async Task<CommandResult> ListResourcesInProtectionGroup(AwsShieldListResourcesInProtectionGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsShieldListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsShieldTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsShieldUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApplicationLayerAutomaticResponse(AwsShieldUpdateApplicationLayerAutomaticResponseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEmergencyContactSettings(AwsShieldUpdateEmergencyContactSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldUpdateEmergencyContactSettingsOptions(), token);
    }

    public async Task<CommandResult> UpdateProtectionGroup(AwsShieldUpdateProtectionGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSubscription(AwsShieldUpdateSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsShieldUpdateSubscriptionOptions(), token);
    }
}