using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-traffic-mirror-filter-rule")]
public record AwsEc2DeleteTrafficMirrorFilterRuleOptions(
[property: CommandSwitch("--traffic-mirror-filter-rule-id")] string TrafficMirrorFilterRuleId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}