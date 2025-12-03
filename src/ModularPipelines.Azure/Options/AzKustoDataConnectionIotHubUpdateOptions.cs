using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "data-connection", "iot-hub", "update")]
public record AzKustoDataConnectionIotHubUpdateOptions : AzOptions
{
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

    [CliOption("--event-system-properties")]
    public string? EventSystemProperties { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--iot-hub-resource-id")]
    public string? IotHubResourceId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mapping-rule-name")]
    public string? MappingRuleName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--shared-access-policy-name")]
    public string? SharedAccessPolicyName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--table-name")]
    public string? TableName { get; set; }
}