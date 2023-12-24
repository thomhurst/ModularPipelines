using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-insights", "describe-component-configuration-recommendation")]
public record AwsApplicationInsightsDescribeComponentConfigurationRecommendationOptions(
[property: CommandSwitch("--resource-group-name")] string ResourceGroupName,
[property: CommandSwitch("--component-name")] string ComponentName,
[property: CommandSwitch("--tier")] string Tier
) : AwsOptions
{
    [CommandSwitch("--workload-name")]
    public string? WorkloadName { get; set; }

    [CommandSwitch("--recommendation-type")]
    public string? RecommendationType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}