using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisvideo", "get-signaling-channel-endpoint")]
public record AwsKinesisvideoGetSignalingChannelEndpointOptions(
[property: CliOption("--channel-arn")] string ChannelArn
) : AwsOptions
{
    [CliOption("--single-master-channel-endpoint-configuration")]
    public string? SingleMasterChannelEndpointConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}