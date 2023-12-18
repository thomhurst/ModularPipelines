using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "protection", "enable-for-vm")]
public record AzBackupProtectionEnableForVmOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName,
[property: CommandSwitch("--vm")] string Vm
) : AzOptions
{
    [CommandSwitch("--disk-list-setting")]
    public string? DiskListSetting { get; set; }

    [CommandSwitch("--diskslist")]
    public string? Diskslist { get; set; }

    [BooleanCommandSwitch("--exclude-all-data-disks")]
    public bool? ExcludeAllDataDisks { get; set; }
}

