using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "protection", "enable-for-urewl")]
public record AzBackupProtectionEnableForAzurewlOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--protectable-item-name")] string ProtectableItemName,
[property: CommandSwitch("--protectable-item-type")] string ProtectableItemType,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server-name")] string ServerName,
[property: CommandSwitch("--vault-name")] string VaultName,
[property: CommandSwitch("--workload-type")] string WorkloadType
) : AzOptions
{
    [CommandSwitch("--disk-list-setting")]
    public string? DiskListSetting { get; set; }

    [CommandSwitch("--diskslist")]
    public string? Diskslist { get; set; }

    [BooleanCommandSwitch("--exclude-all-data-disks")]
    public bool? ExcludeAllDataDisks { get; set; }
}