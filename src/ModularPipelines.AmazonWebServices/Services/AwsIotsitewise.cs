using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsIotsitewise
{
    public AwsIotsitewise(
        AwsIotsitewiseWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsIotsitewiseWait Wait { get; }

    public async Task<CommandResult> AssociateAssets(AwsIotsitewiseAssociateAssetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateTimeSeriesToAssetProperty(AwsIotsitewiseAssociateTimeSeriesToAssetPropertyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchAssociateProjectAssets(AwsIotsitewiseBatchAssociateProjectAssetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDisassociateProjectAssets(AwsIotsitewiseBatchDisassociateProjectAssetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetAssetPropertyAggregates(AwsIotsitewiseBatchGetAssetPropertyAggregatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetAssetPropertyValue(AwsIotsitewiseBatchGetAssetPropertyValueOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetAssetPropertyValueHistory(AwsIotsitewiseBatchGetAssetPropertyValueHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchPutAssetPropertyValue(AwsIotsitewiseBatchPutAssetPropertyValueOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAccessPolicy(AwsIotsitewiseCreateAccessPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAsset(AwsIotsitewiseCreateAssetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAssetModel(AwsIotsitewiseCreateAssetModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAssetModelCompositeModel(AwsIotsitewiseCreateAssetModelCompositeModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBulkImportJob(AwsIotsitewiseCreateBulkImportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDashboard(AwsIotsitewiseCreateDashboardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGateway(AwsIotsitewiseCreateGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePortal(AwsIotsitewiseCreatePortalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProject(AwsIotsitewiseCreateProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAccessPolicy(AwsIotsitewiseDeleteAccessPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAsset(AwsIotsitewiseDeleteAssetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAssetModel(AwsIotsitewiseDeleteAssetModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAssetModelCompositeModel(AwsIotsitewiseDeleteAssetModelCompositeModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDashboard(AwsIotsitewiseDeleteDashboardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGateway(AwsIotsitewiseDeleteGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePortal(AwsIotsitewiseDeletePortalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProject(AwsIotsitewiseDeleteProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTimeSeries(AwsIotsitewiseDeleteTimeSeriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseDeleteTimeSeriesOptions(), token);
    }

    public async Task<CommandResult> DescribeAccessPolicy(AwsIotsitewiseDescribeAccessPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAction(AwsIotsitewiseDescribeActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAsset(AwsIotsitewiseDescribeAssetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAssetCompositeModel(AwsIotsitewiseDescribeAssetCompositeModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAssetModel(AwsIotsitewiseDescribeAssetModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAssetModelCompositeModel(AwsIotsitewiseDescribeAssetModelCompositeModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAssetProperty(AwsIotsitewiseDescribeAssetPropertyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeBulkImportJob(AwsIotsitewiseDescribeBulkImportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDashboard(AwsIotsitewiseDescribeDashboardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDefaultEncryptionConfiguration(AwsIotsitewiseDescribeDefaultEncryptionConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseDescribeDefaultEncryptionConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeGateway(AwsIotsitewiseDescribeGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeGatewayCapabilityConfiguration(AwsIotsitewiseDescribeGatewayCapabilityConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeLoggingOptions(AwsIotsitewiseDescribeLoggingOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseDescribeLoggingOptionsOptions(), token);
    }

    public async Task<CommandResult> DescribePortal(AwsIotsitewiseDescribePortalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeProject(AwsIotsitewiseDescribeProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStorageConfiguration(AwsIotsitewiseDescribeStorageConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseDescribeStorageConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeTimeSeries(AwsIotsitewiseDescribeTimeSeriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseDescribeTimeSeriesOptions(), token);
    }

    public async Task<CommandResult> DisassociateAssets(AwsIotsitewiseDisassociateAssetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateTimeSeriesFromAssetProperty(AwsIotsitewiseDisassociateTimeSeriesFromAssetPropertyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExecuteAction(AwsIotsitewiseExecuteActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExecuteQuery(AwsIotsitewiseExecuteQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAssetPropertyAggregates(AwsIotsitewiseGetAssetPropertyAggregatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAssetPropertyValue(AwsIotsitewiseGetAssetPropertyValueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseGetAssetPropertyValueOptions(), token);
    }

    public async Task<CommandResult> GetAssetPropertyValueHistory(AwsIotsitewiseGetAssetPropertyValueHistoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseGetAssetPropertyValueHistoryOptions(), token);
    }

    public async Task<CommandResult> GetInterpolatedAssetPropertyValues(AwsIotsitewiseGetInterpolatedAssetPropertyValuesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAccessPolicies(AwsIotsitewiseListAccessPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseListAccessPoliciesOptions(), token);
    }

    public async Task<CommandResult> ListActions(AwsIotsitewiseListActionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssetModelCompositeModels(AwsIotsitewiseListAssetModelCompositeModelsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssetModelProperties(AwsIotsitewiseListAssetModelPropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssetModels(AwsIotsitewiseListAssetModelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseListAssetModelsOptions(), token);
    }

    public async Task<CommandResult> ListAssetProperties(AwsIotsitewiseListAssetPropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssetRelationships(AwsIotsitewiseListAssetRelationshipsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssets(AwsIotsitewiseListAssetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseListAssetsOptions(), token);
    }

    public async Task<CommandResult> ListAssociatedAssets(AwsIotsitewiseListAssociatedAssetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListBulkImportJobs(AwsIotsitewiseListBulkImportJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseListBulkImportJobsOptions(), token);
    }

    public async Task<CommandResult> ListCompositionRelationships(AwsIotsitewiseListCompositionRelationshipsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDashboards(AwsIotsitewiseListDashboardsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListGateways(AwsIotsitewiseListGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseListGatewaysOptions(), token);
    }

    public async Task<CommandResult> ListPortals(AwsIotsitewiseListPortalsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseListPortalsOptions(), token);
    }

    public async Task<CommandResult> ListProjectAssets(AwsIotsitewiseListProjectAssetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListProjects(AwsIotsitewiseListProjectsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsIotsitewiseListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTimeSeries(AwsIotsitewiseListTimeSeriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotsitewiseListTimeSeriesOptions(), token);
    }

    public async Task<CommandResult> PutDefaultEncryptionConfiguration(AwsIotsitewisePutDefaultEncryptionConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutLoggingOptions(AwsIotsitewisePutLoggingOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutStorageConfiguration(AwsIotsitewisePutStorageConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsIotsitewiseTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsIotsitewiseUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAccessPolicy(AwsIotsitewiseUpdateAccessPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAsset(AwsIotsitewiseUpdateAssetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAssetModel(AwsIotsitewiseUpdateAssetModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAssetModelCompositeModel(AwsIotsitewiseUpdateAssetModelCompositeModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAssetProperty(AwsIotsitewiseUpdateAssetPropertyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDashboard(AwsIotsitewiseUpdateDashboardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGateway(AwsIotsitewiseUpdateGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGatewayCapabilityConfiguration(AwsIotsitewiseUpdateGatewayCapabilityConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePortal(AwsIotsitewiseUpdatePortalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateProject(AwsIotsitewiseUpdateProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}