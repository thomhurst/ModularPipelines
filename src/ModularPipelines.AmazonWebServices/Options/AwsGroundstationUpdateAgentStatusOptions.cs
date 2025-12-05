using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("groundstation", "update-agent-status")]
public record AwsGroundstationUpdateAgentStatusOptions(
[property: CliOption("--agent-id")] string AgentId,
[property: CliOption("--aggregate-status")] string AggregateStatus,
[property: CliOption("--component-statuses")] string[] ComponentStatuses,
[property: CliOption("--task-id")] string TaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}