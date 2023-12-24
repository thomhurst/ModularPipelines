using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "get-prefetch-schedule")]
public record AwsMediatailorGetPrefetchScheduleOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--playback-configuration-name")] string PlaybackConfigurationName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}