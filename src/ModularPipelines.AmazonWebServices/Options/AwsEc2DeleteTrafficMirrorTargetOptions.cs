using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-traffic-mirror-target")]
public record AwsEc2DeleteTrafficMirrorTargetOptions(
[property: CliOption("--traffic-mirror-target-id")] string TrafficMirrorTargetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}