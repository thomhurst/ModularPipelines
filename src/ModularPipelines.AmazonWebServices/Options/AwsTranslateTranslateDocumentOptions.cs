using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("translate", "translate-document")]
public record AwsTranslateTranslateDocumentOptions(
[property: CliOption("--document")] string Document,
[property: CliOption("--source-language-code")] string SourceLanguageCode,
[property: CliOption("--target-language-code")] string TargetLanguageCode,
[property: CliOption("--document-content")] string DocumentContent
) : AwsOptions
{
    [CliOption("--terminology-names")]
    public string[]? TerminologyNames { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}