using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arc-zonal-shift", "update-zonal-shift")]
public record AwsArcZonalShiftUpdateZonalShiftOptions(
[property: CommandSwitch("--zonal-shift-id")] string ZonalShiftId
) : AwsOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--expires-in")]
    public string? ExpiresIn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}