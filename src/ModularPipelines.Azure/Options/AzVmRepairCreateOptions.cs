using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "repair", "create")]
public record AzVmRepairCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--associate-public-ip")]
    public bool? AssociatePublicIp { get; set; }

    [CliOption("--copy-disk-name")]
    public string? CopyDiskName { get; set; }

    [CliOption("--distro")]
    public string? Distro { get; set; }

    [CliFlag("--enable-nested")]
    public bool? EnableNested { get; set; }

    [CliOption("--repair-group-name")]
    public string? RepairGroupName { get; set; }

    [CliOption("--repair-password")]
    public string? RepairPassword { get; set; }

    [CliOption("--repair-username")]
    public string? RepairUsername { get; set; }

    [CliOption("--repair-vm-name")]
    public string? RepairVmName { get; set; }

    [CliFlag("--unlock-encrypted-vm")]
    public bool? UnlockEncryptedVm { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}