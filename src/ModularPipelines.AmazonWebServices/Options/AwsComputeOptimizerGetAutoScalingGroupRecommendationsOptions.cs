using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute-optimizer", "get-auto-scaling-group-recommendations")]
public record AwsComputeOptimizerGetAutoScalingGroupRecommendationsOptions : AwsOptions
{
    [CliOption("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CliOption("--auto-scaling-group-arns")]
    public string[]? AutoScalingGroupArns { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--recommendation-preferences")]
    public string? RecommendationPreferences { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}