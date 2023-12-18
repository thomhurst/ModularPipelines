using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool", "add")]
public record AzAksNodepoolAddOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [BooleanCommandSwitch("--allowed-host-ports")]
    public bool? AllowedHostPorts { get; set; }

    [CommandSwitch("--asg-ids")]
    public string? AsgIds { get; set; }

    [CommandSwitch("--drain-timeout")]
    public string? DrainTimeout { get; set; }

    [BooleanCommandSwitch("--enable-cluster-autoscaler")]
    public bool? EnableClusterAutoscaler { get; set; }

    [BooleanCommandSwitch("--enable-encryption-at-host")]
    public bool? EnableEncryptionAtHost { get; set; }

    [BooleanCommandSwitch("--enable-fips-image")]
    public bool? EnableFipsImage { get; set; }

    [BooleanCommandSwitch("--enable-node-public-ip")]
    public bool? EnableNodePublicIp { get; set; }

    [BooleanCommandSwitch("--enable-ultra-ssd")]
    public bool? EnableUltraSsd { get; set; }

    [CommandSwitch("--eviction-policy")]
    public string? EvictionPolicy { get; set; }

    [CommandSwitch("--gpu-instance-profile")]
    public string? GpuInstanceProfile { get; set; }

    [CommandSwitch("--host-group-id")]
    public string? HostGroupId { get; set; }

    [CommandSwitch("--kubelet-config")]
    public string? KubeletConfig { get; set; }

    [CommandSwitch("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--linux-os-config")]
    public string? LinuxOsConfig { get; set; }

    [CommandSwitch("--max-count")]
    public int? MaxCount { get; set; }

    [CommandSwitch("--max-pods")]
    public string? MaxPods { get; set; }

    [CommandSwitch("--max-surge")]
    public string? MaxSurge { get; set; }

    [CommandSwitch("--min-count")]
    public int? MinCount { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-count")]
    public int? NodeCount { get; set; }

    [CommandSwitch("--node-osdisk-size")]
    public string? NodeOsdiskSize { get; set; }

    [CommandSwitch("--node-osdisk-type")]
    public string? NodeOsdiskType { get; set; }

    [CommandSwitch("--node-public-ip-prefix-id")]
    public string? NodePublicIpPrefixId { get; set; }

    [CommandSwitch("--node-taints")]
    public string? NodeTaints { get; set; }

    [CommandSwitch("--node-vm-size")]
    public string? NodeVmSize { get; set; }

    [CommandSwitch("--os-sku")]
    public string? OsSku { get; set; }

    [CommandSwitch("--os-type")]
    public string? OsType { get; set; }

    [CommandSwitch("--pod-subnet-id")]
    public string? PodSubnetId { get; set; }

    [CommandSwitch("--ppg")]
    public string? Ppg { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--scale-down-mode")]
    public string? ScaleDownMode { get; set; }

    [CommandSwitch("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CommandSwitch("--spot-max-price")]
    public string? SpotMaxPrice { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vnet-subnet-id")]
    public string? VnetSubnetId { get; set; }

    [CommandSwitch("--zones")]
    public string? Zones { get; set; }
}

