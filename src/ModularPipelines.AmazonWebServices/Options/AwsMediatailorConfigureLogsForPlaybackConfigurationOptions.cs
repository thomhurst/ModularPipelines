using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "configure-logs-for-playback-configuration")]
public record AwsMediatailorConfigureLogsForPlaybackConfigurationOptions(
[property: CliOption("--percent-enabled")] int PercentEnabled,
[property: CliOption("--playback-configuration-name")] string PlaybackConfigurationName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}