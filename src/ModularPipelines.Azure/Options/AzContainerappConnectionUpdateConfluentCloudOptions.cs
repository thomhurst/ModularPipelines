using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connection", "update", "confluent-cloud")]
public record AzContainerappConnectionUpdateConfluentCloudOptions(
[property: CommandSwitch("--connection")] string Connection
) : AzOptions
{
    [CommandSwitch("--bootstrap-server")]
    public string? BootstrapServer { get; set; }

    [CommandSwitch("--client-type")]
    public string? ClientType { get; set; }

    [CommandSwitch("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CommandSwitch("--kafka-key")]
    public string? KafkaKey { get; set; }

    [CommandSwitch("--kafka-secret")]
    public string? KafkaSecret { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--schema-key")]
    public string? SchemaKey { get; set; }

    [CommandSwitch("--schema-registry")]
    public string? SchemaRegistry { get; set; }

    [CommandSwitch("--schema-secret")]
    public string? SchemaSecret { get; set; }

    [CommandSwitch("--source-id")]
    public string? SourceId { get; set; }

    [CommandSwitch("--vault-id")]
    public string? VaultId { get; set; }
}

