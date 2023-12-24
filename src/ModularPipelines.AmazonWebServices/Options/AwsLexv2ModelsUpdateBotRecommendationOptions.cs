using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "update-bot-recommendation")]
public record AwsLexv2ModelsUpdateBotRecommendationOptions(
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--bot-version")] string BotVersion,
[property: CommandSwitch("--locale-id")] string LocaleId,
[property: CommandSwitch("--bot-recommendation-id")] string BotRecommendationId,
[property: CommandSwitch("--encryption-setting")] string EncryptionSetting
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}