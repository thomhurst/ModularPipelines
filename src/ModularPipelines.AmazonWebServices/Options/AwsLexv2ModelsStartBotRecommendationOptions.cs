using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "start-bot-recommendation")]
public record AwsLexv2ModelsStartBotRecommendationOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-version")] string BotVersion,
[property: CliOption("--locale-id")] string LocaleId,
[property: CliOption("--transcript-source-setting")] string TranscriptSourceSetting
) : AwsOptions
{
    [CliOption("--encryption-setting")]
    public string? EncryptionSetting { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}