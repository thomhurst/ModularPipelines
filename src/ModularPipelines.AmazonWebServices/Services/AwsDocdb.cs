using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsDocdb
{
    public AwsDocdb(
        AwsDocdbWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsDocdbWait Wait { get; }

    public async Task<CommandResult> AddSourceIdentifierToSubscription(AwsDocdbAddSourceIdentifierToSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddTagsToResource(AwsDocdbAddTagsToResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ApplyPendingMaintenanceAction(AwsDocdbApplyPendingMaintenanceActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyDbClusterParameterGroup(AwsDocdbCopyDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyDbClusterSnapshot(AwsDocdbCopyDbClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbCluster(AwsDocdbCreateDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbClusterParameterGroup(AwsDocdbCreateDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbClusterSnapshot(AwsDocdbCreateDbClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbInstance(AwsDocdbCreateDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbSubnetGroup(AwsDocdbCreateDbSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEventSubscription(AwsDocdbCreateEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGlobalCluster(AwsDocdbCreateGlobalClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbCluster(AwsDocdbDeleteDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbClusterParameterGroup(AwsDocdbDeleteDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbClusterSnapshot(AwsDocdbDeleteDbClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbInstance(AwsDocdbDeleteDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbSubnetGroup(AwsDocdbDeleteDbSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEventSubscription(AwsDocdbDeleteEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGlobalCluster(AwsDocdbDeleteGlobalClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCertificates(AwsDocdbDescribeCertificatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbDescribeCertificatesOptions(), token);
    }

    public async Task<CommandResult> DescribeDbClusterParameterGroups(AwsDocdbDescribeDbClusterParameterGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbDescribeDbClusterParameterGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbClusterParameters(AwsDocdbDescribeDbClusterParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbClusterSnapshotAttributes(AwsDocdbDescribeDbClusterSnapshotAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbClusterSnapshots(AwsDocdbDescribeDbClusterSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbDescribeDbClusterSnapshotsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbClusters(AwsDocdbDescribeDbClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbDescribeDbClustersOptions(), token);
    }

    public async Task<CommandResult> DescribeDbEngineVersions(AwsDocdbDescribeDbEngineVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbDescribeDbEngineVersionsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbInstances(AwsDocdbDescribeDbInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbDescribeDbInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeDbSubnetGroups(AwsDocdbDescribeDbSubnetGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbDescribeDbSubnetGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeEngineDefaultClusterParameters(AwsDocdbDescribeEngineDefaultClusterParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEventCategories(AwsDocdbDescribeEventCategoriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbDescribeEventCategoriesOptions(), token);
    }

    public async Task<CommandResult> DescribeEventSubscriptions(AwsDocdbDescribeEventSubscriptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbDescribeEventSubscriptionsOptions(), token);
    }

    public async Task<CommandResult> DescribeEvents(AwsDocdbDescribeEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbDescribeEventsOptions(), token);
    }

    public async Task<CommandResult> DescribeGlobalClusters(AwsDocdbDescribeGlobalClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbDescribeGlobalClustersOptions(), token);
    }

    public async Task<CommandResult> DescribeOrderableDbInstanceOptions(AwsDocdbDescribeOrderableDbInstanceOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePendingMaintenanceActions(AwsDocdbDescribePendingMaintenanceActionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbDescribePendingMaintenanceActionsOptions(), token);
    }

    public async Task<CommandResult> FailoverDbCluster(AwsDocdbFailoverDbClusterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbFailoverDbClusterOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsDocdbListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbCluster(AwsDocdbModifyDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbClusterParameterGroup(AwsDocdbModifyDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbClusterSnapshotAttribute(AwsDocdbModifyDbClusterSnapshotAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbInstance(AwsDocdbModifyDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbSubnetGroup(AwsDocdbModifyDbSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyEventSubscription(AwsDocdbModifyEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyGlobalCluster(AwsDocdbModifyGlobalClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebootDbInstance(AwsDocdbRebootDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveFromGlobalCluster(AwsDocdbRemoveFromGlobalClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveSourceIdentifierFromSubscription(AwsDocdbRemoveSourceIdentifierFromSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTagsFromResource(AwsDocdbRemoveTagsFromResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetDbClusterParameterGroup(AwsDocdbResetDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreDbClusterFromSnapshot(AwsDocdbRestoreDbClusterFromSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreDbClusterToPointInTime(AwsDocdbRestoreDbClusterToPointInTimeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartDbCluster(AwsDocdbStartDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopDbCluster(AwsDocdbStopDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}