using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("kusto", "data-connection", "event-hub", "data-connection-validation")]
public record AzKustoDataConnectionEventHubDataConnectionValidationOptions : AzOptions
{
    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--compression")]
    public string? Compression { get; set; }

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

    [CliOption("--event-hub-resource-id")]
    public string? EventHubResourceId { get; set; }

    [CliOption("--event-system-properties")]
    public string? EventSystemProperties { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

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

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--table-name")]
    public string? TableName { get; set; }
}