using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "env", "create")]
public record AzContainerappEnvCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--certificate-file")]
    public string? CertificateFile { get; set; }

    [CliOption("--certificate-password")]
    public string? CertificatePassword { get; set; }

    [CliOption("--custom-domain-dns-suffix")]
    public string? CustomDomainDnsSuffix { get; set; }

    [CliOption("--dapr-instrumentation-key")]
    public string? DaprInstrumentationKey { get; set; }

    [CliOption("--docker-bridge-cidr")]
    public string? DockerBridgeCidr { get; set; }

    [CliFlag("--enable-dedicated-gpu")]
    public bool? EnableDedicatedGpu { get; set; }

    [CliFlag("--enable-mtls")]
    public bool? EnableMtls { get; set; }

    [CliFlag("--enable-workload-profiles")]
    public bool? EnableWorkloadProfiles { get; set; }

    [CliOption("--infrastructure-resource-group")]
    public string? InfrastructureResourceGroup { get; set; }

    [CliOption("--infrastructure-subnet-resource-id")]
    public string? InfrastructureSubnetResourceId { get; set; }

    [CliFlag("--internal-only")]
    public bool? InternalOnly { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--logs-destination")]
    public string? LogsDestination { get; set; }

    [CliFlag("--logs-dynamic-json-columns")]
    public bool? LogsDynamicJsonColumns { get; set; }

    [CliOption("--logs-workspace-id")]
    public string? LogsWorkspaceId { get; set; }

    [CliOption("--logs-workspace-key")]
    public string? LogsWorkspaceKey { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--platform-reserved-cidr")]
    public string? PlatformReservedCidr { get; set; }

    [CliOption("--platform-reserved-dns-ip")]
    public string? PlatformReservedDnsIp { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}