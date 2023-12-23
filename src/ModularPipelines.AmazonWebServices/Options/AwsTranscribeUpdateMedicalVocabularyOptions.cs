using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "update-medical-vocabulary")]
public record AwsTranscribeUpdateMedicalVocabularyOptions(
[property: CommandSwitch("--vocabulary-name")] string VocabularyName,
[property: CommandSwitch("--language-code")] string LanguageCode,
[property: CommandSwitch("--vocabulary-file-uri")] string VocabularyFileUri
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}