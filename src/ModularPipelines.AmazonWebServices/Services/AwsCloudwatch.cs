using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsCloudwatch
{
    public AwsCloudwatch(
        AwsCloudwatchWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsCloudwatchWait Wait { get; }

    public async Task<CommandResult> DeleteAlarms(AwsCloudwatchDeleteAlarmsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAnomalyDetector(AwsCloudwatchDeleteAnomalyDetectorOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudwatchDeleteAnomalyDetectorOptions(), token);
    }

    public async Task<CommandResult> DeleteDashboards(AwsCloudwatchDeleteDashboardsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInsightRules(AwsCloudwatchDeleteInsightRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMetricStream(AwsCloudwatchDeleteMetricStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAlarmHistory(AwsCloudwatchDescribeAlarmHistoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudwatchDescribeAlarmHistoryOptions(), token);
    }

    public async Task<CommandResult> DescribeAlarms(AwsCloudwatchDescribeAlarmsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudwatchDescribeAlarmsOptions(), token);
    }

    public async Task<CommandResult> DescribeAlarmsForMetric(AwsCloudwatchDescribeAlarmsForMetricOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAnomalyDetectors(AwsCloudwatchDescribeAnomalyDetectorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudwatchDescribeAnomalyDetectorsOptions(), token);
    }

    public async Task<CommandResult> DescribeInsightRules(AwsCloudwatchDescribeInsightRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudwatchDescribeInsightRulesOptions(), token);
    }

    public async Task<CommandResult> DisableAlarmActions(AwsCloudwatchDisableAlarmActionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableInsightRules(AwsCloudwatchDisableInsightRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableAlarmActions(AwsCloudwatchEnableAlarmActionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableInsightRules(AwsCloudwatchEnableInsightRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDashboard(AwsCloudwatchGetDashboardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInsightRuleReport(AwsCloudwatchGetInsightRuleReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMetricData(AwsCloudwatchGetMetricDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMetricStatistics(AwsCloudwatchGetMetricStatisticsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMetricStream(AwsCloudwatchGetMetricStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMetricWidgetImage(AwsCloudwatchGetMetricWidgetImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDashboards(AwsCloudwatchListDashboardsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudwatchListDashboardsOptions(), token);
    }

    public async Task<CommandResult> ListManagedInsightRules(AwsCloudwatchListManagedInsightRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMetricStreams(AwsCloudwatchListMetricStreamsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudwatchListMetricStreamsOptions(), token);
    }

    public async Task<CommandResult> ListMetrics(AwsCloudwatchListMetricsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudwatchListMetricsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsCloudwatchListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAnomalyDetector(AwsCloudwatchPutAnomalyDetectorOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudwatchPutAnomalyDetectorOptions(), token);
    }

    public async Task<CommandResult> PutCompositeAlarm(AwsCloudwatchPutCompositeAlarmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDashboard(AwsCloudwatchPutDashboardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutInsightRule(AwsCloudwatchPutInsightRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutManagedInsightRules(AwsCloudwatchPutManagedInsightRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutMetricAlarm(AwsCloudwatchPutMetricAlarmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutMetricData(AwsCloudwatchPutMetricDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutMetricStream(AwsCloudwatchPutMetricStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetAlarmState(AwsCloudwatchSetAlarmStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMetricStreams(AwsCloudwatchStartMetricStreamsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopMetricStreams(AwsCloudwatchStopMetricStreamsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsCloudwatchTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsCloudwatchUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}