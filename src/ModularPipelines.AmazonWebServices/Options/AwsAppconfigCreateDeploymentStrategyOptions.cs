using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "create-deployment-strategy")]
public record AwsAppconfigCreateDeploymentStrategyOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--deployment-duration-in-minutes")] int DeploymentDurationInMinutes,
[property: CommandSwitch("--growth-factor")] float GrowthFactor
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--final-bake-time-in-minutes")]
    public int? FinalBakeTimeInMinutes { get; set; }

    [CommandSwitch("--growth-type")]
    public string? GrowthType { get; set; }

    [CommandSwitch("--replicate-to")]
    public string? ReplicateTo { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}