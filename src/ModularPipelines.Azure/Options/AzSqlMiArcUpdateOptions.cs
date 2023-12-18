using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi-arc", "update")]
public record AzSqlMiArcUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--ad-encryption-types")]
    public string? AdEncryptionTypes { get; set; }

    [BooleanCommandSwitch("--agent-enabled")]
    public bool? AgentEnabled { get; set; }

    [CommandSwitch("--annotations")]
    public string? Annotations { get; set; }

    [CommandSwitch("--cert-private-key-file")]
    public string? CertPrivateKeyFile { get; set; }

    [CommandSwitch("--cert-public-key-file")]
    public string? CertPublicKeyFile { get; set; }

    [CommandSwitch("--cores-limit")]
    public string? CoresLimit { get; set; }

    [CommandSwitch("--cores-request")]
    public string? CoresRequest { get; set; }

    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CommandSwitch("--keytab-secret")]
    public string? KeytabSecret { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--memory-limit")]
    public string? MemoryLimit { get; set; }

    [CommandSwitch("--memory-request")]
    public string? MemoryRequest { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--orchestrator-replicas")]
    public string? OrchestratorReplicas { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--preferred-primary-replica")]
    public string? PreferredPrimaryReplica { get; set; }

    [CommandSwitch("--readable-secondaries")]
    public string? ReadableSecondaries { get; set; }

    [CommandSwitch("--replicas")]
    public string? Replicas { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--retention-days")]
    public string? RetentionDays { get; set; }

    [CommandSwitch("--service-annotations")]
    public string? ServiceAnnotations { get; set; }

    [CommandSwitch("--service-cert-secret")]
    public string? ServiceCertSecret { get; set; }

    [CommandSwitch("--service-labels")]
    public string? ServiceLabels { get; set; }

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

    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}