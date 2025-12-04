using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "env", "update")]
public record AzContainerappEnvUpdateOptions : AzOptions
{
    [CliOption("--certificate-file")]
    public string? CertificateFile { get; set; }

    [CliOption("--certificate-password")]
    public string? CertificatePassword { get; set; }

    [CliOption("--custom-domain-dns-suffix")]
    public string? CustomDomainDnsSuffix { get; set; }

    [CliFlag("--enable-mtls")]
    public bool? EnableMtls { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--logs-destination")]
    public string? LogsDestination { get; set; }

    [CliFlag("--logs-dynamic-json-columns")]
    public bool? LogsDynamicJsonColumns { get; set; }

    [CliOption("--logs-workspace-id")]
    public string? LogsWorkspaceId { get; set; }

    [CliOption("--logs-workspace-key")]
    public string? LogsWorkspaceKey { get; set; }

    [CliOption("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CliOption("--min-nodes")]
    public string? MinNodes { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--workload-profile-name")]
    public string? WorkloadProfileName { get; set; }

    [CliOption("--workload-profile-type")]
    public string? WorkloadProfileType { get; set; }
}