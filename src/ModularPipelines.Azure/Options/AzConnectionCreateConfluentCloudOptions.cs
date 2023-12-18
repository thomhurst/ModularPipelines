using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connection", "create", "confluent-cloud")]
public record AzConnectionCreateConfluentCloudOptions(
[property: CommandSwitch("--bootstrap-server")] string BootstrapServer,
[property: CommandSwitch("--kafka-key")] string KafkaKey,
[property: CommandSwitch("--kafka-secret")] string KafkaSecret,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--schema-key")] string SchemaKey,
[property: CommandSwitch("--schema-registry")] string SchemaRegistry,
[property: CommandSwitch("--schema-secret")] string SchemaSecret
) : AzOptions
{
    [CommandSwitch("--client-type")]
    public string? ClientType { get; set; }

    [CommandSwitch("--connection")]
    public string? Connection { get; set; }

    [CommandSwitch("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}