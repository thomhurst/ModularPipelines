using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workbench", "instances", "create")]
public record GcloudWorkbenchInstancesCreateOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--disable-proxy-access")]
    public bool? DisableProxyAccess { get; set; }

    [CliOption("--instance-owners")]
    public string? InstanceOwners { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliFlag("--disable-public-ip")]
    public bool? DisablePublicIp { get; set; }

    [CliFlag("--enable-ip-forwarding")]
    public bool? EnableIpForwarding { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--service-account-email")]
    public string? ServiceAccountEmail { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--accelerator-core-count")]
    public string? AcceleratorCoreCount { get; set; }

    [CliOption("--accelerator-type")]
    public string? AcceleratorType { get; set; }

    [CliOption("--boot-disk-encryption")]
    public string? BootDiskEncryption { get; set; }

    [CliOption("--boot-disk-size")]
    public string? BootDiskSize { get; set; }

    [CliOption("--boot-disk-type")]
    public string? BootDiskType { get; set; }

    [CliOption("--boot-disk-kms-key")]
    public string? BootDiskKmsKey { get; set; }

    [CliOption("--boot-disk-encryption-key-keyring")]
    public string? BootDiskEncryptionKeyKeyring { get; set; }

    [CliOption("--boot-disk-encryption-key-location")]
    public string? BootDiskEncryptionKeyLocation { get; set; }

    [CliOption("--boot-disk-encryption-key-project")]
    public string? BootDiskEncryptionKeyProject { get; set; }

    [CliOption("--container-repository")]
    public string? ContainerRepository { get; set; }

    [CliOption("--container-tag")]
    public string? ContainerTag { get; set; }

    [CliOption("--vm-image-project")]
    public string? VmImageProject { get; set; }

    [CliOption("--vm-image-family")]
    public string? VmImageFamily { get; set; }

    [CliOption("--vm-image-name")]
    public string? VmImageName { get; set; }

    [CliOption("--custom-gpu-driver-path")]
    public string? CustomGpuDriverPath { get; set; }

    [CliFlag("--install-gpu-driver")]
    public bool? InstallGpuDriver { get; set; }

    [CliOption("--data-disk-encryption")]
    public string? DataDiskEncryption { get; set; }

    [CliOption("--data-disk-size")]
    public string? DataDiskSize { get; set; }

    [CliOption("--data-disk-type")]
    public string? DataDiskType { get; set; }

    [CliOption("--data-disk-kms-key")]
    public string? DataDiskKmsKey { get; set; }

    [CliOption("--data-disk-encryption-key-keyring")]
    public string? DataDiskEncryptionKeyKeyring { get; set; }

    [CliOption("--data-disk-encryption-key-location")]
    public string? DataDiskEncryptionKeyLocation { get; set; }

    [CliOption("--data-disk-encryption-key-project")]
    public string? DataDiskEncryptionKeyProject { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--nic-type")]
    public string? NicType { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subnet-region")]
    public string? SubnetRegion { get; set; }

    [CliOption("--shielded-integrity-monitoring")]
    public string? ShieldedIntegrityMonitoring { get; set; }

    [CliOption("--shielded-secure-boot")]
    public string? ShieldedSecureBoot { get; set; }

    [CliOption("--shielded-vtpm")]
    public string? ShieldedVtpm { get; set; }
}