using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "delete-vocabulary-filter")]
public record AwsTranscribeDeleteVocabularyFilterOptions(
[property: CommandSwitch("--vocabulary-filter-name")] string VocabularyFilterName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}