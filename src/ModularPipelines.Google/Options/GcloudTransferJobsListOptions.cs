using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "jobs", "list")]
public record GcloudTransferJobsListOptions : GcloudOptions
{
    [CommandSwitch("--limit")]
    public string? Limit { get; set; }

    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--job-names")]
    public string[]? JobNames { get; set; }

    [CommandSwitch("--job-statuses")]
    public string[]? JobStatuses { get; set; }

    [BooleanCommandSwitch("--expand-table")]
    public bool? ExpandTable { get; set; }
}