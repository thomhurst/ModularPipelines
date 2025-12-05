using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "update")]
public record AzSpringUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--build-pool-size")]
    public string? BuildPoolSize { get; set; }

    [CliFlag("--enable-dataplane-public-endpoint")]
    public bool? EnableDataplanePublicEndpoint { get; set; }

    [CliFlag("--enable-planned-maintenance")]
    public bool? EnablePlannedMaintenance { get; set; }

    [CliOption("--ingress-read-timeout")]
    public string? IngressReadTimeout { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--planned-maintenance-day")]
    public string? PlannedMaintenanceDay { get; set; }

    [CliOption("--planned-maintenance-start-hour")]
    public string? PlannedMaintenanceStartHour { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}