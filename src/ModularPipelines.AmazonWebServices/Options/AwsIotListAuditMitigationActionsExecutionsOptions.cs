using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "list-audit-mitigation-actions-executions")]
public record AwsIotListAuditMitigationActionsExecutionsOptions(
[property: CommandSwitch("--task-id")] string TaskId,
[property: CommandSwitch("--finding-id")] string FindingId
) : AwsOptions
{
    [CommandSwitch("--action-status")]
    public string? ActionStatus { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}