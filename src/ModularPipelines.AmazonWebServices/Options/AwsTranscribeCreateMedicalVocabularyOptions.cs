using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "create-medical-vocabulary")]
public record AwsTranscribeCreateMedicalVocabularyOptions(
[property: CliOption("--vocabulary-name")] string VocabularyName,
[property: CliOption("--language-code")] string LanguageCode,
[property: CliOption("--vocabulary-file-uri")] string VocabularyFileUri
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}