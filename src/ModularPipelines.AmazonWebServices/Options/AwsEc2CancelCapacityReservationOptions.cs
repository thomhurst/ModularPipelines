using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "cancel-capacity-reservation")]
public record AwsEc2CancelCapacityReservationOptions(
[property: CommandSwitch("--capacity-reservation-id")] string CapacityReservationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}