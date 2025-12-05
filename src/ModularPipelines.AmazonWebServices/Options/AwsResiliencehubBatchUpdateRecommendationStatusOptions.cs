using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "batch-update-recommendation-status")]
public record AwsResiliencehubBatchUpdateRecommendationStatusOptions(
[property: CliOption("--app-arn")] string AppArn,
[property: CliOption("--request-entries")] string[] RequestEntries
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}