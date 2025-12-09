using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("robomaker", "create-simulation-application")]
public record AwsRobomakerCreateSimulationApplicationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--simulation-software-suite")] string SimulationSoftwareSuite,
[property: CliOption("--robot-software-suite")] string RobotSoftwareSuite
) : AwsOptions
{
    [CliOption("--sources")]
    public string[]? Sources { get; set; }

    [CliOption("--rendering-engine")]
    public string? RenderingEngine { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}