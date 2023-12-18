using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi-arc", "create")]
public record AzSqlMiArcCreateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--ad-account-name")]
    public int? AdAccountName { get; set; }

    [CommandSwitch("--ad-connector-name")]
    public string? AdConnectorName { get; set; }

    [CommandSwitch("--ad-encryption-types")]
    public string? AdEncryptionTypes { get; set; }

    [CommandSwitch("--admin-login-secret")]
    public string? AdminLoginSecret { get; set; }

    [BooleanCommandSwitch("--agent-enabled")]
    public bool? AgentEnabled { get; set; }

    [CommandSwitch("--annotations")]
    public string? Annotations { get; set; }

    [CommandSwitch("--cert-private-key-file")]
    public string? CertPrivateKeyFile { get; set; }

    [CommandSwitch("--cert-public-key-file")]
    public string? CertPublicKeyFile { get; set; }

    [CommandSwitch("--collation")]
    public string? Collation { get; set; }

    [CommandSwitch("--cores-limit")]
    public string? CoresLimit { get; set; }

    [CommandSwitch("--cores-request")]
    public string? CoresRequest { get; set; }

    [CommandSwitch("--custom-location")]
    public string? CustomLocation { get; set; }

    [CommandSwitch("--dev")]
    public string? Dev { get; set; }

    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CommandSwitch("--keytab-secret")]
    public string? KeytabSecret { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--memory-limit")]
    public string? MemoryLimit { get; set; }

    [CommandSwitch("--memory-request")]
    public string? MemoryRequest { get; set; }

    [BooleanCommandSwitch("--no-external-endpoint")]
    public bool? NoExternalEndpoint { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--orchestrator-replicas")]
    public string? OrchestratorReplicas { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--primary-dns-name")]
    public string? PrimaryDnsName { get; set; }

    [CommandSwitch("--primary-port-number")]
    public string? PrimaryPortNumber { get; set; }

    [CommandSwitch("--readable-secondaries")]
    public string? ReadableSecondaries { get; set; }

    [CommandSwitch("--replicas")]
    public string? Replicas { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--retention-days")]
    public string? RetentionDays { get; set; }

    [CommandSwitch("--secondary-dns-name")]
    public string? SecondaryDnsName { get; set; }

    [CommandSwitch("--secondary-port-number")]
    public string? SecondaryPortNumber { get; set; }

    [CommandSwitch("--service-annotations")]
    public string? ServiceAnnotations { get; set; }

    [CommandSwitch("--service-cert-secret")]
    public string? ServiceCertSecret { get; set; }

    [CommandSwitch("--service-labels")]
    public string? ServiceLabels { get; set; }

    [CommandSwitch("--service-type")]
    public string? ServiceType { get; set; }

    [CommandSwitch("--storage-annotations")]
    public string? StorageAnnotations { get; set; }

    [CommandSwitch("--storage-class-backups")]
    public string? StorageClassBackups { get; set; }

    [CommandSwitch("--storage-class-data")]
    public string? StorageClassData { get; set; }

    [CommandSwitch("--storage-class-datalogs")]
    public string? StorageClassDatalogs { get; set; }

    [CommandSwitch("--storage-class-logs")]
    public string? StorageClassLogs { get; set; }

    [CommandSwitch("--storage-class-orchestrator-logs")]
    public string? StorageClassOrchestratorLogs { get; set; }

    [CommandSwitch("--storage-labels")]
    public string? StorageLabels { get; set; }

    [CommandSwitch("--sync-secondary-to-commit")]
    public string? SyncSecondaryToCommit { get; set; }

    [CommandSwitch("--tde-mode")]
    public string? TdeMode { get; set; }

    [CommandSwitch("--tde-protector-private-key-file")]
    public string? TdeProtectorPrivateKeyFile { get; set; }

    [CommandSwitch("--tde-protector-public-key-file")]
    public string? TdeProtectorPublicKeyFile { get; set; }

    [CommandSwitch("--tde-protector-secret")]
    public string? TdeProtectorSecret { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }

    [CommandSwitch("--trace-flags")]
    public string? TraceFlags { get; set; }

    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }

    [CommandSwitch("--volume-size-backups")]
    public string? VolumeSizeBackups { get; set; }

    [CommandSwitch("--volume-size-data")]
    public string? VolumeSizeData { get; set; }

    [CommandSwitch("--volume-size-datalogs")]
    public string? VolumeSizeDatalogs { get; set; }

    [CommandSwitch("--volume-size-logs")]
    public string? VolumeSizeLogs { get; set; }

    [CommandSwitch("--volume-size-orchestrator-logs")]
    public string? VolumeSizeOrchestratorLogs { get; set; }
}