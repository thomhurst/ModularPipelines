using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connection", "create", "confluent-cloud")]
public record AzConnectionCreateConfluentCloudOptions(
[property: CliOption("--bootstrap-server")] string BootstrapServer,
[property: CliOption("--kafka-key")] string KafkaKey,
[property: CliOption("--kafka-secret")] string KafkaSecret,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--schema-key")] string SchemaKey,
[property: CliOption("--schema-registry")] string SchemaRegistry,
[property: CliOption("--schema-secret")] string SchemaSecret
) : AzOptions
{
    [CliOption("--client-type")]
    public string? ClientType { get; set; }

    [CliOption("--connection")]
    public string? Connection { get; set; }

    [CliOption("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}