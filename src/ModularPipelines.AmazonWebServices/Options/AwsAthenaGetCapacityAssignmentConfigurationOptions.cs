using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "get-capacity-assignment-configuration")]
public record AwsAthenaGetCapacityAssignmentConfigurationOptions(
[property: CliOption("--capacity-reservation-name")] string CapacityReservationName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}