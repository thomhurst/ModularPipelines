using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsDms
{
    public AwsDms(
        AwsDmsWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsDmsWait Wait { get; }

    public async Task<CommandResult> AddTagsToResource(AwsDmsAddTagsToResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ApplyPendingMaintenanceAction(AwsDmsApplyPendingMaintenanceActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchStartRecommendations(AwsDmsBatchStartRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsBatchStartRecommendationsOptions(), token);
    }

    public async Task<CommandResult> CancelReplicationTaskAssessmentRun(AwsDmsCancelReplicationTaskAssessmentRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataProvider(AwsDmsCreateDataProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEndpoint(AwsDmsCreateEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEventSubscription(AwsDmsCreateEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFleetAdvisorCollector(AwsDmsCreateFleetAdvisorCollectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInstanceProfile(AwsDmsCreateInstanceProfileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsCreateInstanceProfileOptions(), token);
    }

    public async Task<CommandResult> CreateMigrationProject(AwsDmsCreateMigrationProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReplicationConfig(AwsDmsCreateReplicationConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReplicationInstance(AwsDmsCreateReplicationInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReplicationSubnetGroup(AwsDmsCreateReplicationSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReplicationTask(AwsDmsCreateReplicationTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCertificate(AwsDmsDeleteCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConnection(AwsDmsDeleteConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDataProvider(AwsDmsDeleteDataProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEndpoint(AwsDmsDeleteEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEventSubscription(AwsDmsDeleteEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFleetAdvisorCollector(AwsDmsDeleteFleetAdvisorCollectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFleetAdvisorDatabases(AwsDmsDeleteFleetAdvisorDatabasesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInstanceProfile(AwsDmsDeleteInstanceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMigrationProject(AwsDmsDeleteMigrationProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReplicationConfig(AwsDmsDeleteReplicationConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReplicationInstance(AwsDmsDeleteReplicationInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReplicationSubnetGroup(AwsDmsDeleteReplicationSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReplicationTask(AwsDmsDeleteReplicationTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReplicationTaskAssessmentRun(AwsDmsDeleteReplicationTaskAssessmentRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccountAttributes(AwsDmsDescribeAccountAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeAccountAttributesOptions(), token);
    }

    public async Task<CommandResult> DescribeApplicableIndividualAssessments(AwsDmsDescribeApplicableIndividualAssessmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeApplicableIndividualAssessmentsOptions(), token);
    }

    public async Task<CommandResult> DescribeCertificates(AwsDmsDescribeCertificatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeCertificatesOptions(), token);
    }

    public async Task<CommandResult> DescribeConnections(AwsDmsDescribeConnectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeConnectionsOptions(), token);
    }

    public async Task<CommandResult> DescribeConversionConfiguration(AwsDmsDescribeConversionConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDataProviders(AwsDmsDescribeDataProvidersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeDataProvidersOptions(), token);
    }

    public async Task<CommandResult> DescribeEndpointSettings(AwsDmsDescribeEndpointSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEndpointTypes(AwsDmsDescribeEndpointTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeEndpointTypesOptions(), token);
    }

    public async Task<CommandResult> DescribeEndpoints(AwsDmsDescribeEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeEndpointsOptions(), token);
    }

    public async Task<CommandResult> DescribeEngineVersions(AwsDmsDescribeEngineVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeEngineVersionsOptions(), token);
    }

    public async Task<CommandResult> DescribeEventCategories(AwsDmsDescribeEventCategoriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeEventCategoriesOptions(), token);
    }

    public async Task<CommandResult> DescribeEventSubscriptions(AwsDmsDescribeEventSubscriptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeEventSubscriptionsOptions(), token);
    }

    public async Task<CommandResult> DescribeEvents(AwsDmsDescribeEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeEventsOptions(), token);
    }

    public async Task<CommandResult> DescribeExtensionPackAssociations(AwsDmsDescribeExtensionPackAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFleetAdvisorCollectors(AwsDmsDescribeFleetAdvisorCollectorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeFleetAdvisorCollectorsOptions(), token);
    }

    public async Task<CommandResult> DescribeFleetAdvisorDatabases(AwsDmsDescribeFleetAdvisorDatabasesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeFleetAdvisorDatabasesOptions(), token);
    }

    public async Task<CommandResult> DescribeFleetAdvisorLsaAnalysis(AwsDmsDescribeFleetAdvisorLsaAnalysisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeFleetAdvisorLsaAnalysisOptions(), token);
    }

    public async Task<CommandResult> DescribeFleetAdvisorSchemaObjectSummary(AwsDmsDescribeFleetAdvisorSchemaObjectSummaryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeFleetAdvisorSchemaObjectSummaryOptions(), token);
    }

    public async Task<CommandResult> DescribeFleetAdvisorSchemas(AwsDmsDescribeFleetAdvisorSchemasOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeFleetAdvisorSchemasOptions(), token);
    }

    public async Task<CommandResult> DescribeInstanceProfiles(AwsDmsDescribeInstanceProfilesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeInstanceProfilesOptions(), token);
    }

    public async Task<CommandResult> DescribeMetadataModelAssessments(AwsDmsDescribeMetadataModelAssessmentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMetadataModelConversions(AwsDmsDescribeMetadataModelConversionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMetadataModelExportsAsScript(AwsDmsDescribeMetadataModelExportsAsScriptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMetadataModelExportsToTarget(AwsDmsDescribeMetadataModelExportsToTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMetadataModelImports(AwsDmsDescribeMetadataModelImportsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMigrationProjects(AwsDmsDescribeMigrationProjectsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeMigrationProjectsOptions(), token);
    }

    public async Task<CommandResult> DescribeOrderableReplicationInstances(AwsDmsDescribeOrderableReplicationInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeOrderableReplicationInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribePendingMaintenanceActions(AwsDmsDescribePendingMaintenanceActionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribePendingMaintenanceActionsOptions(), token);
    }

    public async Task<CommandResult> DescribeRecommendationLimitations(AwsDmsDescribeRecommendationLimitationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeRecommendationLimitationsOptions(), token);
    }

    public async Task<CommandResult> DescribeRecommendations(AwsDmsDescribeRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeRecommendationsOptions(), token);
    }

    public async Task<CommandResult> DescribeRefreshSchemasStatus(AwsDmsDescribeRefreshSchemasStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeReplicationConfigs(AwsDmsDescribeReplicationConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeReplicationConfigsOptions(), token);
    }

    public async Task<CommandResult> DescribeReplicationInstanceTaskLogs(AwsDmsDescribeReplicationInstanceTaskLogsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeReplicationInstances(AwsDmsDescribeReplicationInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeReplicationInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeReplicationSubnetGroups(AwsDmsDescribeReplicationSubnetGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeReplicationSubnetGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeReplicationTableStatistics(AwsDmsDescribeReplicationTableStatisticsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeReplicationTaskAssessmentResults(AwsDmsDescribeReplicationTaskAssessmentResultsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeReplicationTaskAssessmentResultsOptions(), token);
    }

    public async Task<CommandResult> DescribeReplicationTaskAssessmentRuns(AwsDmsDescribeReplicationTaskAssessmentRunsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeReplicationTaskAssessmentRunsOptions(), token);
    }

    public async Task<CommandResult> DescribeReplicationTaskIndividualAssessments(AwsDmsDescribeReplicationTaskIndividualAssessmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeReplicationTaskIndividualAssessmentsOptions(), token);
    }

    public async Task<CommandResult> DescribeReplicationTasks(AwsDmsDescribeReplicationTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeReplicationTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeReplications(AwsDmsDescribeReplicationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsDescribeReplicationsOptions(), token);
    }

    public async Task<CommandResult> DescribeSchemas(AwsDmsDescribeSchemasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTableStatistics(AwsDmsDescribeTableStatisticsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportMetadataModelAssessment(AwsDmsExportMetadataModelAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportCertificate(AwsDmsImportCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsDmsListTagsForResourceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsListTagsForResourceOptions(), token);
    }

    public async Task<CommandResult> ModifyConversionConfiguration(AwsDmsModifyConversionConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDataProvider(AwsDmsModifyDataProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyEndpoint(AwsDmsModifyEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyEventSubscription(AwsDmsModifyEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyInstanceProfile(AwsDmsModifyInstanceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyMigrationProject(AwsDmsModifyMigrationProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyReplicationConfig(AwsDmsModifyReplicationConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyReplicationInstance(AwsDmsModifyReplicationInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyReplicationSubnetGroup(AwsDmsModifyReplicationSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyReplicationTask(AwsDmsModifyReplicationTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MoveReplicationTask(AwsDmsMoveReplicationTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebootReplicationInstance(AwsDmsRebootReplicationInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RefreshSchemas(AwsDmsRefreshSchemasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReloadReplicationTables(AwsDmsReloadReplicationTablesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReloadTables(AwsDmsReloadTablesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTagsFromResource(AwsDmsRemoveTagsFromResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RunFleetAdvisorLsaAnalysis(AwsDmsRunFleetAdvisorLsaAnalysisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsRunFleetAdvisorLsaAnalysisOptions(), token);
    }

    public async Task<CommandResult> StartExtensionPackAssociation(AwsDmsStartExtensionPackAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMetadataModelAssessment(AwsDmsStartMetadataModelAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMetadataModelConversion(AwsDmsStartMetadataModelConversionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMetadataModelExportAsScript(AwsDmsStartMetadataModelExportAsScriptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMetadataModelExportToTarget(AwsDmsStartMetadataModelExportToTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMetadataModelImport(AwsDmsStartMetadataModelImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartRecommendations(AwsDmsStartRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartReplication(AwsDmsStartReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartReplicationTask(AwsDmsStartReplicationTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartReplicationTaskAssessment(AwsDmsStartReplicationTaskAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartReplicationTaskAssessmentRun(AwsDmsStartReplicationTaskAssessmentRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopReplication(AwsDmsStopReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopReplicationTask(AwsDmsStopReplicationTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestConnection(AwsDmsTestConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSubscriptionsToEventBridge(AwsDmsUpdateSubscriptionsToEventBridgeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsUpdateSubscriptionsToEventBridgeOptions(), token);
    }
}