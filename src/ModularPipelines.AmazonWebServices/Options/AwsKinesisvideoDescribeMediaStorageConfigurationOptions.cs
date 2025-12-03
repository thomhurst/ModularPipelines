using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisvideo", "describe-media-storage-configuration")]
public record AwsKinesisvideoDescribeMediaStorageConfigurationOptions : AwsOptions
{
    [CliOption("--channel-name")]
    public string? ChannelName { get; set; }

    [CliOption("--channel-arn")]
    public string? ChannelArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}