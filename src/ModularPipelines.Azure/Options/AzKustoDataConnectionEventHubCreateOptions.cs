using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "data-connection", "event-hub", "create")]
public record AzKustoDataConnectionEventHubCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--data-connection-name")] string DataConnectionName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--compression")]
    public string? Compression { get; set; }

    [CliOption("--consumer-group")]
    public string? ConsumerGroup { get; set; }

    [CliOption("--data-format")]
    public string? DataFormat { get; set; }

    [CliOption("--database-routing")]
    public string? DatabaseRouting { get; set; }

    [CliOption("--event-hub-resource-id")]
    public string? EventHubResourceId { get; set; }

    [CliOption("--event-system-properties")]
    public string? EventSystemProperties { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-identity")]
    public string? ManagedIdentity { get; set; }

    [CliOption("--mapping-rule-name")]
    public string? MappingRuleName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--table-name")]
    public string? TableName { get; set; }
}