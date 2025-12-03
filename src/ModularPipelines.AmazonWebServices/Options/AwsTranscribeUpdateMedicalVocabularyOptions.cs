using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "update-medical-vocabulary")]
public record AwsTranscribeUpdateMedicalVocabularyOptions(
[property: CliOption("--vocabulary-name")] string VocabularyName,
[property: CliOption("--language-code")] string LanguageCode,
[property: CliOption("--vocabulary-file-uri")] string VocabularyFileUri
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}