using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "connection", "create", "confluent-cloud")]
public record AzSpringConnectionCreateConfluentCloudOptions(
[property: CliOption("--bootstrap-server")] string BootstrapServer,
[property: CliOption("--kafka-key")] string KafkaKey,
[property: CliOption("--kafka-secret")] string KafkaSecret,
[property: CliOption("--schema-key")] string SchemaKey,
[property: CliOption("--schema-registry")] string SchemaRegistry,
[property: CliOption("--schema-secret")] string SchemaSecret
) : AzOptions
{
    [CliOption("--app")]
    public string? App { get; set; }

    [CliOption("--client-type")]
    public string? ClientType { get; set; }

    [CliOption("--connection")]
    public string? Connection { get; set; }

    [CliOption("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--source-id")]
    public string? SourceId { get; set; }

    [CliOption("--vault-id")]
    public string? VaultId { get; set; }
}