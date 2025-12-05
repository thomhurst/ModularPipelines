using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "associate-default-vocabulary")]
public record AwsConnectAssociateDefaultVocabularyOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--language-code")] string LanguageCode
) : AwsOptions
{
    [CliOption("--vocabulary-id")]
    public string? VocabularyId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}