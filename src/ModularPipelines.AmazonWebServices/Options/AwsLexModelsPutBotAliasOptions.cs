using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "put-bot-alias")]
public record AwsLexModelsPutBotAliasOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--bot-version")] string BotVersion,
[property: CommandSwitch("--bot-name")] string BotName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--checksum")]
    public string? Checksum { get; set; }

    [CommandSwitch("--conversation-logs")]
    public string? ConversationLogs { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}