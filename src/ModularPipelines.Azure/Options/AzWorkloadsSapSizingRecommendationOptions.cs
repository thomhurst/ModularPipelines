using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads", "sap-sizing-recommendation")]
public record AzWorkloadsSapSizingRecommendationOptions : AzOptions
{
    [CommandSwitch("--app-location")]
    public string? AppLocation { get; set; }

    [CommandSwitch("--database-type")]
    public string? DatabaseType { get; set; }

    [CommandSwitch("--db-memory")]
    public string? DbMemory { get; set; }

    [CommandSwitch("--db-scale-method")]
    public string? DbScaleMethod { get; set; }

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

    [CommandSwitch("--saps")]
    public string? Saps { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

