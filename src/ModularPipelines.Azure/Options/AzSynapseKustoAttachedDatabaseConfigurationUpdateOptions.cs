using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "kusto", "attached-database-configuration", "update")]
public record AzSynapseKustoAttachedDatabaseConfigurationUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--attached-database-configuration-name")]
    public bool? AttachedDatabaseConfigurationName { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--default-principals-modification-kind")]
    public string? DefaultPrincipalsModificationKind { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kusto-pool-name")]
    public string? KustoPoolName { get; set; }

    [CliOption("--kusto-pool-resource-id")]
    public string? KustoPoolResourceId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

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

    [CliOption("--table-level-sharing-properties")]
    public string? TableLevelSharingProperties { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}