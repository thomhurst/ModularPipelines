using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "delete-robot-application")]
public record AwsRobomakerDeleteRobotApplicationOptions(
[property: CommandSwitch("--application")] string Application
) : AwsOptions
{
    [CommandSwitch("--application-version")]
    public string? ApplicationVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}