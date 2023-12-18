using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aro", "create")]
public record AzAroCreateOptions(
[property: CommandSwitch("--master-subnet")] string MasterSubnet,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--worker-subnet")] string WorkerSubnet
) : AzOptions
{
    [CommandSwitch("--apiserver-visibility")]
    public string? ApiserverVisibility { get; set; }

    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--client-secret")]
    public string? ClientSecret { get; set; }

    [CommandSwitch("--cluster-resource-group")]
    public string? ClusterResourceGroup { get; set; }

    [CommandSwitch("--disk-encryption-set")]
    public string? DiskEncryptionSet { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [BooleanCommandSwitch("--enable-preconfigured-nsg")]
    public bool? EnablePreconfiguredNsg { get; set; }

    [BooleanCommandSwitch("--fips")]
    public bool? Fips { get; set; }

    [CommandSwitch("--ingress-visibility")]
    public string? IngressVisibility { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--master-enc-host")]
    public bool? MasterEncHost { get; set; }

    [CommandSwitch("--master-vm-size")]
    public string? MasterVmSize { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--outbound-type")]
    public string? OutboundType { get; set; }

    [CommandSwitch("--pod-cidr")]
    public string? PodCidr { get; set; }

    [CommandSwitch("--pull-secret")]
    public string? PullSecret { get; set; }

    [CommandSwitch("--service-cidr")]
    public string? ServiceCidr { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }

    [CommandSwitch("--vnet-resource-group")]
    public string? VnetResourceGroup { get; set; }

    [CommandSwitch("--worker-count")]
    public int? WorkerCount { get; set; }

    [BooleanCommandSwitch("--worker-enc-host")]
    public bool? WorkerEncHost { get; set; }

    [CommandSwitch("--worker-vm-disk-size-gb")]
    public string? WorkerVmDiskSizeGb { get; set; }

    [CommandSwitch("--worker-vm-size")]
    public string? WorkerVmSize { get; set; }
}