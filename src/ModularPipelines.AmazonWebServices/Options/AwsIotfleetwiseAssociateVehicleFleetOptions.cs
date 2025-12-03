using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotfleetwise", "associate-vehicle-fleet")]
public record AwsIotfleetwiseAssociateVehicleFleetOptions(
[property: CliOption("--vehicle-name")] string VehicleName,
[property: CliOption("--fleet-id")] string FleetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}