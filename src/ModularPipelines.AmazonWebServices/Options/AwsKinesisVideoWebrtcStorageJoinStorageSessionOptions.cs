using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis-video-webrtc-storage", "join-storage-session")]
public record AwsKinesisVideoWebrtcStorageJoinStorageSessionOptions(
[property: CommandSwitch("--channel-arn")] string ChannelArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}