using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "delete-channel-flow")]
public record AwsChimeSdkMessagingDeleteChannelFlowOptions(
[property: CommandSwitch("--channel-flow-arn")] string ChannelFlowArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}