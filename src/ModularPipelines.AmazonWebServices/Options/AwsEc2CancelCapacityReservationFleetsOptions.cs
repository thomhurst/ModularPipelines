using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "cancel-capacity-reservation-fleets")]
public record AwsEc2CancelCapacityReservationFleetsOptions(
[property: CliOption("--capacity-reservation-fleet-ids")] string[] CapacityReservationFleetIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}