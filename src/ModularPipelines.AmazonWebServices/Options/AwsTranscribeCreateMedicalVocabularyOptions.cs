using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "create-medical-vocabulary")]
public record AwsTranscribeCreateMedicalVocabularyOptions(
[property: CommandSwitch("--vocabulary-name")] string VocabularyName,
[property: CommandSwitch("--language-code")] string LanguageCode,
[property: CommandSwitch("--vocabulary-file-uri")] string VocabularyFileUri
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}