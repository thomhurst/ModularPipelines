using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsLakeformation
{
    public AwsLakeformation(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddLfTagsToResource(AwsLakeformationAddLfTagsToResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssumeDecoratedRoleWithSaml(AwsLakeformationAssumeDecoratedRoleWithSamlOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchGrantPermissions(AwsLakeformationBatchGrantPermissionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchRevokePermissions(AwsLakeformationBatchRevokePermissionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelTransaction(AwsLakeformationCancelTransactionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CommitTransaction(AwsLakeformationCommitTransactionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDataCellsFilter(AwsLakeformationCreateDataCellsFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateLakeFormationIdentityCenterConfiguration(AwsLakeformationCreateLakeFormationIdentityCenterConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationCreateLakeFormationIdentityCenterConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateLakeFormationOptIn(AwsLakeformationCreateLakeFormationOptInOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateLfTag(AwsLakeformationCreateLfTagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDataCellsFilter(AwsLakeformationDeleteDataCellsFilterOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationDeleteDataCellsFilterOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteLakeFormationIdentityCenterConfiguration(AwsLakeformationDeleteLakeFormationIdentityCenterConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationDeleteLakeFormationIdentityCenterConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteLakeFormationOptIn(AwsLakeformationDeleteLakeFormationOptInOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteLfTag(AwsLakeformationDeleteLfTagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteObjectsOnCancel(AwsLakeformationDeleteObjectsOnCancelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeregisterResource(AwsLakeformationDeregisterResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLakeFormationIdentityCenterConfiguration(AwsLakeformationDescribeLakeFormationIdentityCenterConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationDescribeLakeFormationIdentityCenterConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeResource(AwsLakeformationDescribeResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTransaction(AwsLakeformationDescribeTransactionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExtendTransaction(AwsLakeformationExtendTransactionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationExtendTransactionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDataCellsFilter(AwsLakeformationGetDataCellsFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDataLakeSettings(AwsLakeformationGetDataLakeSettingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationGetDataLakeSettingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEffectivePermissionsForPath(AwsLakeformationGetEffectivePermissionsForPathOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetLfTag(AwsLakeformationGetLfTagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetQueryState(AwsLakeformationGetQueryStateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetQueryStatistics(AwsLakeformationGetQueryStatisticsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetResourceLfTags(AwsLakeformationGetResourceLfTagsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTableObjects(AwsLakeformationGetTableObjectsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTemporaryGluePartitionCredentials(AwsLakeformationGetTemporaryGluePartitionCredentialsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTemporaryGlueTableCredentials(AwsLakeformationGetTemporaryGlueTableCredentialsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetWorkUnitResults(AwsLakeformationGetWorkUnitResultsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetWorkUnits(AwsLakeformationGetWorkUnitsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GrantPermissions(AwsLakeformationGrantPermissionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListDataCellsFilter(AwsLakeformationListDataCellsFilterOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationListDataCellsFilterOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListLakeFormationOptIns(AwsLakeformationListLakeFormationOptInsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationListLakeFormationOptInsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListLfTags(AwsLakeformationListLfTagsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationListLfTagsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListPermissions(AwsLakeformationListPermissionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationListPermissionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListResources(AwsLakeformationListResourcesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationListResourcesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTableStorageOptimizers(AwsLakeformationListTableStorageOptimizersOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTransactions(AwsLakeformationListTransactionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationListTransactionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutDataLakeSettings(AwsLakeformationPutDataLakeSettingsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RegisterResource(AwsLakeformationRegisterResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RemoveLfTagsFromResource(AwsLakeformationRemoveLfTagsFromResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RevokePermissions(AwsLakeformationRevokePermissionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchDatabasesByLfTags(AwsLakeformationSearchDatabasesByLfTagsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchTablesByLfTags(AwsLakeformationSearchTablesByLfTagsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartQueryPlanning(AwsLakeformationStartQueryPlanningOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartTransaction(AwsLakeformationStartTransactionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationStartTransactionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateDataCellsFilter(AwsLakeformationUpdateDataCellsFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateLakeFormationIdentityCenterConfiguration(AwsLakeformationUpdateLakeFormationIdentityCenterConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationUpdateLakeFormationIdentityCenterConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateLfTag(AwsLakeformationUpdateLfTagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateResource(AwsLakeformationUpdateResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateTableObjects(AwsLakeformationUpdateTableObjectsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateTableStorageOptimizer(AwsLakeformationUpdateTableStorageOptimizerOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}