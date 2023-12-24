using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "get-capacity-assignment-configuration")]
public record AwsAthenaGetCapacityAssignmentConfigurationOptions(
[property: CommandSwitch("--capacity-reservation-name")] string CapacityReservationName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}