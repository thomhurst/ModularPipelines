using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsElasticache
{
    public AwsElasticache(
        AwsElasticacheWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsElasticacheWait Wait { get; }

    public async Task<CommandResult> AddTagsToResource(AwsElasticacheAddTagsToResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AuthorizeCacheSecurityGroupIngress(AwsElasticacheAuthorizeCacheSecurityGroupIngressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchApplyUpdateAction(AwsElasticacheBatchApplyUpdateActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchStopUpdateAction(AwsElasticacheBatchStopUpdateActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CompleteMigration(AwsElasticacheCompleteMigrationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyServerlessCacheSnapshot(AwsElasticacheCopyServerlessCacheSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopySnapshot(AwsElasticacheCopySnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCacheCluster(AwsElasticacheCreateCacheClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCacheParameterGroup(AwsElasticacheCreateCacheParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCacheSecurityGroup(AwsElasticacheCreateCacheSecurityGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCacheSubnetGroup(AwsElasticacheCreateCacheSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGlobalReplicationGroup(AwsElasticacheCreateGlobalReplicationGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReplicationGroup(AwsElasticacheCreateReplicationGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateServerlessCache(AwsElasticacheCreateServerlessCacheOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateServerlessCacheSnapshot(AwsElasticacheCreateServerlessCacheSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSnapshot(AwsElasticacheCreateSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUser(AwsElasticacheCreateUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUserGroup(AwsElasticacheCreateUserGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DecreaseNodeGroupsInGlobalReplicationGroup(AwsElasticacheDecreaseNodeGroupsInGlobalReplicationGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DecreaseReplicaCount(AwsElasticacheDecreaseReplicaCountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCacheCluster(AwsElasticacheDeleteCacheClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCacheParameterGroup(AwsElasticacheDeleteCacheParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCacheSecurityGroup(AwsElasticacheDeleteCacheSecurityGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCacheSubnetGroup(AwsElasticacheDeleteCacheSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGlobalReplicationGroup(AwsElasticacheDeleteGlobalReplicationGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReplicationGroup(AwsElasticacheDeleteReplicationGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteServerlessCache(AwsElasticacheDeleteServerlessCacheOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteServerlessCacheSnapshot(AwsElasticacheDeleteServerlessCacheSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSnapshot(AwsElasticacheDeleteSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUser(AwsElasticacheDeleteUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUserGroup(AwsElasticacheDeleteUserGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCacheClusters(AwsElasticacheDescribeCacheClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeCacheClustersOptions(), token);
    }

    public async Task<CommandResult> DescribeCacheEngineVersions(AwsElasticacheDescribeCacheEngineVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeCacheEngineVersionsOptions(), token);
    }

    public async Task<CommandResult> DescribeCacheParameterGroups(AwsElasticacheDescribeCacheParameterGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeCacheParameterGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeCacheParameters(AwsElasticacheDescribeCacheParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCacheSecurityGroups(AwsElasticacheDescribeCacheSecurityGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeCacheSecurityGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeCacheSubnetGroups(AwsElasticacheDescribeCacheSubnetGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeCacheSubnetGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeEngineDefaultParameters(AwsElasticacheDescribeEngineDefaultParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEvents(AwsElasticacheDescribeEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeEventsOptions(), token);
    }

    public async Task<CommandResult> DescribeGlobalReplicationGroups(AwsElasticacheDescribeGlobalReplicationGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeGlobalReplicationGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeReplicationGroups(AwsElasticacheDescribeReplicationGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeReplicationGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeReservedCacheNodes(AwsElasticacheDescribeReservedCacheNodesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeReservedCacheNodesOptions(), token);
    }

    public async Task<CommandResult> DescribeReservedCacheNodesOfferings(AwsElasticacheDescribeReservedCacheNodesOfferingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeReservedCacheNodesOfferingsOptions(), token);
    }

    public async Task<CommandResult> DescribeServerlessCacheSnapshots(AwsElasticacheDescribeServerlessCacheSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeServerlessCacheSnapshotsOptions(), token);
    }

    public async Task<CommandResult> DescribeServerlessCaches(AwsElasticacheDescribeServerlessCachesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeServerlessCachesOptions(), token);
    }

    public async Task<CommandResult> DescribeServiceUpdates(AwsElasticacheDescribeServiceUpdatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeServiceUpdatesOptions(), token);
    }

    public async Task<CommandResult> DescribeSnapshots(AwsElasticacheDescribeSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeSnapshotsOptions(), token);
    }

    public async Task<CommandResult> DescribeUpdateActions(AwsElasticacheDescribeUpdateActionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeUpdateActionsOptions(), token);
    }

    public async Task<CommandResult> DescribeUserGroups(AwsElasticacheDescribeUserGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeUserGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeUsers(AwsElasticacheDescribeUsersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheDescribeUsersOptions(), token);
    }

    public async Task<CommandResult> DisassociateGlobalReplicationGroup(AwsElasticacheDisassociateGlobalReplicationGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportServerlessCacheSnapshot(AwsElasticacheExportServerlessCacheSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> FailoverGlobalReplicationGroup(AwsElasticacheFailoverGlobalReplicationGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> IncreaseNodeGroupsInGlobalReplicationGroup(AwsElasticacheIncreaseNodeGroupsInGlobalReplicationGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> IncreaseReplicaCount(AwsElasticacheIncreaseReplicaCountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAllowedNodeTypeModifications(AwsElasticacheListAllowedNodeTypeModificationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheListAllowedNodeTypeModificationsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsElasticacheListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyCacheCluster(AwsElasticacheModifyCacheClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyCacheParameterGroup(AwsElasticacheModifyCacheParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyCacheSubnetGroup(AwsElasticacheModifyCacheSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyGlobalReplicationGroup(AwsElasticacheModifyGlobalReplicationGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyReplicationGroup(AwsElasticacheModifyReplicationGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyReplicationGroupShardConfiguration(AwsElasticacheModifyReplicationGroupShardConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyServerlessCache(AwsElasticacheModifyServerlessCacheOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyUser(AwsElasticacheModifyUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyUserGroup(AwsElasticacheModifyUserGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PurchaseReservedCacheNodesOffering(AwsElasticachePurchaseReservedCacheNodesOfferingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebalanceSlotsInGlobalReplicationGroup(AwsElasticacheRebalanceSlotsInGlobalReplicationGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebootCacheCluster(AwsElasticacheRebootCacheClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTagsFromResource(AwsElasticacheRemoveTagsFromResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetCacheParameterGroup(AwsElasticacheResetCacheParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RevokeCacheSecurityGroupIngress(AwsElasticacheRevokeCacheSecurityGroupIngressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMigration(AwsElasticacheStartMigrationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestFailover(AwsElasticacheTestFailoverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestMigration(AwsElasticacheTestMigrationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}