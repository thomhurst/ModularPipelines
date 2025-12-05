using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis", "put-record")]
public record AwsKinesisPutRecordOptions(
[property: CliOption("--data")] string Data,
[property: CliOption("--partition-key")] string PartitionKey
) : AwsOptions
{
    [CliOption("--stream-name")]
    public string? StreamName { get; set; }

    [CliOption("--explicit-hash-key")]
    public string? ExplicitHashKey { get; set; }

    [CliOption("--sequence-number-for-ordering")]
    public string? SequenceNumberForOrdering { get; set; }

    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}