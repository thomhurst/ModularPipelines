using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "tables", "create")]
public record GcloudBigtableInstancesTablesCreateOptions(
[property: CliArgument] string Table,
[property: CliArgument] string Instance,
[property: CliOption("--column-families")] string[] ColumnFamilies
) : GcloudOptions
{
    [CliOption("--change-stream-retention-period")]
    public string? ChangeStreamRetentionPeriod { get; set; }

    [CliFlag("--deletion-protection")]
    public bool? DeletionProtection { get; set; }

    [CliOption("--splits")]
    public string[]? Splits { get; set; }
}