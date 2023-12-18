using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("orbital", "spacecraft", "list")]
public record AzOrbitalSpacecraftListOptions(
[property: CommandSwitch("--contact-profile")] string ContactProfile,
[property: CommandSwitch("--end-time")] string EndTime,
[property: CommandSwitch("--ground-station-name")] string GroundStationName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--spacecraft-name")] string SpacecraftName,
[property: CommandSwitch("--start-time")] string StartTime
) : AzOptions
{
    [CommandSwitch("--skiptoken")]
    public string? Skiptoken { get; set; }
}