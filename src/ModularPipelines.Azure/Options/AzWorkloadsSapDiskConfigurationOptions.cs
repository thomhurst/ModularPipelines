using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads", "sap-disk-configuration")]
public record AzWorkloadsSapDiskConfigurationOptions : AzOptions
{
    [CommandSwitch("--app-location")]
    public string? AppLocation { get; set; }

    [CommandSwitch("--database-type")]
    public string? DatabaseType { get; set; }

    [CommandSwitch("--db-vm-sku")]
    public string? DbVmSku { get; set; }

    [CommandSwitch("--deployment-type")]
    public string? DeploymentType { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--sap-product")]
    public string? SapProduct { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}