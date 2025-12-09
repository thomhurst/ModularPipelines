using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "detect-pii-entities")]
public record AwsComprehendDetectPiiEntitiesOptions(
[property: CliOption("--text")] string Text,
[property: CliOption("--language-code")] string LanguageCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}