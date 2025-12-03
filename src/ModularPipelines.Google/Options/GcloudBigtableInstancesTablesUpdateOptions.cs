using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "tables", "update")]
public record GcloudBigtableInstancesTablesUpdateOptions(
[property: CliArgument] string Table,
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--deletion-protection")]
    public bool? DeletionProtection { get; set; }

    [CliOption("--change-stream-retention-period")]
    public string? ChangeStreamRetentionPeriod { get; set; }

    [CliFlag("--clear-change-stream-retention-period")]
    public bool? ClearChangeStreamRetentionPeriod { get; set; }
}