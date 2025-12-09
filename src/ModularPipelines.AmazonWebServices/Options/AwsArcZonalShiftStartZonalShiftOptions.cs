using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arc-zonal-shift", "start-zonal-shift")]
public record AwsArcZonalShiftStartZonalShiftOptions(
[property: CliOption("--away-from")] string AwayFrom,
[property: CliOption("--comment")] string Comment,
[property: CliOption("--expires-in")] string ExpiresIn,
[property: CliOption("--resource-identifier")] string ResourceIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}