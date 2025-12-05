using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-network-insights-path")]
public record AwsEc2DeleteNetworkInsightsPathOptions(
[property: CliOption("--network-insights-path-id")] string NetworkInsightsPathId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}