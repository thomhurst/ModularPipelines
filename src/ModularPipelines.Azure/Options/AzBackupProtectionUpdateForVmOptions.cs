using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "protection", "update-for-vm")]
public record AzBackupProtectionUpdateForVmOptions : AzOptions
{
    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--disk-list-setting")]
    public string? DiskListSetting { get; set; }

    [CliOption("--diskslist")]
    public string? Diskslist { get; set; }

    [CliFlag("--exclude-all-data-disks")]
    public bool? ExcludeAllDataDisks { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--item-name")]
    public string? ItemName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}