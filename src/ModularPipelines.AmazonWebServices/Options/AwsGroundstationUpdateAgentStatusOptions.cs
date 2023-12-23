using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("groundstation", "update-agent-status")]
public record AwsGroundstationUpdateAgentStatusOptions(
[property: CommandSwitch("--agent-id")] string AgentId,
[property: CommandSwitch("--aggregate-status")] string AggregateStatus,
[property: CommandSwitch("--component-statuses")] string[] ComponentStatuses,
[property: CommandSwitch("--task-id")] string TaskId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}