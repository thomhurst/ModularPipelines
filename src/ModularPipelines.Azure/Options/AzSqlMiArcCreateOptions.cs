using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "mi-arc", "create")]
public record AzSqlMiArcCreateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--ad-account-name")]
    public int? AdAccountName { get; set; }

    [CliOption("--ad-connector-name")]
    public string? AdConnectorName { get; set; }

    [CliOption("--ad-encryption-types")]
    public string? AdEncryptionTypes { get; set; }

    [CliOption("--admin-login-secret")]
    public string? AdminLoginSecret { get; set; }

    [CliFlag("--agent-enabled")]
    public bool? AgentEnabled { get; set; }

    [CliOption("--annotations")]
    public string? Annotations { get; set; }

    [CliOption("--cert-private-key-file")]
    public string? CertPrivateKeyFile { get; set; }

    [CliOption("--cert-public-key-file")]
    public string? CertPublicKeyFile { get; set; }

    [CliOption("--collation")]
    public string? Collation { get; set; }

    [CliOption("--cores-limit")]
    public string? CoresLimit { get; set; }

    [CliOption("--cores-request")]
    public string? CoresRequest { get; set; }

    [CliOption("--custom-location")]
    public string? CustomLocation { get; set; }

    [CliOption("--dev")]
    public string? Dev { get; set; }

    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliOption("--keytab-secret")]
    public string? KeytabSecret { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--memory-limit")]
    public string? MemoryLimit { get; set; }

    [CliOption("--memory-request")]
    public string? MemoryRequest { get; set; }

    [CliFlag("--no-external-endpoint")]
    public bool? NoExternalEndpoint { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--orchestrator-replicas")]
    public string? OrchestratorReplicas { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--primary-dns-name")]
    public string? PrimaryDnsName { get; set; }

    [CliOption("--primary-port-number")]
    public string? PrimaryPortNumber { get; set; }

    [CliOption("--readable-secondaries")]
    public string? ReadableSecondaries { get; set; }

    [CliOption("--replicas")]
    public string? Replicas { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--retention-days")]
    public string? RetentionDays { get; set; }

    [CliOption("--secondary-dns-name")]
    public string? SecondaryDnsName { get; set; }

    [CliOption("--secondary-port-number")]
    public string? SecondaryPortNumber { get; set; }

    [CliOption("--service-annotations")]
    public string? ServiceAnnotations { get; set; }

    [CliOption("--service-cert-secret")]
    public string? ServiceCertSecret { get; set; }

    [CliOption("--service-labels")]
    public string? ServiceLabels { get; set; }

    [CliOption("--service-type")]
    public string? ServiceType { get; set; }

    [CliOption("--storage-annotations")]
    public string? StorageAnnotations { get; set; }

    [CliOption("--storage-class-backups")]
    public string? StorageClassBackups { get; set; }

    [CliOption("--storage-class-data")]
    public string? StorageClassData { get; set; }

    [CliOption("--storage-class-datalogs")]
    public string? StorageClassDatalogs { get; set; }

    [CliOption("--storage-class-logs")]
    public string? StorageClassLogs { get; set; }

    [CliOption("--storage-class-orchestrator-logs")]
    public string? StorageClassOrchestratorLogs { get; set; }

    [CliOption("--storage-labels")]
    public string? StorageLabels { get; set; }

    [CliOption("--sync-secondary-to-commit")]
    public string? SyncSecondaryToCommit { get; set; }

    [CliOption("--tde-mode")]
    public string? TdeMode { get; set; }

    [CliOption("--tde-protector-private-key-file")]
    public string? TdeProtectorPrivateKeyFile { get; set; }

    [CliOption("--tde-protector-public-key-file")]
    public string? TdeProtectorPublicKeyFile { get; set; }

    [CliOption("--tde-protector-secret")]
    public string? TdeProtectorSecret { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--time-zone")]
    public string? TimeZone { get; set; }

    [CliOption("--trace-flags")]
    public string? TraceFlags { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }

    [CliOption("--volume-size-backups")]
    public string? VolumeSizeBackups { get; set; }

    [CliOption("--volume-size-data")]
    public string? VolumeSizeData { get; set; }

    [CliOption("--volume-size-datalogs")]
    public string? VolumeSizeDatalogs { get; set; }

    [CliOption("--volume-size-logs")]
    public string? VolumeSizeLogs { get; set; }

    [CliOption("--volume-size-orchestrator-logs")]
    public string? VolumeSizeOrchestratorLogs { get; set; }
}