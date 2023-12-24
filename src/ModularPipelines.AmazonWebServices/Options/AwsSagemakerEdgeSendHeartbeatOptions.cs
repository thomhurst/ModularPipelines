using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-edge", "send-heartbeat")]
public record AwsSagemakerEdgeSendHeartbeatOptions(
[property: CommandSwitch("--agent-version")] string AgentVersion,
[property: CommandSwitch("--device-name")] string DeviceName,
[property: CommandSwitch("--device-fleet-name")] string DeviceFleetName
) : AwsOptions
{
    [CommandSwitch("--agent-metrics")]
    public string[]? AgentMetrics { get; set; }

    [CommandSwitch("--models")]
    public string[]? Models { get; set; }

    [CommandSwitch("--deployment-result")]
    public string? DeploymentResult { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}