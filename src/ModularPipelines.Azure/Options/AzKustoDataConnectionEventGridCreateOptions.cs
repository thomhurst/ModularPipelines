using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "data-connection", "event-grid", "create")]
public record AzKustoDataConnectionEventGridCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--data-connection-name")] string DataConnectionName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--blob-storage-event-type")]
    public string? BlobStorageEventType { get; set; }

    [CommandSwitch("--consumer-group")]
    public string? ConsumerGroup { get; set; }

    [CommandSwitch("--data-format")]
    public string? DataFormat { get; set; }

    [CommandSwitch("--database-routing")]
    public string? DatabaseRouting { get; set; }

    [CommandSwitch("--event-grid-resource-id")]
    public string? EventGridResourceId { get; set; }

    [CommandSwitch("--event-hub-resource-id")]
    public string? EventHubResourceId { get; set; }

    [BooleanCommandSwitch("--ignore-first-record")]
    public bool? IgnoreFirstRecord { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-identity")]
    public string? ManagedIdentity { get; set; }

    [CommandSwitch("--mapping-rule-name")]
    public string? MappingRuleName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--storage-account-resource-id")]
    public int? StorageAccountResourceId { get; set; }

    [CommandSwitch("--table-name")]
    public string? TableName { get; set; }
}