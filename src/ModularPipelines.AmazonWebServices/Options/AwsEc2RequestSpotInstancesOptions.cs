using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "request-spot-instances")]
public record AwsEc2RequestSpotInstancesOptions : AwsOptions
{
    [CommandSwitch("--availability-zone-group")]
    public string? AvailabilityZoneGroup { get; set; }

    [CommandSwitch("--block-duration-minutes")]
    public int? BlockDurationMinutes { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--launch-group")]
    public string? LaunchGroup { get; set; }

    [CommandSwitch("--launch-specification")]
    public string? LaunchSpecification { get; set; }

    [CommandSwitch("--spot-price")]
    public string? SpotPrice { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--valid-from")]
    public long? ValidFrom { get; set; }

    [CommandSwitch("--valid-until")]
    public long? ValidUntil { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--instance-interruption-behavior")]
    public string? InstanceInterruptionBehavior { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}