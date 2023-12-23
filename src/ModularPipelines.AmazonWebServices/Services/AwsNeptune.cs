using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsNeptune
{
    public AwsNeptune(
        AwsNeptuneWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsNeptuneWait Wait { get; }

    public async Task<CommandResult> AddRoleToDbCluster(AwsNeptuneAddRoleToDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddSourceIdentifierToSubscription(AwsNeptuneAddSourceIdentifierToSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddTagsToResource(AwsNeptuneAddTagsToResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ApplyPendingMaintenanceAction(AwsNeptuneApplyPendingMaintenanceActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyDbClusterParameterGroup(AwsNeptuneCopyDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyDbClusterSnapshot(AwsNeptuneCopyDbClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyDbParameterGroup(AwsNeptuneCopyDbParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbCluster(AwsNeptuneCreateDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbClusterEndpoint(AwsNeptuneCreateDbClusterEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbClusterParameterGroup(AwsNeptuneCreateDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbClusterSnapshot(AwsNeptuneCreateDbClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbInstance(AwsNeptuneCreateDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbParameterGroup(AwsNeptuneCreateDbParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbSubnetGroup(AwsNeptuneCreateDbSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEventSubscription(AwsNeptuneCreateEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGlobalCluster(AwsNeptuneCreateGlobalClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbCluster(AwsNeptuneDeleteDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbClusterEndpoint(AwsNeptuneDeleteDbClusterEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbClusterParameterGroup(AwsNeptuneDeleteDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbClusterSnapshot(AwsNeptuneDeleteDbClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbInstance(AwsNeptuneDeleteDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbParameterGroup(AwsNeptuneDeleteDbParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbSubnetGroup(AwsNeptuneDeleteDbSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEventSubscription(AwsNeptuneDeleteEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGlobalCluster(AwsNeptuneDeleteGlobalClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbClusterEndpoints(AwsNeptuneDescribeDbClusterEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneDescribeDbClusterEndpointsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbClusterParameterGroups(AwsNeptuneDescribeDbClusterParameterGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneDescribeDbClusterParameterGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbClusterParameters(AwsNeptuneDescribeDbClusterParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbClusterSnapshotAttributes(AwsNeptuneDescribeDbClusterSnapshotAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbClusterSnapshots(AwsNeptuneDescribeDbClusterSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneDescribeDbClusterSnapshotsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbClusters(AwsNeptuneDescribeDbClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneDescribeDbClustersOptions(), token);
    }

    public async Task<CommandResult> DescribeDbEngineVersions(AwsNeptuneDescribeDbEngineVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneDescribeDbEngineVersionsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbInstances(AwsNeptuneDescribeDbInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneDescribeDbInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeDbParameterGroups(AwsNeptuneDescribeDbParameterGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneDescribeDbParameterGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbParameters(AwsNeptuneDescribeDbParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbSubnetGroups(AwsNeptuneDescribeDbSubnetGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneDescribeDbSubnetGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeEngineDefaultClusterParameters(AwsNeptuneDescribeEngineDefaultClusterParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEngineDefaultParameters(AwsNeptuneDescribeEngineDefaultParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEventCategories(AwsNeptuneDescribeEventCategoriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneDescribeEventCategoriesOptions(), token);
    }

    public async Task<CommandResult> DescribeEventSubscriptions(AwsNeptuneDescribeEventSubscriptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneDescribeEventSubscriptionsOptions(), token);
    }

    public async Task<CommandResult> DescribeEvents(AwsNeptuneDescribeEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneDescribeEventsOptions(), token);
    }

    public async Task<CommandResult> DescribeGlobalClusters(AwsNeptuneDescribeGlobalClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneDescribeGlobalClustersOptions(), token);
    }

    public async Task<CommandResult> DescribeOrderableDbInstanceOptions(AwsNeptuneDescribeOrderableDbInstanceOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePendingMaintenanceActions(AwsNeptuneDescribePendingMaintenanceActionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneDescribePendingMaintenanceActionsOptions(), token);
    }

    public async Task<CommandResult> DescribeValidDbInstanceModifications(AwsNeptuneDescribeValidDbInstanceModificationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> FailoverDbCluster(AwsNeptuneFailoverDbClusterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneFailoverDbClusterOptions(), token);
    }

    public async Task<CommandResult> FailoverGlobalCluster(AwsNeptuneFailoverGlobalClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsNeptuneListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbCluster(AwsNeptuneModifyDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbClusterEndpoint(AwsNeptuneModifyDbClusterEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbClusterParameterGroup(AwsNeptuneModifyDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbClusterSnapshotAttribute(AwsNeptuneModifyDbClusterSnapshotAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbInstance(AwsNeptuneModifyDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbParameterGroup(AwsNeptuneModifyDbParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbSubnetGroup(AwsNeptuneModifyDbSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyEventSubscription(AwsNeptuneModifyEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyGlobalCluster(AwsNeptuneModifyGlobalClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PromoteReadReplicaDbCluster(AwsNeptunePromoteReadReplicaDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebootDbInstance(AwsNeptuneRebootDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveFromGlobalCluster(AwsNeptuneRemoveFromGlobalClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveRoleFromDbCluster(AwsNeptuneRemoveRoleFromDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveSourceIdentifierFromSubscription(AwsNeptuneRemoveSourceIdentifierFromSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTagsFromResource(AwsNeptuneRemoveTagsFromResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetDbClusterParameterGroup(AwsNeptuneResetDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetDbParameterGroup(AwsNeptuneResetDbParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreDbClusterFromSnapshot(AwsNeptuneRestoreDbClusterFromSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreDbClusterToPointInTime(AwsNeptuneRestoreDbClusterToPointInTimeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartDbCluster(AwsNeptuneStartDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopDbCluster(AwsNeptuneStopDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}