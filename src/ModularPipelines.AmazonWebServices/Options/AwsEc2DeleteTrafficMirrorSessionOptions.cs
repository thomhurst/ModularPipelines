using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-traffic-mirror-session")]
public record AwsEc2DeleteTrafficMirrorSessionOptions(
[property: CliOption("--traffic-mirror-session-id")] string TrafficMirrorSessionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}