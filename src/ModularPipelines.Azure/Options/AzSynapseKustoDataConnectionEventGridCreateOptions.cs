using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "kusto", "data-connection", "event-grid", "create")]
public record AzSynapseKustoDataConnectionEventGridCreateOptions(
[property: CliOption("--data-connection-name")] string DataConnectionName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--kusto-pool-name")] string KustoPoolName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--blob-storage-event-type")]
    public string? BlobStorageEventType { get; set; }

    [CliOption("--consumer-group")]
    public string? ConsumerGroup { get; set; }

    [CliOption("--data-format")]
    public string? DataFormat { get; set; }

    [CliOption("--event-hub-resource-id")]
    public string? EventHubResourceId { get; set; }

    [CliFlag("--ignore-first-record")]
    public bool? IgnoreFirstRecord { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mapping-rule-name")]
    public string? MappingRuleName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--storage-account-resource-id")]
    public int? StorageAccountResourceId { get; set; }

    [CliOption("--table-name")]
    public string? TableName { get; set; }
}