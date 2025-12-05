using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-spot-fleet-request")]
public record AwsEc2ModifySpotFleetRequestOptions(
[property: CliOption("--spot-fleet-request-id")] string SpotFleetRequestId
) : AwsOptions
{
    [CliOption("--excess-capacity-termination-policy")]
    public string? ExcessCapacityTerminationPolicy { get; set; }

    [CliOption("--launch-template-configs")]
    public string[]? LaunchTemplateConfigs { get; set; }

    [CliOption("--target-capacity")]
    public int? TargetCapacity { get; set; }

    [CliOption("--on-demand-target-capacity")]
    public int? OnDemandTargetCapacity { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}