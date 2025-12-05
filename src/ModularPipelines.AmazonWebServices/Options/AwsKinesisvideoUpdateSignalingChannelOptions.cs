using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisvideo", "update-signaling-channel")]
public record AwsKinesisvideoUpdateSignalingChannelOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CliOption("--single-master-configuration")]
    public string? SingleMasterConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}