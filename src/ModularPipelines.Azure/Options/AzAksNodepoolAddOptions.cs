using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "nodepool", "add")]
public record AzAksNodepoolAddOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CliFlag("--allowed-host-ports")]
    public bool? AllowedHostPorts { get; set; }

    [CliOption("--asg-ids")]
    public string? AsgIds { get; set; }

    [CliOption("--drain-timeout")]
    public string? DrainTimeout { get; set; }

    [CliFlag("--enable-cluster-autoscaler")]
    public bool? EnableClusterAutoscaler { get; set; }

    [CliFlag("--enable-encryption-at-host")]
    public bool? EnableEncryptionAtHost { get; set; }

    [CliFlag("--enable-fips-image")]
    public bool? EnableFipsImage { get; set; }

    [CliFlag("--enable-node-public-ip")]
    public bool? EnableNodePublicIp { get; set; }

    [CliFlag("--enable-ultra-ssd")]
    public bool? EnableUltraSsd { get; set; }

    [CliOption("--eviction-policy")]
    public string? EvictionPolicy { get; set; }

    [CliOption("--gpu-instance-profile")]
    public string? GpuInstanceProfile { get; set; }

    [CliOption("--host-group-id")]
    public string? HostGroupId { get; set; }

    [CliOption("--kubelet-config")]
    public string? KubeletConfig { get; set; }

    [CliOption("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--linux-os-config")]
    public string? LinuxOsConfig { get; set; }

    [CliOption("--max-count")]
    public int? MaxCount { get; set; }

    [CliOption("--max-pods")]
    public string? MaxPods { get; set; }

    [CliOption("--max-surge")]
    public string? MaxSurge { get; set; }

    [CliOption("--min-count")]
    public int? MinCount { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-count")]
    public int? NodeCount { get; set; }

    [CliOption("--node-osdisk-size")]
    public string? NodeOsdiskSize { get; set; }

    [CliOption("--node-osdisk-type")]
    public string? NodeOsdiskType { get; set; }

    [CliOption("--node-public-ip-prefix-id")]
    public string? NodePublicIpPrefixId { get; set; }

    [CliOption("--node-taints")]
    public string? NodeTaints { get; set; }

    [CliOption("--node-vm-size")]
    public string? NodeVmSize { get; set; }

    [CliOption("--os-sku")]
    public string? OsSku { get; set; }

    [CliOption("--os-type")]
    public string? OsType { get; set; }

    [CliOption("--pod-subnet-id")]
    public string? PodSubnetId { get; set; }

    [CliOption("--ppg")]
    public string? Ppg { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--scale-down-mode")]
    public string? ScaleDownMode { get; set; }

    [CliOption("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CliOption("--spot-max-price")]
    public string? SpotMaxPrice { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vnet-subnet-id")]
    public string? VnetSubnetId { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}