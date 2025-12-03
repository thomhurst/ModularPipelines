using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "update-deployment-strategy")]
public record AwsAppconfigUpdateDeploymentStrategyOptions(
[property: CliOption("--deployment-strategy-id")] string DeploymentStrategyId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--deployment-duration-in-minutes")]
    public int? DeploymentDurationInMinutes { get; set; }

    [CliOption("--final-bake-time-in-minutes")]
    public int? FinalBakeTimeInMinutes { get; set; }

    [CliOption("--growth-factor")]
    public float? GrowthFactor { get; set; }

    [CliOption("--growth-type")]
    public string? GrowthType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}