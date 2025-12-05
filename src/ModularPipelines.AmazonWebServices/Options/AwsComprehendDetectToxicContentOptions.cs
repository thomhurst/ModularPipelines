using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "detect-toxic-content")]
public record AwsComprehendDetectToxicContentOptions(
[property: CliOption("--text-segments")] string[] TextSegments,
[property: CliOption("--language-code")] string LanguageCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}