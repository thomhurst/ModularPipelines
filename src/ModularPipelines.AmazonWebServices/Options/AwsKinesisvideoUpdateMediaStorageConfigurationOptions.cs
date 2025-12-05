using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisvideo", "update-media-storage-configuration")]
public record AwsKinesisvideoUpdateMediaStorageConfigurationOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--media-storage-configuration")] string MediaStorageConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}