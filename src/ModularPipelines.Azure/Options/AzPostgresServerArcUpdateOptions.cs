using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "server-arc", "update")]
public record AzPostgresServerArcUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
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

    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}