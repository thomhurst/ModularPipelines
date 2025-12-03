using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "create-channel")]
public record AwsChimeSdkMessagingCreateChannelOptions(
[property: CliOption("--app-instance-arn")] string AppInstanceArn,
[property: CliOption("--name")] string Name,
[property: CliOption("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--privacy")]
    public string? Privacy { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--channel-id")]
    public string? ChannelId { get; set; }

    [CliOption("--member-arns")]
    public string[]? MemberArns { get; set; }

    [CliOption("--moderator-arns")]
    public string[]? ModeratorArns { get; set; }

    [CliOption("--elastic-channel-configuration")]
    public string? ElasticChannelConfiguration { get; set; }

    [CliOption("--expiration-settings")]
    public string? ExpirationSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}