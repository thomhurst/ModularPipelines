using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("orbital", "spacecraft", "list-available-contact")]
public record AzOrbitalSpacecraftListAvailableContactOptions(
[property: CliOption("--contact-profile")] string ContactProfile,
[property: CliOption("--end-time")] string EndTime,
[property: CliOption("--ground-station-name")] string GroundStationName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--spacecraft-name")] string SpacecraftName,
[property: CliOption("--start-time")] string StartTime
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}