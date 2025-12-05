using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "update-vocabulary-filter")]
public record AwsTranscribeUpdateVocabularyFilterOptions(
[property: CliOption("--vocabulary-filter-name")] string VocabularyFilterName
) : AwsOptions
{
    [CliOption("--words")]
    public string[]? Words { get; set; }

    [CliOption("--vocabulary-filter-file-uri")]
    public string? VocabularyFilterFileUri { get; set; }

    [CliOption("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}