using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "start-recommendations")]
public record AwsDmsStartRecommendationsOptions(
[property: CliOption("--database-id")] string DatabaseId,
[property: CliOption("--settings")] string Settings
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}