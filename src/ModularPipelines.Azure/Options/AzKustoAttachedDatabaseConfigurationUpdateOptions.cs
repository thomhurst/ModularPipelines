using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "attached-database-configuration", "update")]
public record AzKustoAttachedDatabaseConfigurationUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--attached-database-configuration-name")]
    public bool? AttachedDatabaseConfigurationName { get; set; }

    [CommandSwitch("--cluster-name")]
    public string? ClusterName { get; set; }

    [CommandSwitch("--cluster-resource-id")]
    public string? ClusterResourceId { get; set; }

    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--default-principals-modification-kind")]
    public string? DefaultPrincipalsModificationKind { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--table-level")]
    public string? TableLevel { get; set; }
}