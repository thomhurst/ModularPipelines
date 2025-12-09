using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "create-bot")]
public record AwsLexv2ModelsCreateBotOptions(
[property: CliOption("--bot-name")] string BotName,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--data-privacy")] string DataPrivacy,
[property: CliOption("--idle-session-ttl-in-seconds")] int IdleSessionTtlInSeconds
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--bot-tags")]
    public IEnumerable<KeyValue>? BotTags { get; set; }

    [CliOption("--test-bot-alias-tags")]
    public IEnumerable<KeyValue>? TestBotAliasTags { get; set; }

    [CliOption("--bot-type")]
    public string? BotType { get; set; }

    [CliOption("--bot-members")]
    public string[]? BotMembers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}