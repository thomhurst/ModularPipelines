using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "delete-traffic-distribution-group")]
public record AwsConnectDeleteTrafficDistributionGroupOptions(
[property: CliOption("--traffic-distribution-group-id")] string TrafficDistributionGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}