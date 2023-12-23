using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "update-robot-application")]
public record AwsRobomakerUpdateRobotApplicationOptions(
[property: CommandSwitch("--application")] string Application,
[property: CommandSwitch("--robot-software-suite")] string RobotSoftwareSuite
) : AwsOptions
{
    [CommandSwitch("--sources")]
    public string[]? Sources { get; set; }

    [CommandSwitch("--current-revision-id")]
    public string? CurrentRevisionId { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}