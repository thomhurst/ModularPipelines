using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "create-prefetch-schedule")]
public record AwsMediatailorCreatePrefetchScheduleOptions(
[property: CommandSwitch("--consumption")] string Consumption,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--playback-configuration-name")] string PlaybackConfigurationName,
[property: CommandSwitch("--retrieval")] string Retrieval
) : AwsOptions
{
    [CommandSwitch("--stream-id")]
    public string? StreamId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}