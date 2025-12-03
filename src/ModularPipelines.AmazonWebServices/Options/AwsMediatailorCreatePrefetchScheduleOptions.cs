using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "create-prefetch-schedule")]
public record AwsMediatailorCreatePrefetchScheduleOptions(
[property: CliOption("--consumption")] string Consumption,
[property: CliOption("--name")] string Name,
[property: CliOption("--playback-configuration-name")] string PlaybackConfigurationName,
[property: CliOption("--retrieval")] string Retrieval
) : AwsOptions
{
    [CliOption("--stream-id")]
    public string? StreamId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}