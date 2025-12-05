using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-traffic-mirror-filter-rule")]
public record AwsEc2DeleteTrafficMirrorFilterRuleOptions(
[property: CliOption("--traffic-mirror-filter-rule-id")] string TrafficMirrorFilterRuleId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}