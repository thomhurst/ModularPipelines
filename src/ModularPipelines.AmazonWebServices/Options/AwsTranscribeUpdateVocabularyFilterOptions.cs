using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "update-vocabulary-filter")]
public record AwsTranscribeUpdateVocabularyFilterOptions(
[property: CommandSwitch("--vocabulary-filter-name")] string VocabularyFilterName
) : AwsOptions
{
    [CommandSwitch("--words")]
    public string[]? Words { get; set; }

    [CommandSwitch("--vocabulary-filter-file-uri")]
    public string? VocabularyFilterFileUri { get; set; }

    [CommandSwitch("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}