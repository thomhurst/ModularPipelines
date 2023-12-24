using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "describe-simulation-application")]
public record AwsRobomakerDescribeSimulationApplicationOptions(
[property: CommandSwitch("--application")] string Application
) : AwsOptions
{
    [CommandSwitch("--application-version")]
    public string? ApplicationVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}