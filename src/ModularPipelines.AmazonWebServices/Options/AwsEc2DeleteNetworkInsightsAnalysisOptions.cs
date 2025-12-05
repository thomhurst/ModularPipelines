using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-network-insights-analysis")]
public record AwsEc2DeleteNetworkInsightsAnalysisOptions(
[property: CliOption("--network-insights-analysis-id")] string NetworkInsightsAnalysisId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}