using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "create-bot-locale")]
public record AwsLexv2ModelsCreateBotLocaleOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-version")] string BotVersion,
[property: CliOption("--locale-id")] string LocaleId,
[property: CliOption("--nlu-intent-confidence-threshold")] double NluIntentConfidenceThreshold
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--voice-settings")]
    public string? VoiceSettings { get; set; }

    [CliOption("--generative-ai-settings")]
    public string? GenerativeAiSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}