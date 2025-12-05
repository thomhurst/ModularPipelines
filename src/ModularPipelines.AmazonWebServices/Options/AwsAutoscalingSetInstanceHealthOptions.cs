using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "set-instance-health")]
public record AwsAutoscalingSetInstanceHealthOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--health-status")] string HealthStatus
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}