using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "cancel-capacity-reservation-fleets")]
public record AwsEc2CancelCapacityReservationFleetsOptions(
[property: CommandSwitch("--capacity-reservation-fleet-ids")] string[] CapacityReservationFleetIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}