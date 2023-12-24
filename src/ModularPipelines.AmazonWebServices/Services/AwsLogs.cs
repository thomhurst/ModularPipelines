using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsLogs
{
    public AwsLogs(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateKmsKey(AwsLogsAssociateKmsKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelExportTask(AwsLogsCancelExportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDelivery(AwsLogsCreateDeliveryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateExportTask(AwsLogsCreateExportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLogAnomalyDetector(AwsLogsCreateLogAnomalyDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLogGroup(AwsLogsCreateLogGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLogStream(AwsLogsCreateLogStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAccountPolicy(AwsLogsDeleteAccountPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDataProtectionPolicy(AwsLogsDeleteDataProtectionPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDelivery(AwsLogsDeleteDeliveryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDeliveryDestination(AwsLogsDeleteDeliveryDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDeliveryDestinationPolicy(AwsLogsDeleteDeliveryDestinationPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDeliverySource(AwsLogsDeleteDeliverySourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDestination(AwsLogsDeleteDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLogAnomalyDetector(AwsLogsDeleteLogAnomalyDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLogGroup(AwsLogsDeleteLogGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLogStream(AwsLogsDeleteLogStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMetricFilter(AwsLogsDeleteMetricFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteQueryDefinition(AwsLogsDeleteQueryDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsLogsDeleteResourcePolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsDeleteResourcePolicyOptions(), token);
    }

    public async Task<CommandResult> DeleteRetentionPolicy(AwsLogsDeleteRetentionPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSubscriptionFilter(AwsLogsDeleteSubscriptionFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccountPolicies(AwsLogsDescribeAccountPoliciesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDeliveries(AwsLogsDescribeDeliveriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsDescribeDeliveriesOptions(), token);
    }

    public async Task<CommandResult> DescribeDeliveryDestinations(AwsLogsDescribeDeliveryDestinationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsDescribeDeliveryDestinationsOptions(), token);
    }

    public async Task<CommandResult> DescribeDeliverySources(AwsLogsDescribeDeliverySourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsDescribeDeliverySourcesOptions(), token);
    }

    public async Task<CommandResult> DescribeDestinations(AwsLogsDescribeDestinationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsDescribeDestinationsOptions(), token);
    }

    public async Task<CommandResult> DescribeExportTasks(AwsLogsDescribeExportTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsDescribeExportTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeLogGroups(AwsLogsDescribeLogGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsDescribeLogGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeLogStreams(AwsLogsDescribeLogStreamsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsDescribeLogStreamsOptions(), token);
    }

    public async Task<CommandResult> DescribeMetricFilters(AwsLogsDescribeMetricFiltersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsDescribeMetricFiltersOptions(), token);
    }

    public async Task<CommandResult> DescribeQueries(AwsLogsDescribeQueriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsDescribeQueriesOptions(), token);
    }

    public async Task<CommandResult> DescribeQueryDefinitions(AwsLogsDescribeQueryDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsDescribeQueryDefinitionsOptions(), token);
    }

    public async Task<CommandResult> DescribeResourcePolicies(AwsLogsDescribeResourcePoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsDescribeResourcePoliciesOptions(), token);
    }

    public async Task<CommandResult> DescribeSubscriptionFilters(AwsLogsDescribeSubscriptionFiltersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateKmsKey(AwsLogsDisassociateKmsKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsDisassociateKmsKeyOptions(), token);
    }

    public async Task<CommandResult> FilterLogEvents(AwsLogsFilterLogEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsFilterLogEventsOptions(), token);
    }

    public async Task<CommandResult> GetDataProtectionPolicy(AwsLogsGetDataProtectionPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDelivery(AwsLogsGetDeliveryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDeliveryDestination(AwsLogsGetDeliveryDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDeliveryDestinationPolicy(AwsLogsGetDeliveryDestinationPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDeliverySource(AwsLogsGetDeliverySourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLogAnomalyDetector(AwsLogsGetLogAnomalyDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLogEvents(AwsLogsGetLogEventsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLogGroupFields(AwsLogsGetLogGroupFieldsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsGetLogGroupFieldsOptions(), token);
    }

    public async Task<CommandResult> GetLogRecord(AwsLogsGetLogRecordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetQueryResults(AwsLogsGetQueryResultsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAnomalies(AwsLogsListAnomaliesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsListAnomaliesOptions(), token);
    }

    public async Task<CommandResult> ListLogAnomalyDetectors(AwsLogsListLogAnomalyDetectorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsListLogAnomalyDetectorsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsLogsListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAccountPolicy(AwsLogsPutAccountPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDataProtectionPolicy(AwsLogsPutDataProtectionPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDeliveryDestination(AwsLogsPutDeliveryDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDeliveryDestinationPolicy(AwsLogsPutDeliveryDestinationPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDeliverySource(AwsLogsPutDeliverySourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDestination(AwsLogsPutDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDestinationPolicy(AwsLogsPutDestinationPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutLogEvents(AwsLogsPutLogEventsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutMetricFilter(AwsLogsPutMetricFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutQueryDefinition(AwsLogsPutQueryDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsLogsPutResourcePolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsPutResourcePolicyOptions(), token);
    }

    public async Task<CommandResult> PutRetentionPolicy(AwsLogsPutRetentionPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutSubscriptionFilter(AwsLogsPutSubscriptionFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartQuery(AwsLogsStartQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopQuery(AwsLogsStopQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsLogsTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Tail(AwsLogsTailOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLogsTailOptions(), token);
    }

    public async Task<CommandResult> TestMetricFilter(AwsLogsTestMetricFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsLogsUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAnomaly(AwsLogsUpdateAnomalyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLogAnomalyDetector(AwsLogsUpdateLogAnomalyDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}