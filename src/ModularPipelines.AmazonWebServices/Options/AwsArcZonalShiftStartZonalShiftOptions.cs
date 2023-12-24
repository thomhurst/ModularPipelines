using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arc-zonal-shift", "start-zonal-shift")]
public record AwsArcZonalShiftStartZonalShiftOptions(
[property: CommandSwitch("--away-from")] string AwayFrom,
[property: CommandSwitch("--comment")] string Comment,
[property: CommandSwitch("--expires-in")] string ExpiresIn,
[property: CommandSwitch("--resource-identifier")] string ResourceIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}