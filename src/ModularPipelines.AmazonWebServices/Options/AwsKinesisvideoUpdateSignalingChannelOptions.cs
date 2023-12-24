using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisvideo", "update-signaling-channel")]
public record AwsKinesisvideoUpdateSignalingChannelOptions(
[property: CommandSwitch("--channel-arn")] string ChannelArn,
[property: CommandSwitch("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CommandSwitch("--single-master-configuration")]
    public string? SingleMasterConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}