using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "update-bot")]
public record AwsLexv2ModelsUpdateBotOptions(
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--bot-name")] string BotName,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--data-privacy")] string DataPrivacy,
[property: CommandSwitch("--idle-session-ttl-in-seconds")] int IdleSessionTtlInSeconds
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--bot-type")]
    public string? BotType { get; set; }

    [CommandSwitch("--bot-members")]
    public string[]? BotMembers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}