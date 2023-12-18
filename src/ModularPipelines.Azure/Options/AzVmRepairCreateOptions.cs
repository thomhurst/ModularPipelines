using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "repair", "create")]
public record AzVmRepairCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--associate-public-ip")]
    public bool? AssociatePublicIp { get; set; }

    [CommandSwitch("--copy-disk-name")]
    public string? CopyDiskName { get; set; }

    [CommandSwitch("--distro")]
    public string? Distro { get; set; }

    [BooleanCommandSwitch("--enable-nested")]
    public bool? EnableNested { get; set; }

    [CommandSwitch("--repair-group-name")]
    public string? RepairGroupName { get; set; }

    [CommandSwitch("--repair-password")]
    public string? RepairPassword { get; set; }

    [CommandSwitch("--repair-username")]
    public string? RepairUsername { get; set; }

    [CommandSwitch("--repair-vm-name")]
    public string? RepairVmName { get; set; }

    [BooleanCommandSwitch("--unlock-encrypted-vm")]
    public bool? UnlockEncryptedVm { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

