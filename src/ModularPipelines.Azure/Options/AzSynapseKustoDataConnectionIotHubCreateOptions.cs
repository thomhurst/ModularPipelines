using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "kusto", "data-connection", "iot-hub", "create")]
public record AzSynapseKustoDataConnectionIotHubCreateOptions(
[property: CliOption("--data-connection-name")] string DataConnectionName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--kusto-pool-name")] string KustoPoolName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--consumer-group")]
    public string? ConsumerGroup { get; set; }

    [CliOption("--data-format")]
    public string? DataFormat { get; set; }

    [CliOption("--event-system-properties")]
    public string? EventSystemProperties { get; set; }

    [CliOption("--iot-hub-resource-id")]
    public string? IotHubResourceId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mapping-rule-name")]
    public string? MappingRuleName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--shared-access-policy-name")]
    public string? SharedAccessPolicyName { get; set; }

    [CliOption("--table-name")]
    public string? TableName { get; set; }
}