using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "create-bot-alias")]
public record AwsLexv2ModelsCreateBotAliasOptions(
[property: CliOption("--bot-alias-name")] string BotAliasName,
[property: CliOption("--bot-id")] string BotId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--bot-version")]
    public string? BotVersion { get; set; }

    [CliOption("--bot-alias-locale-settings")]
    public IEnumerable<KeyValue>? BotAliasLocaleSettings { get; set; }

    [CliOption("--conversation-log-settings")]
    public string? ConversationLogSettings { get; set; }

    [CliOption("--sentiment-analysis-settings")]
    public string? SentimentAnalysisSettings { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}