using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qldb", "stream-journal-to-kinesis")]
public record AwsQldbStreamJournalToKinesisOptions(
[property: CliOption("--ledger-name")] string LedgerName,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--inclusive-start-time")] long InclusiveStartTime,
[property: CliOption("--kinesis-configuration")] string KinesisConfiguration,
[property: CliOption("--stream-name")] string StreamName
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--exclusive-end-time")]
    public long? ExclusiveEndTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}