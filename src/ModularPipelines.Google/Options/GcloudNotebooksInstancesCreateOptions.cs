using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "instances", "create")]
public record GcloudNotebooksInstancesCreateOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location,
[property: CliOption("--container-repository")] string ContainerRepository,
[property: CliOption("--container-tag")] string ContainerTag,
[property: CliOption("--environment")] string Environment,
[property: CliOption("--environment-location")] string EnvironmentLocation,
[property: CliOption("--vm-image-project")] string VmImageProject,
[property: CliOption("--vm-image-family")] string VmImageFamily,
[property: CliOption("--vm-image-name")] string VmImageName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--instance-owners")]
    public string? InstanceOwners { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--post-startup-script")]
    public string? PostStartupScript { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliFlag("--shielded-integrity-monitoring")]
    public bool? ShieldedIntegrityMonitoring { get; set; }

    [CliFlag("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [CliFlag("--shielded-vtpm")]
    public bool? ShieldedVtpm { get; set; }

    [CliOption("--accelerator-core-count")]
    public string? AcceleratorCoreCount { get; set; }

    [CliOption("--accelerator-type")]
    public string? AcceleratorType { get; set; }

    [CliOption("--boot-disk-size")]
    public string? BootDiskSize { get; set; }

    [CliOption("--boot-disk-type")]
    public string? BootDiskType { get; set; }

    [CliOption("--custom-gpu-driver-path")]
    public string? CustomGpuDriverPath { get; set; }

    [CliFlag("--install-gpu-driver")]
    public bool? InstallGpuDriver { get; set; }

    [CliOption("--data-disk-size")]
    public string? DataDiskSize { get; set; }

    [CliOption("--data-disk-type")]
    public string? DataDiskType { get; set; }

    [CliFlag("--no-remove-data-disk")]
    public bool? NoRemoveDataDisk { get; set; }

    [CliOption("--disk-encryption")]
    public string? DiskEncryption { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliFlag("--no-proxy-access")]
    public bool? NoProxyAccess { get; set; }

    [CliFlag("--no-public-ip")]
    public bool? NoPublicIp { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subnet-region")]
    public string? SubnetRegion { get; set; }

    [CliOption("--reservation")]
    public string? Reservation { get; set; }

    [CliOption("--reservation-affinity")]
    public string? ReservationAffinity { get; set; }
}