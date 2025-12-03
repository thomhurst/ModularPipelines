using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("urestackhci", "virtualmachine", "create")]
public record AzAzurestackhciVirtualmachineCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--admin-username")]
    public string? AdminUsername { get; set; }

    [CliFlag("--allow-password-auth")]
    public bool? AllowPasswordAuth { get; set; }

    [CliOption("--computer-name")]
    public string? ComputerName { get; set; }

    [CliOption("--data-disk")]
    public string? DataDisk { get; set; }

    [CliFlag("--disable-vm-management")]
    public bool? DisableVmManagement { get; set; }

    [CliFlag("--enable-tpm")]
    public bool? EnableTpm { get; set; }

    [CliOption("--extended-location")]
    public string? ExtendedLocation { get; set; }

    [CliOption("--hardware-profile")]
    public string? HardwareProfile { get; set; }

    [CliOption("--image-reference")]
    public string? ImageReference { get; set; }

    [CliOption("--linux-configuration")]
    public string? LinuxConfiguration { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--network-profile")]
    public string? NetworkProfile { get; set; }

    [CliOption("--nic-id")]
    public string? NicId { get; set; }

    [CliOption("--os-disk")]
    public string? OsDisk { get; set; }

    [CliOption("--os-profile")]
    public string? OsProfile { get; set; }

    [CliOption("--security-profile")]
    public string? SecurityProfile { get; set; }

    [CliOption("--ssh-public-keys")]
    public string? SshPublicKeys { get; set; }

    [CliOption("--storage-profile")]
    public string? StorageProfile { get; set; }

    [CliOption("--storagepath-id")]
    public string? StoragepathId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vm-size")]
    public string? VmSize { get; set; }

    [CliOption("--windows-configuration")]
    public string? WindowsConfiguration { get; set; }
}