using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "update")]
public record AzSpringUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--build-pool-size")]
    public string? BuildPoolSize { get; set; }

    [BooleanCommandSwitch("--enable-dataplane-public-endpoint")]
    public bool? EnableDataplanePublicEndpoint { get; set; }

    [BooleanCommandSwitch("--enable-planned-maintenance")]
    public bool? EnablePlannedMaintenance { get; set; }

    [CommandSwitch("--ingress-read-timeout")]
    public string? IngressReadTimeout { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--planned-maintenance-day")]
    public string? PlannedMaintenanceDay { get; set; }

    [CommandSwitch("--planned-maintenance-start-hour")]
    public string? PlannedMaintenanceStartHour { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

