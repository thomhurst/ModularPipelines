using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "put-capacity-assignment-configuration")]
public record AwsAthenaPutCapacityAssignmentConfigurationOptions(
[property: CommandSwitch("--capacity-reservation-name")] string CapacityReservationName,
[property: CommandSwitch("--capacity-assignments")] string[] CapacityAssignments
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}