using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "update-deployment-strategy")]
public record AwsAppconfigUpdateDeploymentStrategyOptions(
[property: CommandSwitch("--deployment-strategy-id")] string DeploymentStrategyId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--deployment-duration-in-minutes")]
    public int? DeploymentDurationInMinutes { get; set; }

    [CommandSwitch("--final-bake-time-in-minutes")]
    public int? FinalBakeTimeInMinutes { get; set; }

    [CommandSwitch("--growth-factor")]
    public float? GrowthFactor { get; set; }

    [CommandSwitch("--growth-type")]
    public string? GrowthType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}