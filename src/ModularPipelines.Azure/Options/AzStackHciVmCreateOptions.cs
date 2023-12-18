using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack-hci-vm", "create")]
public record AzStackHciVmCreateOptions(
[property: CommandSwitch("--custom-location")] string CustomLocation,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--admin-password")]
    public string? AdminPassword { get; set; }

    [CommandSwitch("--admin-username")]
    public string? AdminUsername { get; set; }

    [BooleanCommandSwitch("--attach-data-disks")]
    public bool? AttachDataDisks { get; set; }

    [CommandSwitch("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CommandSwitch("--computer-name")]
    public string? ComputerName { get; set; }

    [BooleanCommandSwitch("--enable-agent")]
    public bool? EnableAgent { get; set; }

    [BooleanCommandSwitch("--enable-secure-boot")]
    public bool? EnableSecureBoot { get; set; }

    [BooleanCommandSwitch("--enable-vm-config-agent")]
    public bool? EnableVmConfigAgent { get; set; }

    [BooleanCommandSwitch("--enable-vtpm")]
    public bool? EnableVtpm { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--nics")]
    public string? Nics { get; set; }

    [CommandSwitch("--os-disk-name")]
    public string? OsDiskName { get; set; }

    [CommandSwitch("--os-type")]
    public string? OsType { get; set; }

    [CommandSwitch("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CommandSwitch("--security-type")]
    public string? SecurityType { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }

    [CommandSwitch("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CommandSwitch("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

    [CommandSwitch("--storage-path-id")]
    public string? StoragePathId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}