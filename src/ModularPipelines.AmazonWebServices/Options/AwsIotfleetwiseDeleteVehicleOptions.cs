using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotfleetwise", "delete-vehicle")]
public record AwsIotfleetwiseDeleteVehicleOptions(
[property: CommandSwitch("--vehicle-name")] string VehicleName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}