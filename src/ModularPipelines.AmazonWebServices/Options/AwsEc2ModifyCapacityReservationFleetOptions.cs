using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-capacity-reservation-fleet")]
public record AwsEc2ModifyCapacityReservationFleetOptions(
[property: CliOption("--capacity-reservation-fleet-id")] string CapacityReservationFleetId
) : AwsOptions
{
    [CliOption("--total-target-capacity")]
    public int? TotalTargetCapacity { get; set; }

    [CliOption("--end-date")]
    public long? EndDate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}