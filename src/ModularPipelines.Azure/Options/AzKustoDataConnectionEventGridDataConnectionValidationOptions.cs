using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("kusto", "data-connection", "event-grid", "data-connection-validation")]
public record AzKustoDataConnectionEventGridDataConnectionValidationOptions : AzOptions
{
    [CliOption("--blob-storage-event-type")]
    public string? BlobStorageEventType { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--consumer-group")]
    public string? ConsumerGroup { get; set; }

    [CliOption("--data-connection-name")]
    public string? DataConnectionName { get; set; }

    [CliOption("--data-format")]
    public string? DataFormat { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--database-routing")]
    public string? DatabaseRouting { get; set; }

    [CliOption("--event-grid-resource-id")]
    public string? EventGridResourceId { get; set; }

    [CliOption("--event-hub-resource-id")]
    public string? EventHubResourceId { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--ignore-first-record")]
    public bool? IgnoreFirstRecord { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-identity")]
    public string? ManagedIdentity { get; set; }

    [CliOption("--mapping-rule-name")]
    public string? MappingRuleName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--storage-account-resource-id")]
    public int? StorageAccountResourceId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--table-name")]
    public string? TableName { get; set; }
}