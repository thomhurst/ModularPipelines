using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "describe-robot-application")]
public record AwsRobomakerDescribeRobotApplicationOptions(
[property: CommandSwitch("--application")] string Application
) : AwsOptions
{
    [CommandSwitch("--application-version")]
    public string? ApplicationVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}