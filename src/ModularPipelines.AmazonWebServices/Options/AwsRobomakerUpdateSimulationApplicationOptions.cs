using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "update-simulation-application")]
public record AwsRobomakerUpdateSimulationApplicationOptions(
[property: CommandSwitch("--application")] string Application,
[property: CommandSwitch("--simulation-software-suite")] string SimulationSoftwareSuite,
[property: CommandSwitch("--robot-software-suite")] string RobotSoftwareSuite
) : AwsOptions
{
    [CommandSwitch("--sources")]
    public string[]? Sources { get; set; }

    [CommandSwitch("--rendering-engine")]
    public string? RenderingEngine { get; set; }

    [CommandSwitch("--current-revision-id")]
    public string? CurrentRevisionId { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}