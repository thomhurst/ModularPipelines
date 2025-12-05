using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-templates", "create-with-container")]
public record GcloudComputeInstanceTemplatesCreateWithContainerOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--boot-disk-auto-delete")]
    public bool? BootDiskAutoDelete { get; set; }

    [CliOption("--boot-disk-device-name")]
    public string? BootDiskDeviceName { get; set; }

    [CliOption("--boot-disk-provisioned-iops")]
    public string? BootDiskProvisionedIops { get; set; }

    [CliOption("--boot-disk-provisioned-throughput")]
    public string? BootDiskProvisionedThroughput { get; set; }

    [CliOption("--boot-disk-size")]
    public string? BootDiskSize { get; set; }

    [CliOption("--boot-disk-type")]
    public string? BootDiskType { get; set; }

    [CliFlag("--can-ip-forward")]
    public bool? CanIpForward { get; set; }

    [CliFlag("--confidential-compute")]
    public bool? ConfidentialCompute { get; set; }

    [CliOption("--container-arg")]
    public string? ContainerArg { get; set; }

    [CliOption("--container-command")]
    public string? ContainerCommand { get; set; }

    [CliOption("--container-env")]
    public IEnumerable<KeyValue>? ContainerEnv { get; set; }

    [CliOption("--container-env-file")]
    public string? ContainerEnvFile { get; set; }

    [CliOption("--container-image")]
    public string? ContainerImage { get; set; }

    [CliOption("--container-mount-disk")]
    public string[]? ContainerMountDisk { get; set; }

    [CliOption("--container-mount-host-path")]
    public string[]? ContainerMountHostPath { get; set; }

    [CliOption("--container-mount-tmpfs")]
    public string[]? ContainerMountTmpfs { get; set; }

    [CliFlag("--container-privileged")]
    public bool? ContainerPrivileged { get; set; }

    [CliOption("--container-restart-policy")]
    public string? ContainerRestartPolicy { get; set; }

    [CliFlag("--container-stdin")]
    public bool? ContainerStdin { get; set; }

    [CliFlag("--container-tty")]
    public bool? ContainerTty { get; set; }

    [CliOption("--create-disk")]
    public string[]? CreateDisk { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--disk")]
    public string[]? Disk { get; set; }

    [CliOption("--external-ipv6-address")]
    public string? ExternalIpv6Address { get; set; }

    [CliOption("--external-ipv6-prefix-length")]
    public string? ExternalIpv6PrefixLength { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--maintenance-policy")]
    public string? MaintenancePolicy { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--metadata-from-file")]
    public string[]? MetadataFromFile { get; set; }

    [CliOption("--min-cpu-platform")]
    public string? MinCpuPlatform { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--network-interface")]
    public string[]? NetworkInterface { get; set; }

    [CliOption("--network-tier")]
    public string? NetworkTier { get; set; }

    [CliFlag("--preemptible")]
    public bool? Preemptible { get; set; }

    [CliOption("--private-ipv6-google-access-type")]
    public string? PrivateIpv6GoogleAccessType { get; set; }

    [CliOption("--private-network-ip")]
    public string? PrivateNetworkIp { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliFlag("--restart-on-failure")]
    public bool? RestartOnFailure { get; set; }

    [CliFlag("--shielded-integrity-monitoring")]
    public bool? ShieldedIntegrityMonitoring { get; set; }

    [CliFlag("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [CliFlag("--shielded-vtpm")]
    public bool? ShieldedVtpm { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--address")]
    public string? Address { get; set; }

    [CliFlag("--no-address")]
    public bool? NoAddress { get; set; }

    [CliOption("--custom-cpu")]
    public string? CustomCpu { get; set; }

    [CliOption("--custom-memory")]
    public string? CustomMemory { get; set; }

    [CliFlag("--custom-extensions")]
    public bool? CustomExtensions { get; set; }

    [CliOption("--custom-vm-type")]
    public string? CustomVmType { get; set; }

    [CliOption("--image-project")]
    public string? ImageProject { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--image-family")]
    public string? ImageFamily { get; set; }

    [CliOption("--scopes")]
    public string[]? Scopes { get; set; }

    [CliFlag("--no-scopes")]
    public bool? NoScopes { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliFlag("--no-service-account")]
    public bool? NoServiceAccount { get; set; }
}