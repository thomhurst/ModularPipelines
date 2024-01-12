using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "sinks", "update")]
public record GcloudLoggingSinksUpdateOptions(
[property: PositionalArgument] string SinkName,
[property: PositionalArgument] string Destination
) : GcloudOptions
{
    [CommandSwitch("--add-exclusion")]
    public string[]? AddExclusion { get; set; }

    [BooleanCommandSwitch("--clear-exclusions")]
    public bool? ClearExclusions { get; set; }

    [CommandSwitch("--custom-writer-identity")]
    public string? CustomWriterIdentity { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--log-filter")]
    public string? LogFilter { get; set; }

    [CommandSwitch("--remove-exclusions")]
    public string[]? RemoveExclusions { get; set; }

    [CommandSwitch("--update-exclusion")]
    public string[]? UpdateExclusion { get; set; }

    [BooleanCommandSwitch("--use-partitioned-tables")]
    public bool? UsePartitionedTables { get; set; }

    [CommandSwitch("--billing-account")]
    public string? BillingAccount { get; set; }

    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--project")]
    public new string? Project { get; set; }
}