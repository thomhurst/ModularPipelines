using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "protection", "enable-for-urefileshare")]
public record AzBackupProtectionEnableForAzurefileshareOptions(
[property: CommandSwitch("--azure-file-share")] string AzureFileShare,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-account")] int StorageAccount,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--disk-list-setting")]
    public string? DiskListSetting { get; set; }

    [CommandSwitch("--diskslist")]
    public string? Diskslist { get; set; }

    [BooleanCommandSwitch("--exclude-all-data-disks")]
    public bool? ExcludeAllDataDisks { get; set; }
}