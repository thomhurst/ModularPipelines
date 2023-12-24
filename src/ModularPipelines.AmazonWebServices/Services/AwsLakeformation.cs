using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> AddLfTagsToResource(AwsLakeformationAddLfTagsToResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssumeDecoratedRoleWithSaml(AwsLakeformationAssumeDecoratedRoleWithSamlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGrantPermissions(AwsLakeformationBatchGrantPermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchRevokePermissions(AwsLakeformationBatchRevokePermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelTransaction(AwsLakeformationCancelTransactionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CommitTransaction(AwsLakeformationCommitTransactionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataCellsFilter(AwsLakeformationCreateDataCellsFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLakeFormationIdentityCenterConfiguration(AwsLakeformationCreateLakeFormationIdentityCenterConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationCreateLakeFormationIdentityCenterConfigurationOptions(), token);
    }

    public async Task<CommandResult> CreateLakeFormationOptIn(AwsLakeformationCreateLakeFormationOptInOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLfTag(AwsLakeformationCreateLfTagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDataCellsFilter(AwsLakeformationDeleteDataCellsFilterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationDeleteDataCellsFilterOptions(), token);
    }

    public async Task<CommandResult> DeleteLakeFormationIdentityCenterConfiguration(AwsLakeformationDeleteLakeFormationIdentityCenterConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationDeleteLakeFormationIdentityCenterConfigurationOptions(), token);
    }

    public async Task<CommandResult> DeleteLakeFormationOptIn(AwsLakeformationDeleteLakeFormationOptInOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLfTag(AwsLakeformationDeleteLfTagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteObjectsOnCancel(AwsLakeformationDeleteObjectsOnCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterResource(AwsLakeformationDeregisterResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeLakeFormationIdentityCenterConfiguration(AwsLakeformationDescribeLakeFormationIdentityCenterConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationDescribeLakeFormationIdentityCenterConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeResource(AwsLakeformationDescribeResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTransaction(AwsLakeformationDescribeTransactionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExtendTransaction(AwsLakeformationExtendTransactionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationExtendTransactionOptions(), token);
    }

    public async Task<CommandResult> GetDataCellsFilter(AwsLakeformationGetDataCellsFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDataLakeSettings(AwsLakeformationGetDataLakeSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationGetDataLakeSettingsOptions(), token);
    }

    public async Task<CommandResult> GetEffectivePermissionsForPath(AwsLakeformationGetEffectivePermissionsForPathOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLfTag(AwsLakeformationGetLfTagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetQueryState(AwsLakeformationGetQueryStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetQueryStatistics(AwsLakeformationGetQueryStatisticsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourceLfTags(AwsLakeformationGetResourceLfTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTableObjects(AwsLakeformationGetTableObjectsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTemporaryGluePartitionCredentials(AwsLakeformationGetTemporaryGluePartitionCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTemporaryGlueTableCredentials(AwsLakeformationGetTemporaryGlueTableCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWorkUnitResults(AwsLakeformationGetWorkUnitResultsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWorkUnits(AwsLakeformationGetWorkUnitsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GrantPermissions(AwsLakeformationGrantPermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDataCellsFilter(AwsLakeformationListDataCellsFilterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationListDataCellsFilterOptions(), token);
    }

    public async Task<CommandResult> ListLakeFormationOptIns(AwsLakeformationListLakeFormationOptInsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationListLakeFormationOptInsOptions(), token);
    }

    public async Task<CommandResult> ListLfTags(AwsLakeformationListLfTagsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationListLfTagsOptions(), token);
    }

    public async Task<CommandResult> ListPermissions(AwsLakeformationListPermissionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationListPermissionsOptions(), token);
    }

    public async Task<CommandResult> ListResources(AwsLakeformationListResourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationListResourcesOptions(), token);
    }

    public async Task<CommandResult> ListTableStorageOptimizers(AwsLakeformationListTableStorageOptimizersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTransactions(AwsLakeformationListTransactionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationListTransactionsOptions(), token);
    }

    public async Task<CommandResult> PutDataLakeSettings(AwsLakeformationPutDataLakeSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterResource(AwsLakeformationRegisterResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveLfTagsFromResource(AwsLakeformationRemoveLfTagsFromResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RevokePermissions(AwsLakeformationRevokePermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchDatabasesByLfTags(AwsLakeformationSearchDatabasesByLfTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchTablesByLfTags(AwsLakeformationSearchTablesByLfTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartQueryPlanning(AwsLakeformationStartQueryPlanningOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartTransaction(AwsLakeformationStartTransactionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationStartTransactionOptions(), token);
    }

    public async Task<CommandResult> UpdateDataCellsFilter(AwsLakeformationUpdateDataCellsFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLakeFormationIdentityCenterConfiguration(AwsLakeformationUpdateLakeFormationIdentityCenterConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLakeformationUpdateLakeFormationIdentityCenterConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdateLfTag(AwsLakeformationUpdateLfTagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateResource(AwsLakeformationUpdateResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTableObjects(AwsLakeformationUpdateTableObjectsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTableStorageOptimizer(AwsLakeformationUpdateTableStorageOptimizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}