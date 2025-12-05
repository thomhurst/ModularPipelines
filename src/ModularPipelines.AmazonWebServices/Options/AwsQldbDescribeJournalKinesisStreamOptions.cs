using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qldb", "describe-journal-kinesis-stream")]
public record AwsQldbDescribeJournalKinesisStreamOptions(
[property: CliOption("--ledger-name")] string LedgerName,
[property: CliOption("--stream-id")] string StreamId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}