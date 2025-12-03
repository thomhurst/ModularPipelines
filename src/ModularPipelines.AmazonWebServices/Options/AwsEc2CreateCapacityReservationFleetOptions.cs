using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-capacity-reservation-fleet")]
public record AwsEc2CreateCapacityReservationFleetOptions(
[property: CliOption("--instance-type-specifications")] string[] InstanceTypeSpecifications,
[property: CliOption("--total-target-capacity")] int TotalTargetCapacity
) : AwsOptions
{
    [CliOption("--allocation-strategy")]
    public string? AllocationStrategy { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tenancy")]
    public string? Tenancy { get; set; }

    [CliOption("--end-date")]
    public long? EndDate { get; set; }

    [CliOption("--instance-match-criteria")]
    public string? InstanceMatchCriteria { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}