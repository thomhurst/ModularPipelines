using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "server-arc", "create")]
public record AzPostgresServerArcCreateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--ad-account-name")]
    public int? AdAccountName { get; set; }

    [CliOption("--ad-connector-name")]
    public string? AdConnectorName { get; set; }

    [CliOption("--admin-login-secret")]
    public string? AdminLoginSecret { get; set; }

    [CliOption("--cert-private-key-file")]
    public string? CertPrivateKeyFile { get; set; }

    [CliOption("--cert-public-key-file")]
    public string? CertPublicKeyFile { get; set; }

    [CliOption("--cores-limit")]
    public string? CoresLimit { get; set; }

    [CliOption("--cores-request")]
    public string? CoresRequest { get; set; }

    [CliOption("--dev")]
    public string? Dev { get; set; }

    [CliOption("--dns-name")]
    public string? DnsName { get; set; }

    [CliOption("--extensions")]
    public string? Extensions { get; set; }

    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliOption("--keytab-secret")]
    public string? KeytabSecret { get; set; }

    [CliOption("--log-level")]
    public string? LogLevel { get; set; }

    [CliOption("--memory-limit")]
    public string? MemoryLimit { get; set; }

    [CliOption("--memory-request")]
    public string? MemoryRequest { get; set; }

    [CliFlag("--no-external-endpoint")]
    public bool? NoExternalEndpoint { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--retention-days")]
    public string? RetentionDays { get; set; }

    [CliOption("--service-annotations")]
    public string? ServiceAnnotations { get; set; }

    [CliOption("--service-cert-secret")]
    public string? ServiceCertSecret { get; set; }

    [CliOption("--service-labels")]
    public string? ServiceLabels { get; set; }

    [CliOption("--service-type")]
    public string? ServiceType { get; set; }

    [CliOption("--storage-class-backups")]
    public string? StorageClassBackups { get; set; }

    [CliOption("--storage-class-data")]
    public string? StorageClassData { get; set; }

    [CliOption("--storage-class-logs")]
    public string? StorageClassLogs { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }

    [CliOption("--volume-size-backups")]
    public string? VolumeSizeBackups { get; set; }

    [CliOption("--volume-size-data")]
    public string? VolumeSizeData { get; set; }

    [CliOption("--volume-size-logs")]
    public string? VolumeSizeLogs { get; set; }
}