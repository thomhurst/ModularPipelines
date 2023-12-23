using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsRds
{
    public AwsRds(
        AwsRdsWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsRdsWait Wait { get; }

    public async Task<CommandResult> AddOptionToOptionGroup(AwsRdsAddOptionToOptionGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddRoleToDbCluster(AwsRdsAddRoleToDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddRoleToDbInstance(AwsRdsAddRoleToDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddSourceIdentifierToSubscription(AwsRdsAddSourceIdentifierToSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddTagsToResource(AwsRdsAddTagsToResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ApplyPendingMaintenanceAction(AwsRdsApplyPendingMaintenanceActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AuthorizeDbSecurityGroupIngress(AwsRdsAuthorizeDbSecurityGroupIngressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BacktrackDbCluster(AwsRdsBacktrackDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelExportTask(AwsRdsCancelExportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyDbClusterParameterGroup(AwsRdsCopyDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyDbClusterSnapshot(AwsRdsCopyDbClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyDbParameterGroup(AwsRdsCopyDbParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyDbSnapshot(AwsRdsCopyDbSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyOptionGroup(AwsRdsCopyOptionGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBlueGreenDeployment(AwsRdsCreateBlueGreenDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomDbEngineVersion(AwsRdsCreateCustomDbEngineVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbCluster(AwsRdsCreateDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbClusterEndpoint(AwsRdsCreateDbClusterEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbClusterParameterGroup(AwsRdsCreateDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbClusterSnapshot(AwsRdsCreateDbClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbInstance(AwsRdsCreateDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbInstanceReadReplica(AwsRdsCreateDbInstanceReadReplicaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbParameterGroup(AwsRdsCreateDbParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbProxy(AwsRdsCreateDbProxyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbProxyEndpoint(AwsRdsCreateDbProxyEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbSecurityGroup(AwsRdsCreateDbSecurityGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbSnapshot(AwsRdsCreateDbSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDbSubnetGroup(AwsRdsCreateDbSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEventSubscription(AwsRdsCreateEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGlobalCluster(AwsRdsCreateGlobalClusterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsCreateGlobalClusterOptions(), token);
    }

    public async Task<CommandResult> CreateIntegration(AwsRdsCreateIntegrationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOptionGroup(AwsRdsCreateOptionGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTenantDatabase(AwsRdsCreateTenantDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBlueGreenDeployment(AwsRdsDeleteBlueGreenDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomDbEngineVersion(AwsRdsDeleteCustomDbEngineVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbCluster(AwsRdsDeleteDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbClusterAutomatedBackup(AwsRdsDeleteDbClusterAutomatedBackupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbClusterEndpoint(AwsRdsDeleteDbClusterEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbClusterParameterGroup(AwsRdsDeleteDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbClusterSnapshot(AwsRdsDeleteDbClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbInstance(AwsRdsDeleteDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbInstanceAutomatedBackup(AwsRdsDeleteDbInstanceAutomatedBackupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDeleteDbInstanceAutomatedBackupOptions(), token);
    }

    public async Task<CommandResult> DeleteDbParameterGroup(AwsRdsDeleteDbParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbProxy(AwsRdsDeleteDbProxyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbProxyEndpoint(AwsRdsDeleteDbProxyEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbSecurityGroup(AwsRdsDeleteDbSecurityGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbSnapshot(AwsRdsDeleteDbSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDbSubnetGroup(AwsRdsDeleteDbSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEventSubscription(AwsRdsDeleteEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGlobalCluster(AwsRdsDeleteGlobalClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIntegration(AwsRdsDeleteIntegrationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOptionGroup(AwsRdsDeleteOptionGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTenantDatabase(AwsRdsDeleteTenantDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterDbProxyTargets(AwsRdsDeregisterDbProxyTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccountAttributes(AwsRdsDescribeAccountAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeAccountAttributesOptions(), token);
    }

    public async Task<CommandResult> DescribeBlueGreenDeployments(AwsRdsDescribeBlueGreenDeploymentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeBlueGreenDeploymentsOptions(), token);
    }

    public async Task<CommandResult> DescribeCertificates(AwsRdsDescribeCertificatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeCertificatesOptions(), token);
    }

    public async Task<CommandResult> DescribeDbClusterAutomatedBackups(AwsRdsDescribeDbClusterAutomatedBackupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbClusterAutomatedBackupsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbClusterBacktracks(AwsRdsDescribeDbClusterBacktracksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbClusterEndpoints(AwsRdsDescribeDbClusterEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbClusterEndpointsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbClusterParameterGroups(AwsRdsDescribeDbClusterParameterGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbClusterParameterGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbClusterParameters(AwsRdsDescribeDbClusterParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbClusterSnapshotAttributes(AwsRdsDescribeDbClusterSnapshotAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbClusterSnapshots(AwsRdsDescribeDbClusterSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbClusterSnapshotsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbClusters(AwsRdsDescribeDbClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbClustersOptions(), token);
    }

    public async Task<CommandResult> DescribeDbEngineVersions(AwsRdsDescribeDbEngineVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbEngineVersionsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbInstanceAutomatedBackups(AwsRdsDescribeDbInstanceAutomatedBackupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbInstanceAutomatedBackupsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbInstances(AwsRdsDescribeDbInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeDbLogFiles(AwsRdsDescribeDbLogFilesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbParameterGroups(AwsRdsDescribeDbParameterGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbParameterGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbParameters(AwsRdsDescribeDbParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbProxies(AwsRdsDescribeDbProxiesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbProxiesOptions(), token);
    }

    public async Task<CommandResult> DescribeDbProxyEndpoints(AwsRdsDescribeDbProxyEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbProxyEndpointsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbProxyTargetGroups(AwsRdsDescribeDbProxyTargetGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbProxyTargets(AwsRdsDescribeDbProxyTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbRecommendations(AwsRdsDescribeDbRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbRecommendationsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbSecurityGroups(AwsRdsDescribeDbSecurityGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbSecurityGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbSnapshotAttributes(AwsRdsDescribeDbSnapshotAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDbSnapshotTenantDatabases(AwsRdsDescribeDbSnapshotTenantDatabasesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbSnapshotTenantDatabasesOptions(), token);
    }

    public async Task<CommandResult> DescribeDbSnapshots(AwsRdsDescribeDbSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbSnapshotsOptions(), token);
    }

    public async Task<CommandResult> DescribeDbSubnetGroups(AwsRdsDescribeDbSubnetGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeDbSubnetGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeEngineDefaultClusterParameters(AwsRdsDescribeEngineDefaultClusterParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEngineDefaultParameters(AwsRdsDescribeEngineDefaultParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEventCategories(AwsRdsDescribeEventCategoriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeEventCategoriesOptions(), token);
    }

    public async Task<CommandResult> DescribeEventSubscriptions(AwsRdsDescribeEventSubscriptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeEventSubscriptionsOptions(), token);
    }

    public async Task<CommandResult> DescribeEvents(AwsRdsDescribeEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeEventsOptions(), token);
    }

    public async Task<CommandResult> DescribeExportTasks(AwsRdsDescribeExportTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeExportTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeGlobalClusters(AwsRdsDescribeGlobalClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeGlobalClustersOptions(), token);
    }

    public async Task<CommandResult> DescribeIntegrations(AwsRdsDescribeIntegrationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeIntegrationsOptions(), token);
    }

    public async Task<CommandResult> DescribeOptionGroupOptions(AwsRdsDescribeOptionGroupOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeOptionGroups(AwsRdsDescribeOptionGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeOptionGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeOrderableDbInstanceOptions(AwsRdsDescribeOrderableDbInstanceOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePendingMaintenanceActions(AwsRdsDescribePendingMaintenanceActionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribePendingMaintenanceActionsOptions(), token);
    }

    public async Task<CommandResult> DescribeReservedDbInstances(AwsRdsDescribeReservedDbInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeReservedDbInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeReservedDbInstancesOfferings(AwsRdsDescribeReservedDbInstancesOfferingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeReservedDbInstancesOfferingsOptions(), token);
    }

    public async Task<CommandResult> DescribeSourceRegions(AwsRdsDescribeSourceRegionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeSourceRegionsOptions(), token);
    }

    public async Task<CommandResult> DescribeTenantDatabases(AwsRdsDescribeTenantDatabasesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsDescribeTenantDatabasesOptions(), token);
    }

    public async Task<CommandResult> DescribeValidDbInstanceModifications(AwsRdsDescribeValidDbInstanceModificationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableHttpEndpoint(AwsRdsDisableHttpEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DownloadDbLogFilePortion(AwsRdsDownloadDbLogFilePortionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableHttpEndpoint(AwsRdsEnableHttpEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> FailoverDbCluster(AwsRdsFailoverDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> FailoverGlobalCluster(AwsRdsFailoverGlobalClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateDbAuthToken(AwsRdsGenerateDbAuthTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsRdsListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyActivityStream(AwsRdsModifyActivityStreamOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsModifyActivityStreamOptions(), token);
    }

    public async Task<CommandResult> ModifyCertificates(AwsRdsModifyCertificatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsModifyCertificatesOptions(), token);
    }

    public async Task<CommandResult> ModifyCurrentDbClusterCapacity(AwsRdsModifyCurrentDbClusterCapacityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyCustomDbEngineVersion(AwsRdsModifyCustomDbEngineVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbCluster(AwsRdsModifyDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbClusterEndpoint(AwsRdsModifyDbClusterEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbClusterParameterGroup(AwsRdsModifyDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbClusterSnapshotAttribute(AwsRdsModifyDbClusterSnapshotAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbInstance(AwsRdsModifyDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbParameterGroup(AwsRdsModifyDbParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbProxy(AwsRdsModifyDbProxyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbProxyEndpoint(AwsRdsModifyDbProxyEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbProxyTargetGroup(AwsRdsModifyDbProxyTargetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbRecommendation(AwsRdsModifyDbRecommendationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbSnapshot(AwsRdsModifyDbSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbSnapshotAttribute(AwsRdsModifyDbSnapshotAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDbSubnetGroup(AwsRdsModifyDbSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyEventSubscription(AwsRdsModifyEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyGlobalCluster(AwsRdsModifyGlobalClusterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsModifyGlobalClusterOptions(), token);
    }

    public async Task<CommandResult> ModifyTenantDatabase(AwsRdsModifyTenantDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PromoteReadReplica(AwsRdsPromoteReadReplicaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PromoteReadReplicaDbCluster(AwsRdsPromoteReadReplicaDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PurchaseReservedDbInstancesOffering(AwsRdsPurchaseReservedDbInstancesOfferingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebootDbCluster(AwsRdsRebootDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebootDbInstance(AwsRdsRebootDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterDbProxyTargets(AwsRdsRegisterDbProxyTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveFromGlobalCluster(AwsRdsRemoveFromGlobalClusterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsRemoveFromGlobalClusterOptions(), token);
    }

    public async Task<CommandResult> RemoveOptionFromOptionGroup(AwsRdsRemoveOptionFromOptionGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveRoleFromDbCluster(AwsRdsRemoveRoleFromDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveRoleFromDbInstance(AwsRdsRemoveRoleFromDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveSourceIdentifierFromSubscription(AwsRdsRemoveSourceIdentifierFromSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTagsFromResource(AwsRdsRemoveTagsFromResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetDbClusterParameterGroup(AwsRdsResetDbClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetDbParameterGroup(AwsRdsResetDbParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreDbClusterFromS3(AwsRdsRestoreDbClusterFromS3Options options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreDbClusterFromSnapshot(AwsRdsRestoreDbClusterFromSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreDbClusterToPointInTime(AwsRdsRestoreDbClusterToPointInTimeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreDbInstanceFromDbSnapshot(AwsRdsRestoreDbInstanceFromDbSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreDbInstanceFromS3(AwsRdsRestoreDbInstanceFromS3Options options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreDbInstanceToPointInTime(AwsRdsRestoreDbInstanceToPointInTimeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RevokeDbSecurityGroupIngress(AwsRdsRevokeDbSecurityGroupIngressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartActivityStream(AwsRdsStartActivityStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartDbCluster(AwsRdsStartDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartDbInstance(AwsRdsStartDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartDbInstanceAutomatedBackupsReplication(AwsRdsStartDbInstanceAutomatedBackupsReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartExportTask(AwsRdsStartExportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopActivityStream(AwsRdsStopActivityStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopDbCluster(AwsRdsStopDbClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopDbInstance(AwsRdsStopDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopDbInstanceAutomatedBackupsReplication(AwsRdsStopDbInstanceAutomatedBackupsReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SwitchoverBlueGreenDeployment(AwsRdsSwitchoverBlueGreenDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SwitchoverGlobalCluster(AwsRdsSwitchoverGlobalClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SwitchoverReadReplica(AwsRdsSwitchoverReadReplicaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}