using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db", "create")]
public record AzSqlDbCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server")] string Server
) : AzOptions
{
    [BooleanCommandSwitch("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CommandSwitch("--auto-pause-delay")]
    public string? AutoPauseDelay { get; set; }

    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--backup-storage-redundancy")]
    public string? BackupStorageRedundancy { get; set; }

    [CommandSwitch("--capacity")]
    public string? Capacity { get; set; }

    [CommandSwitch("--catalog-collation")]
    public string? CatalogCollation { get; set; }

    [CommandSwitch("--collation")]
    public string? Collation { get; set; }

    [CommandSwitch("--compute-model")]
    public string? ComputeModel { get; set; }

    [CommandSwitch("--edition")]
    public string? Edition { get; set; }

    [CommandSwitch("--elastic-pool")]
    public string? ElasticPool { get; set; }

    [CommandSwitch("--encryption-protector")]
    public string? EncryptionProtector { get; set; }

    [BooleanCommandSwitch("--encryption-protector-auto-rotation")]
    public bool? EncryptionProtectorAutoRotation { get; set; }

    [CommandSwitch("--exhaustion-behavior")]
    public string? ExhaustionBehavior { get; set; }

    [CommandSwitch("--family")]
    public string? Family { get; set; }

    [CommandSwitch("--federated-client-id")]
    public string? FederatedClientId { get; set; }

    [BooleanCommandSwitch("--free-limit")]
    public bool? FreeLimit { get; set; }

    [CommandSwitch("--ha-replicas")]
    public string? HaReplicas { get; set; }

    [CommandSwitch("--keys")]
    public string? Keys { get; set; }

    [CommandSwitch("--ledger-on")]
    public string? LedgerOn { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--maint-config-id")]
    public string? MaintConfigId { get; set; }

    [CommandSwitch("--max-size")]
    public string? MaxSize { get; set; }

    [CommandSwitch("--min-capacity")]
    public string? MinCapacity { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--preferred-enclave-type")]
    public string? PreferredEnclaveType { get; set; }

    [CommandSwitch("--read-scale")]
    public string? ReadScale { get; set; }

    [CommandSwitch("--sample-name")]
    public string? SampleName { get; set; }

    [CommandSwitch("--service-level-objective")]
    public string? ServiceLevelObjective { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--umi")]
    public string? Umi { get; set; }

    [CommandSwitch("--yes")]
    public bool? Yes { get; set; } = true;

    [BooleanCommandSwitch("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}