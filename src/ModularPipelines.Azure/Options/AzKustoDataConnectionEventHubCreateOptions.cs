using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "data-connection", "event-hub", "create")]
public record AzKustoDataConnectionEventHubCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--data-connection-name")] string DataConnectionName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--compression")]
    public string? Compression { get; set; }

    [CommandSwitch("--consumer-group")]
    public string? ConsumerGroup { get; set; }

    [CommandSwitch("--data-format")]
    public string? DataFormat { get; set; }

    [CommandSwitch("--database-routing")]
    public string? DatabaseRouting { get; set; }

    [CommandSwitch("--event-hub-resource-id")]
    public string? EventHubResourceId { get; set; }

    [CommandSwitch("--event-system-properties")]
    public string? EventSystemProperties { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-identity")]
    public string? ManagedIdentity { get; set; }

    [CommandSwitch("--mapping-rule-name")]
    public string? MappingRuleName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--table-name")]
    public string? TableName { get; set; }
}