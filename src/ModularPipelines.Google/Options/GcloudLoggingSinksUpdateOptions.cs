using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "sinks", "update")]
public record GcloudLoggingSinksUpdateOptions(
[property: CliArgument] string SinkName,
[property: CliArgument] string Destination
) : GcloudOptions
{
    [CliOption("--add-exclusion")]
    public string[]? AddExclusion { get; set; }

    [CliFlag("--clear-exclusions")]
    public bool? ClearExclusions { get; set; }

    [CliOption("--custom-writer-identity")]
    public string? CustomWriterIdentity { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--log-filter")]
    public string? LogFilter { get; set; }

    [CliOption("--remove-exclusions")]
    public string[]? RemoveExclusions { get; set; }

    [CliOption("--update-exclusion")]
    public string[]? UpdateExclusion { get; set; }

    [CliFlag("--use-partitioned-tables")]
    public bool? UsePartitionedTables { get; set; }

    [CliOption("--billing-account")]
    public string? BillingAccount { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}