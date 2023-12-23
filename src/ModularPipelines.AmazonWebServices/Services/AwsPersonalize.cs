using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsPersonalize
{
    public AwsPersonalize(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateBatchInferenceJob(AwsPersonalizeCreateBatchInferenceJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBatchSegmentJob(AwsPersonalizeCreateBatchSegmentJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCampaign(AwsPersonalizeCreateCampaignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataset(AwsPersonalizeCreateDatasetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDatasetExportJob(AwsPersonalizeCreateDatasetExportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDatasetGroup(AwsPersonalizeCreateDatasetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDatasetImportJob(AwsPersonalizeCreateDatasetImportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEventTracker(AwsPersonalizeCreateEventTrackerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFilter(AwsPersonalizeCreateFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMetricAttribution(AwsPersonalizeCreateMetricAttributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRecommender(AwsPersonalizeCreateRecommenderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSchema(AwsPersonalizeCreateSchemaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSolution(AwsPersonalizeCreateSolutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSolutionVersion(AwsPersonalizeCreateSolutionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCampaign(AwsPersonalizeDeleteCampaignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDataset(AwsPersonalizeDeleteDatasetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDatasetGroup(AwsPersonalizeDeleteDatasetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEventTracker(AwsPersonalizeDeleteEventTrackerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFilter(AwsPersonalizeDeleteFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMetricAttribution(AwsPersonalizeDeleteMetricAttributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRecommender(AwsPersonalizeDeleteRecommenderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSchema(AwsPersonalizeDeleteSchemaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSolution(AwsPersonalizeDeleteSolutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAlgorithm(AwsPersonalizeDescribeAlgorithmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeBatchInferenceJob(AwsPersonalizeDescribeBatchInferenceJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeBatchSegmentJob(AwsPersonalizeDescribeBatchSegmentJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCampaign(AwsPersonalizeDescribeCampaignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDataset(AwsPersonalizeDescribeDatasetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDatasetExportJob(AwsPersonalizeDescribeDatasetExportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDatasetGroup(AwsPersonalizeDescribeDatasetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDatasetImportJob(AwsPersonalizeDescribeDatasetImportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEventTracker(AwsPersonalizeDescribeEventTrackerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFeatureTransformation(AwsPersonalizeDescribeFeatureTransformationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFilter(AwsPersonalizeDescribeFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMetricAttribution(AwsPersonalizeDescribeMetricAttributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRecipe(AwsPersonalizeDescribeRecipeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRecommender(AwsPersonalizeDescribeRecommenderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSchema(AwsPersonalizeDescribeSchemaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSolution(AwsPersonalizeDescribeSolutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSolutionVersion(AwsPersonalizeDescribeSolutionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSolutionMetrics(AwsPersonalizeGetSolutionMetricsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListBatchInferenceJobs(AwsPersonalizeListBatchInferenceJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListBatchInferenceJobsOptions(), token);
    }

    public async Task<CommandResult> ListBatchSegmentJobs(AwsPersonalizeListBatchSegmentJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListBatchSegmentJobsOptions(), token);
    }

    public async Task<CommandResult> ListCampaigns(AwsPersonalizeListCampaignsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListCampaignsOptions(), token);
    }

    public async Task<CommandResult> ListDatasetExportJobs(AwsPersonalizeListDatasetExportJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListDatasetExportJobsOptions(), token);
    }

    public async Task<CommandResult> ListDatasetGroups(AwsPersonalizeListDatasetGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListDatasetGroupsOptions(), token);
    }

    public async Task<CommandResult> ListDatasetImportJobs(AwsPersonalizeListDatasetImportJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListDatasetImportJobsOptions(), token);
    }

    public async Task<CommandResult> ListDatasets(AwsPersonalizeListDatasetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListDatasetsOptions(), token);
    }

    public async Task<CommandResult> ListEventTrackers(AwsPersonalizeListEventTrackersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListEventTrackersOptions(), token);
    }

    public async Task<CommandResult> ListFilters(AwsPersonalizeListFiltersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListFiltersOptions(), token);
    }

    public async Task<CommandResult> ListMetricAttributionMetrics(AwsPersonalizeListMetricAttributionMetricsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListMetricAttributionMetricsOptions(), token);
    }

    public async Task<CommandResult> ListMetricAttributions(AwsPersonalizeListMetricAttributionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListMetricAttributionsOptions(), token);
    }

    public async Task<CommandResult> ListRecipes(AwsPersonalizeListRecipesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListRecipesOptions(), token);
    }

    public async Task<CommandResult> ListRecommenders(AwsPersonalizeListRecommendersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListRecommendersOptions(), token);
    }

    public async Task<CommandResult> ListSchemas(AwsPersonalizeListSchemasOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListSchemasOptions(), token);
    }

    public async Task<CommandResult> ListSolutionVersions(AwsPersonalizeListSolutionVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListSolutionVersionsOptions(), token);
    }

    public async Task<CommandResult> ListSolutions(AwsPersonalizeListSolutionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeListSolutionsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsPersonalizeListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartRecommender(AwsPersonalizeStartRecommenderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopRecommender(AwsPersonalizeStopRecommenderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopSolutionVersionCreation(AwsPersonalizeStopSolutionVersionCreationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsPersonalizeTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsPersonalizeUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCampaign(AwsPersonalizeUpdateCampaignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDataset(AwsPersonalizeUpdateDatasetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMetricAttribution(AwsPersonalizeUpdateMetricAttributionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeUpdateMetricAttributionOptions(), token);
    }

    public async Task<CommandResult> UpdateRecommender(AwsPersonalizeUpdateRecommenderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}