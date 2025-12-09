using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("workloads", "sap-disk-configuration")]
public record AzWorkloadsSapDiskConfigurationOptions : AzOptions
{
    [CliOption("--app-location")]
    public string? AppLocation { get; set; }

    [CliOption("--database-type")]
    public string? DatabaseType { get; set; }

    [CliOption("--db-vm-sku")]
    public string? DbVmSku { get; set; }

    [CliOption("--deployment-type")]
    public string? DeploymentType { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--sap-product")]
    public string? SapProduct { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}