using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "jobs", "list")]
public record GcloudTransferJobsListOptions : GcloudOptions
{
    [CliOption("--limit")]
    public string? Limit { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--job-names")]
    public string[]? JobNames { get; set; }

    [CliOption("--job-statuses")]
    public string[]? JobStatuses { get; set; }

    [CliFlag("--expand-table")]
    public bool? ExpandTable { get; set; }
}