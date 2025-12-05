using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "update-bot")]
public record AwsLexv2ModelsUpdateBotOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-name")] string BotName,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--data-privacy")] string DataPrivacy,
[property: CliOption("--idle-session-ttl-in-seconds")] int IdleSessionTtlInSeconds
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--bot-type")]
    public string? BotType { get; set; }

    [CliOption("--bot-members")]
    public string[]? BotMembers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}