using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "update-bot-recommendation")]
public record AwsLexv2ModelsUpdateBotRecommendationOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-version")] string BotVersion,
[property: CliOption("--locale-id")] string LocaleId,
[property: CliOption("--bot-recommendation-id")] string BotRecommendationId,
[property: CliOption("--encryption-setting")] string EncryptionSetting
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}