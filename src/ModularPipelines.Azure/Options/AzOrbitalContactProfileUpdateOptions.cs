using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("orbital", "contact-profile", "update")]
public record AzOrbitalContactProfileUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--auto-tracking")]
    public string? AutoTracking { get; set; }

    [CliOption("--contact-profile-name")]
    public string? ContactProfileName { get; set; }

    [CliOption("--event-hub-uri")]
    public string? EventHubUri { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

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

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}