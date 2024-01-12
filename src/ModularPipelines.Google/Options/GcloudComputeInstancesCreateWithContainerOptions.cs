using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "create-with-container")]
public record GcloudComputeInstancesCreateWithContainerOptions(
[property: PositionalArgument] string InstanceNames
) : GcloudOptions
{
    [CommandSwitch("--accelerator")]
    public string[]? Accelerator { get; set; }

    [BooleanCommandSwitch("--boot-disk-auto-delete")]
    public bool? BootDiskAutoDelete { get; set; }

    [CommandSwitch("--boot-disk-device-name")]
    public string? BootDiskDeviceName { get; set; }

    [CommandSwitch("--boot-disk-provisioned-iops")]
    public string? BootDiskProvisionedIops { get; set; }

    [CommandSwitch("--boot-disk-provisioned-throughput")]
    public string? BootDiskProvisionedThroughput { get; set; }

    [CommandSwitch("--boot-disk-size")]
    public string? BootDiskSize { get; set; }

    [CommandSwitch("--boot-disk-type")]
    public string? BootDiskType { get; set; }

    [BooleanCommandSwitch("--can-ip-forward")]
    public bool? CanIpForward { get; set; }

    [BooleanCommandSwitch("--confidential-compute")]
    public bool? ConfidentialCompute { get; set; }

    [CommandSwitch("--container-arg")]
    public string? ContainerArg { get; set; }

    [CommandSwitch("--container-command")]
    public string? ContainerCommand { get; set; }

    [CommandSwitch("--container-env")]
    public IEnumerable<KeyValue>? ContainerEnv { get; set; }

    [CommandSwitch("--container-env-file")]
    public string? ContainerEnvFile { get; set; }

    [CommandSwitch("--container-image")]
    public string? ContainerImage { get; set; }

    [CommandSwitch("--container-mount-disk")]
    public string[]? ContainerMountDisk { get; set; }

    [CommandSwitch("--container-mount-host-path")]
    public string[]? ContainerMountHostPath { get; set; }

    [CommandSwitch("--container-mount-tmpfs")]
    public string[]? ContainerMountTmpfs { get; set; }

    [BooleanCommandSwitch("--container-privileged")]
    public bool? ContainerPrivileged { get; set; }

    [CommandSwitch("--container-restart-policy")]
    public string? ContainerRestartPolicy { get; set; }

    [BooleanCommandSwitch("--container-stdin")]
    public bool? ContainerStdin { get; set; }

    [BooleanCommandSwitch("--container-tty")]
    public bool? ContainerTty { get; set; }

    [CommandSwitch("--create-disk")]
    public string[]? CreateDisk { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--disk")]
    public string[]? Disk { get; set; }

    [CommandSwitch("--[no-]enable-nested-virtualization")]
    public string[]? NoEnableNestedVirtualization { get; set; }

    [CommandSwitch("--external-ipv6-address")]
    public string? ExternalIpv6Address { get; set; }

    [CommandSwitch("--external-ipv6-prefix-length")]
    public string? ExternalIpv6PrefixLength { get; set; }

    [CommandSwitch("--instance-termination-action")]
    public string? InstanceTerminationAction { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--local-ssd-recovery-timeout")]
    public string? LocalSsdRecoveryTimeout { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--maintenance-policy")]
    public string? MaintenancePolicy { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--metadata-from-file")]
    public string[]? MetadataFromFile { get; set; }

    [CommandSwitch("--min-cpu-platform")]
    public string? MinCpuPlatform { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--network-interface")]
    public string[]? NetworkInterface { get; set; }

    [CommandSwitch("--network-performance-configs")]
    public string[]? NetworkPerformanceConfigs { get; set; }

    [CommandSwitch("--network-tier")]
    public string? NetworkTier { get; set; }

    [BooleanCommandSwitch("--preemptible")]
    public bool? Preemptible { get; set; }

    [CommandSwitch("--private-ipv6-google-access-type")]
    public string? PrivateIpv6GoogleAccessType { get; set; }

    [CommandSwitch("--private-network-ip")]
    public string? PrivateNetworkIp { get; set; }

    [CommandSwitch("--provisioning-model")]
    public string? ProvisioningModel { get; set; }

    [BooleanCommandSwitch("--restart-on-failure")]
    public bool? RestartOnFailure { get; set; }

    [BooleanCommandSwitch("--shielded-integrity-monitoring")]
    public bool? ShieldedIntegrityMonitoring { get; set; }

    [BooleanCommandSwitch("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [BooleanCommandSwitch("--shielded-vtpm")]
    public bool? ShieldedVtpm { get; set; }

    [CommandSwitch("--source-instance-template")]
    public string? SourceInstanceTemplate { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--threads-per-core")]
    public string? ThreadsPerCore { get; set; }

    [CommandSwitch("--visible-core-count")]
    public string? VisibleCoreCount { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [BooleanCommandSwitch("--no-address")]
    public bool? NoAddress { get; set; }

    [CommandSwitch("--custom-cpu")]
    public string? CustomCpu { get; set; }

    [CommandSwitch("--custom-memory")]
    public string? CustomMemory { get; set; }

    [BooleanCommandSwitch("--custom-extensions")]
    public bool? CustomExtensions { get; set; }

    [CommandSwitch("--custom-vm-type")]
    public string? CustomVmType { get; set; }

    [CommandSwitch("--image-project")]
    public string? ImageProject { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--image-family")]
    public string? ImageFamily { get; set; }

    [BooleanCommandSwitch("--public-ptr")]
    public bool? PublicPtr { get; set; }

    [BooleanCommandSwitch("--no-public-ptr")]
    public bool? NoPublicPtr { get; set; }

    [CommandSwitch("--public-ptr-domain")]
    public string? PublicPtrDomain { get; set; }

    [BooleanCommandSwitch("--no-public-ptr-domain")]
    public bool? NoPublicPtrDomain { get; set; }

    [CommandSwitch("--scopes")]
    public string[]? Scopes { get; set; }

    [BooleanCommandSwitch("--no-scopes")]
    public bool? NoScopes { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [BooleanCommandSwitch("--no-service-account")]
    public bool? NoServiceAccount { get; set; }
}