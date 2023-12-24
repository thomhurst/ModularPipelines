using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsKinesis
{
    public AwsKinesis(
        AwsKinesisWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsKinesisWait Wait { get; }

    public async Task<CommandResult> AddTagsToStream(AwsKinesisAddTagsToStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStream(AwsKinesisCreateStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DecreaseStreamRetentionPeriod(AwsKinesisDecreaseStreamRetentionPeriodOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsKinesisDeleteResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStream(AwsKinesisDeleteStreamOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisDeleteStreamOptions(), token);
    }

    public async Task<CommandResult> DeregisterStreamConsumer(AwsKinesisDeregisterStreamConsumerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisDeregisterStreamConsumerOptions(), token);
    }

    public async Task<CommandResult> DescribeLimits(AwsKinesisDescribeLimitsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisDescribeLimitsOptions(), token);
    }

    public async Task<CommandResult> DescribeStream(AwsKinesisDescribeStreamOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisDescribeStreamOptions(), token);
    }

    public async Task<CommandResult> DescribeStreamConsumer(AwsKinesisDescribeStreamConsumerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisDescribeStreamConsumerOptions(), token);
    }

    public async Task<CommandResult> DescribeStreamSummary(AwsKinesisDescribeStreamSummaryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisDescribeStreamSummaryOptions(), token);
    }

    public async Task<CommandResult> DisableEnhancedMonitoring(AwsKinesisDisableEnhancedMonitoringOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableEnhancedMonitoring(AwsKinesisEnableEnhancedMonitoringOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRecords(AwsKinesisGetRecordsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourcePolicy(AwsKinesisGetResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetShardIterator(AwsKinesisGetShardIteratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> IncreaseStreamRetentionPeriod(AwsKinesisIncreaseStreamRetentionPeriodOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListShards(AwsKinesisListShardsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisListShardsOptions(), token);
    }

    public async Task<CommandResult> ListStreamConsumers(AwsKinesisListStreamConsumersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStreams(AwsKinesisListStreamsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisListStreamsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForStream(AwsKinesisListTagsForStreamOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisListTagsForStreamOptions(), token);
    }

    public async Task<CommandResult> MergeShards(AwsKinesisMergeShardsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutRecord(AwsKinesisPutRecordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutRecords(AwsKinesisPutRecordsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsKinesisPutResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterStreamConsumer(AwsKinesisRegisterStreamConsumerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTagsFromStream(AwsKinesisRemoveTagsFromStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SplitShard(AwsKinesisSplitShardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartStreamEncryption(AwsKinesisStartStreamEncryptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopStreamEncryption(AwsKinesisStopStreamEncryptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateShardCount(AwsKinesisUpdateShardCountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateStreamMode(AwsKinesisUpdateStreamModeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}