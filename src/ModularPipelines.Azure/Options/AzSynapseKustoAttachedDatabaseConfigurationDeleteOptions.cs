using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "kusto", "attached-database-configuration", "delete")]
public record AzSynapseKustoAttachedDatabaseConfigurationDeleteOptions : AzOptions
{
    [CliFlag("--attached-database-configuration-name")]
    public bool? AttachedDatabaseConfigurationName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kusto-pool-name")]
    public string? KustoPoolName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}