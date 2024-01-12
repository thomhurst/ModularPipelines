using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workbench", "instances", "create")]
public record GcloudWorkbenchInstancesCreateOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--disable-proxy-access")]
    public bool? DisableProxyAccess { get; set; }

    [CommandSwitch("--instance-owners")]
    public string? InstanceOwners { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [BooleanCommandSwitch("--disable-public-ip")]
    public bool? DisablePublicIp { get; set; }

    [BooleanCommandSwitch("--enable-ip-forwarding")]
    public bool? EnableIpForwarding { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--service-account-email")]
    public string? ServiceAccountEmail { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--accelerator-core-count")]
    public string? AcceleratorCoreCount { get; set; }

    [CommandSwitch("--accelerator-type")]
    public string? AcceleratorType { get; set; }

    [CommandSwitch("--boot-disk-encryption")]
    public string? BootDiskEncryption { get; set; }

    [CommandSwitch("--boot-disk-size")]
    public string? BootDiskSize { get; set; }

    [CommandSwitch("--boot-disk-type")]
    public string? BootDiskType { get; set; }

    [CommandSwitch("--boot-disk-kms-key")]
    public string? BootDiskKmsKey { get; set; }

    [CommandSwitch("--boot-disk-encryption-key-keyring")]
    public string? BootDiskEncryptionKeyKeyring { get; set; }

    [CommandSwitch("--boot-disk-encryption-key-location")]
    public string? BootDiskEncryptionKeyLocation { get; set; }

    [CommandSwitch("--boot-disk-encryption-key-project")]
    public string? BootDiskEncryptionKeyProject { get; set; }

    [CommandSwitch("--container-repository")]
    public string? ContainerRepository { get; set; }

    [CommandSwitch("--container-tag")]
    public string? ContainerTag { get; set; }

    [CommandSwitch("--vm-image-project")]
    public string? VmImageProject { get; set; }

    [CommandSwitch("--vm-image-family")]
    public string? VmImageFamily { get; set; }

    [CommandSwitch("--vm-image-name")]
    public string? VmImageName { get; set; }

    [CommandSwitch("--custom-gpu-driver-path")]
    public string? CustomGpuDriverPath { get; set; }

    [BooleanCommandSwitch("--install-gpu-driver")]
    public bool? InstallGpuDriver { get; set; }

    [CommandSwitch("--data-disk-encryption")]
    public string? DataDiskEncryption { get; set; }

    [CommandSwitch("--data-disk-size")]
    public string? DataDiskSize { get; set; }

    [CommandSwitch("--data-disk-type")]
    public string? DataDiskType { get; set; }

    [CommandSwitch("--data-disk-kms-key")]
    public string? DataDiskKmsKey { get; set; }

    [CommandSwitch("--data-disk-encryption-key-keyring")]
    public string? DataDiskEncryptionKeyKeyring { get; set; }

    [CommandSwitch("--data-disk-encryption-key-location")]
    public string? DataDiskEncryptionKeyLocation { get; set; }

    [CommandSwitch("--data-disk-encryption-key-project")]
    public string? DataDiskEncryptionKeyProject { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--nic-type")]
    public string? NicType { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subnet-region")]
    public string? SubnetRegion { get; set; }

    [CommandSwitch("--shielded-integrity-monitoring")]
    public string? ShieldedIntegrityMonitoring { get; set; }

    [CommandSwitch("--shielded-secure-boot")]
    public string? ShieldedSecureBoot { get; set; }

    [CommandSwitch("--shielded-vtpm")]
    public string? ShieldedVtpm { get; set; }
}