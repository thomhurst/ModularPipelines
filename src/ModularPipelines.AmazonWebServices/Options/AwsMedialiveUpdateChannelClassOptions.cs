using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "update-channel-class")]
public record AwsMedialiveUpdateChannelClassOptions(
[property: CommandSwitch("--channel-class")] string ChannelClass,
[property: CommandSwitch("--channel-id")] string ChannelId
) : AwsOptions
{
    [CommandSwitch("--destinations")]
    public string[]? Destinations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}