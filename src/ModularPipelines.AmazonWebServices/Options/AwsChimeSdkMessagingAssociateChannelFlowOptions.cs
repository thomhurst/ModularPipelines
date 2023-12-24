using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "associate-channel-flow")]
public record AwsChimeSdkMessagingAssociateChannelFlowOptions(
[property: CommandSwitch("--channel-arn")] string ChannelArn,
[property: CommandSwitch("--channel-flow-arn")] string ChannelFlowArn,
[property: CommandSwitch("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}