using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "data-connection", "iot-hub", "data-connection-validation")]
public record AzKustoDataConnectionIotHubDataConnectionValidationOptions : AzOptions
{
    [CommandSwitch("--cluster-name")]
    public string? ClusterName { get; set; }

    [CommandSwitch("--consumer-group")]
    public string? ConsumerGroup { get; set; }

    [CommandSwitch("--data-connection-name")]
    public string? DataConnectionName { get; set; }

    [CommandSwitch("--data-format")]
    public string? DataFormat { get; set; }

    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--database-routing")]
    public string? DatabaseRouting { get; set; }

    [CommandSwitch("--event-system-properties")]
    public string? EventSystemProperties { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--iot-hub-resource-id")]
    public string? IotHubResourceId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--mapping-rule-name")]
    public string? MappingRuleName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--shared-access-policy-name")]
    public string? SharedAccessPolicyName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--table-name")]
    public string? TableName { get; set; }
}