using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "db", "update")]
public record AzSqlDbUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CliOption("--auto-pause-delay")]
    public string? AutoPauseDelay { get; set; }

    [CliOption("--backup-storage-redundancy")]
    public string? BackupStorageRedundancy { get; set; }

    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--compute-model")]
    public string? ComputeModel { get; set; }

    [CliOption("--edition")]
    public string? Edition { get; set; }

    [CliOption("--elastic-pool")]
    public string? ElasticPool { get; set; }

    [CliOption("--encryption-protector")]
    public string? EncryptionProtector { get; set; }

    [CliFlag("--encryption-protector-auto-rotation")]
    public bool? EncryptionProtectorAutoRotation { get; set; }

    [CliOption("--exhaustion-behavior")]
    public string? ExhaustionBehavior { get; set; }

    [CliOption("--family")]
    public string? Family { get; set; }

    [CliOption("--federated-client-id")]
    public string? FederatedClientId { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--free-limit")]
    public bool? FreeLimit { get; set; }

    [CliOption("--ha-replicas")]
    public string? HaReplicas { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--keys")]
    public string? Keys { get; set; }

    [CliOption("--keys-to-remove")]
    public string? KeysToRemove { get; set; }

    [CliOption("--maint-config-id")]
    public string? MaintConfigId { get; set; }

    [CliOption("--max-size")]
    public string? MaxSize { get; set; }

    [CliOption("--min-capacity")]
    public string? MinCapacity { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--preferred-enclave-type")]
    public string? PreferredEnclaveType { get; set; }

    [CliOption("--read-scale")]
    public string? ReadScale { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--service-objective")]
    public string? ServiceObjective { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--umi")]
    public string? Umi { get; set; }

    [CliFlag("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}