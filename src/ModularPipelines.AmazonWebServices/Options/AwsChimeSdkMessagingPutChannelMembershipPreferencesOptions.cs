using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "put-channel-membership-preferences")]
public record AwsChimeSdkMessagingPutChannelMembershipPreferencesOptions(
[property: CommandSwitch("--channel-arn")] string ChannelArn,
[property: CommandSwitch("--member-arn")] string MemberArn,
[property: CommandSwitch("--chime-bearer")] string ChimeBearer,
[property: CommandSwitch("--preferences")] string Preferences
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}