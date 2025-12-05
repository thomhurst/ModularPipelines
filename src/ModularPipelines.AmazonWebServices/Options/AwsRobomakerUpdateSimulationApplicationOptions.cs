using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("robomaker", "update-simulation-application")]
public record AwsRobomakerUpdateSimulationApplicationOptions(
[property: CliOption("--application")] string Application,
[property: CliOption("--simulation-software-suite")] string SimulationSoftwareSuite,
[property: CliOption("--robot-software-suite")] string RobotSoftwareSuite
) : AwsOptions
{
    [CliOption("--sources")]
    public string[]? Sources { get; set; }

    [CliOption("--rendering-engine")]
    public string? RenderingEngine { get; set; }

    [CliOption("--current-revision-id")]
    public string? CurrentRevisionId { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}