using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisvideo", "update-media-storage-configuration")]
public record AwsKinesisvideoUpdateMediaStorageConfigurationOptions(
[property: CommandSwitch("--channel-arn")] string ChannelArn,
[property: CommandSwitch("--media-storage-configuration")] string MediaStorageConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}