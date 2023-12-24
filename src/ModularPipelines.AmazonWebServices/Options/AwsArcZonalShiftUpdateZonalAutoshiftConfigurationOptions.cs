using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arc-zonal-shift", "update-zonal-autoshift-configuration")]
public record AwsArcZonalShiftUpdateZonalAutoshiftConfigurationOptions(
[property: CommandSwitch("--resource-identifier")] string ResourceIdentifier,
[property: CommandSwitch("--zonal-autoshift-status")] string ZonalAutoshiftStatus
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}