using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "kusto", "attached-database-configuration", "create")]
public record AzSynapseKustoAttachedDatabaseConfigurationCreateOptions(
[property: CliFlag("--attached-database-configuration-name")] bool AttachedDatabaseConfigurationName,
[property: CliOption("--kusto-pool-name")] string KustoPoolName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--default-principals-modification-kind")]
    public string? DefaultPrincipalsModificationKind { get; set; }

    [CliOption("--kusto-pool-resource-id")]
    public string? KustoPoolResourceId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--table-level-sharing-properties")]
    public string? TableLevelSharingProperties { get; set; }
}