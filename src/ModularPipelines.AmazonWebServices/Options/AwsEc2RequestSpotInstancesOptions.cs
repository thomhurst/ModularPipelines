using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "request-spot-instances")]
public record AwsEc2RequestSpotInstancesOptions : AwsOptions
{
    [CliOption("--availability-zone-group")]
    public string? AvailabilityZoneGroup { get; set; }

    [CliOption("--block-duration-minutes")]
    public int? BlockDurationMinutes { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--launch-group")]
    public string? LaunchGroup { get; set; }

    [CliOption("--launch-specification")]
    public string? LaunchSpecification { get; set; }

    [CliOption("--spot-price")]
    public string? SpotPrice { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--valid-from")]
    public long? ValidFrom { get; set; }

    [CliOption("--valid-until")]
    public long? ValidUntil { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--instance-interruption-behavior")]
    public string? InstanceInterruptionBehavior { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}