using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notebooks", "instances", "create")]
public record GcloudNotebooksInstancesCreateOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--container-repository")] string ContainerRepository,
[property: CommandSwitch("--container-tag")] string ContainerTag,
[property: CommandSwitch("--environment")] string Environment,
[property: CommandSwitch("--environment-location")] string EnvironmentLocation,
[property: CommandSwitch("--vm-image-project")] string VmImageProject,
[property: CommandSwitch("--vm-image-family")] string VmImageFamily,
[property: CommandSwitch("--vm-image-name")] string VmImageName
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--instance-owners")]
    public string? InstanceOwners { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--post-startup-script")]
    public string? PostStartupScript { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [BooleanCommandSwitch("--shielded-integrity-monitoring")]
    public bool? ShieldedIntegrityMonitoring { get; set; }

    [BooleanCommandSwitch("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [BooleanCommandSwitch("--shielded-vtpm")]
    public bool? ShieldedVtpm { get; set; }

    [CommandSwitch("--accelerator-core-count")]
    public string? AcceleratorCoreCount { get; set; }

    [CommandSwitch("--accelerator-type")]
    public string? AcceleratorType { get; set; }

    [CommandSwitch("--boot-disk-size")]
    public string? BootDiskSize { get; set; }

    [CommandSwitch("--boot-disk-type")]
    public string? BootDiskType { get; set; }

    [CommandSwitch("--custom-gpu-driver-path")]
    public string? CustomGpuDriverPath { get; set; }

    [BooleanCommandSwitch("--install-gpu-driver")]
    public bool? InstallGpuDriver { get; set; }

    [CommandSwitch("--data-disk-size")]
    public string? DataDiskSize { get; set; }

    [CommandSwitch("--data-disk-type")]
    public string? DataDiskType { get; set; }

    [BooleanCommandSwitch("--no-remove-data-disk")]
    public bool? NoRemoveDataDisk { get; set; }

    [CommandSwitch("--disk-encryption")]
    public string? DiskEncryption { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [BooleanCommandSwitch("--no-proxy-access")]
    public bool? NoProxyAccess { get; set; }

    [BooleanCommandSwitch("--no-public-ip")]
    public bool? NoPublicIp { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subnet-region")]
    public string? SubnetRegion { get; set; }

    [CommandSwitch("--reservation")]
    public string? Reservation { get; set; }

    [CommandSwitch("--reservation-affinity")]
    public string? ReservationAffinity { get; set; }
}