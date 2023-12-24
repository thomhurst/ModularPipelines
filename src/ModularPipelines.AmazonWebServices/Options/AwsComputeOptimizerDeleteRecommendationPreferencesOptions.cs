using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute-optimizer", "delete-recommendation-preferences")]
public record AwsComputeOptimizerDeleteRecommendationPreferencesOptions(
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--recommendation-preference-names")] string[] RecommendationPreferenceNames
) : AwsOptions
{
    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}