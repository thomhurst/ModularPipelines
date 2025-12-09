using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "connection", "update", "confluent-cloud")]
public record AzContainerappConnectionUpdateConfluentCloudOptions(
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

    [CliOption("--name")]
    public string? Name { get; set; }

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

    [CliOption("--source-id")]
    public string? SourceId { get; set; }

    [CliOption("--vault-id")]
    public string? VaultId { get; set; }
}