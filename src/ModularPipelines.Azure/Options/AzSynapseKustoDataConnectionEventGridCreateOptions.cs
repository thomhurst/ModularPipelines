using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "kusto", "data-connection", "event-grid", "create")]
public record AzSynapseKustoDataConnectionEventGridCreateOptions(
[property: CommandSwitch("--data-connection-name")] string DataConnectionName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--kusto-pool-name")] string KustoPoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--blob-storage-event-type")]
    public string? BlobStorageEventType { get; set; }

    [CommandSwitch("--consumer-group")]
    public string? ConsumerGroup { get; set; }

    [CommandSwitch("--data-format")]
    public string? DataFormat { get; set; }

    [CommandSwitch("--event-hub-resource-id")]
    public string? EventHubResourceId { get; set; }

    [BooleanCommandSwitch("--ignore-first-record")]
    public bool? IgnoreFirstRecord { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--mapping-rule-name")]
    public string? MappingRuleName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--storage-account-resource-id")]
    public int? StorageAccountResourceId { get; set; }

    [CommandSwitch("--table-name")]
    public string? TableName { get; set; }
}