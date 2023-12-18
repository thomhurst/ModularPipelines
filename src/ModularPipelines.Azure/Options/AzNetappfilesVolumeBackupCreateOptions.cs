using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume", "backup", "create")]
public record AzNetappfilesVolumeBackupCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--backup-name")] string BackupName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--use-existing-snapshot")]
    public bool? UseExistingSnapshot { get; set; }
}