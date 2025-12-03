using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-edge", "send-heartbeat")]
public record AwsSagemakerEdgeSendHeartbeatOptions(
[property: CliOption("--agent-version")] string AgentVersion,
[property: CliOption("--device-name")] string DeviceName,
[property: CliOption("--device-fleet-name")] string DeviceFleetName
) : AwsOptions
{
    [CliOption("--agent-metrics")]
    public string[]? AgentMetrics { get; set; }

    [CliOption("--models")]
    public string[]? Models { get; set; }

    [CliOption("--deployment-result")]
    public string? DeploymentResult { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}