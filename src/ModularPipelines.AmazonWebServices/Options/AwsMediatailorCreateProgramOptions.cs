using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "create-program")]
public record AwsMediatailorCreateProgramOptions(
[property: CommandSwitch("--channel-name")] string ChannelName,
[property: CommandSwitch("--program-name")] string ProgramName,
[property: CommandSwitch("--schedule-configuration")] string ScheduleConfiguration,
[property: CommandSwitch("--source-location-name")] string SourceLocationName
) : AwsOptions
{
    [CommandSwitch("--ad-breaks")]
    public string[]? AdBreaks { get; set; }

    [CommandSwitch("--live-source-name")]
    public string? LiveSourceName { get; set; }

    [CommandSwitch("--vod-source-name")]
    public string? VodSourceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}