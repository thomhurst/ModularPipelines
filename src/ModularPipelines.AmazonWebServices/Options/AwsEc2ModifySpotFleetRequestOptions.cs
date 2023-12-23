using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-spot-fleet-request")]
public record AwsEc2ModifySpotFleetRequestOptions(
[property: CommandSwitch("--spot-fleet-request-id")] string SpotFleetRequestId
) : AwsOptions
{
    [CommandSwitch("--excess-capacity-termination-policy")]
    public string? ExcessCapacityTerminationPolicy { get; set; }

    [CommandSwitch("--launch-template-configs")]
    public string[]? LaunchTemplateConfigs { get; set; }

    [CommandSwitch("--target-capacity")]
    public int? TargetCapacity { get; set; }

    [CommandSwitch("--on-demand-target-capacity")]
    public int? OnDemandTargetCapacity { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}