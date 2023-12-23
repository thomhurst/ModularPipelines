using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-traffic-mirror-filter")]
public record AwsEc2DeleteTrafficMirrorFilterOptions(
[property: CommandSwitch("--traffic-mirror-filter-id")] string TrafficMirrorFilterId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}