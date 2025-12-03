using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("robomaker", "create-robot-application")]
public record AwsRobomakerCreateRobotApplicationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--robot-software-suite")] string RobotSoftwareSuite
) : AwsOptions
{
    [CliOption("--sources")]
    public string[]? Sources { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}