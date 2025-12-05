using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("workloads", "sap-database-instance", "show")]
public record AzWorkloadsSapDatabaseInstanceShowOptions : AzOptions
{
    [CliOption("--database-instance-name")]
    public string? DatabaseInstanceName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sap-virtual-instance-name")]
    public string? SapVirtualInstanceName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}