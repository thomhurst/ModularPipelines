using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "batch-detect-targeted-sentiment")]
public record AwsComprehendBatchDetectTargetedSentimentOptions(
[property: CliOption("--text-list")] string[] TextList,
[property: CliOption("--language-code")] string LanguageCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}