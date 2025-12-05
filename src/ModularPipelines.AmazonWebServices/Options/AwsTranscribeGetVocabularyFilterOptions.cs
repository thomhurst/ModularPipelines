using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "get-vocabulary-filter")]
public record AwsTranscribeGetVocabularyFilterOptions(
[property: CliOption("--vocabulary-filter-name")] string VocabularyFilterName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}