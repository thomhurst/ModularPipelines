using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "tables", "create")]
public record GcloudBigtableInstancesTablesCreateOptions(
[property: PositionalArgument] string Table,
[property: PositionalArgument] string Instance,
[property: CommandSwitch("--column-families")] string[] ColumnFamilies
) : GcloudOptions
{
    [CommandSwitch("--change-stream-retention-period")]
    public string? ChangeStreamRetentionPeriod { get; set; }

    [BooleanCommandSwitch("--deletion-protection")]
    public bool? DeletionProtection { get; set; }

    [CommandSwitch("--splits")]
    public string[]? Splits { get; set; }
}