using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "put-channel-expiration-settings")]
public record AwsChimeSdkMessagingPutChannelExpirationSettingsOptions(
[property: CommandSwitch("--channel-arn")] string ChannelArn
) : AwsOptions
{
    [CommandSwitch("--chime-bearer")]
    public string? ChimeBearer { get; set; }

    [CommandSwitch("--expiration-settings")]
    public string? ExpirationSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}