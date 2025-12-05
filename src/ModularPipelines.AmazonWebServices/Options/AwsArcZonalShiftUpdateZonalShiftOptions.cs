using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arc-zonal-shift", "update-zonal-shift")]
public record AwsArcZonalShiftUpdateZonalShiftOptions(
[property: CliOption("--zonal-shift-id")] string ZonalShiftId
) : AwsOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--expires-in")]
    public string? ExpiresIn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}