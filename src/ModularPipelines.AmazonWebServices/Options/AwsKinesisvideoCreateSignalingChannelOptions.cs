using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisvideo", "create-signaling-channel")]
public record AwsKinesisvideoCreateSignalingChannelOptions(
[property: CommandSwitch("--channel-name")] string ChannelName
) : AwsOptions
{
    [CommandSwitch("--channel-type")]
    public string? ChannelType { get; set; }

    [CommandSwitch("--single-master-configuration")]
    public string? SingleMasterConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}