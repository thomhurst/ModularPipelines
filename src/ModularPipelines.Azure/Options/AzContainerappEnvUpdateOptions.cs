using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "env", "update")]
public record AzContainerappEnvUpdateOptions : AzOptions
{
    [CommandSwitch("--certificate-file")]
    public string? CertificateFile { get; set; }

    [CommandSwitch("--certificate-password")]
    public string? CertificatePassword { get; set; }

    [CommandSwitch("--custom-domain-dns-suffix")]
    public string? CustomDomainDnsSuffix { get; set; }

    [BooleanCommandSwitch("--enable-mtls")]
    public bool? EnableMtls { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--logs-destination")]
    public string? LogsDestination { get; set; }

    [BooleanCommandSwitch("--logs-dynamic-json-columns")]
    public bool? LogsDynamicJsonColumns { get; set; }

    [CommandSwitch("--logs-workspace-id")]
    public string? LogsWorkspaceId { get; set; }

    [CommandSwitch("--logs-workspace-key")]
    public string? LogsWorkspaceKey { get; set; }

    [CommandSwitch("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CommandSwitch("--min-nodes")]
    public string? MinNodes { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--workload-profile-name")]
    public string? WorkloadProfileName { get; set; }

    [CommandSwitch("--workload-profile-type")]
    public string? WorkloadProfileType { get; set; }
}