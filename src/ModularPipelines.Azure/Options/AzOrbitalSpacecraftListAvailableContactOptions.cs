using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("orbital", "spacecraft", "list-available-contact")]
public record AzOrbitalSpacecraftListAvailableContactOptions(
[property: CommandSwitch("--contact-profile")] string ContactProfile,
[property: CommandSwitch("--end-time")] string EndTime,
[property: CommandSwitch("--ground-station-name")] string GroundStationName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--spacecraft-name")] string SpacecraftName,
[property: CommandSwitch("--start-time")] string StartTime
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}