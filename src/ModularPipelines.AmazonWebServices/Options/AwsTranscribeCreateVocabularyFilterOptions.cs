using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "create-vocabulary-filter")]
public record AwsTranscribeCreateVocabularyFilterOptions(
[property: CommandSwitch("--vocabulary-filter-name")] string VocabularyFilterName,
[property: CommandSwitch("--language-code")] string LanguageCode
) : AwsOptions
{
    [CommandSwitch("--words")]
    public string[]? Words { get; set; }

    [CommandSwitch("--vocabulary-filter-file-uri")]
    public string? VocabularyFilterFileUri { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}