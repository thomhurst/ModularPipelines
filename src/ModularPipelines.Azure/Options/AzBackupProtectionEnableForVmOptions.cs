using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "protection", "enable-for-vm")]
public record AzBackupProtectionEnableForVmOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName,
[property: CliOption("--vm")] string Vm
) : AzOptions
{
    [CliOption("--disk-list-setting")]
    public string? DiskListSetting { get; set; }

    [CliOption("--diskslist")]
    public string? Diskslist { get; set; }

    [CliFlag("--exclude-all-data-disks")]
    public bool? ExcludeAllDataDisks { get; set; }
}