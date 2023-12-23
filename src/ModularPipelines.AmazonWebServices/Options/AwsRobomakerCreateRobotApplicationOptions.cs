using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "create-robot-application")]
public record AwsRobomakerCreateRobotApplicationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--robot-software-suite")] string RobotSoftwareSuite
) : AwsOptions
{
    [CommandSwitch("--sources")]
    public string[]? Sources { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}