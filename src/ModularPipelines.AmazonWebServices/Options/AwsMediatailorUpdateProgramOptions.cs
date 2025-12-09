using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "update-program")]
public record AwsMediatailorUpdateProgramOptions(
[property: CliOption("--channel-name")] string ChannelName,
[property: CliOption("--program-name")] string ProgramName,
[property: CliOption("--schedule-configuration")] string ScheduleConfiguration
) : AwsOptions
{
    [CliOption("--ad-breaks")]
    public string[]? AdBreaks { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}