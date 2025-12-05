using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "update-channel")]
public record AwsMediatailorUpdateChannelOptions(
[property: CliOption("--channel-name")] string ChannelName,
[property: CliOption("--outputs")] string[] Outputs
) : AwsOptions
{
    [CliOption("--filler-slate")]
    public string? FillerSlate { get; set; }

    [CliOption("--time-shift-configuration")]
    public string? TimeShiftConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}