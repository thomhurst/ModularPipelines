using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "describe-bot-recommendation")]
public record AwsLexv2ModelsDescribeBotRecommendationOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-version")] string BotVersion,
[property: CliOption("--locale-id")] string LocaleId,
[property: CliOption("--bot-recommendation-id")] string BotRecommendationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}