using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "kusto", "attached-database-configuration", "create")]
public record AzSynapseKustoAttachedDatabaseConfigurationCreateOptions(
[property: BooleanCommandSwitch("--attached-database-configuration-name")] bool AttachedDatabaseConfigurationName,
[property: CommandSwitch("--kusto-pool-name")] string KustoPoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--default-principals-modification-kind")]
    public string? DefaultPrincipalsModificationKind { get; set; }

    [CommandSwitch("--kusto-pool-resource-id")]
    public string? KustoPoolResourceId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--table-level-sharing-properties")]
    public string? TableLevelSharingProperties { get; set; }
}