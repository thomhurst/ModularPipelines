using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aro", "create")]
public record AzAroCreateOptions(
[property: CliOption("--master-subnet")] string MasterSubnet,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--worker-subnet")] string WorkerSubnet
) : AzOptions
{
    [CliOption("--apiserver-visibility")]
    public string? ApiserverVisibility { get; set; }

    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--client-secret")]
    public string? ClientSecret { get; set; }

    [CliOption("--cluster-resource-group")]
    public string? ClusterResourceGroup { get; set; }

    [CliOption("--disk-encryption-set")]
    public string? DiskEncryptionSet { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliFlag("--enable-preconfigured-nsg")]
    public bool? EnablePreconfiguredNsg { get; set; }

    [CliFlag("--fips")]
    public bool? Fips { get; set; }

    [CliOption("--ingress-visibility")]
    public string? IngressVisibility { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--master-enc-host")]
    public bool? MasterEncHost { get; set; }

    [CliOption("--master-vm-size")]
    public string? MasterVmSize { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--outbound-type")]
    public string? OutboundType { get; set; }

    [CliOption("--pod-cidr")]
    public string? PodCidr { get; set; }

    [CliOption("--pull-secret")]
    public string? PullSecret { get; set; }

    [CliOption("--service-cidr")]
    public string? ServiceCidr { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }

    [CliOption("--vnet-resource-group")]
    public string? VnetResourceGroup { get; set; }

    [CliOption("--worker-count")]
    public int? WorkerCount { get; set; }

    [CliFlag("--worker-enc-host")]
    public bool? WorkerEncHost { get; set; }

    [CliOption("--worker-vm-disk-size-gb")]
    public string? WorkerVmDiskSizeGb { get; set; }

    [CliOption("--worker-vm-size")]
    public string? WorkerVmSize { get; set; }
}