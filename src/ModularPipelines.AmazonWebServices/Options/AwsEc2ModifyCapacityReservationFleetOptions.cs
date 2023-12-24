using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-capacity-reservation-fleet")]
public record AwsEc2ModifyCapacityReservationFleetOptions(
[property: CommandSwitch("--capacity-reservation-fleet-id")] string CapacityReservationFleetId
) : AwsOptions
{
    [CommandSwitch("--total-target-capacity")]
    public int? TotalTargetCapacity { get; set; }

    [CommandSwitch("--end-date")]
    public long? EndDate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}