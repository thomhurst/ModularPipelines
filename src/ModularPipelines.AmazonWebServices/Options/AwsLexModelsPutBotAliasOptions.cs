using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "put-bot-alias")]
public record AwsLexModelsPutBotAliasOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--bot-version")] string BotVersion,
[property: CliOption("--bot-name")] string BotName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--checksum")]
    public string? Checksum { get; set; }

    [CliOption("--conversation-logs")]
    public string? ConversationLogs { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}