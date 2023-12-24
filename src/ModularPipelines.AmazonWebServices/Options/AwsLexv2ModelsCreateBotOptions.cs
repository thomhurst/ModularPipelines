using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "create-bot")]
public record AwsLexv2ModelsCreateBotOptions(
[property: CommandSwitch("--bot-name")] string BotName,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--data-privacy")] string DataPrivacy,
[property: CommandSwitch("--idle-session-ttl-in-seconds")] int IdleSessionTtlInSeconds
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--bot-tags")]
    public IEnumerable<KeyValue>? BotTags { get; set; }

    [CommandSwitch("--test-bot-alias-tags")]
    public IEnumerable<KeyValue>? TestBotAliasTags { get; set; }

    [CommandSwitch("--bot-type")]
    public string? BotType { get; set; }

    [CommandSwitch("--bot-members")]
    public string[]? BotMembers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}