using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis-video-webrtc-storage", "join-storage-session")]
public record AwsKinesisVideoWebrtcStorageJoinStorageSessionOptions(
[property: CliOption("--channel-arn")] string ChannelArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}