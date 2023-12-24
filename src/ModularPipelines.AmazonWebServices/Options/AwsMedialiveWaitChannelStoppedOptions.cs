using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "wait", "channel-stopped")]
public record AwsMedialiveWaitChannelStoppedOptions(
[property: CommandSwitch("--channel-id")] string ChannelId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}