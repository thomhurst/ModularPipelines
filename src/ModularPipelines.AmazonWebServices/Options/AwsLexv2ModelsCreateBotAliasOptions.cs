using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "create-bot-alias")]
public record AwsLexv2ModelsCreateBotAliasOptions(
[property: CommandSwitch("--bot-alias-name")] string BotAliasName,
[property: CommandSwitch("--bot-id")] string BotId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--bot-version")]
    public string? BotVersion { get; set; }

    [CommandSwitch("--bot-alias-locale-settings")]
    public IEnumerable<KeyValue>? BotAliasLocaleSettings { get; set; }

    [CommandSwitch("--conversation-log-settings")]
    public string? ConversationLogSettings { get; set; }

    [CommandSwitch("--sentiment-analysis-settings")]
    public string? SentimentAnalysisSettings { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}