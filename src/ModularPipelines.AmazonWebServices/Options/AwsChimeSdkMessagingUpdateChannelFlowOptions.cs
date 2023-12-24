using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "update-channel-flow")]
public record AwsChimeSdkMessagingUpdateChannelFlowOptions(
[property: CommandSwitch("--channel-flow-arn")] string ChannelFlowArn,
[property: CommandSwitch("--processors")] string[] Processors,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}