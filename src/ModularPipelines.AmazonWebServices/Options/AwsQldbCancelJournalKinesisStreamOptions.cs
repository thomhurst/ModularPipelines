using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qldb", "cancel-journal-kinesis-stream")]
public record AwsQldbCancelJournalKinesisStreamOptions(
[property: CommandSwitch("--ledger-name")] string LedgerName,
[property: CommandSwitch("--stream-id")] string StreamId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}