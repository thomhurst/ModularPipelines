using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "describe-channel-flow")]
public record AwsChimeSdkMessagingDescribeChannelFlowOptions(
[property: CommandSwitch("--channel-flow-arn")] string ChannelFlowArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}