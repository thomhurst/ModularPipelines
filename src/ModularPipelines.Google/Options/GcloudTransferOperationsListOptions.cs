using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "operations", "list")]
public record GcloudTransferOperationsListOptions : GcloudOptions
{
    [CliOption("--limit")]
    public string? Limit { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--job-names")]
    public string[]? JobNames { get; set; }

    [CliOption("--operation-names")]
    public string[]? OperationNames { get; set; }

    [CliOption("--operation-statuses")]
    public string[]? OperationStatuses { get; set; }

    [CliFlag("--expand-table")]
    public bool? ExpandTable { get; set; }
}