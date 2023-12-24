using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "create-channel")]
public record AwsMediatailorCreateChannelOptions(
[property: CommandSwitch("--channel-name")] string ChannelName,
[property: CommandSwitch("--outputs")] string[] Outputs,
[property: CommandSwitch("--playback-mode")] string PlaybackMode
) : AwsOptions
{
    [CommandSwitch("--filler-slate")]
    public string? FillerSlate { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--time-shift-configuration")]
    public string? TimeShiftConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}