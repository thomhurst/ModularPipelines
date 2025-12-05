using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "detach-traffic-sources")]
public record AwsAutoscalingDetachTrafficSourcesOptions(
[property: CliOption("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CliOption("--traffic-sources")] string[] TrafficSources
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}