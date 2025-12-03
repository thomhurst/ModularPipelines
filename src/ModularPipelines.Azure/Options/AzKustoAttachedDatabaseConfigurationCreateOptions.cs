using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "attached-database-configuration", "create")]
public record AzKustoAttachedDatabaseConfigurationCreateOptions(
[property: CliFlag("--attached-database-configuration-name")] bool AttachedDatabaseConfigurationName,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--cluster-resource-id")]
    public string? ClusterResourceId { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--default-principals-modification-kind")]
    public string? DefaultPrincipalsModificationKind { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--table-level")]
    public string? TableLevel { get; set; }
}