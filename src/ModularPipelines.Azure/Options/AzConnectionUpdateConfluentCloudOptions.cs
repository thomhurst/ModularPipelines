using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connection", "update", "confluent-cloud")]
public record AzConnectionUpdateConfluentCloudOptions(
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

    [CommandSwitch("--location")]
    public string? Location { get; set; }

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
}