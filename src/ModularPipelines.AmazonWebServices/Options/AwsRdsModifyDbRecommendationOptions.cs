using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-db-recommendation")]
public record AwsRdsModifyDbRecommendationOptions(
[property: CommandSwitch("--recommendation-id")] string RecommendationId
) : AwsOptions
{
    [CommandSwitch("--locale")]
    public string? Locale { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--recommended-action-updates")]
    public string[]? RecommendedActionUpdates { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}