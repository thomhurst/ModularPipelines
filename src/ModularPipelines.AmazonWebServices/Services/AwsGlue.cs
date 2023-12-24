using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsGlue
{
    public AwsGlue(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchCreatePartition(AwsGlueBatchCreatePartitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDeleteConnection(AwsGlueBatchDeleteConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDeletePartition(AwsGlueBatchDeletePartitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDeleteTable(AwsGlueBatchDeleteTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDeleteTableVersion(AwsGlueBatchDeleteTableVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetBlueprints(AwsGlueBatchGetBlueprintsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetCrawlers(AwsGlueBatchGetCrawlersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetCustomEntityTypes(AwsGlueBatchGetCustomEntityTypesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetDataQualityResult(AwsGlueBatchGetDataQualityResultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetDevEndpoints(AwsGlueBatchGetDevEndpointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetJobs(AwsGlueBatchGetJobsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetPartition(AwsGlueBatchGetPartitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetTableOptimizer(AwsGlueBatchGetTableOptimizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetTriggers(AwsGlueBatchGetTriggersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetWorkflows(AwsGlueBatchGetWorkflowsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchStopJobRun(AwsGlueBatchStopJobRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchUpdatePartition(AwsGlueBatchUpdatePartitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelDataQualityRuleRecommendationRun(AwsGlueCancelDataQualityRuleRecommendationRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelDataQualityRulesetEvaluationRun(AwsGlueCancelDataQualityRulesetEvaluationRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelMlTaskRun(AwsGlueCancelMlTaskRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelStatement(AwsGlueCancelStatementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckSchemaVersionValidity(AwsGlueCheckSchemaVersionValidityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBlueprint(AwsGlueCreateBlueprintOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateClassifier(AwsGlueCreateClassifierOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueCreateClassifierOptions(), token);
    }

    public async Task<CommandResult> CreateConnection(AwsGlueCreateConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCrawler(AwsGlueCreateCrawlerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomEntityType(AwsGlueCreateCustomEntityTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataQualityRuleset(AwsGlueCreateDataQualityRulesetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDatabase(AwsGlueCreateDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDevEndpoint(AwsGlueCreateDevEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateJob(AwsGlueCreateJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMlTransform(AwsGlueCreateMlTransformOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePartition(AwsGlueCreatePartitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePartitionIndex(AwsGlueCreatePartitionIndexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRegistry(AwsGlueCreateRegistryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSchema(AwsGlueCreateSchemaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateScript(AwsGlueCreateScriptOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueCreateScriptOptions(), token);
    }

    public async Task<CommandResult> CreateSecurityConfiguration(AwsGlueCreateSecurityConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSession(AwsGlueCreateSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTable(AwsGlueCreateTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTableOptimizer(AwsGlueCreateTableOptimizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTrigger(AwsGlueCreateTriggerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUserDefinedFunction(AwsGlueCreateUserDefinedFunctionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWorkflow(AwsGlueCreateWorkflowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBlueprint(AwsGlueDeleteBlueprintOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteClassifier(AwsGlueDeleteClassifierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteColumnStatisticsForPartition(AwsGlueDeleteColumnStatisticsForPartitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteColumnStatisticsForTable(AwsGlueDeleteColumnStatisticsForTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConnection(AwsGlueDeleteConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCrawler(AwsGlueDeleteCrawlerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomEntityType(AwsGlueDeleteCustomEntityTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDataQualityRuleset(AwsGlueDeleteDataQualityRulesetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDatabase(AwsGlueDeleteDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDevEndpoint(AwsGlueDeleteDevEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteJob(AwsGlueDeleteJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMlTransform(AwsGlueDeleteMlTransformOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePartition(AwsGlueDeletePartitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePartitionIndex(AwsGlueDeletePartitionIndexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRegistry(AwsGlueDeleteRegistryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsGlueDeleteResourcePolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueDeleteResourcePolicyOptions(), token);
    }

    public async Task<CommandResult> DeleteSchema(AwsGlueDeleteSchemaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSchemaVersions(AwsGlueDeleteSchemaVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSecurityConfiguration(AwsGlueDeleteSecurityConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSession(AwsGlueDeleteSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTable(AwsGlueDeleteTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTableOptimizer(AwsGlueDeleteTableOptimizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTableVersion(AwsGlueDeleteTableVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTrigger(AwsGlueDeleteTriggerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUserDefinedFunction(AwsGlueDeleteUserDefinedFunctionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWorkflow(AwsGlueDeleteWorkflowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBlueprint(AwsGlueGetBlueprintOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBlueprintRun(AwsGlueGetBlueprintRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBlueprintRuns(AwsGlueGetBlueprintRunsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCatalogImportStatus(AwsGlueGetCatalogImportStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetCatalogImportStatusOptions(), token);
    }

    public async Task<CommandResult> GetClassifier(AwsGlueGetClassifierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetClassifiers(AwsGlueGetClassifiersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetClassifiersOptions(), token);
    }

    public async Task<CommandResult> GetColumnStatisticsForPartition(AwsGlueGetColumnStatisticsForPartitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetColumnStatisticsForTable(AwsGlueGetColumnStatisticsForTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetColumnStatisticsTaskRun(AwsGlueGetColumnStatisticsTaskRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetColumnStatisticsTaskRuns(AwsGlueGetColumnStatisticsTaskRunsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnection(AwsGlueGetConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnections(AwsGlueGetConnectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetConnectionsOptions(), token);
    }

    public async Task<CommandResult> GetCrawler(AwsGlueGetCrawlerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCrawlerMetrics(AwsGlueGetCrawlerMetricsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetCrawlerMetricsOptions(), token);
    }

    public async Task<CommandResult> GetCrawlers(AwsGlueGetCrawlersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetCrawlersOptions(), token);
    }

    public async Task<CommandResult> GetCustomEntityType(AwsGlueGetCustomEntityTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDataCatalogEncryptionSettings(AwsGlueGetDataCatalogEncryptionSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetDataCatalogEncryptionSettingsOptions(), token);
    }

    public async Task<CommandResult> GetDataQualityResult(AwsGlueGetDataQualityResultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDataQualityRuleRecommendationRun(AwsGlueGetDataQualityRuleRecommendationRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDataQualityRuleset(AwsGlueGetDataQualityRulesetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDataQualityRulesetEvaluationRun(AwsGlueGetDataQualityRulesetEvaluationRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDatabase(AwsGlueGetDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDatabases(AwsGlueGetDatabasesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetDatabasesOptions(), token);
    }

    public async Task<CommandResult> GetDataflowGraph(AwsGlueGetDataflowGraphOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetDataflowGraphOptions(), token);
    }

    public async Task<CommandResult> GetDevEndpoint(AwsGlueGetDevEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDevEndpoints(AwsGlueGetDevEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetDevEndpointsOptions(), token);
    }

    public async Task<CommandResult> GetJob(AwsGlueGetJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetJobBookmark(AwsGlueGetJobBookmarkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetJobRun(AwsGlueGetJobRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetJobRuns(AwsGlueGetJobRunsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetJobs(AwsGlueGetJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetJobsOptions(), token);
    }

    public async Task<CommandResult> GetMapping(AwsGlueGetMappingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMlTaskRun(AwsGlueGetMlTaskRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMlTaskRuns(AwsGlueGetMlTaskRunsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMlTransform(AwsGlueGetMlTransformOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMlTransforms(AwsGlueGetMlTransformsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetMlTransformsOptions(), token);
    }

    public async Task<CommandResult> GetPartition(AwsGlueGetPartitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPartitionIndexes(AwsGlueGetPartitionIndexesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPartitions(AwsGlueGetPartitionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPlan(AwsGlueGetPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRegistry(AwsGlueGetRegistryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourcePolicies(AwsGlueGetResourcePoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetResourcePoliciesOptions(), token);
    }

    public async Task<CommandResult> GetResourcePolicy(AwsGlueGetResourcePolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetResourcePolicyOptions(), token);
    }

    public async Task<CommandResult> GetSchema(AwsGlueGetSchemaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSchemaByDefinition(AwsGlueGetSchemaByDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSchemaVersion(AwsGlueGetSchemaVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetSchemaVersionOptions(), token);
    }

    public async Task<CommandResult> GetSchemaVersionsDiff(AwsGlueGetSchemaVersionsDiffOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSecurityConfiguration(AwsGlueGetSecurityConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSecurityConfigurations(AwsGlueGetSecurityConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetSecurityConfigurationsOptions(), token);
    }

    public async Task<CommandResult> GetSession(AwsGlueGetSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetStatement(AwsGlueGetStatementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTable(AwsGlueGetTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTableOptimizer(AwsGlueGetTableOptimizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTableVersion(AwsGlueGetTableVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTableVersions(AwsGlueGetTableVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTables(AwsGlueGetTablesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTags(AwsGlueGetTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTrigger(AwsGlueGetTriggerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTriggers(AwsGlueGetTriggersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueGetTriggersOptions(), token);
    }

    public async Task<CommandResult> GetUnfilteredPartitionMetadata(AwsGlueGetUnfilteredPartitionMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUnfilteredPartitionsMetadata(AwsGlueGetUnfilteredPartitionsMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUnfilteredTableMetadata(AwsGlueGetUnfilteredTableMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUserDefinedFunction(AwsGlueGetUserDefinedFunctionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUserDefinedFunctions(AwsGlueGetUserDefinedFunctionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWorkflow(AwsGlueGetWorkflowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWorkflowRun(AwsGlueGetWorkflowRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWorkflowRunProperties(AwsGlueGetWorkflowRunPropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWorkflowRuns(AwsGlueGetWorkflowRunsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportCatalogToGlue(AwsGlueImportCatalogToGlueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueImportCatalogToGlueOptions(), token);
    }

    public async Task<CommandResult> ListBlueprints(AwsGlueListBlueprintsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListBlueprintsOptions(), token);
    }

    public async Task<CommandResult> ListColumnStatisticsTaskRuns(AwsGlueListColumnStatisticsTaskRunsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListColumnStatisticsTaskRunsOptions(), token);
    }

    public async Task<CommandResult> ListCrawlers(AwsGlueListCrawlersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListCrawlersOptions(), token);
    }

    public async Task<CommandResult> ListCrawls(AwsGlueListCrawlsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCustomEntityTypes(AwsGlueListCustomEntityTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListCustomEntityTypesOptions(), token);
    }

    public async Task<CommandResult> ListDataQualityResults(AwsGlueListDataQualityResultsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListDataQualityResultsOptions(), token);
    }

    public async Task<CommandResult> ListDataQualityRuleRecommendationRuns(AwsGlueListDataQualityRuleRecommendationRunsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListDataQualityRuleRecommendationRunsOptions(), token);
    }

    public async Task<CommandResult> ListDataQualityRulesetEvaluationRuns(AwsGlueListDataQualityRulesetEvaluationRunsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListDataQualityRulesetEvaluationRunsOptions(), token);
    }

    public async Task<CommandResult> ListDataQualityRulesets(AwsGlueListDataQualityRulesetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListDataQualityRulesetsOptions(), token);
    }

    public async Task<CommandResult> ListDevEndpoints(AwsGlueListDevEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListDevEndpointsOptions(), token);
    }

    public async Task<CommandResult> ListJobs(AwsGlueListJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListJobsOptions(), token);
    }

    public async Task<CommandResult> ListMlTransforms(AwsGlueListMlTransformsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListMlTransformsOptions(), token);
    }

    public async Task<CommandResult> ListRegistries(AwsGlueListRegistriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListRegistriesOptions(), token);
    }

    public async Task<CommandResult> ListSchemaVersions(AwsGlueListSchemaVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSchemas(AwsGlueListSchemasOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListSchemasOptions(), token);
    }

    public async Task<CommandResult> ListSessions(AwsGlueListSessionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListSessionsOptions(), token);
    }

    public async Task<CommandResult> ListStatements(AwsGlueListStatementsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTableOptimizerRuns(AwsGlueListTableOptimizerRunsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTriggers(AwsGlueListTriggersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListTriggersOptions(), token);
    }

    public async Task<CommandResult> ListWorkflows(AwsGlueListWorkflowsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueListWorkflowsOptions(), token);
    }

    public async Task<CommandResult> PutDataCatalogEncryptionSettings(AwsGluePutDataCatalogEncryptionSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsGluePutResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutSchemaVersionMetadata(AwsGluePutSchemaVersionMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutWorkflowRunProperties(AwsGluePutWorkflowRunPropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> QuerySchemaVersionMetadata(AwsGlueQuerySchemaVersionMetadataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueQuerySchemaVersionMetadataOptions(), token);
    }

    public async Task<CommandResult> RegisterSchemaVersion(AwsGlueRegisterSchemaVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveSchemaVersionMetadata(AwsGlueRemoveSchemaVersionMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetJobBookmark(AwsGlueResetJobBookmarkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResumeWorkflowRun(AwsGlueResumeWorkflowRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RunStatement(AwsGlueRunStatementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchTables(AwsGlueSearchTablesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueSearchTablesOptions(), token);
    }

    public async Task<CommandResult> StartBlueprintRun(AwsGlueStartBlueprintRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartColumnStatisticsTaskRun(AwsGlueStartColumnStatisticsTaskRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartCrawler(AwsGlueStartCrawlerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartCrawlerSchedule(AwsGlueStartCrawlerScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartDataQualityRuleRecommendationRun(AwsGlueStartDataQualityRuleRecommendationRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartDataQualityRulesetEvaluationRun(AwsGlueStartDataQualityRulesetEvaluationRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartExportLabelsTaskRun(AwsGlueStartExportLabelsTaskRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartImportLabelsTaskRun(AwsGlueStartImportLabelsTaskRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartJobRun(AwsGlueStartJobRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMlEvaluationTaskRun(AwsGlueStartMlEvaluationTaskRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMlLabelingSetGenerationTaskRun(AwsGlueStartMlLabelingSetGenerationTaskRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartTrigger(AwsGlueStartTriggerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartWorkflowRun(AwsGlueStartWorkflowRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopColumnStatisticsTaskRun(AwsGlueStopColumnStatisticsTaskRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopCrawler(AwsGlueStopCrawlerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopCrawlerSchedule(AwsGlueStopCrawlerScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopSession(AwsGlueStopSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopTrigger(AwsGlueStopTriggerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopWorkflowRun(AwsGlueStopWorkflowRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsGlueTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsGlueUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBlueprint(AwsGlueUpdateBlueprintOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateClassifier(AwsGlueUpdateClassifierOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueUpdateClassifierOptions(), token);
    }

    public async Task<CommandResult> UpdateColumnStatisticsForPartition(AwsGlueUpdateColumnStatisticsForPartitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateColumnStatisticsForTable(AwsGlueUpdateColumnStatisticsForTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConnection(AwsGlueUpdateConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCrawler(AwsGlueUpdateCrawlerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCrawlerSchedule(AwsGlueUpdateCrawlerScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDataQualityRuleset(AwsGlueUpdateDataQualityRulesetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDatabase(AwsGlueUpdateDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDevEndpoint(AwsGlueUpdateDevEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateJob(AwsGlueUpdateJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateJobFromSourceControl(AwsGlueUpdateJobFromSourceControlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueUpdateJobFromSourceControlOptions(), token);
    }

    public async Task<CommandResult> UpdateMlTransform(AwsGlueUpdateMlTransformOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePartition(AwsGlueUpdatePartitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRegistry(AwsGlueUpdateRegistryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSchema(AwsGlueUpdateSchemaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSourceControlFromJob(AwsGlueUpdateSourceControlFromJobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGlueUpdateSourceControlFromJobOptions(), token);
    }

    public async Task<CommandResult> UpdateTable(AwsGlueUpdateTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTableOptimizer(AwsGlueUpdateTableOptimizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTrigger(AwsGlueUpdateTriggerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateUserDefinedFunction(AwsGlueUpdateUserDefinedFunctionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateWorkflow(AwsGlueUpdateWorkflowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}