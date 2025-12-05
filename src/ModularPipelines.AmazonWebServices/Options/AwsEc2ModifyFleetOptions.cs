using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-fleet")]
public record AwsEc2ModifyFleetOptions(
[property: CliOption("--fleet-id")] string FleetId
) : AwsOptions
{
    [CliOption("--excess-capacity-termination-policy")]
    public string? ExcessCapacityTerminationPolicy { get; set; }

    [CliOption("--launch-template-configs")]
    public string[]? LaunchTemplateConfigs { get; set; }

    [CliOption("--target-capacity-specification")]
    public string? TargetCapacitySpecification { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}