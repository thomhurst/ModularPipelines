using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "create-vocabulary")]
public record AwsTranscribeCreateVocabularyOptions(
[property: CliOption("--vocabulary-name")] string VocabularyName,
[property: CliOption("--language-code")] string LanguageCode
) : AwsOptions
{
    [CliOption("--phrases")]
    public string[]? Phrases { get; set; }

    [CliOption("--vocabulary-file-uri")]
    public string? VocabularyFileUri { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}