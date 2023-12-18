using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("orbital", "spacecraft", "contact", "create")]
public record AzOrbitalSpacecraftContactCreateOptions(
[property: CommandSwitch("--contact-name")] string ContactName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--spacecraft-name")] string SpacecraftName
) : AzOptions
{
    [CommandSwitch("--contact-profile")]
    public string? ContactProfile { get; set; }

    [CommandSwitch("--ground-station-name")]
    public string? GroundStationName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--reservation-end-time")]
    public string? ReservationEndTime { get; set; }

    [CommandSwitch("--reservation-start-time")]
    public string? ReservationStartTime { get; set; }
}