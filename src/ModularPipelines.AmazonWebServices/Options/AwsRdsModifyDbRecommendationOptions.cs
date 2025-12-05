using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "modify-db-recommendation")]
public record AwsRdsModifyDbRecommendationOptions(
[property: CliOption("--recommendation-id")] string RecommendationId
) : AwsOptions
{
    [CliOption("--locale")]
    public string? Locale { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--recommended-action-updates")]
    public string[]? RecommendedActionUpdates { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}