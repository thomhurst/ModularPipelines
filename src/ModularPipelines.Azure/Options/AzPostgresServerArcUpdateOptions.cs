using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "server-arc", "update")]
public record AzPostgresServerArcUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
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

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}