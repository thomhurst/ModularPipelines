using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "elastic-pool", "create")]
public record AzSqlElasticPoolCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server")] string Server
) : AzOptions
{
    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--db-dtu-max")]
    public string? DbDtuMax { get; set; }

    [CliOption("--db-dtu-min")]
    public string? DbDtuMin { get; set; }

    [CliOption("--edition")]
    public string? Edition { get; set; }

    [CliOption("--family")]
    public string? Family { get; set; }

    [CliOption("--ha-replicas")]
    public string? HaReplicas { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--maint-config-id")]
    public string? MaintConfigId { get; set; }

    [CliOption("--max-size")]
    public string? MaxSize { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--preferred-enclave-type")]
    public string? PreferredEnclaveType { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}