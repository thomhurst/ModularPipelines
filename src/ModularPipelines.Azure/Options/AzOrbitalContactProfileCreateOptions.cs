using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("orbital", "contact-profile", "create")]
public record AzOrbitalContactProfileCreateOptions(
[property: CommandSwitch("--contact-profile-name")] string ContactProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--auto-tracking")]
    public string? AutoTracking { get; set; }

    [CommandSwitch("--event-hub-uri")]
    public string? EventHubUri { get; set; }

    [CommandSwitch("--links")]
    public string? Links { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--min-elevation")]
    public string? MinElevation { get; set; }

    [CommandSwitch("--min-viable-duration")]
    public string? MinViableDuration { get; set; }

    [CommandSwitch("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

