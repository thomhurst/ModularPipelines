using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "detect-key-phrases")]
public record AwsComprehendDetectKeyPhrasesOptions(
[property: CliOption("--text")] string Text,
[property: CliOption("--language-code")] string LanguageCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}