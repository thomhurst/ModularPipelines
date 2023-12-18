using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("urestackhci", "virtualmachine", "create")]
public record AzAzurestackhciVirtualmachineCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--admin-password")]
    public string? AdminPassword { get; set; }

    [CommandSwitch("--admin-username")]
    public string? AdminUsername { get; set; }

    [BooleanCommandSwitch("--allow-password-auth")]
    public bool? AllowPasswordAuth { get; set; }

    [CommandSwitch("--computer-name")]
    public string? ComputerName { get; set; }

    [CommandSwitch("--data-disk")]
    public string? DataDisk { get; set; }

    [BooleanCommandSwitch("--disable-vm-management")]
    public bool? DisableVmManagement { get; set; }

    [BooleanCommandSwitch("--enable-tpm")]
    public bool? EnableTpm { get; set; }

    [CommandSwitch("--extended-location")]
    public string? ExtendedLocation { get; set; }

    [CommandSwitch("--hardware-profile")]
    public string? HardwareProfile { get; set; }

    [CommandSwitch("--image-reference")]
    public string? ImageReference { get; set; }

    [CommandSwitch("--linux-configuration")]
    public string? LinuxConfiguration { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--network-profile")]
    public string? NetworkProfile { get; set; }

    [CommandSwitch("--nic-id")]
    public string? NicId { get; set; }

    [CommandSwitch("--os-disk")]
    public string? OsDisk { get; set; }

    [CommandSwitch("--os-profile")]
    public string? OsProfile { get; set; }

    [CommandSwitch("--security-profile")]
    public string? SecurityProfile { get; set; }

    [CommandSwitch("--ssh-public-keys")]
    public string? SshPublicKeys { get; set; }

    [CommandSwitch("--storage-profile")]
    public string? StorageProfile { get; set; }

    [CommandSwitch("--storagepath-id")]
    public string? StoragepathId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vm-size")]
    public string? VmSize { get; set; }

    [CommandSwitch("--windows-configuration")]
    public string? WindowsConfiguration { get; set; }
}