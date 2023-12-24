using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsXray
{
    public AwsXray(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchGetTraces(AwsXrayBatchGetTracesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGroup(AwsXrayCreateGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSamplingRule(AwsXrayCreateSamplingRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGroup(AwsXrayDeleteGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsXrayDeleteGroupOptions(), token);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsXrayDeleteResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSamplingRule(AwsXrayDeleteSamplingRuleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsXrayDeleteSamplingRuleOptions(), token);
    }

    public async Task<CommandResult> GetEncryptionConfig(AwsXrayGetEncryptionConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsXrayGetEncryptionConfigOptions(), token);
    }

    public async Task<CommandResult> GetGroup(AwsXrayGetGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsXrayGetGroupOptions(), token);
    }

    public async Task<CommandResult> GetGroups(AwsXrayGetGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsXrayGetGroupsOptions(), token);
    }

    public async Task<CommandResult> GetInsight(AwsXrayGetInsightOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInsightEvents(AwsXrayGetInsightEventsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInsightImpactGraph(AwsXrayGetInsightImpactGraphOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInsightSummaries(AwsXrayGetInsightSummariesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSamplingRules(AwsXrayGetSamplingRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsXrayGetSamplingRulesOptions(), token);
    }

    public async Task<CommandResult> GetSamplingStatisticSummaries(AwsXrayGetSamplingStatisticSummariesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsXrayGetSamplingStatisticSummariesOptions(), token);
    }

    public async Task<CommandResult> GetSamplingTargets(AwsXrayGetSamplingTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServiceGraph(AwsXrayGetServiceGraphOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTimeSeriesServiceStatistics(AwsXrayGetTimeSeriesServiceStatisticsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTraceGraph(AwsXrayGetTraceGraphOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTraceSummaries(AwsXrayGetTraceSummariesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListResourcePolicies(AwsXrayListResourcePoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsXrayListResourcePoliciesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsXrayListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutEncryptionConfig(AwsXrayPutEncryptionConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsXrayPutResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutTelemetryRecords(AwsXrayPutTelemetryRecordsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutTraceSegments(AwsXrayPutTraceSegmentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsXrayTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsXrayUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGroup(AwsXrayUpdateGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsXrayUpdateGroupOptions(), token);
    }

    public async Task<CommandResult> UpdateSamplingRule(AwsXrayUpdateSamplingRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}