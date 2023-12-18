using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("orbital", "spacecraft", "delete")]
public record AzOrbitalSpacecraftDeleteOptions(
[property: CommandSwitch("--contact-profile")] string ContactProfile,
[property: CommandSwitch("--end-time")] string EndTime,
[property: CommandSwitch("--ground-station-name")] string GroundStationName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--spacecraft-name")] string SpacecraftName,
[property: CommandSwitch("--start-time")] string StartTime
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

