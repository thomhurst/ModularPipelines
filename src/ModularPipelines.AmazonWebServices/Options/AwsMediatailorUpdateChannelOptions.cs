using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "update-channel")]
public record AwsMediatailorUpdateChannelOptions(
[property: CommandSwitch("--channel-name")] string ChannelName,
[property: CommandSwitch("--outputs")] string[] Outputs
) : AwsOptions
{
    [CommandSwitch("--filler-slate")]
    public string? FillerSlate { get; set; }

    [CommandSwitch("--time-shift-configuration")]
    public string? TimeShiftConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}