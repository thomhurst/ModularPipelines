using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("m2", "update-environment")]
public record AwsM2UpdateEnvironmentOptions(
[property: CliOption("--environment-id")] string EnvironmentId
) : AwsOptions
{
    [CliOption("--desired-capacity")]
    public int? DesiredCapacity { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}