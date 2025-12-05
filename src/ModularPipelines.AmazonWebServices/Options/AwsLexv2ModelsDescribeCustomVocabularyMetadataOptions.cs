using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "describe-custom-vocabulary-metadata")]
public record AwsLexv2ModelsDescribeCustomVocabularyMetadataOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-version")] string BotVersion,
[property: CliOption("--locale-id")] string LocaleId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}