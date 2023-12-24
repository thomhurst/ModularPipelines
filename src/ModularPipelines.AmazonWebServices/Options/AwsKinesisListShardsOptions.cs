using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis", "list-shards")]
public record AwsKinesisListShardsOptions : AwsOptions
{
    [CommandSwitch("--stream-name")]
    public string? StreamName { get; set; }

    [CommandSwitch("--exclusive-start-shard-id")]
    public string? ExclusiveStartShardId { get; set; }

    [CommandSwitch("--stream-creation-timestamp")]
    public long? StreamCreationTimestamp { get; set; }

    [CommandSwitch("--shard-filter")]
    public string? ShardFilter { get; set; }

    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}