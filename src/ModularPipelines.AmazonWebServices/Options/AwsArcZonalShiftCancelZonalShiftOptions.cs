using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arc-zonal-shift", "cancel-zonal-shift")]
public record AwsArcZonalShiftCancelZonalShiftOptions(
[property: CommandSwitch("--zonal-shift-id")] string ZonalShiftId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}