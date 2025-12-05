using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "create-program")]
public record AwsMediatailorCreateProgramOptions(
[property: CliOption("--channel-name")] string ChannelName,
[property: CliOption("--program-name")] string ProgramName,
[property: CliOption("--schedule-configuration")] string ScheduleConfiguration,
[property: CliOption("--source-location-name")] string SourceLocationName
) : AwsOptions
{
    [CliOption("--ad-breaks")]
    public string[]? AdBreaks { get; set; }

    [CliOption("--live-source-name")]
    public string? LiveSourceName { get; set; }

    [CliOption("--vod-source-name")]
    public string? VodSourceName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}