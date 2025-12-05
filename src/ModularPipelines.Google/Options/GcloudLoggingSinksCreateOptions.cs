using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "sinks", "create")]
public record GcloudLoggingSinksCreateOptions(
[property: CliArgument] string SinkName,
[property: CliArgument] string Destination
) : GcloudOptions
{
    [CliOption("--custom-writer-identity")]
    public string? CustomWriterIdentity { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--exclusion")]
    public string[]? Exclusion { get; set; }

    [CliFlag("--include-children")]
    public bool? IncludeChildren { get; set; }

    [CliOption("--log-filter")]
    public string? LogFilter { get; set; }

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