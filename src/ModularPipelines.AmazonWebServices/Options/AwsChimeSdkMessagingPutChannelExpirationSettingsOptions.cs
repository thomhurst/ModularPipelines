using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "put-channel-expiration-settings")]
public record AwsChimeSdkMessagingPutChannelExpirationSettingsOptions(
[property: CliOption("--channel-arn")] string ChannelArn
) : AwsOptions
{
    [CliOption("--chime-bearer")]
    public string? ChimeBearer { get; set; }

    [CliOption("--expiration-settings")]
    public string? ExpirationSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}