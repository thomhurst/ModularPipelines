using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-traffic-mirror-filter")]
public record AwsEc2DeleteTrafficMirrorFilterOptions(
[property: CliOption("--traffic-mirror-filter-id")] string TrafficMirrorFilterId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}