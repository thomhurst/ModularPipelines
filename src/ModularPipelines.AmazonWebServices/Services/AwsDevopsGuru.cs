using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsDevopsGuru
{
    public AwsDevopsGuru(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddNotificationChannel(AwsDevopsGuruAddNotificationChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInsight(AwsDevopsGuruDeleteInsightOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccountHealth(AwsDevopsGuruDescribeAccountHealthOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruDescribeAccountHealthOptions(), token);
    }

    public async Task<CommandResult> DescribeAccountOverview(AwsDevopsGuruDescribeAccountOverviewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAnomaly(AwsDevopsGuruDescribeAnomalyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEventSourcesConfig(AwsDevopsGuruDescribeEventSourcesConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruDescribeEventSourcesConfigOptions(), token);
    }

    public async Task<CommandResult> DescribeFeedback(AwsDevopsGuruDescribeFeedbackOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruDescribeFeedbackOptions(), token);
    }

    public async Task<CommandResult> DescribeInsight(AwsDevopsGuruDescribeInsightOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeOrganizationHealth(AwsDevopsGuruDescribeOrganizationHealthOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruDescribeOrganizationHealthOptions(), token);
    }

    public async Task<CommandResult> DescribeOrganizationOverview(AwsDevopsGuruDescribeOrganizationOverviewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeOrganizationResourceCollectionHealth(AwsDevopsGuruDescribeOrganizationResourceCollectionHealthOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeResourceCollectionHealth(AwsDevopsGuruDescribeResourceCollectionHealthOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeServiceIntegration(AwsDevopsGuruDescribeServiceIntegrationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruDescribeServiceIntegrationOptions(), token);
    }

    public async Task<CommandResult> GetCostEstimation(AwsDevopsGuruGetCostEstimationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruGetCostEstimationOptions(), token);
    }

    public async Task<CommandResult> GetResourceCollection(AwsDevopsGuruGetResourceCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAnomaliesForInsight(AwsDevopsGuruListAnomaliesForInsightOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAnomalousLogGroups(AwsDevopsGuruListAnomalousLogGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEvents(AwsDevopsGuruListEventsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListInsights(AwsDevopsGuruListInsightsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMonitoredResources(AwsDevopsGuruListMonitoredResourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruListMonitoredResourcesOptions(), token);
    }

    public async Task<CommandResult> ListNotificationChannels(AwsDevopsGuruListNotificationChannelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruListNotificationChannelsOptions(), token);
    }

    public async Task<CommandResult> ListOrganizationInsights(AwsDevopsGuruListOrganizationInsightsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRecommendations(AwsDevopsGuruListRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutFeedback(AwsDevopsGuruPutFeedbackOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruPutFeedbackOptions(), token);
    }

    public async Task<CommandResult> RemoveNotificationChannel(AwsDevopsGuruRemoveNotificationChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchInsights(AwsDevopsGuruSearchInsightsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchOrganizationInsights(AwsDevopsGuruSearchOrganizationInsightsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartCostEstimation(AwsDevopsGuruStartCostEstimationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEventSourcesConfig(AwsDevopsGuruUpdateEventSourcesConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruUpdateEventSourcesConfigOptions(), token);
    }

    public async Task<CommandResult> UpdateResourceCollection(AwsDevopsGuruUpdateResourceCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateServiceIntegration(AwsDevopsGuruUpdateServiceIntegrationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}