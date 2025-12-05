using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("orbital", "contact-profile", "create")]
public record AzOrbitalContactProfileCreateOptions(
[property: CliOption("--contact-profile-name")] string ContactProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--auto-tracking")]
    public string? AutoTracking { get; set; }

    [CliOption("--event-hub-uri")]
    public string? EventHubUri { get; set; }

    [CliOption("--links")]
    public string? Links { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--min-elevation")]
    public string? MinElevation { get; set; }

    [CliOption("--min-viable-duration")]
    public string? MinViableDuration { get; set; }

    [CliOption("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}