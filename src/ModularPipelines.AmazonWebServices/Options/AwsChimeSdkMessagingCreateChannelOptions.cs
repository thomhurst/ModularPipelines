using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "create-channel")]
public record AwsChimeSdkMessagingCreateChannelOptions(
[property: CommandSwitch("--app-instance-arn")] string AppInstanceArn,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--privacy")]
    public string? Privacy { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--channel-id")]
    public string? ChannelId { get; set; }

    [CommandSwitch("--member-arns")]
    public string[]? MemberArns { get; set; }

    [CommandSwitch("--moderator-arns")]
    public string[]? ModeratorArns { get; set; }

    [CommandSwitch("--elastic-channel-configuration")]
    public string? ElasticChannelConfiguration { get; set; }

    [CommandSwitch("--expiration-settings")]
    public string? ExpirationSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}