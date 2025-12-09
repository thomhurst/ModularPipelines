using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-network-insights-access-scope-analysis")]
public record AwsEc2DeleteNetworkInsightsAccessScopeAnalysisOptions(
[property: CliOption("--network-insights-access-scope-analysis-id")] string NetworkInsightsAccessScopeAnalysisId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}