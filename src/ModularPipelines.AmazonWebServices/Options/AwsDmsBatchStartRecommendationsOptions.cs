using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "batch-start-recommendations")]
public record AwsDmsBatchStartRecommendationsOptions : AwsOptions
{
    [CliOption("--data")]
    public string[]? Data { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}