using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-routing-profile-agent-availability-timer")]
public record AwsConnectUpdateRoutingProfileAgentAvailabilityTimerOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--routing-profile-id")] string RoutingProfileId,
[property: CliOption("--agent-availability-timer")] string AgentAvailabilityTimer
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}