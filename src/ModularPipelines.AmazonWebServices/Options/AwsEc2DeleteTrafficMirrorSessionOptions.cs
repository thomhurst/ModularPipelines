using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-traffic-mirror-session")]
public record AwsEc2DeleteTrafficMirrorSessionOptions(
[property: CommandSwitch("--traffic-mirror-session-id")] string TrafficMirrorSessionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}