using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "operations", "list")]
public record GcloudTransferOperationsListOptions : GcloudOptions
{
    [CommandSwitch("--limit")]
    public string? Limit { get; set; }

    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--job-names")]
    public string[]? JobNames { get; set; }

    [CommandSwitch("--operation-names")]
    public string[]? OperationNames { get; set; }

    [CommandSwitch("--operation-statuses")]
    public string[]? OperationStatuses { get; set; }

    [BooleanCommandSwitch("--expand-table")]
    public bool? ExpandTable { get; set; }
}