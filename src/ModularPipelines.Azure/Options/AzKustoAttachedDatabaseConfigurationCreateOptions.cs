using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "attached-database-configuration", "create")]
public record AzKustoAttachedDatabaseConfigurationCreateOptions(
[property: BooleanCommandSwitch("--attached-database-configuration-name")] bool AttachedDatabaseConfigurationName,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--cluster-resource-id")]
    public string? ClusterResourceId { get; set; }

    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--default-principals-modification-kind")]
    public string? DefaultPrincipalsModificationKind { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--table-level")]
    public string? TableLevel { get; set; }
}