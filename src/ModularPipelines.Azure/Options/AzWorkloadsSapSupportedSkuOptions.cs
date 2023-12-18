using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads", "sap-supported-sku")]
public record AzWorkloadsSapSupportedSkuOptions : AzOptions
{
    [CommandSwitch("--app-location")]
    public string? AppLocation { get; set; }

    [CommandSwitch("--database-type")]
    public string? DatabaseType { get; set; }

    [CommandSwitch("--deployment-type")]
    public string? DeploymentType { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--high-availability-type")]
    public string? HighAvailabilityType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--sap-product")]
    public string? SapProduct { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}