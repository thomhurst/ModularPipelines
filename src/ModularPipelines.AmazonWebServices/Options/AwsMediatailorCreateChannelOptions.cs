using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "create-channel")]
public record AwsMediatailorCreateChannelOptions(
[property: CliOption("--channel-name")] string ChannelName,
[property: CliOption("--outputs")] string[] Outputs,
[property: CliOption("--playback-mode")] string PlaybackMode
) : AwsOptions
{
    [CliOption("--filler-slate")]
    public string? FillerSlate { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--time-shift-configuration")]
    public string? TimeShiftConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}