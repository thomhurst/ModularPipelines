using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "describe-traffic-distribution-group")]
public record AwsConnectDescribeTrafficDistributionGroupOptions(
[property: CliOption("--traffic-distribution-group-id")] string TrafficDistributionGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}