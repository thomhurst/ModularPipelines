using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> AddNotificationChannel(AwsDevopsGuruAddNotificationChannelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteInsight(AwsDevopsGuruDeleteInsightOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAccountHealth(AwsDevopsGuruDescribeAccountHealthOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruDescribeAccountHealthOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAccountOverview(AwsDevopsGuruDescribeAccountOverviewOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAnomaly(AwsDevopsGuruDescribeAnomalyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEventSourcesConfig(AwsDevopsGuruDescribeEventSourcesConfigOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruDescribeEventSourcesConfigOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeFeedback(AwsDevopsGuruDescribeFeedbackOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruDescribeFeedbackOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInsight(AwsDevopsGuruDescribeInsightOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeOrganizationHealth(AwsDevopsGuruDescribeOrganizationHealthOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruDescribeOrganizationHealthOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeOrganizationOverview(AwsDevopsGuruDescribeOrganizationOverviewOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeOrganizationResourceCollectionHealth(AwsDevopsGuruDescribeOrganizationResourceCollectionHealthOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeResourceCollectionHealth(AwsDevopsGuruDescribeResourceCollectionHealthOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeServiceIntegration(AwsDevopsGuruDescribeServiceIntegrationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruDescribeServiceIntegrationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetCostEstimation(AwsDevopsGuruGetCostEstimationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruGetCostEstimationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetResourceCollection(AwsDevopsGuruGetResourceCollectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListAnomaliesForInsight(AwsDevopsGuruListAnomaliesForInsightOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListAnomalousLogGroups(AwsDevopsGuruListAnomalousLogGroupsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListEvents(AwsDevopsGuruListEventsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListInsights(AwsDevopsGuruListInsightsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMonitoredResources(AwsDevopsGuruListMonitoredResourcesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruListMonitoredResourcesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListNotificationChannels(AwsDevopsGuruListNotificationChannelsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruListNotificationChannelsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListOrganizationInsights(AwsDevopsGuruListOrganizationInsightsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListRecommendations(AwsDevopsGuruListRecommendationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutFeedback(AwsDevopsGuruPutFeedbackOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruPutFeedbackOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RemoveNotificationChannel(AwsDevopsGuruRemoveNotificationChannelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchInsights(AwsDevopsGuruSearchInsightsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchOrganizationInsights(AwsDevopsGuruSearchOrganizationInsightsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartCostEstimation(AwsDevopsGuruStartCostEstimationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateEventSourcesConfig(AwsDevopsGuruUpdateEventSourcesConfigOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevopsGuruUpdateEventSourcesConfigOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateResourceCollection(AwsDevopsGuruUpdateResourceCollectionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateServiceIntegration(AwsDevopsGuruUpdateServiceIntegrationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}