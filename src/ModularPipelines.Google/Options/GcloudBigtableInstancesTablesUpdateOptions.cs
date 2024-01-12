using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "tables", "update")]
public record GcloudBigtableInstancesTablesUpdateOptions(
[property: PositionalArgument] string Table,
[property: PositionalArgument] string Instance
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--deletion-protection")]
    public bool? DeletionProtection { get; set; }

    [CommandSwitch("--change-stream-retention-period")]
    public string? ChangeStreamRetentionPeriod { get; set; }

    [BooleanCommandSwitch("--clear-change-stream-retention-period")]
    public bool? ClearChangeStreamRetentionPeriod { get; set; }
}