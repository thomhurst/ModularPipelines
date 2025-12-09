using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "get-prefetch-schedule")]
public record AwsMediatailorGetPrefetchScheduleOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--playback-configuration-name")] string PlaybackConfigurationName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}