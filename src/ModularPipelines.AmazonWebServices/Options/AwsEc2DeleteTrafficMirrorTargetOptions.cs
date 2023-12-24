using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-traffic-mirror-target")]
public record AwsEc2DeleteTrafficMirrorTargetOptions(
[property: CommandSwitch("--traffic-mirror-target-id")] string TrafficMirrorTargetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}