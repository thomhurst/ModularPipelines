using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "update-program")]
public record AwsMediatailorUpdateProgramOptions(
[property: CommandSwitch("--channel-name")] string ChannelName,
[property: CommandSwitch("--program-name")] string ProgramName,
[property: CommandSwitch("--schedule-configuration")] string ScheduleConfiguration
) : AwsOptions
{
    [CommandSwitch("--ad-breaks")]
    public string[]? AdBreaks { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}