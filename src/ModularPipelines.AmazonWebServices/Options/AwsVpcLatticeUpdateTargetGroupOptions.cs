using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "update-target-group")]
public record AwsVpcLatticeUpdateTargetGroupOptions(
[property: CommandSwitch("--health-check")] string HealthCheck,
[property: CommandSwitch("--target-group-identifier")] string TargetGroupIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}