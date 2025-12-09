using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "create-deployment-strategy")]
public record AwsAppconfigCreateDeploymentStrategyOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--deployment-duration-in-minutes")] int DeploymentDurationInMinutes,
[property: CliOption("--growth-factor")] float GrowthFactor
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--final-bake-time-in-minutes")]
    public int? FinalBakeTimeInMinutes { get; set; }

    [CliOption("--growth-type")]
    public string? GrowthType { get; set; }

    [CliOption("--replicate-to")]
    public string? ReplicateTo { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}