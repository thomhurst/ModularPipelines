using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "update-vocabulary")]
public record AwsTranscribeUpdateVocabularyOptions(
[property: CommandSwitch("--vocabulary-name")] string VocabularyName,
[property: CommandSwitch("--language-code")] string LanguageCode
) : AwsOptions
{
    [CommandSwitch("--phrases")]
    public string[]? Phrases { get; set; }

    [CommandSwitch("--vocabulary-file-uri")]
    public string? VocabularyFileUri { get; set; }

    [CommandSwitch("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}