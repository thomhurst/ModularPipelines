using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("kusto", "attached-database-configuration", "show")]
public record AzKustoAttachedDatabaseConfigurationShowOptions : AzOptions
{
    [CliFlag("--attached-database-configuration-name")]
    public bool? AttachedDatabaseConfigurationName { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}