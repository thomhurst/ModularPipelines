using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "detach-traffic-sources")]
public record AwsAutoscalingDetachTrafficSourcesOptions(
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CommandSwitch("--traffic-sources")] string[] TrafficSources
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}