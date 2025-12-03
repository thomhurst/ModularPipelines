using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connection", "update", "confluent-cloud")]
public record AzConnectionUpdateConfluentCloudOptions(
[property: CliOption("--connection")] string Connection
) : AzOptions
{
    [CliOption("--bootstrap-server")]
    public string? BootstrapServer { get; set; }

    [CliOption("--client-type")]
    public string? ClientType { get; set; }

    [CliOption("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CliOption("--kafka-key")]
    public string? KafkaKey { get; set; }

    [CliOption("--kafka-secret")]
    public string? KafkaSecret { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--schema-key")]
    public string? SchemaKey { get; set; }

    [CliOption("--schema-registry")]
    public string? SchemaRegistry { get; set; }

    [CliOption("--schema-secret")]
    public string? SchemaSecret { get; set; }
}