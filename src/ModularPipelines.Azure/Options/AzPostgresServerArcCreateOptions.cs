using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "server-arc", "create")]
public record AzPostgresServerArcCreateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--ad-account-name")]
    public int? AdAccountName { get; set; }

    [CommandSwitch("--ad-connector-name")]
    public string? AdConnectorName { get; set; }

    [CommandSwitch("--admin-login-secret")]
    public string? AdminLoginSecret { get; set; }

    [CommandSwitch("--cert-private-key-file")]
    public string? CertPrivateKeyFile { get; set; }

    [CommandSwitch("--cert-public-key-file")]
    public string? CertPublicKeyFile { get; set; }

    [CommandSwitch("--cores-limit")]
    public string? CoresLimit { get; set; }

    [CommandSwitch("--cores-request")]
    public string? CoresRequest { get; set; }

    [CommandSwitch("--dev")]
    public string? Dev { get; set; }

    [CommandSwitch("--dns-name")]
    public string? DnsName { get; set; }

    [CommandSwitch("--extensions")]
    public string? Extensions { get; set; }

    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CommandSwitch("--keytab-secret")]
    public string? KeytabSecret { get; set; }

    [CommandSwitch("--log-level")]
    public string? LogLevel { get; set; }

    [CommandSwitch("--memory-limit")]
    public string? MemoryLimit { get; set; }

    [CommandSwitch("--memory-request")]
    public string? MemoryRequest { get; set; }

    [BooleanCommandSwitch("--no-external-endpoint")]
    public bool? NoExternalEndpoint { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--retention-days")]
    public string? RetentionDays { get; set; }

    [CommandSwitch("--service-annotations")]
    public string? ServiceAnnotations { get; set; }

    [CommandSwitch("--service-cert-secret")]
    public string? ServiceCertSecret { get; set; }

    [CommandSwitch("--service-labels")]
    public string? ServiceLabels { get; set; }

    [CommandSwitch("--service-type")]
    public string? ServiceType { get; set; }

    [CommandSwitch("--storage-class-backups")]
    public string? StorageClassBackups { get; set; }

    [CommandSwitch("--storage-class-data")]
    public string? StorageClassData { get; set; }

    [CommandSwitch("--storage-class-logs")]
    public string? StorageClassLogs { get; set; }

    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }

    [CommandSwitch("--volume-size-backups")]
    public string? VolumeSizeBackups { get; set; }

    [CommandSwitch("--volume-size-data")]
    public string? VolumeSizeData { get; set; }

    [CommandSwitch("--volume-size-logs")]
    public string? VolumeSizeLogs { get; set; }
}

