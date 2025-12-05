using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "delete-medical-vocabulary")]
public record AwsTranscribeDeleteMedicalVocabularyOptions(
[property: CliOption("--vocabulary-name")] string VocabularyName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}