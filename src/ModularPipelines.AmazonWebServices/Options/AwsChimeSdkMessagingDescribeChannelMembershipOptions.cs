using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "describe-channel-membership")]
public record AwsChimeSdkMessagingDescribeChannelMembershipOptions(
[property: CommandSwitch("--channel-arn")] string ChannelArn,
[property: CommandSwitch("--member-arn")] string MemberArn,
[property: CommandSwitch("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CommandSwitch("--sub-channel-id")]
    public string? SubChannelId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}