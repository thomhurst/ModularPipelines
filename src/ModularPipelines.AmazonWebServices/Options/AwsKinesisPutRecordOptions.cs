using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis", "put-record")]
public record AwsKinesisPutRecordOptions(
[property: CommandSwitch("--data")] string Data,
[property: CommandSwitch("--partition-key")] string PartitionKey
) : AwsOptions
{
    [CommandSwitch("--stream-name")]
    public string? StreamName { get; set; }

    [CommandSwitch("--explicit-hash-key")]
    public string? ExplicitHashKey { get; set; }

    [CommandSwitch("--sequence-number-for-ordering")]
    public string? SequenceNumberForOrdering { get; set; }

    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}