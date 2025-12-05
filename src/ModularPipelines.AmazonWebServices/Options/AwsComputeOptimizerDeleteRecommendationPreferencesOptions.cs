using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute-optimizer", "delete-recommendation-preferences")]
public record AwsComputeOptimizerDeleteRecommendationPreferencesOptions(
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--recommendation-preference-names")] string[] RecommendationPreferenceNames
) : AwsOptions
{
    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}