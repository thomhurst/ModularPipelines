using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "delete-simulation-application")]
public record AwsRobomakerDeleteSimulationApplicationOptions(
[property: CommandSwitch("--application")] string Application
) : AwsOptions
{
    [CommandSwitch("--application-version")]
    public string? ApplicationVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}