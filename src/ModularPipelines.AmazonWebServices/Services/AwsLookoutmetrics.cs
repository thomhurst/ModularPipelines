using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsLookoutmetrics
{
    public AwsLookoutmetrics(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> ActivateAnomalyDetector(AwsLookoutmetricsActivateAnomalyDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BackTestAnomalyDetector(AwsLookoutmetricsBackTestAnomalyDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAlert(AwsLookoutmetricsCreateAlertOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAnomalyDetector(AwsLookoutmetricsCreateAnomalyDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMetricSet(AwsLookoutmetricsCreateMetricSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeactivateAnomalyDetector(AwsLookoutmetricsDeactivateAnomalyDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAlert(AwsLookoutmetricsDeleteAlertOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAnomalyDetector(AwsLookoutmetricsDeleteAnomalyDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAlert(AwsLookoutmetricsDescribeAlertOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAnomalyDetectionExecutions(AwsLookoutmetricsDescribeAnomalyDetectionExecutionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAnomalyDetector(AwsLookoutmetricsDescribeAnomalyDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMetricSet(AwsLookoutmetricsDescribeMetricSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectMetricSetConfig(AwsLookoutmetricsDetectMetricSetConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAnomalyGroup(AwsLookoutmetricsGetAnomalyGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDataQualityMetrics(AwsLookoutmetricsGetDataQualityMetricsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFeedback(AwsLookoutmetricsGetFeedbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSampleData(AwsLookoutmetricsGetSampleDataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLookoutmetricsGetSampleDataOptions(), token);
    }

    public async Task<CommandResult> ListAlerts(AwsLookoutmetricsListAlertsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLookoutmetricsListAlertsOptions(), token);
    }

    public async Task<CommandResult> ListAnomalyDetectors(AwsLookoutmetricsListAnomalyDetectorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLookoutmetricsListAnomalyDetectorsOptions(), token);
    }

    public async Task<CommandResult> ListAnomalyGroupRelatedMetrics(AwsLookoutmetricsListAnomalyGroupRelatedMetricsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAnomalyGroupSummaries(AwsLookoutmetricsListAnomalyGroupSummariesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAnomalyGroupTimeSeries(AwsLookoutmetricsListAnomalyGroupTimeSeriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMetricSets(AwsLookoutmetricsListMetricSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLookoutmetricsListMetricSetsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsLookoutmetricsListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutFeedback(AwsLookoutmetricsPutFeedbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsLookoutmetricsTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsLookoutmetricsUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAlert(AwsLookoutmetricsUpdateAlertOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAnomalyDetector(AwsLookoutmetricsUpdateAnomalyDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMetricSet(AwsLookoutmetricsUpdateMetricSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}