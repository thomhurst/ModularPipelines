using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "delete-vocabulary-filter")]
public record AwsTranscribeDeleteVocabularyFilterOptions(
[property: CliOption("--vocabulary-filter-name")] string VocabularyFilterName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}