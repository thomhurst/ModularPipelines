using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qldb", "stream-journal-to-kinesis")]
public record AwsQldbStreamJournalToKinesisOptions(
[property: CommandSwitch("--ledger-name")] string LedgerName,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--inclusive-start-time")] long InclusiveStartTime,
[property: CommandSwitch("--kinesis-configuration")] string KinesisConfiguration,
[property: CommandSwitch("--stream-name")] string StreamName
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--exclusive-end-time")]
    public long? ExclusiveEndTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}