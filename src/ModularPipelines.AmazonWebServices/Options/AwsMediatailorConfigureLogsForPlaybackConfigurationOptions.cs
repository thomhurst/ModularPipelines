using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "configure-logs-for-playback-configuration")]
public record AwsMediatailorConfigureLogsForPlaybackConfigurationOptions(
[property: CommandSwitch("--percent-enabled")] int PercentEnabled,
[property: CommandSwitch("--playback-configuration-name")] string PlaybackConfigurationName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}