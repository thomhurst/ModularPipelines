using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "set-instance-health")]
public record AwsAutoscalingSetInstanceHealthOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--health-status")] string HealthStatus
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}