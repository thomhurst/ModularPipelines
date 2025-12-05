using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arc-zonal-shift", "update-zonal-autoshift-configuration")]
public record AwsArcZonalShiftUpdateZonalAutoshiftConfigurationOptions(
[property: CliOption("--resource-identifier")] string ResourceIdentifier,
[property: CliOption("--zonal-autoshift-status")] string ZonalAutoshiftStatus
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}