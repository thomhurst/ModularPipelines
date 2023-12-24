using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "create-vocabulary")]
public record AwsTranscribeCreateVocabularyOptions(
[property: CommandSwitch("--vocabulary-name")] string VocabularyName,
[property: CommandSwitch("--language-code")] string LanguageCode
) : AwsOptions
{
    [CommandSwitch("--phrases")]
    public string[]? Phrases { get; set; }

    [CommandSwitch("--vocabulary-file-uri")]
    public string? VocabularyFileUri { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}