using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workloads", "sap-database-instance", "start")]
public record AzWorkloadsSapDatabaseInstanceStartOptions : AzOptions
{
    [CliOption("--database-instance-name")]
    public string? DatabaseInstanceName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sap-virtual-instance-name")]
    public string? SapVirtualInstanceName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}