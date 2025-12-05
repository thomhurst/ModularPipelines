using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("orbital", "spacecraft", "contact", "create")]
public record AzOrbitalSpacecraftContactCreateOptions(
[property: CliOption("--contact-name")] string ContactName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--spacecraft-name")] string SpacecraftName
) : AzOptions
{
    [CliOption("--contact-profile")]
    public string? ContactProfile { get; set; }

    [CliOption("--ground-station-name")]
    public string? GroundStationName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--reservation-end-time")]
    public string? ReservationEndTime { get; set; }

    [CliOption("--reservation-start-time")]
    public string? ReservationStartTime { get; set; }
}