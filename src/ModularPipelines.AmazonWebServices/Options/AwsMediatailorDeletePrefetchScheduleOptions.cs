using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "delete-prefetch-schedule")]
public record AwsMediatailorDeletePrefetchScheduleOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--playback-configuration-name")] string PlaybackConfigurationName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}