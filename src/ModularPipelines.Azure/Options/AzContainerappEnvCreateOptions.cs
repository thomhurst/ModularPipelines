using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "env", "create")]
public record AzContainerappEnvCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--certificate-file")]
    public string? CertificateFile { get; set; }

    [CommandSwitch("--certificate-password")]
    public string? CertificatePassword { get; set; }

    [CommandSwitch("--custom-domain-dns-suffix")]
    public string? CustomDomainDnsSuffix { get; set; }

    [CommandSwitch("--dapr-instrumentation-key")]
    public string? DaprInstrumentationKey { get; set; }

    [CommandSwitch("--docker-bridge-cidr")]
    public string? DockerBridgeCidr { get; set; }

    [BooleanCommandSwitch("--enable-dedicated-gpu")]
    public bool? EnableDedicatedGpu { get; set; }

    [BooleanCommandSwitch("--enable-mtls")]
    public bool? EnableMtls { get; set; }

    [BooleanCommandSwitch("--enable-workload-profiles")]
    public bool? EnableWorkloadProfiles { get; set; }

    [CommandSwitch("--infrastructure-resource-group")]
    public string? InfrastructureResourceGroup { get; set; }

    [CommandSwitch("--infrastructure-subnet-resource-id")]
    public string? InfrastructureSubnetResourceId { get; set; }

    [BooleanCommandSwitch("--internal-only")]
    public bool? InternalOnly { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--logs-destination")]
    public string? LogsDestination { get; set; }

    [BooleanCommandSwitch("--logs-dynamic-json-columns")]
    public bool? LogsDynamicJsonColumns { get; set; }

    [CommandSwitch("--logs-workspace-id")]
    public string? LogsWorkspaceId { get; set; }

    [CommandSwitch("--logs-workspace-key")]
    public string? LogsWorkspaceKey { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--platform-reserved-cidr")]
    public string? PlatformReservedCidr { get; set; }

    [CommandSwitch("--platform-reserved-dns-ip")]
    public string? PlatformReservedDnsIp { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}