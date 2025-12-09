using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-insights", "describe-component-configuration-recommendation")]
public record AwsApplicationInsightsDescribeComponentConfigurationRecommendationOptions(
[property: CliOption("--resource-group-name")] string ResourceGroupName,
[property: CliOption("--component-name")] string ComponentName,
[property: CliOption("--tier")] string Tier
) : AwsOptions
{
    [CliOption("--workload-name")]
    public string? WorkloadName { get; set; }

    [CliOption("--recommendation-type")]
    public string? RecommendationType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}