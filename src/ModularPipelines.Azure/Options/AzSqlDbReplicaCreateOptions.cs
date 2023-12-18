using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db", "replica", "create")]
public record AzSqlDbReplicaCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--partner-server")] string PartnerServer,
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

    [CommandSwitch("--compute-model")]
    public string? ComputeModel { get; set; }

    [CommandSwitch("--elastic-pool")]
    public string? ElasticPool { get; set; }

    [CommandSwitch("--encryption-protector")]
    public string? EncryptionProtector { get; set; }

    [BooleanCommandSwitch("--encryption-protector-auto-rotation")]
    public bool? EncryptionProtectorAutoRotation { get; set; }

    [CommandSwitch("--family")]
    public string? Family { get; set; }

    [CommandSwitch("--federated-client-id")]
    public string? FederatedClientId { get; set; }

    [CommandSwitch("--ha-replicas")]
    public string? HaReplicas { get; set; }

    [CommandSwitch("--keys")]
    public string? Keys { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--min-capacity")]
    public string? MinCapacity { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--partner-database")]
    public string? PartnerDatabase { get; set; }

    [CommandSwitch("--partner-resource-group")]
    public string? PartnerResourceGroup { get; set; }

    [CommandSwitch("--preferred-enclave-type")]
    public string? PreferredEnclaveType { get; set; }

    [CommandSwitch("--read-scale")]
    public string? ReadScale { get; set; }

    [CommandSwitch("--secondary-type")]
    public string? SecondaryType { get; set; }

    [CommandSwitch("--service-level-objective")]
    public string? ServiceLevelObjective { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--umi")]
    public string? Umi { get; set; }

    [BooleanCommandSwitch("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}