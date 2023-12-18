using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connection", "create", "confluent-cloud")]
public record AzContainerappConnectionCreateConfluentCloudOptions(
[property: CommandSwitch("--bootstrap-server")] string BootstrapServer,
[property: CommandSwitch("--kafka-key")] string KafkaKey,
[property: CommandSwitch("--kafka-secret")] string KafkaSecret,
[property: CommandSwitch("--schema-key")] string SchemaKey,
[property: CommandSwitch("--schema-registry")] string SchemaRegistry,
[property: CommandSwitch("--schema-secret")] string SchemaSecret
) : AzOptions
{
    [CommandSwitch("--client-type")]
    public string? ClientType { get; set; }

    [CommandSwitch("--connection")]
    public string? Connection { get; set; }

    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [CommandSwitch("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--source-id")]
    public string? SourceId { get; set; }

    [CommandSwitch("--vault-id")]
    public string? VaultId { get; set; }
}

