using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "put-capacity-assignment-configuration")]
public record AwsAthenaPutCapacityAssignmentConfigurationOptions(
[property: CliOption("--capacity-reservation-name")] string CapacityReservationName,
[property: CliOption("--capacity-assignments")] string[] CapacityAssignments
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}