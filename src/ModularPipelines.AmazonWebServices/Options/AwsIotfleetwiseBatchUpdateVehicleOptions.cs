using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotfleetwise", "batch-update-vehicle")]
public record AwsIotfleetwiseBatchUpdateVehicleOptions(
[property: CliOption("--vehicles")] string[] Vehicles
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}