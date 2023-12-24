using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "list-audit-mitigation-actions-tasks")]
public record AwsIotListAuditMitigationActionsTasksOptions(
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime
) : AwsOptions
{
    [CommandSwitch("--audit-task-id")]
    public string? AuditTaskId { get; set; }

    [CommandSwitch("--finding-id")]
    public string? FindingId { get; set; }

    [CommandSwitch("--task-status")]
    public string? TaskStatus { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}