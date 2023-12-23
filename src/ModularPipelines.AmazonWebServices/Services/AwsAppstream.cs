using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsAppstream
{
    public AwsAppstream(
        AwsAppstreamWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsAppstreamWait Wait { get; }

    public async Task<CommandResult> AssociateAppBlockBuilderAppBlock(AwsAppstreamAssociateAppBlockBuilderAppBlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateApplicationFleet(AwsAppstreamAssociateApplicationFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateApplicationToEntitlement(AwsAppstreamAssociateApplicationToEntitlementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateFleet(AwsAppstreamAssociateFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchAssociateUserStack(AwsAppstreamBatchAssociateUserStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDisassociateUserStack(AwsAppstreamBatchDisassociateUserStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyImage(AwsAppstreamCopyImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAppBlock(AwsAppstreamCreateAppBlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAppBlockBuilder(AwsAppstreamCreateAppBlockBuilderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAppBlockBuilderStreamingUrl(AwsAppstreamCreateAppBlockBuilderStreamingUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateApplication(AwsAppstreamCreateApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDirectoryConfig(AwsAppstreamCreateDirectoryConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEntitlement(AwsAppstreamCreateEntitlementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFleet(AwsAppstreamCreateFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateImageBuilder(AwsAppstreamCreateImageBuilderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateImageBuilderStreamingUrl(AwsAppstreamCreateImageBuilderStreamingUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStack(AwsAppstreamCreateStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStreamingUrl(AwsAppstreamCreateStreamingUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUpdatedImage(AwsAppstreamCreateUpdatedImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUsageReportSubscription(AwsAppstreamCreateUsageReportSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamCreateUsageReportSubscriptionOptions(), token);
    }

    public async Task<CommandResult> CreateUser(AwsAppstreamCreateUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAppBlock(AwsAppstreamDeleteAppBlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAppBlockBuilder(AwsAppstreamDeleteAppBlockBuilderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApplication(AwsAppstreamDeleteApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDirectoryConfig(AwsAppstreamDeleteDirectoryConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEntitlement(AwsAppstreamDeleteEntitlementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFleet(AwsAppstreamDeleteFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteImage(AwsAppstreamDeleteImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteImageBuilder(AwsAppstreamDeleteImageBuilderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteImagePermissions(AwsAppstreamDeleteImagePermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStack(AwsAppstreamDeleteStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUsageReportSubscription(AwsAppstreamDeleteUsageReportSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamDeleteUsageReportSubscriptionOptions(), token);
    }

    public async Task<CommandResult> DeleteUser(AwsAppstreamDeleteUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAppBlockBuilderAppBlockAssociations(AwsAppstreamDescribeAppBlockBuilderAppBlockAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamDescribeAppBlockBuilderAppBlockAssociationsOptions(), token);
    }

    public async Task<CommandResult> DescribeAppBlockBuilders(AwsAppstreamDescribeAppBlockBuildersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamDescribeAppBlockBuildersOptions(), token);
    }

    public async Task<CommandResult> DescribeAppBlocks(AwsAppstreamDescribeAppBlocksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamDescribeAppBlocksOptions(), token);
    }

    public async Task<CommandResult> DescribeApplicationFleetAssociations(AwsAppstreamDescribeApplicationFleetAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamDescribeApplicationFleetAssociationsOptions(), token);
    }

    public async Task<CommandResult> DescribeApplications(AwsAppstreamDescribeApplicationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamDescribeApplicationsOptions(), token);
    }

    public async Task<CommandResult> DescribeDirectoryConfigs(AwsAppstreamDescribeDirectoryConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamDescribeDirectoryConfigsOptions(), token);
    }

    public async Task<CommandResult> DescribeEntitlements(AwsAppstreamDescribeEntitlementsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFleets(AwsAppstreamDescribeFleetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamDescribeFleetsOptions(), token);
    }

    public async Task<CommandResult> DescribeImageBuilders(AwsAppstreamDescribeImageBuildersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamDescribeImageBuildersOptions(), token);
    }

    public async Task<CommandResult> DescribeImagePermissions(AwsAppstreamDescribeImagePermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeImages(AwsAppstreamDescribeImagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamDescribeImagesOptions(), token);
    }

    public async Task<CommandResult> DescribeSessions(AwsAppstreamDescribeSessionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStacks(AwsAppstreamDescribeStacksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamDescribeStacksOptions(), token);
    }

    public async Task<CommandResult> DescribeUsageReportSubscriptions(AwsAppstreamDescribeUsageReportSubscriptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamDescribeUsageReportSubscriptionsOptions(), token);
    }

    public async Task<CommandResult> DescribeUserStackAssociations(AwsAppstreamDescribeUserStackAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamDescribeUserStackAssociationsOptions(), token);
    }

    public async Task<CommandResult> DescribeUsers(AwsAppstreamDescribeUsersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableUser(AwsAppstreamDisableUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateAppBlockBuilderAppBlock(AwsAppstreamDisassociateAppBlockBuilderAppBlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateApplicationFleet(AwsAppstreamDisassociateApplicationFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateApplicationFromEntitlement(AwsAppstreamDisassociateApplicationFromEntitlementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateFleet(AwsAppstreamDisassociateFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableUser(AwsAppstreamEnableUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExpireSession(AwsAppstreamExpireSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssociatedFleets(AwsAppstreamListAssociatedFleetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssociatedStacks(AwsAppstreamListAssociatedStacksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEntitledApplications(AwsAppstreamListEntitledApplicationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsAppstreamListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartAppBlockBuilder(AwsAppstreamStartAppBlockBuilderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartFleet(AwsAppstreamStartFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartImageBuilder(AwsAppstreamStartImageBuilderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopAppBlockBuilder(AwsAppstreamStopAppBlockBuilderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopFleet(AwsAppstreamStopFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopImageBuilder(AwsAppstreamStopImageBuilderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsAppstreamTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsAppstreamUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAppBlockBuilder(AwsAppstreamUpdateAppBlockBuilderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApplication(AwsAppstreamUpdateApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDirectoryConfig(AwsAppstreamUpdateDirectoryConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEntitlement(AwsAppstreamUpdateEntitlementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFleet(AwsAppstreamUpdateFleetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamUpdateFleetOptions(), token);
    }

    public async Task<CommandResult> UpdateImagePermissions(AwsAppstreamUpdateImagePermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateStack(AwsAppstreamUpdateStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}