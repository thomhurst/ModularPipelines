using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsCe
{
    public AwsCe(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateAnomalyMonitor(AwsCeCreateAnomalyMonitorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAnomalySubscription(AwsCeCreateAnomalySubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCostCategoryDefinition(AwsCeCreateCostCategoryDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAnomalyMonitor(AwsCeDeleteAnomalyMonitorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAnomalySubscription(AwsCeDeleteAnomalySubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCostCategoryDefinition(AwsCeDeleteCostCategoryDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCostCategoryDefinition(AwsCeDescribeCostCategoryDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAnomalies(AwsCeGetAnomaliesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAnomalyMonitors(AwsCeGetAnomalyMonitorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCeGetAnomalyMonitorsOptions(), token);
    }

    public async Task<CommandResult> GetAnomalySubscriptions(AwsCeGetAnomalySubscriptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCeGetAnomalySubscriptionsOptions(), token);
    }

    public async Task<CommandResult> GetCostAndUsage(AwsCeGetCostAndUsageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCostAndUsageWithResources(AwsCeGetCostAndUsageWithResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCostCategories(AwsCeGetCostCategoriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCostForecast(AwsCeGetCostForecastOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDimensionValues(AwsCeGetDimensionValuesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetReservationCoverage(AwsCeGetReservationCoverageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetReservationPurchaseRecommendation(AwsCeGetReservationPurchaseRecommendationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetReservationUtilization(AwsCeGetReservationUtilizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRightsizingRecommendation(AwsCeGetRightsizingRecommendationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSavingsPlanPurchaseRecommendationDetails(AwsCeGetSavingsPlanPurchaseRecommendationDetailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSavingsPlansCoverage(AwsCeGetSavingsPlansCoverageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSavingsPlansPurchaseRecommendation(AwsCeGetSavingsPlansPurchaseRecommendationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSavingsPlansUtilization(AwsCeGetSavingsPlansUtilizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSavingsPlansUtilizationDetails(AwsCeGetSavingsPlansUtilizationDetailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTags(AwsCeGetTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUsageForecast(AwsCeGetUsageForecastOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCostAllocationTags(AwsCeListCostAllocationTagsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCeListCostAllocationTagsOptions(), token);
    }

    public async Task<CommandResult> ListCostCategoryDefinitions(AwsCeListCostCategoryDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCeListCostCategoryDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListSavingsPlansPurchaseRecommendationGeneration(AwsCeListSavingsPlansPurchaseRecommendationGenerationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCeListSavingsPlansPurchaseRecommendationGenerationOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsCeListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ProvideAnomalyFeedback(AwsCeProvideAnomalyFeedbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartSavingsPlansPurchaseRecommendationGeneration(AwsCeStartSavingsPlansPurchaseRecommendationGenerationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCeStartSavingsPlansPurchaseRecommendationGenerationOptions(), token);
    }

    public async Task<CommandResult> TagResource(AwsCeTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsCeUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAnomalyMonitor(AwsCeUpdateAnomalyMonitorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAnomalySubscription(AwsCeUpdateAnomalySubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCostAllocationTagsStatus(AwsCeUpdateCostAllocationTagsStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCostCategoryDefinition(AwsCeUpdateCostCategoryDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}