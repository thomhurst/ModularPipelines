using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("robomaker", "delete-simulation-application")]
public record AwsRobomakerDeleteSimulationApplicationOptions(
[property: CliOption("--application")] string Application
) : AwsOptions
{
    [CliOption("--application-version")]
    public string? ApplicationVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}