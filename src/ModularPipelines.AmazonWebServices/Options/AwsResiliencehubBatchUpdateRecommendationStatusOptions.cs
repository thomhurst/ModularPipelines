using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "batch-update-recommendation-status")]
public record AwsResiliencehubBatchUpdateRecommendationStatusOptions(
[property: CommandSwitch("--app-arn")] string AppArn,
[property: CommandSwitch("--request-entries")] string[] RequestEntries
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}