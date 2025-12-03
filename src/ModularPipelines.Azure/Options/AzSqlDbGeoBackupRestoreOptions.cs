using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "db", "geo-backup", "restore")]
public record AzSqlDbGeoBackupRestoreOptions(
[property: CliOption("--dest-database")] string DestDatabase,
[property: CliOption("--dest-server")] string DestServer,
[property: CliOption("--geo-backup-id")] string GeoBackupId,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CliOption("--auto-pause-delay")]
    public string? AutoPauseDelay { get; set; }

    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

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

    [CliOption("--family")]
    public string? Family { get; set; }

    [CliOption("--federated-client-id")]
    public string? FederatedClientId { get; set; }

    [CliOption("--ha-replicas")]
    public string? HaReplicas { get; set; }

    [CliOption("--keys")]
    public string? Keys { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--min-capacity")]
    public string? MinCapacity { get; set; }

    [CliOption("--preferred-enclave-type")]
    public string? PreferredEnclaveType { get; set; }

    [CliOption("--read-scale")]
    public string? ReadScale { get; set; }

    [CliOption("--service-level-objective")]
    public string? ServiceLevelObjective { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--umi")]
    public string? Umi { get; set; }

    [CliFlag("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}