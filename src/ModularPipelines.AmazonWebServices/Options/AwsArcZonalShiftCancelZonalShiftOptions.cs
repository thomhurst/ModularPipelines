using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arc-zonal-shift", "cancel-zonal-shift")]
public record AwsArcZonalShiftCancelZonalShiftOptions(
[property: CliOption("--zonal-shift-id")] string ZonalShiftId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}